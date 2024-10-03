using Elcometer.Core;
using Elcometer.Core.Services;
using Elcometer.Demo.Xamarin.Forms.MVVM;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Elcometer.Demo.Xamarin.Forms.PageModels
{
    public class BatchesPageModel : BaseViewModel
    {
        private INavigationService _navigator;
        private Command<IGaugeBatch> _selectBatchCommand;
        private IGaugeBatch _selectedBatch;
        private ISimpleBatchService _simpleBatchService;

        public BatchesPageModel(ISimpleBatchService simpleBatchService, INavigationService navigator)
        {
            _simpleBatchService = simpleBatchService;
            _navigator = navigator;
        }

        public ObservableCollectionFast<IGaugeBatch> Batches { get; } = new ObservableCollectionFast<IGaugeBatch>();

        public IGauge Gauge { get; set; }

        public Command<IGaugeBatch> SelectBatchCommand
        {
            get
            {
                return _selectBatchCommand ?? (_selectBatchCommand = new Command<IGaugeBatch>(async (x) =>
                {
                    if (x != null)
                    {
                        _simpleBatchService.Batches.Clear();

                        IsBusy = true;

                        // ask the gauge to download the batches into the service
                        await Task.Run(() => Gauge.DownloadBatchesTo(new List<IGaugeBatch> { x }, _simpleBatchService));

                        IsBusy = false;

                        // downloaded ok ?
                        if (_simpleBatchService.Batches.Count > 0)
                        {
                            // show the download batch
                            await _navigator.PushAsync<BatchInfoPageModel>(y => y.Batch = _simpleBatchService.Batches[0]);
                        }

                        SelectedBatch = null;
                    }
                }));
            }
        }

        public IGaugeBatch SelectedBatch
        {
            get { return _selectedBatch; }
            set { Set(() => SelectedBatch, ref _selectedBatch, value); SelectBatchCommand.Execute(value); }
        }

        public override async void ViewClosed()
        {
            await Task.Run(() => Gauge.EndBatching());

            base.ViewClosed();
        }

        public override async void ViewOpened()
        {
            base.ViewOpened();

            IsBusy = true;

            // put the gauge into a mode batches can be downloaded from it
            await Task.Run(() => Gauge.StartBatching());

            // request the batches
            var batches = await Task.Run(() => Gauge.GetBatches());

            IsBusy = false;

            // populate the list
            Batches.Reset(batches);
        }
    }
}