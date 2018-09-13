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
