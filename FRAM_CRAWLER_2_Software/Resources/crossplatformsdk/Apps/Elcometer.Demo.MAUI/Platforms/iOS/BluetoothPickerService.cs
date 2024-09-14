using Elcometer.Core.iOS.Services;
using Elcometer.Core.Services;
using Elcometer.Demo.Xamarin.Forms.Services;
using System.Threading.Tasks;

namespace Elcometer.Demo.Xamarin.Forms.iOS.Services
{
    public class BluetoothPickerService : IBluetoothPickerService
    {
        private ConnectionService _bluetoothService;

        public BluetoothPickerService(IConnectionService connectionService)
        {
            _bluetoothService = connectionService as ConnectionService;
        }

        public Task ShowPickerClassic(string contains = null)
        {
            return _bluetoothService?.ShowPickerClassic(contains);
        }


        public Task ShowPickerLE(string contains = null)
        {
            return _bluetoothService?.ShowPickerLE(contains);
        }
    }
}