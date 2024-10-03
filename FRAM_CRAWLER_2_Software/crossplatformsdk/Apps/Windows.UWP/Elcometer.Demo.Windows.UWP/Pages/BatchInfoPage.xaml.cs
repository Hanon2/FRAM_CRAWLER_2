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
using System.Linq;

namespace Elcometer.Demo.Windows.UWP.Pages
{
    public sealed partial class BatchInfoPage : Page
    {


        public BatchInfoPage()
        {
            this.InitializeComponent();

            BackButton.Click += BackButton_Click;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var batch = e.Parameter as Core.Model.Batch;

            if (batch != null)
            {
                string info = "";

                info += $"Batch: {batch.Name}\r\n";
                info += $"Readings: {batch.IncludedCount}\r\n";
                info += "\r\n";

                foreach (var reading in batch.GetReadings().Select(x => x.Value).OrderBy(x => x.Id).ToList())
                {
                    if (reading.IncludeInStats)
                    {
                        string readings = "";

                        for (int i = 0; i < batch.ColumnCount; i++)
                        {
                            if (!String.IsNullOrEmpty(readings))
                            {
                                readings += ", ";
                            }

                            readings += reading.ToDisplayStringWithUnit(i);
                        }

                        info += $"{reading.Id}: {readings}\r\n";
                    }
                }

                BatchInfoTextBlock.Text = info;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }
    }
}
