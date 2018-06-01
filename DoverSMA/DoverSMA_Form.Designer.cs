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
            this.btnProcessSmaOfferings = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnProcessSmaOfferings
            // 
            this.btnProcessSmaOfferings.Location = new System.Drawing.Point(12, 30);
            this.btnProcessSmaOfferings.Name = "btnProcessSmaOfferings";
            this.btnProcessSmaOfferings.Size = new System.Drawing.Size(167, 23);
            this.btnProcessSmaOfferings.TabIndex = 0;
            this.btnProcessSmaOfferings.Text = "Process Legg SMA Offerings\r\n";
            this.btnProcessSmaOfferings.UseVisualStyleBackColor = true;
            this.btnProcessSmaOfferings.Click += new System.EventHandler(this.btnProcessSmaOfferings_Click);
            // 
            // DoverSMA_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnProcessSmaOfferings);
            this.Name = "DoverSMA_Form";
            this.Text = "DoverSMA_Form";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnProcessSmaOfferings;
    }
}