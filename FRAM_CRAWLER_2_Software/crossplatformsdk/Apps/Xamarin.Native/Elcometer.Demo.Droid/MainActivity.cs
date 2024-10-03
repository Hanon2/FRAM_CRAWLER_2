using Android;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Elcometer.Core.Droid;
using Elcometer.Core.Services;

namespace Elcometer.Demo.Droid
{
    /// <summary>
    /// This activity allows the gauge to be picked from a dialog which will initiate a connection.
    /// Below the pick gauge button is a list of currently connected gauges which can be selected to
    /// begin live measurements or download batch data.
    /// </summary>
    [Activity(Label = "Elcometer.Demo.Droid", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private const int RequestLocationId = 0;
        private Button _buttonSelectBluetoothClassic;
        private Button _buttonSelectBluetoothLE;
        private ArrayAdapter<GaugeListItem> _gauges;
        private ListView _listViewConnectedGauges;
        private TaskCompletionSource<bool> _tcs;

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            // this is called back from android (>= 6.0) when the request for the location permission is granted or refused
            // location permission is required for enumerating nearby bluetooth devices on android 6.0 and above
            switch (requestCode)
            {
                case RequestLocationId:
                    {
                        if (grantResults.All(x => x == Permission.Granted))
                        {
                            _tcs.TrySetResult(true);
                        }
                        else
                        {
                            _tcs.TrySetResult(false);
                        }
                    }
                    break;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            // button that shows classic gauge picker
            _buttonSelectBluetoothClassic = FindViewById<Button>(Resource.Id.selectBluetoothClassic);
            _buttonSelectBluetoothClassic.Click += ButtonSelectBluetoothClassic_Click;


            // button that shows LE gauge picker
            _buttonSelectBluetoothLE = FindViewById<Button>(Resource.Id.selectBluetoothLE);
            _buttonSelectBluetoothLE.Click += ButtonSelectBluetoothLE_Click;

            // list of currently connected gauges
            _gauges = new ArrayAdapter<GaugeListItem>(this, Android.Resource.Layout.SimpleListItem1);
            _listViewConnectedGauges = FindViewById<ListView>(Resource.Id.connectedGauges);
            _listViewConnectedGauges.Adapter = _gauges;
            _listViewConnectedGauges.ItemClick += ListViewConnectedGauges_ItemClick;

            // register to get updates to connected gauges
            ElcometerCore.Instance.GaugeService.Gauges.CollectionChanged += Gauges_CollectionChanged;

            // get current connected list
            UpdateGaugeListView();
        }

        protected override void OnDestroy()
        {
            // unreigster from changes to connected gauges collection
            ElcometerCore.Instance.GaugeService.Gauges.CollectionChanged -= Gauges_CollectionChanged;

            base.OnDestroy();
        }

        private async void ButtonSelectBluetoothClassic_Click(object sender, System.EventArgs e)
        {
            bool hasPermission = true;

            // for android 6 and above we need to request the location permissions to enable a scan for nearby Bluetooth devices
            // https://developer.android.com/about/versions/marshmallow/android-6.0-changes.html#behavior-hardware-id
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                hasPermission = await RequestBluetoothPermission();
            }

            if (hasPermission)
            {
                // the picker does not wait for the gauge to be connected
                // connected gauges will appear in the Gauges collection
                await ElcometerCore.Instance.ConnectionService.ShowPickerClassic(this, Resources.GetString(Resource.String.select_a_bluetooth_device));
            }
        }

        private async void ButtonSelectBluetoothLE_Click(object sender, System.EventArgs e)
        {
            bool hasPermission = true;

            // for android 6 and above we need to request the location permissions to enable a scan for nearby Bluetooth devices
            // https://developer.android.com/about/versions/marshmallow/android-6.0-changes.html#behavior-hardware-id
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                hasPermission = await RequestBluetoothPermission();
            }

            if (hasPermission)
            {
                // the picker does not wait for the gauge to be connected
                // connected gauges will appear in the Gauges collection
                await ElcometerCore.Instance.ConnectionService.ShowPickerLE(this, Resources.GetString(Resource.String.select_a_bluetooth_device));
            }
        }

        private void Gauges_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // when any gauge is connected/disconnect just update the entire list view
            UpdateGaugeListView();
        }

        private void ListViewConnectedGauges_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var gaugeListItem = _gauges.GetItem(e.Position) as GaugeListItem;

            if (gaugeListItem != null)
            {
                // use has selected a gauge so we want to allow them to take live measurements and
                // download batches.
                //
                // we pass the bluetooth description to gauge activty which will be unique
                var activity = new Intent(this, typeof(GaugeActivity));
                activity.PutExtra("GaugeBluetoothId", gaugeListItem.Gauge.DeviceInfo.Description);
                StartActivity(activity);
            }
        }

        private async Task<bool> RequestBluetoothPermission()
        {
            _tcs = new TaskCompletionSource<bool>();

            // we actually need the location permissions to be able to do a scan for nearby bluetooth devices

            var permissions = new List<string>();

            if ((int)Build.VERSION.SdkInt < 31)
            {
                if (CheckSelfPermission(Manifest.Permission.AccessCoarseLocation) != (int)Permission.Granted)
                {
                    permissions.Add(Manifest.Permission.AccessCoarseLocation);
                }

                if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != (int)Permission.Granted)
                {
                    permissions.Add(Manifest.Permission.AccessFineLocation);
                }
            }
            else
            {
                if (CheckSelfPermission(Manifest.Permission.AccessCoarseLocation) != (int)Permission.Granted)
                {
                    permissions.Add(Manifest.Permission.BluetoothScan);
                }

                if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != (int)Permission.Granted)
                {
                    permissions.Add(Manifest.Permission.BluetoothConnect);
                }
            }

            if (permissions.Count == 0)
            {
                return true;
            }

            // Finally request permissions with the list of permissions and Id
            RequestPermissions(permissions.ToArray(), RequestLocationId);

            // indicate permissions granted
            return await _tcs.Task;
        }

        private void UpdateGaugeListView()
        {
            _gauges.Clear();

            // copy the list of connected gauges to the list adaptor
            foreach (var gauge in ElcometerCore.Instance.GaugeService.Gauges)
            {
                _gauges.Add(new GaugeListItem(gauge));
            }
        }

        private class GaugeListItem
        {
            public GaugeListItem(IGauge gauge)
            {
                Gauge = gauge;
            }

            public IGauge Gauge { get; }

            public override string ToString()
            {
                return Gauge.DeviceInfo.Description;
            }
        }
    }
}