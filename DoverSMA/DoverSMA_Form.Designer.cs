namespace DoverSMA
{
    partial class DoverSMA_Form
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
            this.BtnProcessSmaOfferings = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnProcessSmaOfferings
            // 
            this.BtnProcessSmaOfferings.Location = new System.Drawing.Point(12, 30);
            this.BtnProcessSmaOfferings.Name = "BtnProcessSmaOfferings";
            this.BtnProcessSmaOfferings.Size = new System.Drawing.Size(167, 23);
            this.BtnProcessSmaOfferings.TabIndex = 0;
            this.BtnProcessSmaOfferings.Text = "Process Legg SMA Offerings\r\n";
            this.BtnProcessSmaOfferings.UseVisualStyleBackColor = true;
            this.BtnProcessSmaOfferings.Click += new System.EventHandler(this.BtnProcessSmaOfferings_Click);
            // 
            // DoverSMA_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BtnProcessSmaOfferings);
            this.Name = "DoverSMA_Form";
            this.Text = "DoverSMA_Form";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnProcessSmaOfferings;
    }
}