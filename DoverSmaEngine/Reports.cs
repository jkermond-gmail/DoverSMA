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


        private void getTopNSponsorsByAssets(int topN, string sEndOfQtrDate, out List<string> listSponsors)
        {
            string logFuncName = "getTopNSponsorsByAssets: ";

            //LogHelper.WriteLine(logFuncName + " " + topN + "  " + sEndOfQtrDate);

            listSponsors = new List<string>();

            SqlCommand cmd = null;

            string commandText = @"
                SELECT top " + topN + " " ;
            commandText += @"
                SponsorFirmCode, sum(AssetsD) as AssetsTotal
                FROM SmaOfferings o
                inner join SmaFlows on o.SmaOfferingId = SmaFlows.SmaOfferingId
                where FlowDate = @FlowDate and AssetsD is not null
                group by SponsorFirmCode 
                order by AssetsTotal desc 
                ";
            try
            {
                cmd = new SqlCommand
                {
                    Connection = mSqlConn1,
                    CommandText = commandText
                };

                cmd.Parameters.Add("@FlowDate", SqlDbType.Date);
                cmd.Parameters["@FlowDate"].Value = sEndOfQtrDate;

                SqlDataReader dr = null;
                dr = cmd.ExecuteReader();
                int rows = 0;
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        rows += 1;
                        string sponsorFirmCode = "";
                        sponsorFirmCode = dr["SponsorFirmCode"].ToString();
                        listSponsors.Add(sponsorFirmCode);
                    }
                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                LogHelper.WriteLine(logFuncName + " " + ex.Message);
            }
            finally
            {
              //  LogHelper.WriteLine(logFuncName + " done " );
            }
            return;
        }

        private void getCountOfferings(string SponsorFirmCode, string sFlowDate, string MorningstarClassId, out int TheCount)
        {
            string logFuncName = "getCountOfferings: ";

            //LogHelper.WriteLine(logFuncName );

            TheCount = 0;

            SqlCommand cmd = null;

            string commandText = @"
                select count(*) FROM (select distinct SmaStrategy, SponsorFirmCode, MorningstarClassId from SmaOfferings o
                inner join SmaFlows on o.SmaOfferingId = SmaFlows.SmaOfferingId
                where SponsorFirmCode = @SponsorFirmCode and FlowDate = @FlowDate and MorningstarClassId = @MorningstarClassId 
                and AssetsD is not null and AssetsD > 0) as TheCount
                ";
            try
            {
                cmd = new SqlCommand
                {
                    Connection = mSqlConn2,
                    CommandText = commandText
                };

                cmd.Parameters.Add("@FlowDate", SqlDbType.Date);
                cmd.Parameters["@FlowDate"].Value = sFlowDate;
                cmd.Parameters.Add("@SponsorFirmCode", SqlDbType.VarChar);
                cmd.Parameters["@SponsorFirmCode"].Value = SponsorFirmCode;
                cmd.Parameters.Add("@MorningstarClassId", SqlDbType.VarChar);
                cmd.Parameters["@MorningstarClassId"].Value = MorningstarClassId;

                SqlDataReader dr = null;
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    string val = dr[0].ToString();
                    TheCount = Convert.ToInt32(val);
                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                LogHelper.WriteLine(logFuncName + " " + ex.Message);
            }
            finally
            {
              //  LogHelper.WriteLine(logFuncName + " done ");
            }
            return;
        }

        private void getCountOfferingsFinalNet(string SponsorFirmCode, string sFlowDate, string MorningstarClassId, out int TheCount)
        {
            string logFuncName = "getCountOfferingsFinalNet: ";

            //LogHelper.WriteLine(logFuncName );

            TheCount = 0;

            SqlCommand cmd = null;

            string commandText = @"
                select count(*) FROM (select distinct SmaStrategy, SponsorFirmCode, MorningstarClassId from SmaOfferings o
                inner join SmaFlows on o.SmaOfferingId = SmaFlows.SmaOfferingId
                where SponsorFirmCode = @SponsorFirmCode and FlowDate = @FlowDate and MorningstarClassId = @MorningstarClassId 
                and FinalNetFlowsD is not null) as TheCount
                ";
            try
            {
                cmd = new SqlCommand
                {
                    Connection = mSqlConn2,
                    CommandText = commandText
                };

                cmd.Parameters.Add("@FlowDate", SqlDbType.Date);
                cmd.Parameters["@FlowDate"].Value = sFlowDate;
                cmd.Parameters.Add("@SponsorFirmCode", SqlDbType.VarChar);
                cmd.Parameters["@SponsorFirmCode"].Value = SponsorFirmCode;
                cmd.Parameters.Add("@MorningstarClassId", SqlDbType.VarChar);
                cmd.Parameters["@MorningstarClassId"].Value = MorningstarClassId;

                SqlDataReader dr = null;
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    string val = dr[0].ToString();
                    TheCount = Convert.ToInt32(val);
                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                LogHelper.WriteLine(logFuncName + " " + ex.Message);
            }
            finally
            {
                //  LogHelper.WriteLine(logFuncName + " done ");
            }
            return;
        }

        private void getRankOfferings(string SponsorFirmCode, string sFlowDate, string MorningstarClassId, string SmaStrategy, out int Rank)
        {
            string logFuncName = "getRankOfferings: ";

            //LogHelper.WriteLine(logFuncName );

            Rank = 0;

            SqlCommand cmd = null;

            string commandText = @"
                select SmaStrategy, SponsorFirmCode, MorningstarClassId, sum(AssetsD) as AssetsTotal FROM SmaOfferings o
                inner join SmaFlows on o.SmaOfferingId = SmaFlows.SmaOfferingId
                where SponsorFirmCode = @SponsorFirmCode and FlowDate = @FlowDate and MorningstarClassId = @MorningstarClassId 
                and AssetsD is not null and AssetsD > 0
                group by SmaStrategy, SponsorFirmCode, MorningstarClassId
                order by AssetsTotal desc
                ";
            try
            {
                cmd = new SqlCommand
                {
                    Connection = mSqlConn2,
                    CommandText = commandText
                };

                cmd.Parameters.Add("@FlowDate", SqlDbType.Date);
                cmd.Parameters["@FlowDate"].Value = sFlowDate;
                cmd.Parameters.Add("@SponsorFirmCode", SqlDbType.VarChar);
                cmd.Parameters["@SponsorFirmCode"].Value = SponsorFirmCode;
                cmd.Parameters.Add("@MorningstarClassId", SqlDbType.VarChar);
                cmd.Parameters["@MorningstarClassId"].Value = MorningstarClassId;

                SqlDataReader dr = null;
                dr = cmd.ExecuteReader();
                string compareSmaStrategy = "";
                bool found = false;
                if (dr.HasRows)
                {
                    while(dr.Read())
                    {
                        Rank += 1;
                        compareSmaStrategy = dr["SmaStrategy"].ToString();
                        if (SmaStrategy.Equals(compareSmaStrategy))
                        {
                            found = true;
                            break;
                        }
                    }
                }
                dr.Close();
                if (found.Equals(false))
                    found = false;

            }
            catch (SqlException ex)
            {
                LogHelper.WriteLine(logFuncName + " " + ex.Message);
            }
            finally
            {
                //  LogHelper.WriteLine(logFuncName + " done ");
            }
            return;
        }

        private void getRankOfferingsFinalNet(string SponsorFirmCode, string sFlowDate, string MorningstarClassId, string SmaStrategy, out int Rank)
        {
            string logFuncName = "getRankOfferingsFinalNet: ";

            //LogHelper.WriteLine(logFuncName );

            Rank = 0;

            SqlCommand cmd = null;

            string commandText = @"
                select SmaStrategy, SponsorFirmCode, MorningstarClassId, sum(FinalNetFlowsD) as FinalNetTotal FROM SmaOfferings o
                inner join SmaFlows on o.SmaOfferingId = SmaFlows.SmaOfferingId
                where SponsorFirmCode = @SponsorFirmCode and FlowDate = @FlowDate and MorningstarClassId = @MorningstarClassId 
                and FinalNetFlowsD is not null
                group by SmaStrategy, SponsorFirmCode, MorningstarClassId
                order by FinalNetTotal desc
                ";
            try
            {
                cmd = new SqlCommand
                {
                    Connection = mSqlConn2,
                    CommandText = commandText
                };

                cmd.Parameters.Add("@FlowDate", SqlDbType.Date);
                cmd.Parameters["@FlowDate"].Value = sFlowDate;
                cmd.Parameters.Add("@SponsorFirmCode", SqlDbType.VarChar);
                cmd.Parameters["@SponsorFirmCode"].Value = SponsorFirmCode;
                cmd.Parameters.Add("@MorningstarClassId", SqlDbType.VarChar);
                cmd.Parameters["@MorningstarClassId"].Value = MorningstarClassId;

                SqlDataReader dr = null;
                dr = cmd.ExecuteReader();
                string compareSmaStrategy = "";
                bool found = false;
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Rank += 1;
                        compareSmaStrategy = dr["SmaStrategy"].ToString();
                        if (SmaStrategy.Equals(compareSmaStrategy))
                        {
                            found = true;
                            break;
                        }
                    }
                }
                dr.Close();
                if (found.Equals(false))
                    found = false;

            }
            catch (SqlException ex)
            {
                LogHelper.WriteLine(logFuncName + " " + ex.Message);
            }
            finally
            {
                //  LogHelper.WriteLine(logFuncName + " done ");
            }
            return;
        }



        public void GenerateReportManagerDataset(string sStartDate, string sEndDate)
        {
            string logFuncName = "GenerateReportManagerDataset: ";

            //LogHelper.WriteLine(logFuncName);
            string sCurrentDate = sStartDate;
            bool moreDates = true;

            List<string> listSponsors;
            int topN = 20;
            getTopNSponsorsByAssets(topN, sEndDate, out listSponsors);

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


            try
            {
                SqlCommand cmd = new SqlCommand
                {
                    Connection = mSqlConn1,
                    CommandText = @"
                        SELECT o.AssetManagerCode, FlowDate, SmaStrategy, MorningstarClassId, m.CodeDesc as MorningstarClassDesc, 
                        sum(AssetsD) as AssetsTotal, sum(FinalNetFlowsD) as FinalNetTotal
                        FROM SmaOfferings o
                        inner join SmaFlows on o.SmaOfferingId = SmaFlows.SmaOfferingId
                        inner join MorningstarClassifications m on o.MorningstarClassId = m.Code
                        inner join AssetManagers a on o.AssetManagerCode = a.AssetManagerCode
                        where SponsorFirmCode = @SponsorFirmCode
                        and AssetsD is not null and AssetsD > 0 
                        group by o.AssetManagerCode, o.MorningstarClassId, m.CodeDesc, SmaStrategy, FlowDate
                        order by o.MorningstarClassId,  o.AssetManagerCode, SmaStrategy, FlowDate 
                        "
                };

                cmd.Parameters.Add("@SponsorFirmCode", SqlDbType.VarChar);

                foreach (string sponsorFirmCode in listSponsors)
                {
                    cmd.Parameters["@SponsorFirmCode"].Value = sponsorFirmCode;

                    string AssetManagerCode = "";
                    string FlowDate = "";
                    string MorningstarClassId = "";
                    string MorningstarClassDesc = "";
                    string SmaStrategy = "";
                    string prevAssetManagerCode = "";
                    string prevFlowDate = "";
                    string prevMorningstarClassId = "";
                    string prevMorningstarClassDesc = "";
                    string prevSmaStrategy = "";

                    string AssetsTotal = "";
                    string FinalNetTotal = "";
                    string reportLine = "";


                    int count = 0;
                    int rank = 0;
                    int i = 0;

                    SqlDataReader dr = null;
                    dr = cmd.ExecuteReader();
                    int rows = 0;
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            rows += 1;
                            AssetManagerCode = dr["AssetManagerCode"].ToString();
                            FlowDate = dr["FlowDate"].ToString();
                            SmaStrategy = dr["SmaStrategy"].ToString();
                            MorningstarClassId = dr["MorningstarClassId"].ToString();
                            MorningstarClassDesc = dr["MorningstarClassDesc"].ToString();
                            AssetsTotal = dr["AssetsTotal"].ToString();
                            FinalNetTotal = dr["FinalNetTotal"].ToString();

                            if (AssetManagerCode.Equals("nuve") && sponsorFirmCode.Equals("Schwab") && MorningstarClassId.Equals("AL"))
                                i = 0;

                            if (prevAssetManagerCode.Equals(""))
                                prevAssetManagerCode = AssetManagerCode;
                            if (prevFlowDate.Equals(""))
                                prevFlowDate = FlowDate;
                            if (prevSmaStrategy.Equals(""))
                                prevSmaStrategy = SmaStrategy;
                            if (prevMorningstarClassId.Equals(""))
                                prevMorningstarClassId = MorningstarClassId;
                            if (prevMorningstarClassDesc.Equals(""))
                                prevMorningstarClassDesc = MorningstarClassDesc;

                            if ( SmaStrategy.Equals(prevSmaStrategy) && MorningstarClassId.Equals(prevMorningstarClassId) )
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
                                reportLine = sponsorFirmCode + "," + prevAssetManagerCode + "," + prevSmaStrategy + "," + prevMorningstarClassId + "," + prevMorningstarClassDesc;

                                for (i = 0; i < assetsList.Count; i++)
                                    reportLine += "," + assetsList[i];

                                for (i = 0; i < finalNetList.Count; i++)
                                    reportLine += "," + finalNetList[i];

                                for (i = 0; i < assetsList.Count; i++)
                                {
                                    if (assetsList[i].Length > 0)
                                    {
                                        getRankOfferings(sponsorFirmCode, flowDateList[i], prevMorningstarClassId, prevSmaStrategy, out rank);
                                        reportLine += "," + rank.ToString();
                                        getCountOfferings(sponsorFirmCode, flowDateList[i], prevMorningstarClassId, out count);
                                        reportLine += "," + count.ToString();
                                    }
                                    else
                                    {
                                        reportLine += ",,";
                                    }
                                }

                                for (i = 0; i < finalNetList.Count; i++)
                                {
                                    if (finalNetList[i].Length > 0)
                                    {
                                        getRankOfferingsFinalNet(sponsorFirmCode, flowDateList[i], prevMorningstarClassId, prevSmaStrategy, out rank);
                                        reportLine += "," + rank.ToString();
                                        getCountOfferingsFinalNet(sponsorFirmCode, flowDateList[i], prevMorningstarClassId, out count);
                                        reportLine += "," + count.ToString();
                                    }
                                    else
                                    {
                                        reportLine += ",,";
                                    }
                                }


                                LogHelper.WriteLine(reportLine);

                                prevAssetManagerCode = AssetManagerCode;
                                prevSmaStrategy = SmaStrategy;
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
                        reportLine = sponsorFirmCode + "," + AssetManagerCode + "," + SmaStrategy + "," + MorningstarClassId + "," + MorningstarClassDesc;

                        for (i = 0; i < assetsList.Count; i++)
                            reportLine += "," + assetsList[i];

                        for (i = 0; i < finalNetList.Count; i++)
                            reportLine += "," + finalNetList[i];

                        for (i = 0; i < assetsList.Count; i++)
                        {
                            if (assetsList[i].Length > 0)
                            {
                                getRankOfferings(sponsorFirmCode, flowDateList[i], MorningstarClassId, SmaStrategy, out rank);
                                reportLine += "," + rank.ToString();
                                getCountOfferings(sponsorFirmCode, flowDateList[i], MorningstarClassId, out count);
                                reportLine += "," + count.ToString();
                            }
                            else
                            {
                                reportLine += ",,";
                            }
                        }

                        for (i = 0; i < finalNetList.Count; i++)
                        {
                            if (finalNetList[i].Length > 0)
                            {
                                getRankOfferingsFinalNet(sponsorFirmCode, flowDateList[i], MorningstarClassId, SmaStrategy, out rank);
                                reportLine += "," + rank.ToString();
                                getCountOfferingsFinalNet(sponsorFirmCode, flowDateList[i], MorningstarClassId, out count);
                                reportLine += "," + count.ToString();
                            }
                            else
                            {
                                reportLine += ",,";
                            }
                        }

                        LogHelper.WriteLine(reportLine);

                        prevAssetManagerCode = AssetManagerCode;
                        prevSmaStrategy = SmaStrategy;
                        prevMorningstarClassId = MorningstarClassId;
                        prevMorningstarClassDesc = MorningstarClassDesc;

                        for (i = 0; i < assetsList.Count; i++)
                        {
                            assetsList[i] = "";
                            finalNetList[i] = "";
                        }
                    }
                    dr.Close();
                }
            }
            catch (SqlException ex)
            {
                LogHelper.WriteLine(logFuncName + " " + ex.Message);
            }
            finally
            {
                //LogHelper.WriteLine(logFuncName + " done ");
            }
        }

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
