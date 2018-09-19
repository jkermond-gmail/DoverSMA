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
            this.BtnProcessSmaFlows = new System.Windows.Forms.Button();
            this.BtnProcessSmaStrategies = new System.Windows.Forms.Button();
            this.BtnProcessSmaReturns = new System.Windows.Forms.Button();
            this.ListBoxManagers = new System.Windows.Forms.ListBox();
            this.btnConvertFlows = new System.Windows.Forms.Button();
            this.btnCalculateNetFlows = new System.Windows.Forms.Button();
            this.btnConvertReturns = new System.Windows.Forms.Button();
            this.BtnUpdateSmaOfferings = new System.Windows.Forms.Button();
            this.BtnCalculateOpportunityMetrics = new System.Windows.Forms.Button();
            this.BtnCalculateAllMetrics = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnProcessSmaOfferings
            // 
            this.BtnProcessSmaOfferings.Location = new System.Drawing.Point(12, 134);
            this.BtnProcessSmaOfferings.Name = "BtnProcessSmaOfferings";
            this.BtnProcessSmaOfferings.Size = new System.Drawing.Size(167, 23);
            this.BtnProcessSmaOfferings.TabIndex = 0;
            this.BtnProcessSmaOfferings.Text = "Process SMA Offerings\r\n";
            this.BtnProcessSmaOfferings.UseVisualStyleBackColor = true;
            this.BtnProcessSmaOfferings.Click += new System.EventHandler(this.BtnProcessSmaOfferings_Click);
            // 
            // BtnProcessSmaFlows
            // 
            this.BtnProcessSmaFlows.Location = new System.Drawing.Point(12, 163);
            this.BtnProcessSmaFlows.Name = "BtnProcessSmaFlows";
            this.BtnProcessSmaFlows.Size = new System.Drawing.Size(167, 23);
            this.BtnProcessSmaFlows.TabIndex = 1;
            this.BtnProcessSmaFlows.Text = "Process SMA Flows";
            this.BtnProcessSmaFlows.UseVisualStyleBackColor = true;
            this.BtnProcessSmaFlows.Click += new System.EventHandler(this.BtnProcessSmaFlows_Click);
            // 
            // BtnProcessSmaStrategies
            // 
            this.BtnProcessSmaStrategies.Location = new System.Drawing.Point(12, 192);
            this.BtnProcessSmaStrategies.Name = "BtnProcessSmaStrategies";
            this.BtnProcessSmaStrategies.Size = new System.Drawing.Size(167, 23);
            this.BtnProcessSmaStrategies.TabIndex = 2;
            this.BtnProcessSmaStrategies.Text = "Process SMA Strategies";
            this.BtnProcessSmaStrategies.UseVisualStyleBackColor = true;
            this.BtnProcessSmaStrategies.Click += new System.EventHandler(this.BtnProcessSmaStrategies_Click);
            // 
            // BtnProcessSmaReturns
            // 
            this.BtnProcessSmaReturns.Location = new System.Drawing.Point(12, 221);
            this.BtnProcessSmaReturns.Name = "BtnProcessSmaReturns";
            this.BtnProcessSmaReturns.Size = new System.Drawing.Size(167, 23);
            this.BtnProcessSmaReturns.TabIndex = 3;
            this.BtnProcessSmaReturns.Text = "Process SMA Returns";
            this.BtnProcessSmaReturns.UseVisualStyleBackColor = true;
            this.BtnProcessSmaReturns.Click += new System.EventHandler(this.BtnProcessSmaReturns_Click);
            // 
            // ListBoxManagers
            // 
            this.ListBoxManagers.FormattingEnabled = true;
            this.ListBoxManagers.Items.AddRange(new object[] {
            "Allianz",
            "Anchor",
            "Brandes",
            "Congress",
            "Delaware",
            "Franklin Templeton",
            "GW&K",
            "Invesco",
            "Lazard",
            "Legg",
            "Lord Abbett",
            "Nuveen",
            "Principal",
            "Renaissance",
            ""});
            this.ListBoxManagers.Location = new System.Drawing.Point(12, 12);
            this.ListBoxManagers.Name = "ListBoxManagers";
            this.ListBoxManagers.Size = new System.Drawing.Size(120, 95);
            this.ListBoxManagers.TabIndex = 4;
            this.ListBoxManagers.SelectedIndexChanged += new System.EventHandler(this.ListBoxManagers_SelectedIndexChanged);
            // 
            // btnConvertFlows
            // 
            this.btnConvertFlows.Location = new System.Drawing.Point(13, 266);
            this.btnConvertFlows.Name = "btnConvertFlows";
            this.btnConvertFlows.Size = new System.Drawing.Size(166, 23);
            this.btnConvertFlows.TabIndex = 5;
            this.btnConvertFlows.Text = "ConvertFlows";
            this.btnConvertFlows.UseVisualStyleBackColor = true;
            this.btnConvertFlows.Click += new System.EventHandler(this.btnConvertFlows_Click);
            // 
            // btnCalculateNetFlows
            // 
            this.btnCalculateNetFlows.Location = new System.Drawing.Point(12, 337);
            this.btnCalculateNetFlows.Name = "btnCalculateNetFlows";
            this.btnCalculateNetFlows.Size = new System.Drawing.Size(167, 23);
            this.btnCalculateNetFlows.TabIndex = 6;
            this.btnCalculateNetFlows.Text = "CalculateNetFlows()";
            this.btnCalculateNetFlows.UseVisualStyleBackColor = true;
            this.btnCalculateNetFlows.Click += new System.EventHandler(this.btnCalculateNetFlows_Click);
            // 
            // btnConvertReturns
            // 
            this.btnConvertReturns.Location = new System.Drawing.Point(13, 295);
            this.btnConvertReturns.Name = "btnConvertReturns";
            this.btnConvertReturns.Size = new System.Drawing.Size(166, 23);
            this.btnConvertReturns.TabIndex = 7;
            this.btnConvertReturns.Text = "Convert Returns";
            this.btnConvertReturns.UseVisualStyleBackColor = true;
            this.btnConvertReturns.Click += new System.EventHandler(this.btnConvertReturns_Click);
            // 
            // BtnUpdateSmaOfferings
            // 
            this.BtnUpdateSmaOfferings.Location = new System.Drawing.Point(13, 366);
            this.BtnUpdateSmaOfferings.Name = "BtnUpdateSmaOfferings";
            this.BtnUpdateSmaOfferings.Size = new System.Drawing.Size(167, 23);
            this.BtnUpdateSmaOfferings.TabIndex = 8;
            this.BtnUpdateSmaOfferings.Text = "Update SMA Offerings";
            this.BtnUpdateSmaOfferings.UseVisualStyleBackColor = true;
            this.BtnUpdateSmaOfferings.Click += new System.EventHandler(this.BtnUpdateSmaOfferings_Click);
            // 
            // BtnCalculateOpportunityMetrics
            // 
            this.BtnCalculateOpportunityMetrics.Location = new System.Drawing.Point(195, 134);
            this.BtnCalculateOpportunityMetrics.Name = "BtnCalculateOpportunityMetrics";
            this.BtnCalculateOpportunityMetrics.Size = new System.Drawing.Size(185, 23);
            this.BtnCalculateOpportunityMetrics.TabIndex = 11;
            this.BtnCalculateOpportunityMetrics.Text = "BtnCalculateOpportunityMetrics";
            this.BtnCalculateOpportunityMetrics.UseVisualStyleBackColor = true;
            this.BtnCalculateOpportunityMetrics.Click += new System.EventHandler(this.BtnCalculateOpportunityMetrics_Click);
            // 
            // BtnCalculateAllMetrics
            // 
            this.BtnCalculateAllMetrics.Location = new System.Drawing.Point(195, 163);
            this.BtnCalculateAllMetrics.Name = "BtnCalculateAllMetrics";
            this.BtnCalculateAllMetrics.Size = new System.Drawing.Size(185, 23);
            this.BtnCalculateAllMetrics.TabIndex = 12;
            this.BtnCalculateAllMetrics.Text = "BtnCalculateAllMetrics";
            this.BtnCalculateAllMetrics.UseVisualStyleBackColor = true;
            this.BtnCalculateAllMetrics.Click += new System.EventHandler(this.BtnCalculateAllMetrics_Click);
            // 
            // DoverSMA_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BtnCalculateAllMetrics);
            this.Controls.Add(this.BtnCalculateOpportunityMetrics);
            this.Controls.Add(this.BtnUpdateSmaOfferings);
            this.Controls.Add(this.btnConvertReturns);
            this.Controls.Add(this.btnCalculateNetFlows);
            this.Controls.Add(this.btnConvertFlows);
            this.Controls.Add(this.ListBoxManagers);
            this.Controls.Add(this.BtnProcessSmaReturns);
            this.Controls.Add(this.BtnProcessSmaStrategies);
            this.Controls.Add(this.BtnProcessSmaFlows);
            this.Controls.Add(this.BtnProcessSmaOfferings);
            this.Name = "DoverSMA_Form";
            this.Text = "DoverSMA_Form";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnProcessSmaOfferings;
        private System.Windows.Forms.Button BtnProcessSmaFlows;
        private System.Windows.Forms.Button BtnProcessSmaStrategies;
        private System.Windows.Forms.Button BtnProcessSmaReturns;
        private System.Windows.Forms.ListBox ListBoxManagers;
        private System.Windows.Forms.Button btnConvertFlows;
        private System.Windows.Forms.Button btnCalculateNetFlows;
        private System.Windows.Forms.Button btnConvertReturns;
        private System.Windows.Forms.Button BtnUpdateSmaOfferings;
        private System.Windows.Forms.Button BtnCalculateOpportunityMetrics;
        private System.Windows.Forms.Button BtnCalculateAllMetrics;
    }
}