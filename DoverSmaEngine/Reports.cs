using System;
using System.Collections.Generic;
//using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

using DoverUtilities;

namespace DoverSmaEngine
{
    public class Reports
    {
        private SqlConnection mSqlConn1 = null;
        private SqlConnection mSqlConn2 = null;
        private SqlConnection mSqlConn3 = null;
        private string mConnectionString = "";


        #region Constructor
        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        public Reports()
        {
            mConnectionString = @"server=JKERMOND-NEW\SQLEXPRESS2014;database=DoverSma;uid=sa;pwd=M@gichat!";
            mSqlConn1 = new SqlConnection(mConnectionString);
            mSqlConn1.Open();
            mSqlConn2 = new SqlConnection(mConnectionString);
            mSqlConn2.Open();
            mSqlConn3 = new SqlConnection(mConnectionString);
            mSqlConn3.Open();
        }
        #endregion Constructor

        public void GenerateReportSponsorAmounts(string sStartDate, string sEndDate)
        { 
        string sCurrentDate = sStartDate;
        bool moreDates = true;
        string logFuncName = "GenerateReportSponsorAmounts: ";

        //LogHelper.WriteLine(logFuncName + " " + sStartDate + " to " + sEndDate);

            SqlCommand cmd = null;

            string commandText = @"
                SELECT FlowDate, SponsorFirmCode, MorningstarClassId, m.CodeDesc as MorningstarClassDesc, sum(AssetsD) as AssetsTotal, sum(FinalNetFlowsD) as FinalNetTotal
                FROM SmaOfferings
                inner join SmaFlows on SmaOfferings.SmaOfferingId = SmaFlows.SmaOfferingId
                inner join MorningstarClassifications m on SmaOfferings.MorningstarClassId = m.Code
                where SponsorFirmCode in (select SponsorFirmCode from SponsorFirms where InSponsorAmountsScorecard = 'Y')
                group by SponsorFirmCode, SmaOfferings.MorningstarClassId, m.CodeDesc,FlowDate
                order by SponsorFirmCode, SmaOfferings.MorningstarClassId, FlowDate
              ";
            try
            {
                cmd = new SqlCommand
                {
                    Connection = mSqlConn1,
                    CommandText = commandText
                };

                int rows = 0;
                string FlowDate = "";
                string SponsorFirmCode = "";
                string prevSponsorFirmCode = "";
                string MorningstarClassId = "";
                string prevMorningstarClassId = "";
                string MorningstarClassDesc = "";
                string prevMorningstarClassDesc = "";
                string AssetsTotal = "";
                string FinalNetTotal = "";
                string reportLine = "";

                List<string> flowDateList = new List<string>();
                List<string> assetsList = new List<string>();
                List<string> finalNetList = new List<string>();

                sCurrentDate = sStartDate;
                do
                {
                    flowDateList.Add(sCurrentDate);
                    assetsList.Add("");
                    finalNetList.Add("");

                    if (sCurrentDate.Equals(sEndDate))
                        moreDates = false;
                    else
                        sCurrentDate = Utils.CalculateNextEndOfQtrDate(sCurrentDate);
                }
                while (moreDates.Equals(true));

                SqlDataReader dr = null;
                dr = cmd.ExecuteReader();            
                if (dr.HasRows)
                {
                    int i = 0;
                    while (dr.Read())
                    {
                        rows += 1;
                        FlowDate = dr["FlowDate"].ToString();
                        SponsorFirmCode = dr["SponsorFirmCode"].ToString();
                        MorningstarClassId = dr["MorningstarClassId"].ToString();
                        MorningstarClassDesc = dr["MorningstarClassDesc"].ToString();
                        AssetsTotal = dr["AssetsTotal"].ToString();
                        FinalNetTotal = dr["FinalNetTotal"].ToString();

                        if (prevSponsorFirmCode.Equals(""))
                            prevSponsorFirmCode = SponsorFirmCode;
                        if (prevMorningstarClassId.Equals(""))
                            prevMorningstarClassId = MorningstarClassId;
                        if (prevMorningstarClassDesc.Equals(""))
                            prevMorningstarClassDesc = MorningstarClassDesc;

                        if (SponsorFirmCode.Equals(prevSponsorFirmCode) &&  MorningstarClassId.Equals(prevMorningstarClassId))
                        {
                            sCurrentDate = sStartDate;

                            for ( i = 0; i < assetsList.Count; i++)
                            {
                                DateTime dt = DateTime.Parse(FlowDate);
                                string sFlowDate = dt.ToString("MM/dd/yyyy");

                                if (flowDateList[i].Equals(sFlowDate))
                                {
                                    assetsList[i] = AssetsTotal;
                                    finalNetList[i] = FinalNetTotal;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            reportLine = prevSponsorFirmCode + "," + prevMorningstarClassId + "," + prevMorningstarClassDesc;

                            for (i = 0; i < assetsList.Count; i++)
                                reportLine += "," + assetsList[i];

                            for (i = 0; i < finalNetList.Count; i++)
                                reportLine += "," + finalNetList[i];

                            LogHelper.WriteLine(reportLine);
                            prevSponsorFirmCode = SponsorFirmCode;
                            prevMorningstarClassId = MorningstarClassId;
                            prevMorningstarClassDesc = MorningstarClassDesc;
                            for (i = 0; i < assetsList.Count; i++)
                            {
                                assetsList[i] = "";
                                finalNetList[i] = "";
                            }
                            for (i = 0; i < assetsList.Count; i++)
                            {
                                DateTime dt = DateTime.Parse(FlowDate);
                                string sFlowDate = dt.ToString("MM/dd/yyyy");

                                if (flowDateList[i].Equals(sFlowDate))
                                {
                                    assetsList[i] = AssetsTotal;
                                    finalNetList[i] = FinalNetTotal;
                                    break;
                                }
                            }
                        }
                    }
                    reportLine = prevSponsorFirmCode + "," + prevMorningstarClassId + "," + prevMorningstarClassDesc;

                    for (i = 0; i < assetsList.Count; i++)
                        reportLine += "," + assetsList[i];

                    for (i = 0; i < finalNetList.Count; i++)
                        reportLine += "," + finalNetList[i];

                    LogHelper.WriteLine(reportLine);

                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                LogHelper.WriteLine(logFuncName + " " + ex.Message);
            }
            finally
            {
                //LogHelper.WriteLine(logFuncName + " done " );
            }
        }

        public void GenerateReportSponsorAmountsOther(string sStartDate, string sEndDate)
        {
            string sCurrentDate = sStartDate;
            bool moreDates = true;
            string logFuncName = "GenerateReportSponsorAmounts: ";

            //LogHelper.WriteLine(logFuncName + " " + sStartDate + " to " + sEndDate);

            SqlCommand cmd = null;

            string commandText = @"
                SELECT FlowDate, MorningstarClassId, m.CodeDesc as MorningstarClassDesc, sum(AssetsD) as AssetsTotal, sum(FinalNetFlowsD) as FinalNetTotal
                FROM SmaOfferings
                inner join SmaFlows on SmaOfferings.SmaOfferingId = SmaFlows.SmaOfferingId
                inner join MorningstarClassifications m on SmaOfferings.MorningstarClassId = m.Code
                where SponsorFirmCode in (select SponsorFirmCode from SponsorFirms where InSponsorAmountsScorecard = 'N')
                group by SmaOfferings.MorningstarClassId, m.CodeDesc,FlowDate
                order by SmaOfferings.MorningstarClassId, FlowDate
              ";
            try
            {
                cmd = new SqlCommand
                {
                    Connection = mSqlConn1,
                    CommandText = commandText
                };

                int rows = 0;
                string FlowDate = "";
                string MorningstarClassId = "";
                string prevMorningstarClassId = "";
                string MorningstarClassDesc = "";
                string prevMorningstarClassDesc = "";
                string AssetsTotal = "";
                string FinalNetTotal = "";
                string reportLine = "";

                List<string> flowDateList = new List<string>();
                List<string> assetsList = new List<string>();
                List<string> finalNetList = new List<string>();

                sCurrentDate = sStartDate;
                do
                {
                    flowDateList.Add(sCurrentDate);
                    assetsList.Add("");
                    finalNetList.Add("");

                    if (sCurrentDate.Equals(sEndDate))
                        moreDates = false;
                    else
                        sCurrentDate = Utils.CalculateNextEndOfQtrDate(sCurrentDate);
                }
                while (moreDates.Equals(true));

                SqlDataReader dr = null;
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    int i = 0;
                    while (dr.Read())
                    {
                        rows += 1;
                        FlowDate = dr["FlowDate"].ToString();
                        MorningstarClassId = dr["MorningstarClassId"].ToString();
                        MorningstarClassDesc = dr["MorningstarClassDesc"].ToString();
                        AssetsTotal = dr["AssetsTotal"].ToString();
                        FinalNetTotal = dr["FinalNetTotal"].ToString();

                        if (prevMorningstarClassId.Equals(""))
                            prevMorningstarClassId = MorningstarClassId;
                        if (prevMorningstarClassDesc.Equals(""))
                            prevMorningstarClassDesc = MorningstarClassDesc;

                        if (MorningstarClassId.Equals(prevMorningstarClassId))
                        {
                            sCurrentDate = sStartDate;

                            for (i = 0; i < assetsList.Count; i++)
                            {
                                DateTime dt = DateTime.Parse(FlowDate);
                                string sFlowDate = dt.ToString("MM/dd/yyyy");

                                if (flowDateList[i].Equals(sFlowDate))
                                {
                                    assetsList[i] = AssetsTotal;
                                    finalNetList[i] = FinalNetTotal;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            reportLine =  "Other-Less than 3 mgrs.," + prevMorningstarClassId + "," + prevMorningstarClassDesc;

                            for (i = 0; i < assetsList.Count; i++)
                                reportLine += "," + assetsList[i];

                            for (i = 0; i < finalNetList.Count; i++)
                                reportLine += "," + finalNetList[i];

                            LogHelper.WriteLine(reportLine);
                            prevMorningstarClassId = MorningstarClassId;
                            prevMorningstarClassDesc = MorningstarClassDesc;

                            for (i = 0; i < assetsList.Count; i++)
                            {
                                assetsList[i] = "";
                                finalNetList[i] = "";
                            }
                            for (i = 0; i < assetsList.Count; i++)
                            {
                                DateTime dt = DateTime.Parse(FlowDate);
                                string sFlowDate = dt.ToString("MM/dd/yyyy");

                                if (flowDateList[i].Equals(sFlowDate))
                                {
                                    assetsList[i] = AssetsTotal;
                                    finalNetList[i] = FinalNetTotal;
                                    break;
                                }
                            }
                        }
                    }
                    reportLine = "Other-Less than 3 mgrs.," + prevMorningstarClassId + "," + prevMorningstarClassDesc;

                    for (i = 0; i < assetsList.Count; i++)
                        reportLine += "," + assetsList[i];

                    for (i = 0; i < finalNetList.Count; i++)
                        reportLine += "," + finalNetList[i];

                    LogHelper.WriteLine(reportLine);
                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                LogHelper.WriteLine(logFuncName + " " + ex.Message);
            }
            finally
            {
                //LogHelper.WriteLine(logFuncName + " done " );
            }
        }

    }
}
