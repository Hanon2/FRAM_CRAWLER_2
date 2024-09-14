namespace Elcometer.Demo.Windows.Desktop.Winforms
{
    partial class FormGauge
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.uxTableLayoutGauge = new System.Windows.Forms.TableLayoutPanel();
            this.uxTextBoxLive = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.uxCheckBoxExcel = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.uxButtonDownloadBatches = new System.Windows.Forms.Button();
            this.uxButton510Measure = new System.Windows.Forms.Button();
            this.uxTableLayoutGauge.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uxTableLayoutGauge
            // 
            this.uxTableLayoutGauge.ColumnCount = 1;
            this.uxTableLayoutGauge.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.uxTableLayoutGauge.Controls.Add(this.uxTextBoxLive, 0, 2);
            this.uxTableLayoutGauge.Controls.Add(this.label1, 0, 1);
            this.uxTableLayoutGauge.Controls.Add(this.uxCheckBoxExcel, 0, 3);
            this.uxTableLayoutGauge.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.uxTableLayoutGauge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uxTableLayoutGauge.Location = new System.Drawing.Point(8, 8);
            this.uxTableLayoutGauge.Name = "uxTableLayoutGauge";
            this.uxTableLayoutGauge.RowCount = 5;
            this.uxTableLayoutGauge.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.uxTableLayoutGauge.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.uxTableLayoutGauge.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.uxTableLayoutGauge.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.uxTableLayoutGauge.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.uxTableLayoutGauge.Size = new System.Drawing.Size(588, 425);
            this.uxTableLayoutGauge.TabIndex = 0;
            // 
            // uxTextBoxLive
            // 
            this.uxTextBoxLive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uxTextBoxLive.Location = new System.Drawing.Point(3, 69);
            this.uxTextBoxLive.Multiline = true;
            this.uxTextBoxLive.Name = "uxTextBoxLive";
            this.uxTextBoxLive.ReadOnly = true;
            this.uxTextBoxLive.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.uxTextBoxLive.Size = new System.Drawing.Size(582, 295);
            this.uxTextBoxLive.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(335, 28);
            this.label1.TabIndex = 3;
            this.label1.Text = "Live measurements will appear below";
            // 
            // uxCheckBoxExcel
            // 
            this.uxCheckBoxExcel.AutoSize = true;
            this.uxCheckBoxExcel.Location = new System.Drawing.Point(3, 370);
            this.uxCheckBoxExcel.Name = "uxCheckBoxExcel";
            this.uxCheckBoxExcel.Size = new System.Drawing.Size(486, 32);
            this.uxCheckBoxExcel.TabIndex = 4;
            this.uxCheckBoxExcel.Text = "Output readings to current Excel worksheet and cell";
            this.uxCheckBoxExcel.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.uxButtonDownloadBatches);
            this.flowLayoutPanel1.Controls.Add(this.uxButton510Measure);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(588, 38);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // uxButtonDownloadBatches
            // 
            this.uxButtonDownloadBatches.Location = new System.Drawing.Point(3, 4);
            this.uxButtonDownloadBatches.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.uxButtonDownloadBatches.Name = "uxButtonDownloadBatches";
            this.uxButtonDownloadBatches.Size = new System.Drawing.Size(129, 30);
            this.uxButtonDownloadBatches.TabIndex = 2;
            this.uxButtonDownloadBatches.Text = "Download Batches";
            this.uxButtonDownloadBatches.UseVisualStyleBackColor = true;
            this.uxButtonDownloadBatches.Click += new System.EventHandler(this.uxButtonDownloadBatches_Click);
            // 
            // uxButton510Measure
            // 
            this.uxButton510Measure.Location = new System.Drawing.Point(138, 4);
            this.uxButton510Measure.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.uxButton510Measure.Name = "uxButton510Measure";
            this.uxButton510Measure.Size = new System.Drawing.Size(238, 30);
            this.uxButton510Measure.TabIndex = 3;
            this.uxButton510Measure.Text = "510 Measurement";
            this.uxButton510Measure.UseVisualStyleBackColor = true;
            this.uxButton510Measure.Visible = false;
            this.uxButton510Measure.Click += new System.EventHandler(this.uxButton510Measure_Click);
            // 
            // FormGauge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 441);
            this.Controls.Add(this.uxTableLayoutGauge);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormGauge";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormGauge";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormGauge_FormClosed);
            this.Load += new System.EventHandler(this.FormGauge_Load);
            this.uxTableLayoutGauge.ResumeLayout(false);
            this.uxTableLayoutGauge.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel uxTableLayoutGauge;
        private System.Windows.Forms.TextBox uxTextBoxLive;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox uxCheckBoxExcel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button uxButtonDownloadBatches;
        private System.Windows.Forms.Button uxButton510Measure;
    }
}