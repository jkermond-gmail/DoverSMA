using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

using LumenWorks.Framework.IO.Csv;

using DoverUtilities;


namespace DoverSmaEngine
{
    public class Metrics
    {
        private SqlConnection mSqlConn1 = null;
        private SqlConnection mSqlConn2 = null;
        private SqlConnection mSqlConn3 = null;
        private string mConnectionString = "";


        #region Constructor
        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        public Metrics()
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


        public void CalculateOpportunityMetrics(string sStartDate, string sEndDate)
        {
            string sCurrentDate = sStartDate;
            bool moreDates = true;
            string logFuncName = "CalculateOpportunityMetric: ";

            LogHelper.WriteLine(logFuncName + " " + sStartDate + " to " + sEndDate);

            do
            {
                LogHelper.WriteLine(logFuncName + " Processing " + sCurrentDate);

                CalculateOpportunityMetricsForDate("ProductType", sCurrentDate, "AssetsD", "OpProductTypeAssets");
                CalculateOpportunityMetricsForDate("ProductType", sCurrentDate, "GrossFlowsD", "OpProductTypeGrossFlows");
                CalculateOpportunityMetricsForDate("ProductType", sCurrentDate, "RedemptionsD", "OpProductTypeRedemptions");
                CalculateOpportunityMetricsForDate("ProductType", sCurrentDate, "FinalNetFlowsD", "OpProductTypeFinalNetFlows");

                CalculateOpportunityMetricsForDate("MorningstarClass", sCurrentDate, "AssetsD", "OpMorningstarClassAssets");
                CalculateOpportunityMetricsForDate("MorningstarClass", sCurrentDate, "GrossFlowsD", "OpMorningstarClassGrossFlows");
                CalculateOpportunityMetricsForDate("MorningstarClass", sCurrentDate, "RedemptionsD", "OpMorningstarClassRedemptions");
                CalculateOpportunityMetricsForDate("MorningstarClass", sCurrentDate, "FinalNetFlowsD", "OpMorningstarClassFinalNetFlows");

                CalculateOpportunityMetricsForDate("Sponsor", sCurrentDate, "AssetsD", "OpSponsorAssets");
                CalculateOpportunityMetricsForDate("Sponsor", sCurrentDate, "GrossFlowsD", "OpSponsorGrossFlows");
                CalculateOpportunityMetricsForDate("Sponsor", sCurrentDate, "RedemptionsD", "OpSponsorRedemptions");
                CalculateOpportunityMetricsForDate("Sponsor", sCurrentDate, "FinalNetFlowsD", "OpSponsorFinalNetFlows");

                if (sCurrentDate.Equals(sEndDate))
                    moreDates = false;
                else
                    sCurrentDate = Utils.CalculateNextEndOfQtrDate(sCurrentDate);
            }
            while (moreDates.Equals(true));

            LogHelper.WriteLine(logFuncName + " done " + sStartDate + " to " + sEndDate);

        }

        public void CalculateShareMetrics(string sStartDate, string sEndDate)
        {
            string sCurrentDate = sStartDate;
            bool moreDates = true;
            string logFuncName = "CalculateShareMetrics: ";

            LogHelper.WriteLine(logFuncName + " " + sStartDate + " to " + sEndDate);

            do
            {
                LogHelper.WriteLine(logFuncName + " Processing " + sCurrentDate);

                //CalculateShareMetricsForDate( sCurrentDate, "AssetsD", "OpProductTypeAssets", "AssetShareByProductType");

                //CalculateShareMetricsForDate( sCurrentDate, "AssetsD", "OpMorningstarClassAssets", "AssetShareByMorningstarClass");

                //CalculateShareMetricsForDate( sCurrentDate, "AssetsD", "OpSponsorAssets", "AssetShareBySponsor");

                CalculateShareMetricsForDate(sCurrentDate, "FinalNetFlowsD", "OpProductTypeFinalNetFlows", "FinalNetShareByProductType");

                CalculateShareMetricsForDate(sCurrentDate, "FinalNetFlowsD", "OpMorningstarClassFinalNetFlows", "FinalNetShareByMorningstarClass");


                if (sCurrentDate.Equals(sEndDate))
                    moreDates = false;
                else
                    sCurrentDate = Utils.CalculateNextEndOfQtrDate(sCurrentDate);
            }
            while (moreDates.Equals(true));

            LogHelper.WriteLine(logFuncName + " done " + sStartDate + " to " + sEndDate);

        }


        public void CalculateNumAssetsMetrics(string sStartDate, string sEndDate)
        {
            string sCurrentDate = sStartDate;
            bool moreDates = true;
            string logFuncName = "CalculateNumAssetsMetrics: ";

            LogHelper.WriteLine(logFuncName + " " + sStartDate + " to " + sEndDate);

            do
            {
                LogHelper.WriteLine(logFuncName + " Processing " + sCurrentDate);

                //CalculateNumAssetsMetricsForDate("ProductType", sCurrentDate);
                //CalculateNumAssetsMetricsForDate("MorningstarClass", sCurrentDate);
                //CalculateNumAssetsMetricsForDate("Sponsor", sCurrentDate);

                CalculateNumAssetsMetricsForDate("ProductTypeFinalNet", sCurrentDate);
                CalculateNumAssetsMetricsForDate("MorningstarClassFinalNet", sCurrentDate);


                if (sCurrentDate.Equals(sEndDate))
                    moreDates = false;
                else
                    sCurrentDate = Utils.CalculateNextEndOfQtrDate(sCurrentDate);
            }
            while (moreDates.Equals(true));

            LogHelper.WriteLine(logFuncName + " done " + sStartDate + " to " + sEndDate);

        }

        public void CalculateNumManagersMetrics(string sStartDate, string sEndDate)
        {
            string sCurrentDate = sStartDate;
            bool moreDates = true;
            string logFuncName = "CalculateNumManagersMetrics: ";

            LogHelper.WriteLine(logFuncName + " " + sStartDate + " to " + sEndDate);

            do
            {
                LogHelper.WriteLine(logFuncName + " Processing " + sCurrentDate);

                //CalculateNumManagersMetricsForDate("ProductType", sCurrentDate);
                //CalculateNumManagersMetricsForDate("MorningstarClass", sCurrentDate);
                //CalculateNumManagersMetricsForDate("Sponsor", sCurrentDate);
                CalculateNumManagersMetricsForDate("SponsorFinalNet", sCurrentDate);
                
                if (sCurrentDate.Equals(sEndDate))
                    moreDates = false;
                else
                    sCurrentDate = Utils.CalculateNextEndOfQtrDate(sCurrentDate);
            }
            while (moreDates.Equals(true));

            LogHelper.WriteLine(logFuncName + " done " + sStartDate + " to " + sEndDate);

        }



        public void CalculateRankMetrics(string sStartDate, string sEndDate)
        {
            string sCurrentDate = sStartDate;
            bool moreDates = true;
            string logFuncName = "CalculateRankMetrics: ";

            LogHelper.WriteLine(logFuncName + " " + sStartDate + " to " + sEndDate);

            do
            {
                LogHelper.WriteLine(logFuncName + " Processing " + sCurrentDate);

                //CalculateRankMetricsForDate("ProductType", sCurrentDate);
                //CalculateRankMetricsForDate("MorningstarClass", sCurrentDate);
                //CalculateRankMetricsForDate("Sponsor", sCurrentDate);
                CalculateRankMetricsForDate("ProductTypeFinalNet", sCurrentDate);
                CalculateRankMetricsForDate("MorningstarClassFinalNet", sCurrentDate);

                if (sCurrentDate.Equals(sEndDate))
                    moreDates = false;
                else
                    sCurrentDate = Utils.CalculateNextEndOfQtrDate(sCurrentDate);
            }
            while (moreDates.Equals(true));

            LogHelper.WriteLine(logFuncName + " done " + sStartDate + " to " + sEndDate);

        }

        private void CalculateShareMetricsForDate(string sEndOfQtrDate, string columnNumerator, string columnDenominator, string columnToUpdate)
        {
            SqlCommand cmd = null;

            string logFuncName = "CalculateShareMetricsForDate: ";

            int updateCount = 0;

            cmd = new SqlCommand
            {
                Connection = mSqlConn1,
                CommandText = 
                    "Update SmaFlows Set " + columnToUpdate + " = " + columnNumerator + "/" + columnDenominator +
                    " where (" + columnNumerator + " > 0) and (" + columnDenominator + " > 0) and (FlowDate = @FlowDate) "
            };

            cmd.Parameters.Add("@FlowDate", SqlDbType.Date);
            cmd.Parameters["@FlowDate"].Value = sEndOfQtrDate;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                LogHelper.WriteLine(logFuncName + ex.Message);
            }
            finally
            {
                updateCount += 1;
            }
        }


        private void CalculateRankMetricsForDate(string opportunityType, string sEndOfQtrDate /*, string columnToUpdate */)
        {
            SqlCommand cmd = null;
            SqlCommand cmd2 = null;
            SqlCommand cmd3 = null;

            int cmd1Count = 0;

            string logFuncName = "CalculateRankMetricsForDate: ";

            string commandText1 = "";
            string commandText2 = "";
            string commandText3 = "";

            switch (opportunityType)
            {
                case "ProductType":
                    commandText1 = @"
                        SELECT distinct 
                            [SponsorFirmCode]
                            ,[SmaProductTypeCode]
                            ,[MorningstarClassId]
                        FROM [DoverSma].[dbo].[SmaOfferings]
                        where SponsorFirmCode not in ('', 'tbd') and MorningstarClassId not in  ( '0', '', 'tbd')
                        order by SponsorFirmCode, SmaProductTypeCode, MorningstarClassId
                        ";
                    commandText2 =
                        "select SmaFlowId from SmaFlows where SmaOfferingId in " +
                        "(select SmaOfferingId from SmaOfferings " +
                        "where SponsorFirmCode = @SponsorFirmCode and SmaProductTypeCode = @SmaProductTypeCode " +
                        "and MorningstarClassId = @MorningstarClassId) " +
                        "and FlowDate = @FlowDate " +
                        "and AssetShareByProductType > 0 " +
                        "order by AssetShareByProductType desc";
                    commandText3 =
                        "Update SmaFlows Set RankAssetsByProductType = @Rank where SmaFlowId = @SmaFlowId ";
                    break;
                case "ProductTypeFinalNet":
                    commandText1 = @"
                        SELECT distinct 
                            [SponsorFirmCode]
                            ,[SmaProductTypeCode]
                            ,[MorningstarClassId]
                        FROM [DoverSma].[dbo].[SmaOfferings]
                        where SponsorFirmCode not in ('', 'tbd') and MorningstarClassId not in  ( '0', '', 'tbd')
                        order by SponsorFirmCode, SmaProductTypeCode, MorningstarClassId
                        ";
                    commandText2 =
                        "select SmaFlowId from SmaFlows where SmaOfferingId in " +
                        "(select SmaOfferingId from SmaOfferings " +
                        "where SponsorFirmCode = @SponsorFirmCode and SmaProductTypeCode = @SmaProductTypeCode " +
                        "and MorningstarClassId = @MorningstarClassId) " +
                        "and FlowDate = @FlowDate " +
                        "and FinalNetShareByProductType > 0 " +
                        "order by FinalNetShareByProductType desc";
                    commandText3 =
                        "Update SmaFlows Set RankFinalNetByProductType = @Rank where SmaFlowId = @SmaFlowId ";
                    break;
                case "MorningstarClass":
                    commandText1 = @"
                        SELECT distinct 
                            [SponsorFirmCode]
                            ,[MorningstarClassId]
                        FROM [DoverSma].[dbo].[SmaOfferings]
                        where SponsorFirmCode not in ('', 'tbd') and MorningstarClassId not in  ( '0', '', 'tbd')
                        order by SponsorFirmCode, MorningstarClassId
                        ";
                    commandText2 =
                        "select SmaFlowId from SmaFlows where SmaOfferingId in " +
                        "(select SmaOfferingId from SmaOfferings " +
                        "where SponsorFirmCode = @SponsorFirmCode " +
                        "and MorningstarClassId = @MorningstarClassId) " +
                        "and FlowDate = @FlowDate " +
                        "and AssetShareByMorningstarClass > 0 " +
                        "order by AssetShareByMorningstarClass desc";
                    commandText3 =
                        "Update SmaFlows Set RankAssetsByMorningstarClass = @Rank where SmaFlowId = @SmaFlowId ";
                    break;
                case "MorningstarClassFinalNet":
                    commandText1 = @"
                        SELECT distinct 
                            [SponsorFirmCode]
                            ,[MorningstarClassId]
                        FROM [DoverSma].[dbo].[SmaOfferings]
                        where SponsorFirmCode not in ('', 'tbd') and MorningstarClassId not in  ( '0', '', 'tbd')
                        order by SponsorFirmCode, MorningstarClassId
                        ";
                    commandText2 =
                        "select SmaFlowId from SmaFlows where SmaOfferingId in " +
                        "(select SmaOfferingId from SmaOfferings " +
                        "where SponsorFirmCode = @SponsorFirmCode " +
                        "and MorningstarClassId = @MorningstarClassId) " +
                        "and FlowDate = @FlowDate " +
                        "and FinalNetShareByMorningstarClass > 0 " +
                        "order by FinalNetShareByMorningstarClass desc";
                    commandText3 =
                        "Update SmaFlows Set RankFinalNetByMorningstarClass = @Rank where SmaFlowId = @SmaFlowId ";
                    break;
                case "Sponsor":
                    commandText1 = @"
                        SELECT distinct 
                            [SponsorFirmCode]
                        FROM [DoverSma].[dbo].[SmaOfferings]
                        where SponsorFirmCode not in ('', 'tbd') and MorningstarClassId not in  ( '0', '', 'tbd')
                        order by SponsorFirmCode
                        ";
                    commandText2 =
                        "select SmaFlowId from SmaFlows where SmaOfferingId in " +
                        "(select SmaOfferingId from SmaOfferings " +
                        "where SponsorFirmCode = @SponsorFirmCode) " +
                        "and FlowDate = @FlowDate " +
                        "and AssetShareBySponsor > 0 " +
                        "order by AssetShareBySponsor desc";
                    commandText3 =
                        "Update SmaFlows Set RankAssetsBySponsor = @Rank where SmaFlowId = @SmaFlowId ";
                    break;
            }

            int updateCount = 0;

            try
            {
                cmd = new SqlCommand
                {
                    Connection = mSqlConn1,
                    CommandText = commandText1
                };

                cmd2 = new SqlCommand
                {
                    Connection = mSqlConn2,
                    CommandText = commandText2
                };

                cmd3 = new SqlCommand
                {
                    Connection = mSqlConn3,
                    CommandText = commandText3
                };

                switch (opportunityType)
                {
                    case "ProductType":
                    case "ProductTypeFinalNet":
                        cmd2.Parameters.Add("@SponsorFirmCode", SqlDbType.VarChar);
                        cmd2.Parameters.Add("@SmaProductTypeCode", SqlDbType.VarChar);
                        cmd2.Parameters.Add("@MorningstarClassId", SqlDbType.VarChar);
                        cmd2.Parameters.Add("@FlowDate", SqlDbType.Date);
                        cmd3.Parameters.Add("@SponsorFirmCode", SqlDbType.VarChar);
                        cmd3.Parameters.Add("@SmaProductTypeCode", SqlDbType.VarChar);
                        cmd3.Parameters.Add("@MorningstarClassId", SqlDbType.VarChar);
                        cmd3.Parameters.Add("@FlowDate", SqlDbType.Date);
                        cmd3.Parameters.Add("@Rank", SqlDbType.Int);
                        cmd3.Parameters.Add("@SmaFlowId", SqlDbType.Int);
                        break;
                    case "MorningstarClass":
                    case "MorningstarClassFinalNet":
                        cmd2.Parameters.Add("@SponsorFirmCode", SqlDbType.VarChar);
                        cmd2.Parameters.Add("@MorningstarClassId", SqlDbType.VarChar);
                        cmd2.Parameters.Add("@FlowDate", SqlDbType.Date);
                        cmd3.Parameters.Add("@SponsorFirmCode", SqlDbType.VarChar);
                        cmd3.Parameters.Add("@MorningstarClassId", SqlDbType.VarChar);
                        cmd3.Parameters.Add("@FlowDate", SqlDbType.Date);
                        cmd3.Parameters.Add("@Rank", SqlDbType.Int);
                        cmd3.Parameters.Add("@SmaFlowId", SqlDbType.Int);
                        break;
                    case "Sponsor":
                        cmd2.Parameters.Add("@SponsorFirmCode", SqlDbType.VarChar);
                        cmd2.Parameters.Add("@FlowDate", SqlDbType.Date);
                        cmd3.Parameters.Add("@SponsorFirmCode", SqlDbType.VarChar);
                        cmd3.Parameters.Add("@FlowDate", SqlDbType.Date);
                        cmd3.Parameters.Add("@Rank", SqlDbType.Int);
                        cmd3.Parameters.Add("@SmaFlowId", SqlDbType.Int);
                        break;
                }

                SqlDataReader dr = null;
                SqlDataReader dr2 = null;
                int smaFlowId = 0;

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        cmd1Count += 1;

                        string SponsorFirmCode = "";
                        string SmaProductTypeCode = "";
                        string MorningstarClassId = "";

                        switch (opportunityType)
                        {
                            case "ProductType":
                            case "ProductTypeFinalNet":
                                SponsorFirmCode = dr["SponsorFirmCode"].ToString();
                                SmaProductTypeCode = dr["SmaProductTypeCode"].ToString();
                                MorningstarClassId = dr["MorningstarClassId"].ToString();
                                cmd2.Parameters["@SponsorFirmCode"].Value = SponsorFirmCode;
                                cmd2.Parameters["@SmaProductTypeCode"].Value = SmaProductTypeCode;
                                cmd2.Parameters["@MorningstarClassId"].Value = MorningstarClassId;
                                cmd2.Parameters["@FlowDate"].Value = sEndOfQtrDate;
                                cmd3.Parameters["@SponsorFirmCode"].Value = SponsorFirmCode;
                                cmd3.Parameters["@SmaProductTypeCode"].Value = SmaProductTypeCode;
                                cmd3.Parameters["@MorningstarClassId"].Value = MorningstarClassId;
                                cmd3.Parameters["@FlowDate"].Value = sEndOfQtrDate;
                                break;
                            case "MorningstarClass":
                            case "MorningstarClassFinalNet":
                                SponsorFirmCode = dr["SponsorFirmCode"].ToString();
                                MorningstarClassId = dr["MorningstarClassId"].ToString();
                                cmd2.Parameters["@SponsorFirmCode"].Value = SponsorFirmCode;
                                cmd2.Parameters["@MorningstarClassId"].Value = MorningstarClassId;
                                cmd2.Parameters["@FlowDate"].Value = sEndOfQtrDate;
                                cmd3.Parameters["@SponsorFirmCode"].Value = SponsorFirmCode;
                                cmd3.Parameters["@MorningstarClassId"].Value = MorningstarClassId;
                                cmd3.Parameters["@FlowDate"].Value = sEndOfQtrDate;
                                break;
                            case "Sponsor":
                                SponsorFirmCode = dr["SponsorFirmCode"].ToString();
                                cmd2.Parameters["@SponsorFirmCode"].Value = SponsorFirmCode;
                                cmd2.Parameters["@FlowDate"].Value = sEndOfQtrDate;
                                cmd3.Parameters["@SponsorFirmCode"].Value = SponsorFirmCode;
                                cmd3.Parameters["@FlowDate"].Value = sEndOfQtrDate;
                                break;
                        }

                        dr2 = cmd2.ExecuteReader();
                        if (dr2.HasRows)
                        {
                            int rank = 0;
                            while (dr2.Read())
                            {
                                smaFlowId = (int)dr2["SmaFlowId"];
                                cmd3.Parameters["@SmaFlowId"].Value = smaFlowId.ToString();
                                rank += 1;
                                cmd3.Parameters["@Rank"].Value = rank.ToString();

                                //LogHelper.WriteLine(sEndOfQtrDate + "," + SponsorFirmCode + "," + SmaProductTypeCode + "," + MorningstarClassId + "," + columnToSum + "," + sumOfColumn.ToString());
                                try
                                {
                                    cmd3.ExecuteNonQuery();
                                }
                                catch (SqlException ex)
                                {
                                    LogHelper.WriteLine(logFuncName + ex.Message);
                                }
                                finally
                                {
                                    updateCount += 1;
                                }
                            }
                        }
                        dr2.Close();
                    }
                    dr.Close();
                }
            }
            catch (SqlException ex)
            {
                LogHelper.WriteLine(logFuncName + " " + sEndOfQtrDate + " " + ex.Message);
            }
            finally
            {
                //LogHelper.WriteLine(logFuncName + "Rows Updated " + updateCount + " " + sEndOfQtrDate + " " + columnToSum);
                //LogHelper.WriteLine(logFuncName + " cmd1Count " + cmd1Count + " cmd2Count " + cmd2Count);
            }


        }

        private void CalculateNumAssetsMetricsForDate(string opportunityType, string sEndOfQtrDate /*, string columnToUpdate */)
        {
            SqlCommand cmd = null;
            SqlCommand cmd2 = null;
            SqlCommand cmd3 = null;

            int cmd1Count = 0;
            int cmd2Count = 0;

            string logFuncName = "CalculateNumAssetsMetricForDate: ";

            string commandText1 = "";
            string commandText2 = "";
            string commandText3 = "";

            switch (opportunityType)
            {
                case "ProductType":
                    commandText1 = @"
                        SELECT distinct 
                            [SponsorFirmCode]
                            ,[SmaProductTypeCode]
                            ,[MorningstarClassId]
                        FROM [DoverSma].[dbo].[SmaOfferings]
                        where SponsorFirmCode not in ('', 'tbd') and MorningstarClassId not in  ( '0', '', 'tbd')
                        order by SponsorFirmCode, SmaProductTypeCode, MorningstarClassId
                        ";
                    commandText2 =
                        "select count(*) as TheCount from SmaFlows where SmaOfferingId in " +
                        "(select SmaOfferingId from SmaOfferings " +
                        "where SponsorFirmCode = @SponsorFirmCode and SmaProductTypeCode = @SmaProductTypeCode " +
                        "and MorningstarClassId = @MorningstarClassId) " +
                        "and FlowDate = @FlowDate " +
                        "and OpProductTypeAssets > 0";
                    commandText3 =
                        "Update SmaFlows Set NumAssetsByProductType = @NumAssets where SmaOfferingId in " +
                        "(select SmaOfferingId from SmaOfferings " +
                        "where SponsorFirmCode = @SponsorFirmCode " +
                        "and MorningstarClassId = @MorningstarClassId) " +
                        "and FlowDate = @FlowDate " +
                        "and OpProductTypeAssets > 0";
                    break;
                case "ProductTypeFinalNet":
                    commandText1 = @"
                        SELECT distinct 
                            [SponsorFirmCode]
                            ,[SmaProductTypeCode]
                            ,[MorningstarClassId]
                        FROM [DoverSma].[dbo].[SmaOfferings]
                        where SponsorFirmCode not in ('', 'tbd') and MorningstarClassId not in  ( '0', '', 'tbd')
                        order by SponsorFirmCode, SmaProductTypeCode, MorningstarClassId
                        ";
                    commandText2 =
                        "select count(*) as TheCount from SmaFlows where SmaOfferingId in " +
                        "(select SmaOfferingId from SmaOfferings " +
                        "where SponsorFirmCode = @SponsorFirmCode and SmaProductTypeCode = @SmaProductTypeCode " +
                        "and MorningstarClassId = @MorningstarClassId) " +
                        "and FlowDate = @FlowDate " +
                        "and OpProductTypeFinalNetFlows > 0";
                    commandText3 =
                        "Update SmaFlows Set NumFinalNetByProductType = @NumAssets where SmaOfferingId in " +
                        "(select SmaOfferingId from SmaOfferings " +
                        "where SponsorFirmCode = @SponsorFirmCode " +
                        "and MorningstarClassId = @MorningstarClassId) " +
                        "and FlowDate = @FlowDate " +
                        "and OpProductTypeFinalNetFlows > 0";
                    break;
                case "MorningstarClass":
                    commandText1 = @"
                        SELECT distinct 
                            [SponsorFirmCode]
                            ,[MorningstarClassId]
                        FROM [DoverSma].[dbo].[SmaOfferings]
                        where SponsorFirmCode not in ('', 'tbd') and MorningstarClassId not in  ( '0', '', 'tbd')
                        order by SponsorFirmCode, MorningstarClassId
                        ";
                    commandText2 =
                        "select count(*) as TheCount from SmaFlows where SmaOfferingId in " +
                        "(select SmaOfferingId from SmaOfferings " +
                        "where SponsorFirmCode = @SponsorFirmCode " +
                        "and MorningstarClassId = @MorningstarClassId) " +
                        "and FlowDate = @FlowDate " +
                        "and OpMorningstarClassAssets > 0";
                    commandText3 =
                        "Update SmaFlows Set NumAssetsByMorningstarClass = @NumAssets where SmaOfferingId in " +
                        "(select SmaOfferingId from SmaOfferings " +
                        "where SponsorFirmCode = @SponsorFirmCode " +
                        "and MorningstarClassId = @MorningstarClassId) " +
                        "and FlowDate = @FlowDate " +
                        "and OpMorningstarClassAssets > 0";
                    break;
                case "MorningstarClassFinalNet":
                    commandText1 = @"
                        SELECT distinct 
                            [SponsorFirmCode]
                            ,[MorningstarClassId]
                        FROM [DoverSma].[dbo].[SmaOfferings]
                        where SponsorFirmCode not in ('', 'tbd') and MorningstarClassId not in  ( '0', '', 'tbd')
                        order by SponsorFirmCode, MorningstarClassId
                        ";
                    commandText2 =
                        "select count(*) as TheCount from SmaFlows where SmaOfferingId in " +
                        "(select SmaOfferingId from SmaOfferings " +
                        "where SponsorFirmCode = @SponsorFirmCode " +
                        "and MorningstarClassId = @MorningstarClassId) " +
                        "and FlowDate = @FlowDate " +
                        "and OpMorningstarClassFinalNetFlows > 0";
                    commandText3 =
                        "Update SmaFlows Set NumFinalNetByMorningstarClass = @NumAssets where SmaOfferingId in " +
                        "(select SmaOfferingId from SmaOfferings " +
                        "where SponsorFirmCode = @SponsorFirmCode " +
                        "and MorningstarClassId = @MorningstarClassId) " +
                        "and FlowDate = @FlowDate " +
                        "and OpMorningstarClassFinalNetFlows > 0";
                    break;
                case "Sponsor":
                    commandText1 = @"
                        SELECT distinct 
                            [SponsorFirmCode]
                        FROM [DoverSma].[dbo].[SmaOfferings]
                        where SponsorFirmCode not in ('', 'tbd') and MorningstarClassId not in  ( '0', '', 'tbd')
                        order by SponsorFirmCode
                        ";
                    commandText2 =
                        "select count(*) as TheCount from SmaFlows where SmaOfferingId in " +
                        "(select SmaOfferingId from SmaOfferings " +
                        "where SponsorFirmCode = @SponsorFirmCode) " +
                        "and FlowDate = @FlowDate " +
                        "and OpSponsorAssets > 0"; ;
                    commandText3 =
                        "Update SmaFlows Set NumAssetsBySponsor = @NumAssets where SmaOfferingId in " +
                        "(select SmaOfferingId from SmaOfferings " +
                        "where SponsorFirmCode = @SponsorFirmCode) " +
                        "and FlowDate = @FlowDate " +
                        "and OpSponsorAssets > 0";
                    break;
            }

            int theCount = 0;
            int updateCount = 0;

            try
            {
                cmd = new SqlCommand
                {
                    Connection = mSqlConn1,
                    CommandText = commandText1
                };

                cmd2 = new SqlCommand
                {
                    Connection = mSqlConn2,
                    CommandText = commandText2
                };

                cmd3 = new SqlCommand
                {
                    Connection = mSqlConn3,
                    CommandText = commandText3
                };

                switch (opportunityType)
                {
                    case "ProductType":
                    case "ProductTypeFinalNet":
                        cmd2.Parameters.Add("@SponsorFirmCode", SqlDbType.VarChar);
                        cmd2.Parameters.Add("@SmaProductTypeCode", SqlDbType.VarChar);
                        cmd2.Parameters.Add("@MorningstarClassId", SqlDbType.VarChar);
                        cmd2.Parameters.Add("@FlowDate", SqlDbType.Date);
                        cmd3.Parameters.Add("@SponsorFirmCode", SqlDbType.VarChar);
                        cmd3.Parameters.Add("@SmaProductTypeCode", SqlDbType.VarChar);
                        cmd3.Parameters.Add("@MorningstarClassId", SqlDbType.VarChar);
                        cmd3.Parameters.Add("@FlowDate", SqlDbType.Date);
                        cmd3.Parameters.Add("@NumAssets", SqlDbType.Int);
                        break;
                    case "MorningstarClass":
                    case "MorningstarClassFinalNet":
                        cmd2.Parameters.Add("@SponsorFirmCode", SqlDbType.VarChar);
                        cmd2.Parameters.Add("@MorningstarClassId", SqlDbType.VarChar);
                        cmd2.Parameters.Add("@FlowDate", SqlDbType.Date);
                        cmd3.Parameters.Add("@SponsorFirmCode", SqlDbType.VarChar);
                        cmd3.Parameters.Add("@MorningstarClassId", SqlDbType.VarChar);
                        cmd3.Parameters.Add("@FlowDate", SqlDbType.Date);
                        cmd3.Parameters.Add("@NumAssets", SqlDbType.Int);
                        break;
                    case "Sponsor":
                        cmd2.Parameters.Add("@SponsorFirmCode", SqlDbType.VarChar);
                        cmd2.Parameters.Add("@FlowDate", SqlDbType.Date);
                        cmd3.Parameters.Add("@SponsorFirmCode", SqlDbType.VarChar);
                        cmd3.Parameters.Add("@FlowDate", SqlDbType.Date);
                        cmd3.Parameters.Add("@NumAssets", SqlDbType.Int);
                        break;
                }

                SqlDataReader dr = null;
                SqlDataReader dr2 = null;

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        cmd1Count += 1;

                        string SponsorFirmCode = "";
                        string SmaProductTypeCode = "";
                        string MorningstarClassId = "";

                        switch (opportunityType)
                        {
                            case "ProductType":
                            case "ProductTypeFinalNet":
                                SponsorFirmCode = dr["SponsorFirmCode"].ToString();
                                SmaProductTypeCode = dr["SmaProductTypeCode"].ToString();
                                MorningstarClassId = dr["MorningstarClassId"].ToString();
                                cmd2.Parameters["@SponsorFirmCode"].Value = SponsorFirmCode;
                                cmd2.Parameters["@SmaProductTypeCode"].Value = SmaProductTypeCode;
                                cmd2.Parameters["@MorningstarClassId"].Value = MorningstarClassId;
                                cmd2.Parameters["@FlowDate"].Value = sEndOfQtrDate;
                                cmd3.Parameters["@SponsorFirmCode"].Value = SponsorFirmCode;
                                cmd3.Parameters["@SmaProductTypeCode"].Value = SmaProductTypeCode;
                                cmd3.Parameters["@MorningstarClassId"].Value = MorningstarClassId;
                                cmd3.Parameters["@FlowDate"].Value = sEndOfQtrDate;
                                break;
                            case "MorningstarClass":
                            case "MorningstarClassFinalNet":
                                SponsorFirmCode = dr["SponsorFirmCode"].ToString();
                                MorningstarClassId = dr["MorningstarClassId"].ToString();
                                cmd2.Parameters["@SponsorFirmCode"].Value = SponsorFirmCode;
                                cmd2.Parameters["@MorningstarClassId"].Value = MorningstarClassId;
                                cmd2.Parameters["@FlowDate"].Value = sEndOfQtrDate;
                                cmd3.Parameters["@SponsorFirmCode"].Value = SponsorFirmCode;
                                cmd3.Parameters["@MorningstarClassId"].Value = MorningstarClassId;
                                cmd3.Parameters["@FlowDate"].Value = sEndOfQtrDate;
                                break;
                            case "Sponsor":
                                SponsorFirmCode = dr["SponsorFirmCode"].ToString();
                                cmd2.Parameters["@SponsorFirmCode"].Value = SponsorFirmCode;
                                cmd2.Parameters["@FlowDate"].Value = sEndOfQtrDate;
                                cmd3.Parameters["@SponsorFirmCode"].Value = SponsorFirmCode;
                                cmd3.Parameters["@FlowDate"].Value = sEndOfQtrDate;
                                break;
                        }

                        dr2 = cmd2.ExecuteReader();
                        if (dr2.HasRows)
                        {
                            dr2.Read();
                            {
                                cmd2Count += 1;
                                string colVal = dr2["TheCount"].ToString();
                                theCount = Convert.ToInt32(colVal);
                                cmd3.Parameters["@NumAssets"].Value = theCount.ToString();

                                //LogHelper.WriteLine(sEndOfQtrDate + "," + SponsorFirmCode + "," + SmaProductTypeCode + "," + MorningstarClassId + "," + columnToSum + "," + sumOfColumn.ToString());
                                try
                                {
                                    cmd3.ExecuteNonQuery();
                                }
                                catch (SqlException ex)
                                {
                                    LogHelper.WriteLine(logFuncName + ex.Message);
                                }
                                finally
                                {
                                    updateCount += 1;
                                }
                            }
                        }
                        dr2.Close();
                    }
                    dr.Close();
                }
            }
            catch (SqlException ex)
            {
                LogHelper.WriteLine(logFuncName + " " + sEndOfQtrDate + " " + ex.Message);
            }
            finally
            {
                //LogHelper.WriteLine(logFuncName + "Rows Updated " + updateCount + " " + sEndOfQtrDate + " " + columnToSum);
                //LogHelper.WriteLine(logFuncName + " cmd1Count " + cmd1Count + " cmd2Count " + cmd2Count);
            }
        }

        private void CalculateNumManagersMetricsForDate(string opportunityType, string sEndOfQtrDate /*, string columnToUpdate */)
        {
            SqlCommand cmd = null;
            SqlCommand cmd2 = null;
            SqlCommand cmd3 = null;

            int cmd1Count = 0;
            int cmd2Count = 0;

            string logFuncName = "CalculateNumManagersMetricsForDate: ";

            string commandText1 = "";
            string commandText2 = "";
            string commandText3 = "";

            switch (opportunityType)
            {
                case "ProductType":
                    commandText1 = @"
                        SELECT distinct 
                            [SponsorFirmCode]
                            ,[SmaProductTypeCode]
                            ,[MorningstarClassId]
                        FROM [DoverSma].[dbo].[SmaOfferings]
                        where SponsorFirmCode not in ('', 'tbd') and MorningstarClassId not in  ( '0', '', 'tbd')
                        order by SponsorFirmCode, SmaProductTypeCode, MorningstarClassId
                        ";
                    commandText2 =
                        "select count( distinct AssetManagerCode ) as TheCount from SmaFlows where SmaOfferingId in " +
                        "(select SmaOfferingId from SmaOfferings " +
                        "where SponsorFirmCode = @SponsorFirmCode and SmaProductTypeCode = @SmaProductTypeCode " +
                        "and MorningstarClassId = @MorningstarClassId) " +
                        "and FlowDate = @FlowDate " +
                        "and OpProductTypeAssets > 0";
                    commandText3 =
                        "Update SmaFlows Set NumManagersByProductType = @NumManagers where SmaOfferingId in " +
                        "(select SmaOfferingId from SmaOfferings " +
                        "where SponsorFirmCode = @SponsorFirmCode " +
                        "and MorningstarClassId = @MorningstarClassId) " +
                        "and FlowDate = @FlowDate " +
                        "and OpProductTypeAssets > 0";
                    break;
                case "MorningstarClass":
                    commandText1 = @"
                        SELECT distinct 
                            [SponsorFirmCode]
                            ,[MorningstarClassId]
                        FROM [DoverSma].[dbo].[SmaOfferings]
                        where SponsorFirmCode not in ('', 'tbd') and MorningstarClassId not in  ( '0', '', 'tbd')
                        order by SponsorFirmCode, MorningstarClassId
                        ";
                    commandText2 =
                        "select count( distinct AssetManagerCode )  as TheCount from SmaFlows where SmaOfferingId in " +
                        "(select SmaOfferingId from SmaOfferings " +
                        "where SponsorFirmCode = @SponsorFirmCode " +
                        "and MorningstarClassId = @MorningstarClassId) " +
                        "and FlowDate = @FlowDate " +
                        "and OpMorningstarClassAssets > 0";
                    commandText3 =
                        "Update SmaFlows Set NumManagersByMorningstarClass = @NumManagers where SmaOfferingId in " +
                        "(select SmaOfferingId from SmaOfferings " +
                        "where SponsorFirmCode = @SponsorFirmCode " +
                        "and MorningstarClassId = @MorningstarClassId) " +
                        "and FlowDate = @FlowDate " +
                        "and OpMorningstarClassAssets > 0";
                    break;
                case "Sponsor":
                    commandText1 = @"
                        SELECT distinct 
                            [SponsorFirmCode]
                        FROM [DoverSma].[dbo].[SmaOfferings]
                        where SponsorFirmCode not in ('', 'tbd') and MorningstarClassId not in  ( '0', '', 'tbd')
                        order by SponsorFirmCode
                        ";
                    commandText2 =
                        "select count( distinct AssetManagerCode ) as TheCount from SmaFlows where SmaOfferingId in " +
                        "(select SmaOfferingId from SmaOfferings " +
                        "where SponsorFirmCode = @SponsorFirmCode) " +
                        "and FlowDate = @FlowDate " +
                        "and OpSponsorAssets > 0"; ;
                    commandText3 =
                        "Update SmaFlows Set NumManagersBySponsor = @NumManagers where SmaOfferingId in " +
                        "(select SmaOfferingId from SmaOfferings " +
                        "where SponsorFirmCode = @SponsorFirmCode) " +
                        "and FlowDate = @FlowDate " +
                        "and OpSponsorAssets > 0";
                    break;
                case "SponsorFinalNet":
                    commandText1 = @"
                        SELECT distinct 
                            [SponsorFirmCode]
                        FROM [DoverSma].[dbo].[SmaOfferings]
                        where SponsorFirmCode not in ('', 'tbd') and MorningstarClassId not in  ( '0', '', 'tbd')
                        order by SponsorFirmCode
                        ";
                    commandText2 =
                        "select count( distinct AssetManagerCode ) as TheCount from SmaFlows where SmaOfferingId in " +
                        "(select SmaOfferingId from SmaOfferings " +
                        "where SponsorFirmCode = @SponsorFirmCode) " +
                        "and FlowDate = @FlowDate " +
                        "and OpSponsorFinalNetFlows > 0"; ;
                    commandText3 =
                        "Update SmaFlows Set NumManagersBySponsorFinalNet = @NumManagers where SmaOfferingId in " +
                        "(select SmaOfferingId from SmaOfferings " +
                        "where SponsorFirmCode = @SponsorFirmCode) " +
                        "and FlowDate = @FlowDate " +
                        "and OpSponsorFinalNetFlows > 0";
                    break;
            }

            int theCount = 0;
            int updateCount = 0;

            try
            {
                cmd = new SqlCommand
                {
                    Connection = mSqlConn1,
                    CommandText = commandText1
                };

                cmd2 = new SqlCommand
                {
                    Connection = mSqlConn2,
                    CommandText = commandText2
                };

                cmd3 = new SqlCommand
                {
                    Connection = mSqlConn3,
                    CommandText = commandText3
                };

                switch (opportunityType)
                {
                    case "ProductType":
                        cmd2.Parameters.Add("@SponsorFirmCode", SqlDbType.VarChar);
                        cmd2.Parameters.Add("@SmaProductTypeCode", SqlDbType.VarChar);
                        cmd2.Parameters.Add("@MorningstarClassId", SqlDbType.VarChar);
                        cmd2.Parameters.Add("@FlowDate", SqlDbType.Date);
                        cmd3.Parameters.Add("@SponsorFirmCode", SqlDbType.VarChar);
                        cmd3.Parameters.Add("@SmaProductTypeCode", SqlDbType.VarChar);
                        cmd3.Parameters.Add("@MorningstarClassId", SqlDbType.VarChar);
                        cmd3.Parameters.Add("@FlowDate", SqlDbType.Date);
                        cmd3.Parameters.Add("@NumManagers", SqlDbType.Int);
                        break;
                    case "MorningstarClass":
                        cmd2.Parameters.Add("@SponsorFirmCode", SqlDbType.VarChar);
                        cmd2.Parameters.Add("@MorningstarClassId", SqlDbType.VarChar);
                        cmd2.Parameters.Add("@FlowDate", SqlDbType.Date);
                        cmd3.Parameters.Add("@SponsorFirmCode", SqlDbType.VarChar);
                        cmd3.Parameters.Add("@MorningstarClassId", SqlDbType.VarChar);
                        cmd3.Parameters.Add("@FlowDate", SqlDbType.Date);
                        cmd3.Parameters.Add("@NumManagers", SqlDbType.Int);
                        break;
                    case "Sponsor":
                    case "SponsorFinalNet":
                        cmd2.Parameters.Add("@SponsorFirmCode", SqlDbType.VarChar);
                        cmd2.Parameters.Add("@FlowDate", SqlDbType.Date);
                        cmd3.Parameters.Add("@SponsorFirmCode", SqlDbType.VarChar);
                        cmd3.Parameters.Add("@FlowDate", SqlDbType.Date);
                        cmd3.Parameters.Add("@NumManagers", SqlDbType.Int);
                        break;
                }

                SqlDataReader dr = null;
                SqlDataReader dr2 = null;

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        cmd1Count += 1;

                        string SponsorFirmCode = "";
                        string SmaProductTypeCode = "";
                        string MorningstarClassId = "";

                        switch (opportunityType)
                        {
                            case "ProductType":
                                SponsorFirmCode = dr["SponsorFirmCode"].ToString();
                                SmaProductTypeCode = dr["SmaProductTypeCode"].ToString();
                                MorningstarClassId = dr["MorningstarClassId"].ToString();
                                cmd2.Parameters["@SponsorFirmCode"].Value = SponsorFirmCode;
                                cmd2.Parameters["@SmaProductTypeCode"].Value = SmaProductTypeCode;
                                cmd2.Parameters["@MorningstarClassId"].Value = MorningstarClassId;
                                cmd2.Parameters["@FlowDate"].Value = sEndOfQtrDate;
                                cmd3.Parameters["@SponsorFirmCode"].Value = SponsorFirmCode;
                                cmd3.Parameters["@SmaProductTypeCode"].Value = SmaProductTypeCode;
                                cmd3.Parameters["@MorningstarClassId"].Value = MorningstarClassId;
                                cmd3.Parameters["@FlowDate"].Value = sEndOfQtrDate;
                                break;
                            case "MorningstarClass":
                                SponsorFirmCode = dr["SponsorFirmCode"].ToString();
                                MorningstarClassId = dr["MorningstarClassId"].ToString();
                                cmd2.Parameters["@SponsorFirmCode"].Value = SponsorFirmCode;
                                cmd2.Parameters["@MorningstarClassId"].Value = MorningstarClassId;
                                cmd2.Parameters["@FlowDate"].Value = sEndOfQtrDate;
                                cmd3.Parameters["@SponsorFirmCode"].Value = SponsorFirmCode;
                                cmd3.Parameters["@MorningstarClassId"].Value = MorningstarClassId;
                                cmd3.Parameters["@FlowDate"].Value = sEndOfQtrDate;
                                break;
                            case "Sponsor":
                            case "SponsorFinalNet":
                                SponsorFirmCode = dr["SponsorFirmCode"].ToString();
                                cmd2.Parameters["@SponsorFirmCode"].Value = SponsorFirmCode;
                                cmd2.Parameters["@FlowDate"].Value = sEndOfQtrDate;
                                cmd3.Parameters["@SponsorFirmCode"].Value = SponsorFirmCode;
                                cmd3.Parameters["@FlowDate"].Value = sEndOfQtrDate;
                                break;
                        }

                        dr2 = cmd2.ExecuteReader();
                        if (dr2.HasRows)
                        {
                            dr2.Read();
                            {
                                cmd2Count += 1;
                                string colVal = dr2["TheCount"].ToString();
                                theCount = Convert.ToInt32(colVal);
                                cmd3.Parameters["@NumManagers"].Value = theCount.ToString();

                                //LogHelper.WriteLine(sEndOfQtrDate + "," + SponsorFirmCode + "," + SmaProductTypeCode + "," + MorningstarClassId + "," + columnToSum + "," + sumOfColumn.ToString());
                                try
                                {
                                    cmd3.ExecuteNonQuery();
                                }
                                catch (SqlException ex)
                                {
                                    LogHelper.WriteLine(logFuncName + ex.Message);
                                }
                                finally
                                {
                                    updateCount += 1;
                                }
                            }
                        }
                        dr2.Close();
                    }
                    dr.Close();
                }
            }
            catch (SqlException ex)
            {
                LogHelper.WriteLine(logFuncName + " " + sEndOfQtrDate + " " + ex.Message);
            }
            finally
            {
                //LogHelper.WriteLine(logFuncName + "Rows Updated " + updateCount + " " + sEndOfQtrDate + " " + columnToSum);
                //LogHelper.WriteLine(logFuncName + " cmd1Count " + cmd1Count + " cmd2Count " + cmd2Count);
            }
        }




        private void CalculateOpportunityMetricsForDate(string opportunityType, string sEndOfQtrDate, string columnToSum, string columnToUpdate)
        {
            SqlCommand cmd = null;
            SqlCommand cmd2 = null;
            SqlCommand cmd3 = null;

            int cmd1Count = 0;
            int cmd2Count = 0;

            string logFuncName = "CalculateOpportunityMetricForDate: ";

            string commandText1 = "";
            string commandText2 = "";
            string commandText3 = "";

            switch (opportunityType)
            {
                case "ProductType":
                    commandText1 = @"
                        SELECT distinct 
                            [SponsorFirmCode]
                            ,[SmaProductTypeCode]
                            ,[MorningstarClassId]
                        FROM [DoverSma].[dbo].[SmaOfferings]
                        where SponsorFirmCode not in ('', 'tbd') and MorningstarClassId not in  ( '0', '', 'tbd')
                        order by SponsorFirmCode, SmaProductTypeCode, MorningstarClassId
                        ";
                    commandText2 =
                        "select sum(" + columnToSum + ") as TheSum from SmaFlows where SmaOfferingId in " +
                        "(select SmaOfferingId from SmaOfferings " +
                        "where SponsorFirmCode = @SponsorFirmCode and SmaProductTypeCode = @SmaProductTypeCode " +
                        "and MorningstarClassId = @MorningstarClassId) " +
                        "and FlowDate = @FlowDate";
                    commandText3 =
                        "Update SmaFlows Set " + columnToUpdate + " = @" + columnToUpdate + " where (" + columnToSum + " is not NULL) and " +
                        "(FlowDate = @FlowDate) and (SmaOfferingId in " +
                        "(select SmaOfferingId from SmaOfferings " +
                        " where SponsorFirmCode = @SponsorFirmCode and SmaProductTypeCode = @SmaProductTypeCode and MorningstarClassId = @MorningstarClassId))";
                    break;
                case "MorningstarClass":
                    commandText1= @"
                        SELECT distinct 
                            [SponsorFirmCode]
                            ,[MorningstarClassId]
                        FROM [DoverSma].[dbo].[SmaOfferings]
                        where SponsorFirmCode not in ('', 'tbd') and MorningstarClassId not in  ( '0', '', 'tbd')
                        order by SponsorFirmCode, MorningstarClassId
                        ";
                    commandText2 =
                        "select sum(" + columnToSum + ") as TheSum from SmaFlows where SmaOfferingId in " +
                        "(select SmaOfferingId from SmaOfferings " +
                        "where SponsorFirmCode = @SponsorFirmCode " +
                        "and MorningstarClassId = @MorningstarClassId) " +
                        "and FlowDate = @FlowDate";
                    commandText3 =
                        "Update SmaFlows Set " + columnToUpdate + " = @" + columnToUpdate + " where (" + columnToSum + " is not NULL) and " +
                        "(FlowDate = @FlowDate) and (SmaOfferingId in " +
                        "(select SmaOfferingId from SmaOfferings " +
                        " where SponsorFirmCode = @SponsorFirmCode and MorningstarClassId = @MorningstarClassId))";
                    break;
                case "Sponsor":
                    commandText1 = @"
                        SELECT distinct 
                            [SponsorFirmCode]
                        FROM [DoverSma].[dbo].[SmaOfferings]
                        where SponsorFirmCode not in ('', 'tbd') and MorningstarClassId not in  ( '0', '', 'tbd')
                        order by SponsorFirmCode
                        ";
                    commandText2 =
                        "select sum(" + columnToSum + ") as TheSum from SmaFlows where SmaOfferingId in " +
                        "(select SmaOfferingId from SmaOfferings " +
                        "where SponsorFirmCode = @SponsorFirmCode) " +
                        "and FlowDate = @FlowDate";
                    commandText3 =
                        "Update SmaFlows Set " + columnToUpdate + " = @" + columnToUpdate + " where (" + columnToSum + " is not NULL) and " +
                        "(FlowDate = @FlowDate) and (SmaOfferingId in " +
                        "(select SmaOfferingId from SmaOfferings " +
                        " where SponsorFirmCode = @SponsorFirmCode))";
                    break;
            }

            decimal sumOfColumn = 0m;
            int updateCount = 0;

            try
            {
                cmd = new SqlCommand
                {
                    Connection = mSqlConn1,
                    CommandText = commandText1
                };

                cmd2 = new SqlCommand
                {
                    Connection = mSqlConn2,
                    CommandText = commandText2
                };

                cmd3 = new SqlCommand
                {
                    Connection = mSqlConn3,
                    CommandText = commandText3
                };

                switch (opportunityType)
                {
                    case "ProductType":
                        cmd2.Parameters.Add("@SponsorFirmCode", SqlDbType.VarChar);
                        cmd2.Parameters.Add("@SmaProductTypeCode", SqlDbType.VarChar);
                        cmd2.Parameters.Add("@MorningstarClassId", SqlDbType.VarChar);
                        cmd2.Parameters.Add("@FlowDate", SqlDbType.Date);
                        cmd3.Parameters.Add("@SponsorFirmCode", SqlDbType.VarChar);
                        cmd3.Parameters.Add("@SmaProductTypeCode", SqlDbType.VarChar);
                        cmd3.Parameters.Add("@MorningstarClassId", SqlDbType.VarChar);
                        cmd3.Parameters.Add("@FlowDate", SqlDbType.Date);
                        cmd3.Parameters.Add("@" + columnToUpdate, SqlDbType.Decimal);
                        break;
                    case "MorningstarClass":
                        cmd2.Parameters.Add("@SponsorFirmCode", SqlDbType.VarChar);
                        cmd2.Parameters.Add("@MorningstarClassId", SqlDbType.VarChar);
                        cmd2.Parameters.Add("@FlowDate", SqlDbType.Date);
                        cmd3.Parameters.Add("@SponsorFirmCode", SqlDbType.VarChar);
                        cmd3.Parameters.Add("@MorningstarClassId", SqlDbType.VarChar);
                        cmd3.Parameters.Add("@FlowDate", SqlDbType.Date);
                        cmd3.Parameters.Add("@" + columnToUpdate, SqlDbType.Decimal);
                        break;
                    case "Sponsor":
                        cmd2.Parameters.Add("@SponsorFirmCode", SqlDbType.VarChar);
                        cmd2.Parameters.Add("@FlowDate", SqlDbType.Date);
                        cmd3.Parameters.Add("@SponsorFirmCode", SqlDbType.VarChar);
                        cmd3.Parameters.Add("@FlowDate", SqlDbType.Date);
                        cmd3.Parameters.Add("@" + columnToUpdate, SqlDbType.Decimal);
                        break;
                }



                SqlDataReader dr = null;
                SqlDataReader dr2 = null;

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        cmd1Count += 1;

                        string SponsorFirmCode = "";
                        string SmaProductTypeCode = "";
                        string MorningstarClassId = "";

                        switch (opportunityType)
                        {
                            case "ProductType":
                                SponsorFirmCode = dr["SponsorFirmCode"].ToString();
                                SmaProductTypeCode = dr["SmaProductTypeCode"].ToString();
                                MorningstarClassId = dr["MorningstarClassId"].ToString();
                                cmd2.Parameters["@SponsorFirmCode"].Value = SponsorFirmCode;
                                cmd2.Parameters["@SmaProductTypeCode"].Value = SmaProductTypeCode;
                                cmd2.Parameters["@MorningstarClassId"].Value = MorningstarClassId;
                                cmd2.Parameters["@FlowDate"].Value = sEndOfQtrDate;
                                cmd3.Parameters["@SponsorFirmCode"].Value = SponsorFirmCode;
                                cmd3.Parameters["@SmaProductTypeCode"].Value = SmaProductTypeCode;
                                cmd3.Parameters["@MorningstarClassId"].Value = MorningstarClassId;
                                cmd3.Parameters["@FlowDate"].Value = sEndOfQtrDate;
                                break;
                            case "MorningstarClass":
                                SponsorFirmCode = dr["SponsorFirmCode"].ToString();
                                MorningstarClassId = dr["MorningstarClassId"].ToString();
                                cmd2.Parameters["@SponsorFirmCode"].Value = SponsorFirmCode;
                                cmd2.Parameters["@MorningstarClassId"].Value = MorningstarClassId;
                                cmd2.Parameters["@FlowDate"].Value = sEndOfQtrDate;
                                cmd3.Parameters["@SponsorFirmCode"].Value = SponsorFirmCode;
                                cmd3.Parameters["@MorningstarClassId"].Value = MorningstarClassId;
                                cmd3.Parameters["@FlowDate"].Value = sEndOfQtrDate;
                                break;
                            case "Sponsor":
                                SponsorFirmCode = dr["SponsorFirmCode"].ToString();
                                cmd2.Parameters["@SponsorFirmCode"].Value = SponsorFirmCode;
                                cmd2.Parameters["@FlowDate"].Value = sEndOfQtrDate;
                                cmd3.Parameters["@SponsorFirmCode"].Value = SponsorFirmCode;
                                cmd3.Parameters["@FlowDate"].Value = sEndOfQtrDate;
                                break;
                        }

                        dr2 = cmd2.ExecuteReader();
                        if (dr2.HasRows)
                        {
                            dr2.Read();
                            {
                                cmd2Count += 1;
                                string colVal = dr2["TheSum"].ToString();
                                sumOfColumn = Utils.ConvertStringToDecimal(colVal);
                                cmd3.Parameters["@" + columnToUpdate].Value = sumOfColumn.ToString();

                                //LogHelper.WriteLine(sEndOfQtrDate + "," + SponsorFirmCode + "," + SmaProductTypeCode + "," + MorningstarClassId + "," + columnToSum + "," + sumOfColumn.ToString());
                                try
                                {
                                    cmd3.ExecuteNonQuery();
                                }
                                catch (SqlException ex)
                                {
                                    LogHelper.WriteLine(logFuncName + ex.Message);
                                }
                                finally
                                {
                                    updateCount += 1;
                                }
                            }
                        }
                        dr2.Close();
                    }
                    dr.Close();
                }
            }
            catch (SqlException ex)
            {
                LogHelper.WriteLine(logFuncName + " " + sEndOfQtrDate + " " + columnToSum + " " + ex.Message);
            }
            finally
            {
                //LogHelper.WriteLine(logFuncName + "Rows Updated " + updateCount + " " + sEndOfQtrDate + " " + columnToSum);
                //LogHelper.WriteLine(logFuncName + " cmd1Count " + cmd1Count + " cmd2Count " + cmd2Count);
            }


        }
    }
}
