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

        public void CalculateProductTypeMetrics(string sStartDate, string sEndDate)
        {
            

            string sCurrentDate = sStartDate;
            bool moreDates = true;

            do
            {
                CalculateProductTypeMetricsForDate(sCurrentDate, "AssetsD");
                CalculateProductTypeMetricsForDate(sCurrentDate, "GrossFlowsD");
                CalculateProductTypeMetricsForDate(sCurrentDate, "RedemptionsD");
                CalculateProductTypeMetricsForDate(sCurrentDate, "FinalNetFlowsD");

                if (sCurrentDate.Equals(sEndDate))
                    moreDates = false;
                else
                    sCurrentDate = Utils.CalculateNextEndOfQtrDate(sCurrentDate);
            }
            while (moreDates.Equals(true));

        }

        public void CalculateProductTypeMetricsForDate(string sEndOfQtrDate, string columnToSum)
        {

            /*
              
SELECT distinct 
      [DoverSponsorFirmId]
      ,[SmaProductTypeCode]
      ,[MorningstarClassId]
  FROM [DoverSma].[dbo].[SmaOfferings]
  where DoverSponsorFirmId not in ('', 'tbd')
  order by DoverSponsorFirmId, SmaProductTypeCode, MorningstarClassId
             
            */

            SqlCommand cmd = null;
            SqlCommand cmd2 = null;
            int cmd1Count = 0;
            int cmd2Count = 0;

            string logFuncName = "CalculateProductTypeMetricsForDate: ";
            decimal sumOfColumn = 0m;

            int updateCount = 0;

            try
            {
                cmd = new SqlCommand
                {
                    Connection = mSqlConn1,
                    CommandText = @"
                        SELECT distinct 
                            [DoverSponsorFirmId]
                            ,[SmaProductTypeCode]
                            ,[MorningstarClassId]
                        FROM [DoverSma].[dbo].[SmaOfferings]
                        where DoverSponsorFirmId not in ('', 'tbd') and MorningstarClassId not in  ( '0', '', 'tbd')
                        order by DoverSponsorFirmId, SmaProductTypeCode, MorningstarClassId
                        "
                };

                cmd2 = new SqlCommand
                {
                    Connection = mSqlConn2,
                    CommandText = "select sum(" + columnToSum + ") as TheSum from SmaFlows where SmaOfferingId in " +
                                  "(select SmaOfferingId from SmaOfferings " +
                                  "where DoverSponsorFirmId = @DoverSponsorFirmId and SmaProductTypeCode = @SmaProductTypeCode " +
                                  "and MorningstarClassId = @MorningstarClassId) " +
                                  "and FlowDate = @FlowDate"
                };

                cmd2.Parameters.Add("@DoverSponsorFirmId", SqlDbType.VarChar);
                cmd2.Parameters.Add("@SmaProductTypeCode", SqlDbType.VarChar);
                cmd2.Parameters.Add("@MorningstarClassId", SqlDbType.VarChar);
                cmd2.Parameters.Add("@FlowDate", SqlDbType.Date);
                //cmd2.Parameters.Add("@" + columnToSum, SqlDbType.VarChar);

                SqlDataReader dr = null;
                SqlDataReader dr2 = null;

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        cmd1Count += 1;
                        string DoverSponsorFirmId = dr["DoverSponsorFirmId"].ToString();
                        string SmaProductTypeCode = dr["SmaProductTypeCode"].ToString();
                        string MorningstarClassId = dr["MorningstarClassId"].ToString();

                        cmd2.Parameters["@DoverSponsorFirmId"].Value = DoverSponsorFirmId;
                        cmd2.Parameters["@SmaProductTypeCode"].Value = SmaProductTypeCode;
                        cmd2.Parameters["@MorningstarClassId"].Value = MorningstarClassId;
                        cmd2.Parameters["@FlowDate"].Value = sEndOfQtrDate;

                        dr2 = cmd2.ExecuteReader();
                        if (dr2.HasRows)
                        {
                            dr2.Read();
                            {
                                cmd2Count += 1;
                                string colVal = dr2["TheSum"].ToString();
                                sumOfColumn = Utils.ConvertStringToDecimal(colVal);
                                LogHelper.WriteLine(sEndOfQtrDate + "," + DoverSponsorFirmId + "," + SmaProductTypeCode + "," + MorningstarClassId + "," + columnToSum + "," + sumOfColumn.ToString());
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
