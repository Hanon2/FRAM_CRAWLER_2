using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Elcometer.Core.Droid.Services;
using Elcometer.Core.Services;
using Elcometer.Demo.MAUI.Platforms.Android.Services;
using Elcometer.Demo.Xamarin.Forms.Droid.Services;
using Elcometer.Demo.Xamarin.Forms.MVVM;
using Elcometer.Demo.Xamarin.Forms.Services;
using Microsoft.Extensions.Logging;
using triaxis.BluetoothLE;

namespace Elcometer.Demo.MAUI
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        private const int RequestLocationId = 0;
        private TaskCompletionSource<bool> _tcs;
        const int RequestPremissionsId = 5001;

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            // workaround for xamarin forms android not triggering back button button event for software back button
            if (item.ItemId == global::Android.Resource.Id.Home)
            {
                var naviagator = SimpleContainer.Instance.Resolve<INavigationService>();
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

            var helper = SimpleContainer.Instance.Resolve<IMauiHelper>();
            helper.SetMainActivity(this);
        }
    }
}