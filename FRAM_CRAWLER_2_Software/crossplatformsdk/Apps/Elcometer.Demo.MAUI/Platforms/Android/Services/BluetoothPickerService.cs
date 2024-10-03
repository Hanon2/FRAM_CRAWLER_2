using Elcometer.Core.Droid.Services;
using Elcometer.Core.Services;
using Elcometer.Demo.MAUI;
using Elcometer.Demo.MAUI.Platforms.Android.Services;
using Elcometer.Demo.Xamarin.Forms.Services;
using Resource = Elcometer.Demo.MAUI.Resource;

namespace Elcometer.Demo.Xamarin.Forms.Droid.Services
{
    public class BluetoothPickerService : IBluetoothPickerService
    {
        private ConnectionService _bluetoothService;
        private IMauiHelper _mauiHelper;

        public BluetoothPickerService(IConnectionService connectionService, IMauiHelper mauiHelper)
        {
            _bluetoothService = connectionService as ConnectionService;
            _mauiHelper = mauiHelper;
        }

        public async Task ShowPickerClassic(string contains = null)
        {
            var activity = _mauiHelper.GetMainActivity();

            if (activity != null && _bluetoothService != null)
            {
                if (await activity.RequestBluetoothPermission())
                {
                    await _bluetoothService.ShowPickerClassic(activity, activity.Resources.GetString(Resource.String.select_a_bluetooth_device), contains);
                }
            }
        }

        public async Task ShowPickerLE(string contains = null)
        {
            var activity = _mauiHelper.GetMainActivity();

            if (activity != null && _bluetoothService != null)
            {
                if (await activity.RequestBluetoothPermission())
                {
                    await _bluetoothService.ShowPickerLE(activity, activity.Resources.GetString(Resource.String.select_a_bluetooth_device), contains);
                }
            }
        }
    }
}