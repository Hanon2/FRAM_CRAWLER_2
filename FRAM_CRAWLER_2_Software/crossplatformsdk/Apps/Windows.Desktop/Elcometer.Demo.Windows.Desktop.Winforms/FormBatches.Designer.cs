namespace Elcometer.Demo.Windows.Desktop.Winforms
{
    partial class FormBatches
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
            this.uxTableLayoutBatches = new System.Windows.Forms.TableLayoutPanel();
            this.uxListViewBatches = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.uxButtonViewBatch = new System.Windows.Forms.Button();
            this.uxTableLayoutBatches.SuspendLayout();
            this.SuspendLayout();
            // 
            // uxTableLayoutBatches
            // 
            this.uxTableLayoutBatches.ColumnCount = 1;
            this.uxTableLayoutBatches.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.uxTableLayoutBatches.Controls.Add(this.uxButtonViewBatch, 0, 1);
            this.uxTableLayoutBatches.Controls.Add(this.uxListViewBatches, 0, 0);
            this.uxTableLayoutBatches.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uxTableLayoutBatches.Location = new System.Drawing.Point(8, 8);
            this.uxTableLayoutBatches.Name = "uxTableLayoutBatches";
            this.uxTableLayoutBatches.RowCount = 2;
            this.uxTableLayoutBatches.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.uxTableLayoutBatches.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.uxTableLayoutBatches.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.uxTableLayoutBatches.Size = new System.Drawing.Size(588, 425);
            this.uxTableLayoutBatches.TabIndex = 0;
            // 
            // uxListViewBatches
            // 
            this.uxListViewBatches.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.uxListViewBatches.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uxListViewBatches.FullRowSelect = true;
            this.uxListViewBatches.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.uxListViewBatches.HideSelection = false;
            this.uxListViewBatches.Location = new System.Drawing.Point(3, 4);
            this.uxListViewBatches.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.uxListViewBatches.MultiSelect = false;
            this.uxListViewBatches.Name = "uxListViewBatches";
            this.uxListViewBatches.Size = new System.Drawing.Size(582, 379);
            this.uxListViewBatches.TabIndex = 3;
            this.uxListViewBatches.UseCompatibleStateImageBehavior = false;
            this.uxListViewBatches.View = System.Windows.Forms.View.Details;
            this.uxListViewBatches.SelectedIndexChanged += new System.EventHandler(this.uxListViewBatches_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Batch";
            this.columnHeader1.Width = 300;
            // 
            // uxButtonViewBatch
            // 
            this.uxButtonViewBatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uxButtonViewBatch.Location = new System.Drawing.Point(461, 391);
            this.uxButtonViewBatch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.uxButtonViewBatch.Name = "uxButtonViewBatch";
            this.uxButtonViewBatch.Size = new System.Drawing.Size(124, 30);
            this.uxButtonViewBatch.TabIndex = 4;
            this.uxButtonViewBatch.Text = "View Batch";
            this.uxButtonViewBatch.UseVisualStyleBackColor = true;
            this.uxButtonViewBatch.Click += new System.EventHandler(this.uxButtonViewBatch_Click);
            // 
            // FormBatches
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 441);
            this.Controls.Add(this.uxTableLayoutBatches);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormBatches";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormBatches";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormBatches_FormClosing);
            this.Load += new System.EventHandler(this.FormGauge_Load);
            this.uxTableLayoutBatches.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel uxTableLayoutBatches;
        private System.Windows.Forms.ListView uxListViewBatches;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button uxButtonViewBatch;
    }
}