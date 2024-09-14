using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Elcometer.Core.Droid;
using Elcometer.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elcometer.Demo.Droid
{
    /// <summary>
    /// This activity will enumerate all the batches on the gauge and allow one to be selected and its measurements viewed
    /// </summary>
    [Activity(Label = "BatchesActivity")]
    public class BatchesActivity : Activity
    {
        private ArrayAdapter<BatchListItem> _batches;
        private IGauge _gauge;
        private ListView _listViewAvailableBatches;

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            // implements software back button
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            SetContentView(Resource.Layout.Batches);

            // get the unique id for the gauge
            var gaugeBluetoothId = Intent.GetStringExtra("GaugeBluetoothId");

            // get the IGauge interface for this gauge Id
            _gauge = ElcometerCore.Instance.GaugeService.Gauges.FirstOrDefault(x => x.DeviceInfo.Description == gaugeBluetoothId);

            // list of available batches
            _batches = new ArrayAdapter<BatchListItem>(this, Android.Resource.Layout.SimpleListItem1);
            _listViewAvailableBatches = FindViewById<ListView>(Resource.Id.availableBatches);
            _listViewAvailableBatches.Adapter = _batches;
            _listViewAvailableBatches.ItemClick += ListViewAvailableBatches_ItemClick;

            // put the gauge into a mode batches can be downloaded from it
            await Task.Run(() => _gauge.StartBatching());

            // request the batches
            var batches = await Task.Run(() => _gauge.GetBatches());

            // populate the list
            _batches.AddAll(batches.Select(x => new BatchListItem(x)).ToArray());
        }

        protected override void OnDestroy()
        {
            // put the gauge back into live reading mode
            Task.Run(() => _gauge.EndBatching());

            base.OnDestroy();
        }

        private async void ListViewAvailableBatches_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var batchListItem = _batches.GetItem(e.Position) as BatchListItem;

            // make sure its a valid batch item that was picked - should always be...
            if (batchListItem != null)
            {
                // clear the batch service of any previously download batches
                ElcometerCore.Instance.BatchService.Batches.Clear();

                // do the actual download - could take a few seconds depending on the number of readings
                // in a proper application you'd want to show an activity wheel while this is occuring
                await Task.Run(() => _gauge.DownloadBatchesTo(new List<IGaugeBatch> { batchListItem.Batch }, ElcometerCore.Instance.BatchService));

                string info = "";

                // format the batch info into a string
                foreach (var batch in ElcometerCore.Instance.BatchService.Batches)
                {
                    info += $"Batch: {batch.Name}\n";
                    info += $"Readings: {batch.IncludedCount}\n";
                    info += "\n";

                    foreach (var reading in batch.GetReadings().Select(x => x.Value).OrderBy(x => x.Id).ToList())
                    {
                        if (reading.IncludeInStats)
                        {
                            string readings = "";

                            for (int i = 0; i < batch.ColumnCount; i++)
                            {
                                if (!String.IsNullOrEmpty(readings))
                                {
                                    readings += ", ";
                                }

                                readings += reading.ToDisplayStringWithUnit(i);
                            }

                            info += $"{reading.Id}: {readings}\n";
                        }
                    }
                }

                // we pass the batch info string to show in the batch info activity
                var activity = new Intent(this, typeof(BatchInfoActivity));
                activity.PutExtra("BatchInfo", info);
                StartActivity(activity);
            }
        }

        private class BatchListItem
        {
            public BatchListItem(IGaugeBatch batch)
            {
                Batch = batch;
            }

            public IGaugeBatch Batch { get; }

            public override string ToString()
            {
                return Batch.Name;
            }
        }
    }
}