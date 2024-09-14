using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Elcometer.Demo.Droid
{
    /// <summary>
    /// This activity just shows the batch info string of the downloaded batch which was previously constructed in the batches activity
    /// </summary>
    [Activity(Label = "BatchInfoActivity")]
    public class BatchInfoActivity : Activity
    {
        private TextView _textViewBatchInfo;

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

            // get the info for the batch
            var batchInfo = Intent.GetStringExtra("BatchInfo");

            SetContentView(Resource.Layout.BatchInfo);

            // show the batch info in the textview
            _textViewBatchInfo = FindViewById<TextView>(Resource.Id.batchInfo);
            _textViewBatchInfo.Typeface = Typeface.Monospace;
            _textViewBatchInfo.Text = batchInfo;
        }
    }
}