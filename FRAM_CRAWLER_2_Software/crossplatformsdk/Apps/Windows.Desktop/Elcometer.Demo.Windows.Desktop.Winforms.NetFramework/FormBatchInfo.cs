using Elcometer.Core.Model;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Elcometer.Demo.Windows.Desktop.Winforms
{
    public partial class FormBatchInfo : Form
    {
        private Batch _batch;

        public FormBatchInfo(Batch batch)
        {
            InitializeComponent();

            _batch = batch;
        }

        private void FormGauge_Load(object sender, EventArgs e)
        {
            string info = "";

            info += $"Batch: {_batch.Name}\r\n";
            info += $"Readings: {_batch.IncludedCount}\r\n";
            info += "\r\n";

            foreach (var reading in _batch.GetReadings().Select(x => x.Value).OrderBy(x => x.Id).ToList())
            {
                if (reading.IncludeInStats)
                {
                    string readings = "";

                    for (int i = 0; i < _batch.ColumnCount; i++)
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

            uxTextBoxBatch.Text = info;
            uxTextBoxBatch.Select(0, 0);
        }
    }
}