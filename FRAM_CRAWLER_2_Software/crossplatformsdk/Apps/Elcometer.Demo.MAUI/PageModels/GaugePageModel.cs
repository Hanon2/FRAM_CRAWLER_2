using Elcometer.Core.Services;
using Elcometer.Demo.Xamarin.Forms.MVVM;
using System;

namespace Elcometer.Demo.Xamarin.Forms.PageModels
{
    public class GaugePageModel : BaseViewModel
    {
        private Command _downloadBatchesCommand;
        private string _liveMeasurement;
        private IMessagingService _messagingService;
        private INavigationService _navigator;
        private ISimpleBatchService _simpleBatchService;

        public GaugePageModel(IMessagingService messagingService, ISimpleBatchService simpleBatchService, INavigationService navigator)
        {
            _messagingService = messagingService;
            _simpleBatchService = simpleBatchService;
            _navigator = navigator;
        }

        public Command DownloadBatchesCommand
        {
            get
            {
                return _downloadBatchesCommand ?? (_downloadBatchesCommand = new Command(async () =>
                {
                    await _navigator.PushAsync<BatchesPageModel>(x => x.Gauge = Gauge);
                }));
            }
        }

        public IGauge Gauge { get; set; }

        public string LiveMeasurement
        {
            get { return _liveMeasurement; }
            set { Set(() => LiveMeasurement, ref _liveMeasurement, value); }
        }

        public override void ViewClosed()
        {
            // unsubscribe from the live reading messages
            _messagingService.Unsubscribe<ILiveReadingMessageParams>(this, ElcometerCoreMessages.LiveReadingMessage);

            base.ViewClosed();
        }

        public override void ViewOpened()
        {
            base.ViewOpened();

            // register for all live readings (this will be for all connected gauges)
            _messagingService.Subscribe<ILiveReadingMessageParams>(this, ElcometerCoreMessages.LiveReadingMessage, OnLiveReading);
        }

        private void OnLiveReading(object sender, ILiveReadingMessageParams args)
        {
            // we only want to process the live readings from the gauge we are 'viewing'
            if (sender == Gauge)
            {
                string readingString = "";

                // we need to create a dummy batch so we can use it to format the readings into
                // the correct units
                var dummyBatch = args.CreateEmtpyBatch(_simpleBatchService, "");

                // build the reading columns into a single line
                foreach (var reading in args.GetReadings(dummyBatch))
                {
                    if (!String.IsNullOrEmpty(readingString))
                    {
                        readingString += ", ";
                    }

                    // we are combined reading value formatted as strings here - the numeric value is available in the NumericValue property
                    readingString += reading.Value;
                }

                readingString += "\n";

                // add measured reading text to the log string
                LiveMeasurement += readingString;
            }
        }
    }
}