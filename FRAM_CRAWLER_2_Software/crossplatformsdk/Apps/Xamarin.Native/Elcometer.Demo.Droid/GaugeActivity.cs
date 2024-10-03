using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Elcometer.Core.Droid;
using Elcometer.Core.Services;
using System;
using System.Linq;

namespace Elcometer.Demo.Droid
{
    /// <summary>
    /// This activity allows the taking of live measurements for the associated connected gauge.
    ///
    /// The process to download batches can also be started.
    /// </summary>
    [Activity(Label = "GaugeActivity")]
    public class GaugeActivity : Activity
    {
        private Button _buttonDownloadBatches;
        private IGauge _gauge;
        private TextView _textViewInfo;
        private ScrollView _scrollViewInfo;

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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            SetContentView(Resource.Layout.Gauge);

            // button that will allow the selecting of batches on the gauge for viewing
            _buttonDownloadBatches = FindViewById<Button>(Resource.Id.downloadBatches);
            _buttonDownloadBatches.Click += ButtonDownloadBatches_Click;

            // log of live measurements taken
            _textViewInfo = FindViewById<TextView>(Resource.Id.info);
            _textViewInfo.Typeface = Typeface.Monospace;
            _scrollViewInfo= FindViewById<ScrollView>(Resource.Id.infoScroll);

            // get the unique id for the gauge
            var gaugeBluetoothId = Intent.GetStringExtra("GaugeBluetoothId");

            // get the IGauge interface for this gauge Id
            _gauge = ElcometerCore.Instance.GaugeService.Gauges.FirstOrDefault(x => x.DeviceInfo.Description == gaugeBluetoothId);

            // register for all live readings (this will be for all connected gauges)
            ElcometerCore.Instance.MessagingService.Subscribe<ILiveReadingMessageParams>(this, ElcometerCoreMessages.LiveReadingMessage, OnLiveReading);
        }

        protected override void OnDestroy()
        {
            // unsubscribe for live reading messages
            ElcometerCore.Instance.MessagingService.Unsubscribe<ILiveReadingMessageParams>(this, ElcometerCoreMessages.LiveReadingMessage);

            base.OnDestroy();
        }

        private void ButtonDownloadBatches_Click(object sender, EventArgs e)
        {
            // start the process for downloading and viewing a batch
            var activity = new Intent(this, typeof(BatchesActivity));
            activity.PutExtra("GaugeBluetoothId", _gauge.DeviceInfo.Description);
            StartActivity(activity);
        }

        private void ScrollToBottom()
        {
            RunOnUiThread(() =>
            {
                _scrollViewInfo.SmoothScrollTo(0, _textViewInfo.Bottom);
            });
        }

        private void OnLiveReading(object sender, ILiveReadingMessageParams args)
        {
            // we only want to process the live readings from the gauge we are 'viewing'
            if (sender == _gauge)
            {
                string readingString = "";

                // we need to create a dummy batch so we can use it to format the readings into
                // the correct units
                var dummyBatch = args.CreateEmtpyBatch(ElcometerCore.Instance.BatchService, "");

                // build the reading columns into a single line
                foreach (var reading in args.GetReadings(dummyBatch))
                {
                    if (!String.IsNullOrEmpty(readingString))
                    {
                        readingString += ", ";
                    }

                    // we are combined reading value formatted as strings here - the numeric value is available in the NumericValue property
                    readingString += reading.Value;
                }

                readingString += "\n";

                // add reading text
                _textViewInfo.Append(readingString);

                ScrollToBottom();
            }
        }
    }
}