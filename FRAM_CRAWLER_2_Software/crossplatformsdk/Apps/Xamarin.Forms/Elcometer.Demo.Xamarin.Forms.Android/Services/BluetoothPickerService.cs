using Elcometer.Core.Droid.Services;
using Elcometer.Core.Services;
using Elcometer.Demo.Xamarin.Forms.Services;
using System.Threading.Tasks;

namespace Elcometer.Demo.Xamarin.Forms.Droid.Services
{
    public class BluetoothPickerService : IBluetoothPickerService
    {
        private ConnectionService _bluetoothService;

        public BluetoothPickerService(IConnectionService connectionService)
        {
            _bluetoothService = connectionService as ConnectionService;
        }

        public async Task ShowPickerClassic(string contains = null)
        {
            var activity = global::Xamarin.Forms.Forms.Context as MainActivity;

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
            var activity = global::Xamarin.Forms.Forms.Context as MainActivity;

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