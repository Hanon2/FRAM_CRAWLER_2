namespace Elcometer.Demo.Windows.Desktop.Winforms
{
    partial class FormBatchInfo
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
            this.uxTextBoxBatch = new System.Windows.Forms.TextBox();
            this.uxTableLayoutGauge.SuspendLayout();
            this.SuspendLayout();
            // 
            // uxTableLayoutGauge
            // 
            this.uxTableLayoutGauge.ColumnCount = 1;
            this.uxTableLayoutGauge.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.uxTableLayoutGauge.Controls.Add(this.uxTextBoxBatch, 0, 0);
            this.uxTableLayoutGauge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uxTableLayoutGauge.Location = new System.Drawing.Point(8, 8);
            this.uxTableLayoutGauge.Name = "uxTableLayoutGauge";
            this.uxTableLayoutGauge.RowCount = 1;
            this.uxTableLayoutGauge.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.uxTableLayoutGauge.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 425F));
            this.uxTableLayoutGauge.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 425F));
            this.uxTableLayoutGauge.Size = new System.Drawing.Size(588, 425);
            this.uxTableLayoutGauge.TabIndex = 0;
            // 
            // uxTextBoxBatch
            // 
            this.uxTextBoxBatch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uxTextBoxBatch.Location = new System.Drawing.Point(3, 3);
            this.uxTextBoxBatch.Multiline = true;
            this.uxTextBoxBatch.Name = "uxTextBoxBatch";
            this.uxTextBoxBatch.ReadOnly = true;
            this.uxTextBoxBatch.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.uxTextBoxBatch.Size = new System.Drawing.Size(582, 419);
            this.uxTextBoxBatch.TabIndex = 2;
            // 
            // FormBatchInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 441);
            this.Controls.Add(this.uxTableLayoutGauge);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormBatchInfo";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormBatchInfo";
            this.Load += new System.EventHandler(this.FormGauge_Load);
            this.uxTableLayoutGauge.ResumeLayout(false);
            this.uxTableLayoutGauge.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel uxTableLayoutGauge;
        private System.Windows.Forms.TextBox uxTextBoxBatch;
    }
}