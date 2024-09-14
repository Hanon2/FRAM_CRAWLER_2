using System.Threading.Tasks;

namespace Elcometer.Demo.Xamarin.Forms.Services
{
    public interface IBluetoothPickerService
    {
        /// <summary>
        /// Shows the Bluetooth Classic gauge selection picker
        /// </summary>
        /// <param name="contains">filter that only displays gauges that contain given string in picker</param>
        Task ShowPickerClassic(string contains = null);

        /// <summary>
        /// Shows the Bluetooth LE gauge selection picker
        /// </summary>
        /// <param name="contains">filter that only displays gauges that contain given string in picker</param>
        Task ShowPickerLE(string contains = null);
    }
}