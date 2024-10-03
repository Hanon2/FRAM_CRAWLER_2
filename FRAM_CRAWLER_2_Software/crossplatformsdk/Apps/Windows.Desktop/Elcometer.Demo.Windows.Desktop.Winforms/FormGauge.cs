using Elcometer.Core.Model;
using Elcometer.Core.Services;
using Elcometer.Core.Windows;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Elcometer.Demo.Windows.Desktop.Winforms
{
    public partial class FormGauge : Form
    {
        private IGauge _gauge;
        private _Application _excelApp = null;

        #region Elcometer 510 Gauge Live Reading

        // just some example settings to do a pull to
        private const bool LimitEnabled = true;
        private const float LimitValue = 4.0f; // MPA
        private const float TargetRate = 1.0f;
        private const float HoldTime = 0.0f;
        private const AdhesionBatchDollySize DollySize = AdhesionBatchDollySize.Size20MM;
        private const bool PullToMax = false;

        private Gauge510Setup _setup510 = new Gauge510Setup
                (
                       LimitEnabled,
                       LimitValue,
                       TargetRate,
                       HoldTime,
                       DollySize,
                       PullToMax
                );

        private bool _is510Gauge;
        private List<float> _plotPoints = new List<float>();

        #endregion

        public FormGauge(IGauge gauge)
        {
            InitializeComponent();

            _gauge = gauge;
        }

        private void ConnectToExcel()
        {
            if (_excelApp == null)
            {
                try
                {
                    _excelApp = (_Application)ExcelHelper.GetActiveObject("Excel.Application");
                }
                catch (Exception)
                {
                    // this is important. If Excel is not running, GetActiveObject will throw
                    // an exception
                    _excelApp = null;
                }
            }
        }

        private void FormGauge_FormClosed(object sender, FormClosedEventArgs e)
        {
            ElcometerCore.Instance.MessagingService.Unsubscribe<ILiveReadingMessageParams>(this, ElcometerCoreMessages.LiveReadingMessage);

            if (_is510Gauge)
            {
                ElcometerCore.Instance.MessagingService.Unsubscribe<string>(this, ElcometerCoreMessages.AdhesionStartStateMessage);
                ElcometerCore.Instance.MessagingService.Unsubscribe<Gauge510PlotData>(this, ElcometerCoreMessages.AdhesionPlotDataMessage);
                ElcometerCore.Instance.MessagingService.Unsubscribe<Gauge510LiveReading>(this, ElcometerCoreMessages.AdhesionReadingMessage);
            }

            _gauge.PropertyChanged -= Gauge_PropertyChanged;
        }

        private void UpdateControlState()
        {
            uxButtonDownloadBatches.Enabled = _gauge.SupportsBatching;
        }

        private void FormGauge_Load(object sender, EventArgs e)
        {
            _gauge.PropertyChanged += Gauge_PropertyChanged;

            UpdateControlState();

            ElcometerCore.Instance.MessagingService.Subscribe<ILiveReadingMessageParams>(this, ElcometerCoreMessages.LiveReadingMessage, OnLiveReading);

            _is510Gauge = _gauge.GaugeType.Description.Contains("510");

            uxButton510Measure.Visible = _is510Gauge;

            if (_is510Gauge)
            {
                ElcometerCore.Instance.MessagingService.Subscribe<Gauge510PlotData>(this, ElcometerCoreMessages.AdhesionPlotDataMessage, Handle510PlotData);
                ElcometerCore.Instance.MessagingService.Subscribe<Gauge510LiveReading>(this, ElcometerCoreMessages.AdhesionReadingMessage, Handle510LiveReading);
                ElcometerCore.Instance.MessagingService.Subscribe<string>(this, ElcometerCoreMessages.AdhesionStartStateMessage, Handle510ErrorMessage);
            }
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

            int nextRow = 0, nextCol = 0;

            if (uxCheckBoxExcel.Checked)
            {
                ConnectToExcel();

                // valid excel connection ?
                if (_excelApp != null)
                {
                    // get current row and column
                    try
                    {
                        nextRow = _excelApp.get_ActiveCell().Row + 1;
                        nextCol = _excelApp.get_ActiveCell().Column;
                    }
                    catch
                    {
                        // excel probably closed down
                        _excelApp = null;
                    }
                }
            }

            // build the reading columns into a single line
            foreach (var reading in args.GetReadings(dummyBatch))
            {
                if (!String.IsNullOrEmpty(readingString))
                {
                    readingString += ", ";

                    if (_excelApp != null && uxCheckBoxExcel.Checked)
                    {
                        // also step to next excel column
                        var rng = (Microsoft.Office.Interop.Excel.Range)_excelApp.get_ActiveSheet().Cells[_excelApp.get_ActiveCell().Row, _excelApp.get_ActiveCell().Column + 1];
                        rng.Select();
                    }
                }

                // we are combined reading value formatted as strings here - the numeric value is available in the NumericValue property
                readingString += reading.Value;

                if (_excelApp != null && uxCheckBoxExcel.Checked)
                {
                    // save reading to current excel cell
                    _excelApp.get_ActiveCell().Value = reading.Value;
                }
            }

            // step to next line
            if (_excelApp != null && uxCheckBoxExcel.Checked)
            {
                var rng = (Microsoft.Office.Interop.Excel.Range)_excelApp.get_ActiveSheet().Cells[nextRow, nextCol];
                rng.Select();
            }

            readingString += "\r\n";

            // add reading text
            uxTextBoxLive.AppendText(readingString);
            uxTextBoxLive.SelectionStart = uxTextBoxLive.Text.Length;
            uxTextBoxLive.ScrollToCaret();
        }

        private void uxButtonDownloadBatches_Click(object sender, EventArgs e)
        {
            using (var formBatches = new FormBatches(_gauge))
            {
                formBatches.ShowDialog(this);
            }
        }

        #region Elcometer 510 Gauge Live Reading

        // the Elcometer 510 live readings are controlled via the PC

        protected virtual void Handle510LiveReading(object gauge, Gauge510LiveReading liveReading)
        {
            string readingString;

            int nextRow = 0, nextCol = 0;

            if (uxCheckBoxExcel.Checked)
            {
                ConnectToExcel();

                // valid excel connection ?
                if (_excelApp != null)
                {
                    // get current row and column
                    try
                    {
                        nextRow = _excelApp.get_ActiveCell().Row + 1;
                        nextCol = _excelApp.get_ActiveCell().Column;
                    }
                    catch
                    {
                        // excel probably closed down
                        _excelApp = null;
                    }
                }
            }

            readingString = (!liveReading.IsPopped ? ">" : "") +  ElcometerCore.Instance.BatchService.Units.AdhesionUnits.ToDisplayString(liveReading.Pressure);

            if (_excelApp != null && uxCheckBoxExcel.Checked)
            {
                // save reading to current excel cell
                _excelApp.get_ActiveCell().Value = readingString;
            }

            // step to next line
            if (_excelApp != null && uxCheckBoxExcel.Checked)
            {
                var rng = (Microsoft.Office.Interop.Excel.Range)_excelApp.get_ActiveSheet().Cells[nextRow, nextCol];
                rng.Select();
            }

            if (liveReading.PlotPoints.Length > 0)
            {
                string plotPoints = "";

                // get first time point for actual live data
                var time = _plotPoints[0] / _setup510.TargetRate;

                // default reading rate for live readings
                float stepTime = 1.0f / 20.0f;

                // intermediate points
                for (int i = 0; i < liveReading.PlotPoints.Length; i++)
                {
                    plotPoints += "Time: " + time.ToString("F2") + " Pressure: " + ElcometerCore.Instance.BatchService.Units.AdhesionUnits.ToDisplayString(liveReading.PlotPoints[i]) + "\r\n";
                    time += stepTime;
                }

                plotPoints += "\r\n";

                // add plot points
                uxTextBoxLive.AppendText(plotPoints);
            }

            // add reading text
            uxTextBoxLive.AppendText(readingString + "\r\n");
            uxTextBoxLive.SelectionStart = uxTextBoxLive.Text.Length;
            uxTextBoxLive.ScrollToCaret();           
        }

        private void Handle510ErrorMessage(object arg1, string error)
        {
            uxTextBoxLive.AppendText(error);
            uxTextBoxLive.SelectionStart = uxTextBoxLive.Text.Length;
            uxTextBoxLive.ScrollToCaret();
        }

        protected virtual void Handle510PlotData(object gauge, Gauge510PlotData liveReading)
        {
            if (liveReading.PlotPoints.Length > _plotPoints.Count)
            {
                _plotPoints.Clear();
                _plotPoints.AddRange(liveReading.PlotPoints);
            }
        }

        private void uxButton510Measure_Click(object sender, EventArgs e)
        {
            ElcometerCore.Instance.MessagingService.Send<Gauge510Setup>(this, ElcometerCoreMessages.StartAdhesionMeasurementMessage,  _setup510);
        }

        #endregion
    }
}