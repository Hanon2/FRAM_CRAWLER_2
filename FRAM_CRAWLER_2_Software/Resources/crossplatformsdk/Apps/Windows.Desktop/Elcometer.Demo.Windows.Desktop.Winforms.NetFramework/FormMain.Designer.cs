namespace Elcometer.Demo.Windows.Desktop.Winforms
{
    partial class FormMain
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
            this.uxButtonFindGauges = new System.Windows.Forms.Button();
            this.uxListViewGauges = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.uxTableLayoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.uxButtonDisconnectGauge = new System.Windows.Forms.Button();
            this.uxButtonViewGauge = new System.Windows.Forms.Button();
            this.uxTableLayoutMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // uxButtonFindGauges
            // 
            this.uxButtonFindGauges.Location = new System.Drawing.Point(3, 4);
            this.uxButtonFindGauges.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.uxButtonFindGauges.Name = "uxButtonFindGauges";
            this.uxButtonFindGauges.Size = new System.Drawing.Size(129, 30);
            this.uxButtonFindGauges.TabIndex = 0;
            this.uxButtonFindGauges.Text = "Find Gauges";
            this.uxButtonFindGauges.UseVisualStyleBackColor = true;
            this.uxButtonFindGauges.Click += new System.EventHandler(this.uxButtonFindGauges_Click);
            // 
            // uxListViewGauges
            // 
            this.uxListViewGauges.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.uxTableLayoutMain.SetColumnSpan(this.uxListViewGauges, 2);
            this.uxListViewGauges.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uxListViewGauges.FullRowSelect = true;
            this.uxListViewGauges.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.uxListViewGauges.HideSelection = false;
            this.uxListViewGauges.Location = new System.Drawing.Point(3, 42);
            this.uxListViewGauges.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.uxListViewGauges.MultiSelect = false;
            this.uxListViewGauges.Name = "uxListViewGauges";
            this.uxListViewGauges.Size = new System.Drawing.Size(582, 341);
            this.uxListViewGauges.TabIndex = 2;
            this.uxListViewGauges.UseCompatibleStateImageBehavior = false;
            this.uxListViewGauges.View = System.Windows.Forms.View.Details;
            this.uxListViewGauges.SelectedIndexChanged += new System.EventHandler(this.uxListViewGauges_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Connected Gauge";
            this.columnHeader1.Width = 300;
            // 
            // uxTableLayoutMain
            // 
            this.uxTableLayoutMain.ColumnCount = 2;
            this.uxTableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.uxTableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.uxTableLayoutMain.Controls.Add(this.uxButtonDisconnectGauge, 0, 2);
            this.uxTableLayoutMain.Controls.Add(this.uxButtonFindGauges, 0, 0);
            this.uxTableLayoutMain.Controls.Add(this.uxListViewGauges, 0, 1);
            this.uxTableLayoutMain.Controls.Add(this.uxButtonViewGauge, 1, 2);
            this.uxTableLayoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uxTableLayoutMain.Location = new System.Drawing.Point(8, 8);
            this.uxTableLayoutMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.uxTableLayoutMain.Name = "uxTableLayoutMain";
            this.uxTableLayoutMain.RowCount = 3;
            this.uxTableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.uxTableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.uxTableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.uxTableLayoutMain.Size = new System.Drawing.Size(588, 425);
            this.uxTableLayoutMain.TabIndex = 3;
            // 
            // uxButtonDisconnectGauge
            // 
            this.uxButtonDisconnectGauge.Location = new System.Drawing.Point(3, 391);
            this.uxButtonDisconnectGauge.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.uxButtonDisconnectGauge.Name = "uxButtonDisconnectGauge";
            this.uxButtonDisconnectGauge.Size = new System.Drawing.Size(124, 30);
            this.uxButtonDisconnectGauge.TabIndex = 4;
            this.uxButtonDisconnectGauge.Text = "Disconnect Gauge";
            this.uxButtonDisconnectGauge.UseVisualStyleBackColor = true;
            this.uxButtonDisconnectGauge.Click += new System.EventHandler(this.uxButtonDisconnectGauge_Click);
            // 
            // uxButtonViewGauge
            // 
            this.uxButtonViewGauge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uxButtonViewGauge.Location = new System.Drawing.Point(461, 391);
            this.uxButtonViewGauge.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.uxButtonViewGauge.Name = "uxButtonViewGauge";
            this.uxButtonViewGauge.Size = new System.Drawing.Size(124, 30);
            this.uxButtonViewGauge.TabIndex = 3;
            this.uxButtonViewGauge.Text = "View Gauge";
            this.uxButtonViewGauge.UseVisualStyleBackColor = true;
            this.uxButtonViewGauge.Click += new System.EventHandler(this.uxButtonViewGauge_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 441);
            this.Controls.Add(this.uxTableLayoutMain);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormMain";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Elcometer.Demo.Windows.Desktop.Winforms";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.uxTableLayoutMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button uxButtonFindGauges;
        private System.Windows.Forms.ListView uxListViewGauges;
        private System.Windows.Forms.TableLayoutPanel uxTableLayoutMain;
        private System.Windows.Forms.Button uxButtonViewGauge;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button uxButtonDisconnectGauge;
    }
}

