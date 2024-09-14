using Elcometer.Core.Services;
using Elcometer.Demo.Xamarin.Forms.MVVM;
using Elcometer.Demo.Xamarin.Forms.Services;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Elcometer.Demo.Xamarin.Forms.PageModels
{
    public class MainPageModel : BaseViewModel
    {
        private IBluetoothPickerService _bluetoothPickerService;
        private Command _findClassicGaugeCommand;
        private Command _findLEGaugeCommand;
        private IGaugeService _gaugeService;
        private INavigationService _navigator;
        private IGauge _selectedGauge;
        private Command<IGauge> _selectGaugeCommand;

        public MainPageModel(IGaugeService gaugeService, IBluetoothPickerService bluetoothPickerService, INavigationService navigator)
        {
            _gaugeService = gaugeService;
            _bluetoothPickerService = bluetoothPickerService;
            _navigator = navigator;
        }

        public Command FindClassicGaugeCommand
        {
            get
            {
                return _findClassicGaugeCommand ?? (_findClassicGaugeCommand = new Command(async () => await _bluetoothPickerService.ShowPickerClassic()));
            }
        }

        public Command FindLEGaugeCommand
        {
            get
            {
                return _findLEGaugeCommand ?? (_findLEGaugeCommand = new Command(async () => await _bluetoothPickerService.ShowPickerLE()));
            }
        }

        public ObservableCollection<IGauge> Gauges
        {
            get
            {
                return _gaugeService.Gauges;
            }
        }

        public IGauge SelectedGauge
        {
            get { return _selectedGauge; }
            set { Set(() => SelectedGauge, ref _selectedGauge, value); SelectGaugeCommand.Execute(value); }
        }

        public Command<IGauge> SelectGaugeCommand
        {
            get
            {
                return _selectGaugeCommand ?? (_selectGaugeCommand = new Command<IGauge>(async (x) =>
                {
                    if (x != null)
                    {
                        await _navigator.PushAsync<GaugePageModel>(y => y.Gauge = x);

                        SelectedGauge = null;
                    }
                }));
            }
        }
    }
}