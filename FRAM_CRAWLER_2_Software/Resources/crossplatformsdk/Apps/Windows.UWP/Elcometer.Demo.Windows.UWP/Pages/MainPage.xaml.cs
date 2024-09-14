using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Elcometer.Core.UWP;
using System.Collections.Specialized;
using Elcometer.Core.Services;
using Windows.UI.ViewManagement;
using Windows.UI.Core;
using Windows.UI.Core.Preview;

namespace Elcometer.Demo.Windows.UWP.Pages
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            ApplicationView.PreferredLaunchViewSize = new Size(640, 480);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            this.Loaded += MainPage_Loaded;

            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += OnMainPageCloseRequest;

            ConnectGaugeButton.Click += ConnectGaugeButton_Click;
            DisconnectGaugeButton.Click += DisconnectGaugeButton_Click;
            ViewGaugeButton.Click += ViewGaugeButton_Click;
            GaugeListView.SelectionChanged += GaugeListView_SelectionChanged;

            UpdateControlState();
        }

        private void OnMainPageCloseRequest(object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        {
            var deferral = e.GetDeferral();

            ElcometerCore.Instance.GaugeService.Gauges.CollectionChanged -= Gauges_CollectionChanged;

            foreach (var gauge in ElcometerCore.Instance.GaugeService.Gauges)
            {
                ElcometerCore.Instance.GaugeService.Disconnect(gauge);
            }

            deferral.Complete();
        }

        private void GaugeListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateControlState();
        }

        private void ViewGaugeButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GaugePage), GaugeListView.SelectedItem as IGauge);
        }

        private void UpdateControlState()
        {
            DisconnectGaugeButton.IsEnabled = GaugeListView.SelectedItem != null;
            ViewGaugeButton.IsEnabled = GaugeListView.SelectedItem != null;
        }

        private void PopulateGauges()
        {
            GaugeListView.Items.Clear();

            foreach (var gauge in ElcometerCore.Instance.GaugeService.Gauges)
            {
                GaugeListView.Items.Add(gauge);
            }
        }

        private void DisconnectGaugeButton_Click(object sender, RoutedEventArgs e)
        {
            var gauge = GaugeListView.SelectedItem as IGauge;

            if (gauge != null)
            {
                ElcometerCore.Instance.ConnectionService.Disconnect(gauge.DeviceInfo);
            }
        }

        private async void ConnectGaugeButton_Click(object sender, RoutedEventArgs e)
        {
            // get position of connect button
            GeneralTransform ge = ConnectGaugeButton.TransformToVisual(null);
            Point point = ge.TransformPoint(new Point());
            Rect rect = new Rect(point, new Point(point.X + ConnectGaugeButton.ActualWidth, point.Y + ConnectGaugeButton.ActualHeight));

            // show picker below
            await ElcometerCore.Instance.ConnectionService.ShowPickerAsync(rect);
        }


        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            ElcometerCore.Instance.GaugeService.Gauges.CollectionChanged += Gauges_CollectionChanged;

            PopulateGauges();
        }

        private void Gauges_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            PopulateGauges();
        }
    }
}
