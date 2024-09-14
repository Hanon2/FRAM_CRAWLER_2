using Elcometer.Core.Services;
using Elcometer.Core.Windows;
using System;
using System.Windows.Forms;

namespace Elcometer.Demo.Windows.Desktop.Winforms
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            ElcometerCore.Instance.GaugeService.Gauges.CollectionChanged -= Gauges_CollectionChanged;

            foreach (var gauge in ElcometerCore.Instance.GaugeService.Gauges)
            {
                ElcometerCore.Instance.GaugeService.Disconnect(gauge);
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            ElcometerCore.Instance.Initialise();
            ElcometerCore.Instance.GaugeService.Gauges.CollectionChanged += Gauges_CollectionChanged;

            UpdateControlState();
        }

        private void Gauges_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            uxListViewGauges.Items.Clear();

            foreach (var gauge in ElcometerCore.Instance.GaugeService.Gauges)
            {
                uxListViewGauges.Items.Add(new GaugeListViewItem { Gauge = gauge });
            }

            UpdateControlState();
        }

        private void UpdateControlState()
        {
            uxButtonDisconnectGauge.Enabled = uxListViewGauges.SelectedIndices.Count > 0;
            uxButtonViewGauge.Enabled = uxListViewGauges.SelectedIndices.Count > 0;
        }

        private void uxButtonDisconnectGauge_Click(object sender, EventArgs e)
        {
            var gauge = (uxListViewGauges.Items[uxListViewGauges.SelectedIndices[0]] as GaugeListViewItem)?.Gauge;

            if (gauge != null)
            {
                ElcometerCore.Instance.ConnectionService.Disconnect(gauge.DeviceInfo);
                UpdateControlState();
            }
        }

        private void uxButtonFindGauges_Click(object sender, EventArgs e)
        {
            ElcometerCore.Instance.ConnectionService.ShowPicker(this);
        }

        private void uxButtonViewGauge_Click(object sender, EventArgs e)
        {
            using (var formGauge = new FormGauge((uxListViewGauges.Items[uxListViewGauges.SelectedIndices[0]] as GaugeListViewItem)?.Gauge))
            {
                formGauge.ShowDialog(this);
            }
        }

        private void uxListViewGauges_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateControlState();
        }

        public class GaugeListViewItem : ListViewItem
        {
            private IGauge _gauge;

            public IGauge Gauge
            {
                get { return _gauge; }
                set { _gauge = value; Text = String.Format("{0} {1}", _gauge.GaugeType.Description, _gauge.SerialNumber); }
            }
        }
    }
}