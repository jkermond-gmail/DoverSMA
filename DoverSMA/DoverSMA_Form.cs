using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using DoverUtilities;
using DoverSmaEngine;

namespace DoverSMA
{
    public partial class DoverSMA_Form : Form
    {
        private FileProcessor mSmaFileProcessor = null;
        private string mSelectedManager = "";

        public DoverSMA_Form()
        {
            InitializeComponent();
            bool deleteExisting = false;
            LogHelper.StartLog("DoverSmaLog.txt", @"C:\A_Development\visual studio 2017\Projects\DoverSMA\Output\", deleteExisting);
            mSmaFileProcessor = new FileProcessor();
            ListBoxManagers.SetSelected(0, true);
        }


        private void BtnProcessSmaOfferings_Click(object sender, EventArgs e)
        {
            mSmaFileProcessor.ProcessManagerOfferings(mSelectedManager);
        }

        private void BtnProcessSmaFlows_Click(object sender, EventArgs e)
        {
            mSmaFileProcessor.ProcessManagerFlows(mSelectedManager);
        }

        private void BtnProcessSmaStrategies_Click(object sender, EventArgs e)
        {
            mSmaFileProcessor.ProcessManagerStrategies(mSelectedManager);
        }

        private void BtnProcessSmaReturns_Click(object sender, EventArgs e)
        {
            mSmaFileProcessor.ProcessManagerReturns(mSelectedManager);
        }

        private void ListBoxManagers_SelectedIndexChanged(object sender, EventArgs e)
        {
            mSelectedManager = ListBoxManagers.SelectedItem.ToString();
        }

        private void btnConvertFlows_Click(object sender, EventArgs e)
        {
            mSmaFileProcessor.CopyFlowsVarcharDataToDecimal("Assets");
            mSmaFileProcessor.CopyFlowsVarcharDataToDecimal("GrossFlows");
            mSmaFileProcessor.CopyFlowsVarcharDataToDecimal("Redemptions");
            mSmaFileProcessor.CopyFlowsVarcharDataToDecimal("NetFlows");
            mSmaFileProcessor.CopyFlowsVarcharDataToDecimal("DerivedFlows");
        }

        private void btnCalculateNetFlows_Click(object sender, EventArgs e)
        {
            mSmaFileProcessor.CalculateNetFlows();
        }

        private void btnConvertReturns_Click(object sender, EventArgs e)
        {
            mSmaFileProcessor.CopyReturnsVarcharDataToDecimal();
        }
    }
}

