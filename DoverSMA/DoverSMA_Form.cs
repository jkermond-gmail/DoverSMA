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

        public DoverSMA_Form()
        {
            InitializeComponent();
            bool deleteExisting = false;
            LogHelper.StartLog("DoverSmaLog.txt", @"C:\A_Development\visual studio 2017\Projects\DoverSMA\Output\", deleteExisting);
            mSmaFileProcessor = new FileProcessor();            
        }

        private void BtnProcessSmaOfferings_Click(object sender, EventArgs e)
        {
            string filepath = @"C:\A_Dover\Dev\SMA Beta Data-JK";
            string filename = @"LeggSmaOfferings.csv";
            mSmaFileProcessor.ProcessOfferingsFile(Path.Combine(filepath, filename));
        }
    }
}
