using Elcometer.Core.Services;
using Elcometer.Core.Windows;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elcometer.Demo.Windows.Desktop.Winforms
{
    public partial class FormBatches : Form
    {
        private IGauge _gauge;

        public FormBatches(IGauge gauge)
        {
            InitializeComponent();

            _gauge = gauge;
        }

        private async void FormBatches_FormClosing(object sender, FormClosingEventArgs e)
        {
            await Task.Run(() => _gauge.EndBatching());
        }

        private async void FormGauge_Load(object sender, EventArgs e)
        {
            uxListViewBatches.Enabled = false;

            UpdateControlState();

            // put the gauge into a mode batches can be downloaded from it
            await Task.Run(() => _gauge.StartBatching());

            // request the batches
            var batches = await Task.Run(() => _gauge.GetBatches());

            uxListViewBatches.Items.Clear();

            foreach (var batch in batches)
            {
                uxListViewBatches.Items.Add(new BatchListViewItem { Batch = batch });
            }

            uxListViewBatches.Enabled = true;
        }

        private void UpdateControlState()
        {
            uxButtonViewBatch.Enabled = uxListViewBatches.SelectedIndices.Count > 0;
        }

        private async void uxButtonViewBatch_Click(object sender, EventArgs e)
        {
            ElcometerCore.Instance.BatchService.Batches.Clear();

            var gaugeBatch = (uxListViewBatches.Items[uxListViewBatches.SelectedIndices[0]] as BatchListViewItem)?.Batch;

            if (gaugeBatch != null)
            {
                uxButtonViewBatch.Enabled = false;

                // ask the gauge to download the batches into the service
                await Task.Run(() => _gauge.DownloadBatchesTo(new List<IGaugeBatch> { gaugeBatch }, ElcometerCore.Instance.BatchService));

                // downloaded ok ?
                if (ElcometerCore.Instance.BatchService.Batches.Count > 0)
                {
                    using (var formBatchInfo = new FormBatchInfo(ElcometerCore.Instance.BatchService.Batches[0]))
                    {
                        formBatchInfo.ShowDialog(this);
                    }
                }

                uxButtonViewBatch.Enabled = true;
            }
        }

        private void uxListViewBatches_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateControlState();
        }

        public class BatchListViewItem : ListViewItem
        {
            private IGaugeBatch _batch;

            public IGaugeBatch Batch
            {
                get { return _batch; }
                set { _batch = value; Text = Batch.Name; }
            }
        }
    }
}