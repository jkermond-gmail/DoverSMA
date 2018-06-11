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
        private string mFilepath = @"C:\A_Dover\Dev\SMA Beta Data-JK";
        private string mOfferingsAndFlowFilename = @"LeggSmaOfferings.csv";
        private string mStrategiesAndReturnsFilename = @"LeggSmaStrategies.csv";


        public DoverSMA_Form()
        {
            InitializeComponent();
            bool deleteExisting = false;
            LogHelper.StartLog("DoverSmaLog.txt", @"C:\A_Development\visual studio 2017\Projects\DoverSMA\Output\", deleteExisting);
            mSmaFileProcessor = new FileProcessor();            
        }

        private void BtnProcessSmaOfferings_Click(object sender, EventArgs e)
        {
            mSmaFileProcessor.ProcessOfferingsData(Path.Combine(mFilepath, mOfferingsAndFlowFilename));
        }

        private void BtnProcessSmaFlows_Click(object sender, EventArgs e)
        {
            mSmaFileProcessor.ProcessFlowsData(Path.Combine(mFilepath, mOfferingsAndFlowFilename));
        }

        private void BtnProcessSmaStrategies_Click(object sender, EventArgs e)
        {
            mSmaFileProcessor.ProcessStrategiesData(Path.Combine(mFilepath, mStrategiesAndReturnsFilename));
        }

        private void BtnProcessSmaReturns_Click(object sender, EventArgs e)
        {
            mSmaFileProcessor.ProcessReturnsData(Path.Combine(mFilepath, mStrategiesAndReturnsFilename));
        }
    }
}
