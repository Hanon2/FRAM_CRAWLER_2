using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Elcometer.Core.UWP;
using System.Collections.Specialized;
using Elcometer.Core.Services;
using Windows.UI.ViewManagement;
using System;
using Windows.UI.Xaml.Navigation;

namespace Elcometer.Demo.Windows.UWP.Pages
{
    public sealed partial class GaugePage : Page
    {
        private IGauge _gauge;

        public GaugePage()
        {
            this.InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Required;

            BackButton.Click += BackButton_Click;
            this.Loaded += GaugePage_Loaded;
            this.Unloaded += GaugePage_Unloaded;
            DownloadBatchesButton.Click += DownloadBatchesButton_Click;
        }

        private void DownloadBatchesButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BatchesPage), _gauge);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _gauge = e.Parameter as IGauge;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        private void GaugePage_Unloaded(object sender, RoutedEventArgs e)
        {
            ElcometerCore.Instance.MessagingService.Unsubscribe<ILiveReadingMessageParams>(this, ElcometerCoreMessages.LiveReadingMessage);

            _gauge.PropertyChanged -= Gauge_PropertyChanged;
        }

        private void UpdateControlState()
        {
            DownloadBatchesButton.IsEnabled = _gauge.SupportsBatching;
        }

        private void GaugePage_Loaded(object sender, RoutedEventArgs e)
        {
            _gauge.PropertyChanged += Gauge_PropertyChanged;

            UpdateControlState();

            ElcometerCore.Instance.MessagingService.Subscribe<ILiveReadingMessageParams>(this, ElcometerCoreMessages.LiveReadingMessage, OnLiveReading);
        }

        private void Gauge_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateControlState();
        }

        private void OnLiveReading(object arg1, ILiveReadingMessageParams args)
        {
            string readingString = "";

            // we need to create a dummy batch so we can use it to format the readings into
            // the correct units
            var dummyBatch = args.CreateEmtpyBatch(ElcometerCore.Instance.BatchService, "");

            // change units of dummy batch to match gauge (supported on 456, 224, 311, 415)
            //args.ChangeUnits(dummyBatch);

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

            readingString += "\r\n";

            // add reading text
            LiveTextBlock.Text += readingString;
            LiveScrollViewer.ChangeView(null, LiveScrollViewer.ExtentHeight, null);
        }
    }
}
