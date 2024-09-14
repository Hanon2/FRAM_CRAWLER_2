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
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Elcometer.Demo.Windows.UWP.Pages
{
    public sealed partial class BatchesPage : Page
    {
        private IGauge _gauge;
        private bool _inBatching;

        public BatchesPage()
        {
            this.InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Required;

            BackButton.Click += BackButton_Click;
            this.Loaded += BatchesPage_Loaded;
            BatchesListView.SelectionChanged += BatchesListView_SelectionChanged;
        }


        private async void BatchesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ElcometerCore.Instance.BatchService.Batches.Clear();

            var gaugeBatch = BatchesListView.SelectedItem as IGaugeBatch;

            if (gaugeBatch != null)
            {
                IsBusyProgressRing.Visibility = Visibility.Visible;

                // ask the gauge to download the batches into the service
                await Task.Run(() => _gauge.DownloadBatchesTo(new List<IGaugeBatch> { gaugeBatch }, ElcometerCore.Instance.BatchService));

                // downloaded ok ?
                if (ElcometerCore.Instance.BatchService.Batches.Count > 0)
                {
                    Frame.Navigate(typeof(BatchInfoPage), ElcometerCore.Instance.BatchService.Batches[0]);
                }

                IsBusyProgressRing.Visibility = Visibility.Collapsed;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _gauge = e.Parameter as IGauge;
        }

        private async void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                if (_inBatching)
                {
                    await Task.Run(() => _gauge.EndBatching());
                }

                this.Frame.GoBack();
            }
        }

        private async void BatchesPage_Loaded(object sender, RoutedEventArgs e)
        {
            IsBusyProgressRing.Visibility = Visibility.Visible;

            if (!_inBatching)
            {
                await Task.Run(() => _gauge.StartBatching());
                _inBatching = true;
            }

            // request the batches
            var batches = await Task.Run(() => _gauge.GetBatches());

            BatchesListView.Items.Clear();

            foreach (var batch in batches)
            {
                BatchesListView.Items.Add(batch);
            }

            IsBusyProgressRing.Visibility = Visibility.Collapsed;
        }
    }
}
