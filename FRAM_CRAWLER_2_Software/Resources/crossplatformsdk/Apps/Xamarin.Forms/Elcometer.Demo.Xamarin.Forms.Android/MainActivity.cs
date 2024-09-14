using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Views;
using Elcometer.Core.Droid.Services;
using Elcometer.Core.Services;
using Elcometer.Demo.Xamarin.Forms.Droid.Services;
using Elcometer.Demo.Xamarin.Forms.MVVM;
using Elcometer.Demo.Xamarin.Forms.Services;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using triaxis.BluetoothLE;
using Microsoft.Extensions.Logging;

namespace Elcometer.Demo.Xamarin.Forms.Droid
{
    [Activity(Label = "Elcometer.Demo.Xamarin.Forms", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity, ActivityCompat.IOnRequestPermissionsResultCallback
    {
        private const int RequestLocationId = 0;
        private SimpleContainer _container;
        private TaskCompletionSource<bool> _tcs;
        const int RequestPremissionsId = 5001;

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            // workaround for xamarin forms android not triggering back button button event for software back button
            if (item.ItemId == global::Android.Resource.Id.Home)
            {
                var naviagator = _container.Resolve<INavigationService>();
                naviagator.PopAsync();
                return false;
            }

            return base.OnOptionsItemSelected(item);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
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

        public async Task<bool> RequestBluetoothPermission()
        {
            if ((int)Build.VERSION.SdkInt < 23)
            {
                return true;
            }

            _tcs = new TaskCompletionSource<bool>();

            var required = new List<string>();

            if ((int)Build.VERSION.SdkInt < 31)
            {
                if (CheckSelfPermission(Manifest.Permission.AccessCoarseLocation) != (int)Permission.Granted)
                {
                    required.Add(Manifest.Permission.AccessCoarseLocation);
                }

                if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != (int)Permission.Granted)
                {
                    required.Add(Manifest.Permission.AccessFineLocation);
                }
            }
            else
            {
                if (CheckSelfPermission(Manifest.Permission.BluetoothScan) != (int)Permission.Granted)
                {
                    required.Add(Manifest.Permission.BluetoothScan);
                }

                if (CheckSelfPermission(Manifest.Permission.BluetoothConnect) != (int)Permission.Granted)
                {
                    required.Add(Manifest.Permission.BluetoothConnect);
                }
            }

            if (required.Count > 0)
            {
                RequestPermissions(required.ToArray(), RequestPremissionsId);
                return await _tcs.Task;
            }

            // all permissions already got
            return true;
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // as an example of using dependancy injection and MVVM to use the services
            // this demo uses a very basic MVVM framework, but in your app you can use whatever
            // IOC & MVVM framework you wish or just use the singleton "ElcometerCore"

            // these are the platform dependant services registered into the dependacy injector
            // note that the gauge picker has been made available as a cross platform picker as we
            // can now get at the foreground activity via Xamarin.Forms.Forms.Context

            _container = new SimpleContainer();
            _container.RegisterSingleton<IConnectionService, ConnectionService>();
            _container.RegisterSingleton<IPlatformService, PlatformService>();
            _container.RegisterSingleton<IBluetoothPickerService, BluetoothPickerService>();
            _container.RegisterSingleton<IBluetoothLE, triaxis.BluetoothLE.Platform>(new triaxis.BluetoothLE.Platform(new LoggerFactory()));

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(_container));
        }
    }
}