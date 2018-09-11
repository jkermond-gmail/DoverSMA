﻿using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

using LumenWorks.Framework.IO.Csv;

using DoverUtilities;

namespace DoverSmaEngine
{
    public class FileProcessor
    {
        private string[] mEndOfQuarterDates = { "", "03/31/", "06/30/", "09/30/", "12/31/" };
        private string[] mReturnTypes = { "Quarterly", "1 year", "3 year" };
        private string[] mReturnTypeCol = { "", "      1-Year", "         3-Year" };

        // assets, gross flows, redemptions, net flows, derived flows
        private enum FlowTypes { a, g, r, n, d };
        private enum ManagerTypes { Legg, Prin, Alli, Dela, GWNK };
        
        string[] flowTypes = Enum.GetNames(typeof(FlowTypes));

        private SqlConnection mSqlConn1 = null;
        private SqlConnection mSqlConn2 = null;
        private SqlConnection mSqlConn3 = null;
        private string mConnectionString = "";

        private string mFilepath = @"C:\A_Development\visual studio 2017\Projects\DoverSMA\DoverAMFs";

        // (OAFF_) Offering and Flows Files 
        // (SARF_) Strategies and Returns Files

        private string mOAFF_alli = @"OAFF_alli.csv";
        private string mSARF_alli = @"SARF_alli.csv";
        private string mOAFF_anch = @"OAFF_anch.csv";
        private string mSARF_anch = @"SARF_anch.csv";
        private string mOAFF_bran = @"OAFF_bran.csv";
        private string mSARF_bran = @"SARF_bran.csv";
        private string mOAFF_cong = @"OAFF_cong.csv";
        private string mSARF_cong = @"SARF_cong.csv";
        private string mOAFF_dela = @"OAFF_dela.csv";
        private string mSARF_dela = @"SARF_dela.csv";
        private string mOAFF_fran = @"OAFF_fran.csv";
        private string mSARF_fran = @"SARF_fran.csv";
        private string mOAFF_gwnk = @"OAFF_gwnk.csv";
        private string mSARF_gwnk = @"SARF_gwnk.csv";
        private string mOAFF_inve = @"OAFF_inve.csv";
        private string mSARF_inve = @"SARF_inve.csv";
        private string mOAFF_laza = @"OAFF_laza.csv";
        private string mSARF_laza = @"SARF_laza.csv";
        private string mOAFF_legg = @"OAFF_legg.csv";
        private string mSARF_legg = @"SARF_legg.csv";
        private string mOAFF_lord = @"OAFF_lord.csv";
        private string mSARF_lord = @"SARF_lord.csv";
        private string mOAFF_nuve = @"OAFF_nuve.csv";
        private string mOAFF_prin = @"OAFF_prin.csv";
        private string mSARF_prin = @"SARF_prin.csv";
        private string mOAFF_rena = @"OAFF_rena.csv";
        private string mSARF_rena = @"SARF_rena.csv";
        //        private string mOAFF_ = @"OAFF_.csv";
        //        private string mSARF_ = @"SARF_.csv";

        #region Constructor
        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        public FileProcessor()
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

        #region CsvReader
        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private DataTable ReadCsvIntoTable(string filePath)
        {
            DataTable dt = new DataTable();
            try
            {
                bool hasHeaders = true;

                using (CsvReader csv = new CsvReader(new StreamReader(filePath), hasHeaders))
                {
                    csv.DefaultParseErrorAction = ParseErrorAction.RaiseEvent;
                    csv.ParseError += Csv_ParseError;
                    dt.Load(csv);
                }
            }
            catch
            {
            }
            finally
            {
            }
            return (dt);
        }

        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        void Csv_ParseError(object sender, ParseErrorEventArgs e)
        {
            //if (e.Error is MissingFieldCsvException)
            //logHelper.WriteLine("APUtilities: CsvParseError: " + DateTime.Now);
            //logHelper.WriteLine("         CurrentFieldIndex: " + e.Error.CurrentFieldIndex);
            //logHelper.WriteLine("           CurrentPosition: " + e.Error.CurrentPosition);
            //logHelper.WriteLine("        CurrentRecordIndex: " + e.Error.CurrentRecordIndex);
            //logHelper.WriteLine("                   Message: " + e.Error.Message);
            //logHelper.WriteLine("                   RawDate: " + e.Error.RawData);
            //logHelper.WriteLine("--------------------------: " );

            e.Action = ParseErrorAction.AdvanceToNextLine;
        }


        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private string ParseColumn(DataRow dr, string column)
        {
            string value = "";
            if (dr.Table.Columns.Contains(column))
            {
                if (!dr.IsNull(column))
                {
                    value = dr[column].ToString();
                    value = value.Trim();
                }
            }
            return (value);
        }

        #endregion CsvReader

        #region GetStuff_Commented
        //public string GetManagerFilepath()
        //{
        //    return (mFilepath);
        //}

        //public string GetOfferingsAndFlowFilename(string Manager)
        //{
        //    return (mOfferingsAndFlowFilename);
        //}

        //public string GetStrategiesAndReturnsFilename(string Manager)
        //{
        //    return (mStrategiesAndReturnsFilename);
        //}

        #endregion GetStuff_Commented

        #region ProcessManager

        public void ProcessManagerOfferings(string Manager)
        {
            switch (Manager)
            {
                case "Allianz":
                    ProcessOfferingsDataSingleRow(Path.Combine(mFilepath, mOAFF_alli));
                    break;
                case "Anchor":
                    ProcessOfferingsDataSingleRow(Path.Combine(mFilepath, mOAFF_anch));
                    break;
                case "Brandes":
                    ProcessOfferingsDataSingleRow(Path.Combine(mFilepath, mOAFF_bran));
                    break;
                case "Congress":
                    ProcessOfferingsDataSingleRow(Path.Combine(mFilepath, mOAFF_cong));
                    break;
                case "Delaware":
                    ProcessOfferingsDataSingleRow(Path.Combine(mFilepath, mOAFF_dela));
                    break;
                case "Franklin Templeton":
                    ProcessOfferingsDataSingleRow(Path.Combine(mFilepath, mOAFF_fran));
                    break;
                case "GW&K":
                    ProcessOfferingsDataSingleRow(Path.Combine(mFilepath, mOAFF_gwnk));
                    break;
                case "Invesco":
                    ProcessOfferingsDataSingleRow(Path.Combine(mFilepath, mOAFF_inve));
                    break;
                case "Lazard":
                    ProcessOfferingsDataSingleRow(Path.Combine(mFilepath, mOAFF_laza));
                    break;
                case "Legg":
                    ProcessOfferingsDataSingleRow(Path.Combine(mFilepath, mOAFF_legg));
                    break;
                case "Lord Abbett":
                    ProcessOfferingsDataSingleRow(Path.Combine(mFilepath, mOAFF_lord));
                    break;
                case "Nuveen":
                    ProcessOfferingsDataSingleRow(Path.Combine(mFilepath, mOAFF_nuve));
                    break;
                case "Principal":
                    ProcessOfferingsDataSingleRow(Path.Combine(mFilepath, mOAFF_prin));
                    break;
                case "Renaissance":
                    ProcessOfferingsDataSingleRow(Path.Combine(mFilepath, mOAFF_rena));
                    break;
            }
        }

        public void ProcessManagerFlows(string Manager)
        {
            switch (Manager)
            {
                /*
                case "Legg":
                    ProcessFlowsDataSingleRow(Path.Combine(mFilepath, mOAFF_legg));
                    break;
                case "Principal":
                    ProcessFlowsDataSingleRow(Path.Combine(mFilepath, mOAFF_prin));
                    break;
                case "Allianz":
                    ProcessFlowsDataSingleRow(Path.Combine(mFilepath, mOAFF_alli));
                    break;
                case "Delaware":
                    ProcessFlowsDataSingleRow(Path.Combine(mFilepath, mOAFF_dela));
                    break;
                case "GW&K":
                    ProcessFlowsDataSingleRow(Path.Combine(mFilepath, mOAFF_gwnk));
                    break;
                case "Brandes":
                    ProcessFlowsDataSingleRow(Path.Combine(mFilepath, mOAFF_bran));
                    break;
                case "Congress":
                    ProcessFlowsDataSingleRow(Path.Combine(mFilepath, mOAFF_cong));
                    break;
                case "Franklin Templeton":
                    ProcessFlowsDataSingleRow(Path.Combine(mFilepath, mOAFF_fran));
                    break;
                case "Invesco":
                    ProcessFlowsDataSingleRow(Path.Combine(mFilepath, mOAFF_inve));
                    break;
                case "Lazard":
                    ProcessFlowsDataSingleRow(Path.Combine(mFilepath, mOAFF_laza));
                    break;
                //case "Anchor":
                //    ProcessFlowsDataSingleRow(Path.Combine(mFilepath, mOAFF_anch));
                //    break;
                */
                case "Nuveen":
                    ProcessFlowsDataSingleRowNuveen(Path.Combine(mFilepath, mOAFF_nuve));
                    break;
                /*
                case "Renaissance":
                    ProcessFlowsDataSingleRow(Path.Combine(mFilepath, mOAFF_rena));
                    break;
                case "Lord Abbett":
                    ProcessFlowsDataSingleRow(Path.Combine(mFilepath, mOAFF_lord));
                    break;
                */

            }
        }

        public void ProcessManagerStrategies(string Manager)
        {
            switch (Manager)
            {
                case "Legg":
                    ProcessStrategiesData(Path.Combine(mFilepath, mSARF_legg)); 
                    break;
                case "Principal":
                    ProcessStrategiesData(Path.Combine(mFilepath, mSARF_prin)); 
                    break;
                case "Allianz":
                    ProcessStrategiesData(Path.Combine(mFilepath, mSARF_alli)); 
                    break;
                case "Delaware":
                    ProcessStrategiesData(Path.Combine(mFilepath, mSARF_dela));
                    break;
                case "GW&K":
                    ProcessStrategiesData(Path.Combine(mFilepath, mSARF_gwnk)); 
                    break;
                case "Brandes":
                    ProcessStrategiesData(Path.Combine(mFilepath, mSARF_bran));
                    break;
                case "Congress":
                    ProcessStrategiesData(Path.Combine(mFilepath, mSARF_cong));
                    break;
                case "Franklin Templeton":
                    ProcessStrategiesData(Path.Combine(mFilepath, mSARF_fran));
                    break;
                case "Invesco":
                    ProcessStrategiesData(Path.Combine(mFilepath, mSARF_inve));
                    break;
                case "Lazard":
                    ProcessStrategiesData(Path.Combine(mFilepath, mSARF_laza));
                    break;
                case "Anchor":
                    ProcessStrategiesData(Path.Combine(mFilepath, mSARF_anch));
                    break;
                case "Renaissance":
                    ProcessStrategiesData(Path.Combine(mFilepath, mSARF_rena));
                    break;
                case "Lord Abbett":
                    ProcessStrategiesData(Path.Combine(mFilepath, mSARF_lord));
                    break;
            }
        }

        public void ProcessManagerReturns(string Manager)
        {
            switch (Manager)
            {
                case "Legg":
                    ProcessReturnsData(Path.Combine(mFilepath, mSARF_legg));
                    break;
                case "Principal":
                    ProcessReturnsData(Path.Combine(mFilepath, mSARF_prin));
                    break;
                case "Allianz":
                    ProcessReturnsData(Path.Combine(mFilepath, mSARF_alli));
                    break;
                case "Delaware":
                    ProcessReturnsData(Path.Combine(mFilepath, mSARF_dela));
                    break;
                case "GW&K":
                    ProcessReturnsData(Path.Combine(mFilepath, mSARF_gwnk));
                    break;
                case "Brandes":
                    ProcessReturnsData(Path.Combine(mFilepath, mSARF_bran));
                    break;
                case "Congress":
                    ProcessReturnsData(Path.Combine(mFilepath, mSARF_cong));
                    break;
                case "Franklin Templeton":
                    ProcessReturnsData(Path.Combine(mFilepath, mSARF_fran));
                    break;
                case "Invesco":
                    ProcessReturnsData(Path.Combine(mFilepath, mSARF_inve));
                    break;
                case "Lazard":
                    ProcessReturnsData(Path.Combine(mFilepath, mSARF_laza));
                    break;
                case "Anchor":
                    ProcessReturnsData(Path.Combine(mFilepath, mSARF_anch));
                    break;
                case "Renaissance":
                    ProcessReturnsData(Path.Combine(mFilepath, mSARF_rena));
                    break;
                case "Lord Abbett":
                    ProcessReturnsData(Path.Combine(mFilepath, mSARF_lord));
                    break;


            }
        }
        #endregion ProcessManager

        #region OfferingsFlowsDataFunctions

        private string AssetManagerCode( string filePath)
        {
            int startPos = filePath.IndexOf(".csv") - 4;
            return(filePath.Substring(startPos, 4));
        }

        private void DeleteOfferingsData(string assetManagerCode)
        {
            SqlCommand cmd = null;
            string logFuncName = "DeleteOfferingsData: ";
            string sqlDelete = "delete from SmaOfferings ";
            string sqlWhere  = "where AssetManagerCode = '" + assetManagerCode + "'";

            try
            {
                cmd = new SqlCommand
                {
                    Connection = mSqlConn1,
                    CommandText = sqlDelete + sqlWhere
                };
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                LogHelper.WriteLine(logFuncName + " " + assetManagerCode + " " + ex.Message );
            }
            finally
            {
                LogHelper.WriteLine(logFuncName + " " + assetManagerCode);
            }
        }

        private void DeleteFlowsData(string assetManagerCode)
        {
            SqlCommand cmd = null;
            string logFuncName = "DeleteFlowsData: ";
            string sqlDelete = "delete from SmaFlows ";
            string sqlWhere = "where AssetManagerCode = '" + assetManagerCode + "'";

            try
            {
                cmd = new SqlCommand
                {
                    Connection = mSqlConn1,
                    CommandText = sqlDelete + sqlWhere
                };
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                LogHelper.WriteLine(logFuncName + " " + assetManagerCode + " " + ex.Message);
            }
            finally
            {
                LogHelper.WriteLine(logFuncName +  " " + assetManagerCode);
            }
        }

        private void CountOfferingsData(string assetManagerCode)
        {
            SqlCommand cmd = null;
            string logFuncName = "CountOfferingsData: ";
            string sqlSelect = "select count(*) from SmaOfferings ";
            string sqlWhere = "where AssetManagerCode = '" + assetManagerCode + "'";
            int iCount = 0;
            try
            {
                cmd = new SqlCommand
                {
                    Connection = mSqlConn1,
                    CommandText = sqlSelect + sqlWhere
                };
                iCount = (int)cmd.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                LogHelper.WriteLine(logFuncName + " " + assetManagerCode + " " + ex.Message);
            }
            finally
            {
                LogHelper.WriteLine(logFuncName + "Rows Added " + iCount + " " + assetManagerCode);
            }
        }

        private void CountFlowsData(string assetManagerCode)
        {
            SqlCommand cmd = null;
            string logFuncName = "CountFlowsData: ";
            string sqlSelect = "select count(*)  from SmaFlows ";
            string sqlWhere = "where AssetManagerCode = '" + assetManagerCode + "'";
            int iCount = 0;

            try
            {
                cmd = new SqlCommand
                {
                    Connection = mSqlConn1,
                    CommandText = sqlSelect + sqlWhere
                };
                iCount = (int)cmd.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                LogHelper.WriteLine(logFuncName + " " + assetManagerCode + " " + ex.Message);
            }
            finally
            {
                LogHelper.WriteLine(logFuncName + "Rows Added " + iCount + " " + assetManagerCode);
            }
        }
        #endregion OfferingsFlowsDataFunctions

        #region StringToDecimalFunctions


        public void CopyFlowsVarcharDataToDecimal(string colName)
        {
            SqlCommand cmd = null;
            SqlCommand cmd2 = null;
            string logFuncName = "CountFlowsData: ";
            
            int updateCount = 0;

            try
            {
                cmd = new SqlCommand
                {
                    Connection = mSqlConn1,
                    CommandText = "select SmaOfferingId, AssetManagerCode, FlowDate, " + colName + ", " + colName + "D from SmaFlows where isnumeric(" + colName + ")=1"
                    //  and SmaFlowId = '234526' 
                };

                cmd2 = new SqlCommand
                {
                    Connection = mSqlConn2,
                    CommandText = "update SmaFlows set " + colName + "D = @" + colName + "D where SmaOfferingId = @SmaOfferingId and AssetManagerCode = @AssetManagerCode and FlowDate = @FlowDate"
                };

                cmd2.Parameters.Add("@SmaOfferingId", SqlDbType.Int);
                cmd2.Parameters.Add("@AssetManagerCode", SqlDbType.VarChar);
                cmd2.Parameters.Add("@FlowDate", SqlDbType.Date);
                cmd2.Parameters.Add("@" + colName + "D", SqlDbType.Decimal);

                SqlDataReader dr = null;

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Int32 SmaOfferingId = Convert.ToInt32(dr["SmaOfferingId"].ToString());
                        string AssetManagerCode = dr["AssetManagerCode"].ToString();
                        string FlowDate = dr["FlowDate"].ToString();
                        string colVal = dr[colName].ToString();
                        //decimal defaultValue = 0m;
                        //decimal colValD = Utils.ConvertStringToDecimalOld(colVal, defaultValue);
                        decimal colValD = Utils.ConvertStringToDecimal(colVal);
                        cmd2.Parameters["@SmaOfferingId"].Value = SmaOfferingId;
                        cmd2.Parameters["@AssetManagerCode"].Value = AssetManagerCode;
                        cmd2.Parameters["@FlowDate"].Value = FlowDate;
                        cmd2.Parameters["@" + colName + "D"].Value = colValD;
                        // I don't think these are needed below:
                        //cmd2.Parameters["@AssetsD"].Precision = 14;
                        //cmd2.Parameters["@AssetsD"].Scale = 8;

                        cmd2.ExecuteNonQuery();
                        updateCount += 1;
                    }
                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                LogHelper.WriteLine(logFuncName + " " + colName + " " + ex.Message);
            }
            finally
            {
                LogHelper.WriteLine(logFuncName + "Rows Updated " + updateCount + " " + colName);
            }
        }

        public void CopyReturnsVarcharDataToDecimal()
        {
            SqlCommand cmd = null;
            SqlCommand cmd2 = null;
            string logFuncName = "CopyReturnsVarcharDataToDecimal: ";

            int updateCount = 0;

            try
            {
                cmd = new SqlCommand
                {
                    Connection = mSqlConn1,
                    CommandText =  @"
                        SELECT [SmaReturnId]
                        ,[SmaStrategyId]
                        ,[AssetManagerCode]
                        ,[ReturnType]
                        ,[ReturnDate]
                        ,[ReturnValue]
                        FROM[DoverSma].[dbo].[SmaReturns]
                    "
                };

                cmd2 = new SqlCommand
                {
                    Connection = mSqlConn2,
                    CommandText = @"
                        UPDATE SmaReturns
                        SET ReturnValueD = @ReturnValueD
                        WHERE 
                        SmaStrategyId = @SmaStrategyId and
                        AssetManagerCode = @AssetManagerCode and
                        ReturnType = @ReturnType and
                        ReturnDate = @ReturnDate
                        "
                };

                cmd2.Parameters.Add("@SmaStrategyId", SqlDbType.Int);
                cmd2.Parameters.Add("@AssetManagerCode", SqlDbType.VarChar);
                cmd2.Parameters.Add("@ReturnType", SqlDbType.VarChar);
                cmd2.Parameters.Add("@ReturnDate", SqlDbType.Date);
                cmd2.Parameters.Add("@ReturnValueD", SqlDbType.Decimal);

                SqlDataReader dr = null;

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Int32 SmaStrategyId = Convert.ToInt32(dr["SmaStrategyId"].ToString());
                        string AssetManagerCode = dr["AssetManagerCode"].ToString();
                        string ReturnType = dr["ReturnType"].ToString();
                        string ReturnDate = dr["ReturnDate"].ToString();
                        string ReturnValue = dr["ReturnValue"].ToString();
                        decimal ReturnValueD = Utils.ConvertStringToDecimal(ReturnValue);
                        cmd2.Parameters["@SmaStrategyId"].Value = SmaStrategyId;
                        cmd2.Parameters["@AssetManagerCode"].Value = AssetManagerCode;
                        cmd2.Parameters["@ReturnType"].Value = ReturnType;
                        cmd2.Parameters["@ReturnDate"].Value = ReturnDate;
                        cmd2.Parameters["@ReturnValueD"].Value = ReturnValueD;

                        cmd2.ExecuteNonQuery();
                        updateCount += 1;
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
                LogHelper.WriteLine(logFuncName + "Rows Updated " + updateCount );
            }
        }


        #endregion StringToDecimalFunctions

        #region CalculateNetFlowsFunctions

        protected decimal ConvertNullDecinalToZero(object value)
        {
            decimal zero = 0;

            if (value.Equals(DBNull.Value))
                return (zero);
            else
                return((decimal)value);
        }


        private decimal GetPreviousQtrsAssets(int SmaOfferingId, string AssetManagerCode, string sFlowDate)
        {
            string sPrevFlowDate = Utils.CalculatePrevEndOfQtrDate(sFlowDate);

            SqlCommand cmd = null;
            string logFuncName = "GetPreviousQtrsAssets: ";
            decimal assetsD = 0;

            try
            {
                cmd = new SqlCommand
                {
                    // note that this routine uses mSqlConn3 as 1 and 2 are in use by the calling function
                    Connection = mSqlConn3,
                    CommandText = @"
                        SELECT [AssetsD]
                        FROM [DoverSma].[dbo].[SmaFlows]
                        WHERE SmaOfferingId = @SmaOfferingId and AssetManagerCode = @AssetManagerCode and FlowDate = @FlowDate
                    "
                };

                cmd.Parameters.Add("@SmaOfferingId", SqlDbType.Int);
                cmd.Parameters.Add("@AssetManagerCode", SqlDbType.VarChar);
                cmd.Parameters.Add("@FlowDate", SqlDbType.Date);
                cmd.Parameters["@SmaOfferingId"].Value = SmaOfferingId;
                cmd.Parameters["@AssetManagerCode"].Value = AssetManagerCode;
                cmd.Parameters["@FlowDate"].Value = sPrevFlowDate;

                SqlDataReader dr = null;

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    {
                        var tmp = dr["AssetsD"];
                        if (dr["AssetsD"].Equals(DBNull.Value))
                            assetsD = 0;
                        else
                            assetsD = Convert.ToDecimal(dr["AssetsD"].ToString());
                    }
                }
                else 
                {
                    //LogHelper.WriteLine(logFuncName + "No Prev Qtr AssetsD row" );
                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                LogHelper.WriteLine(logFuncName + " " + ex.Message);
            }
            finally
            {
                //LogHelper.WriteLine(logFuncName + " AssetsD " + assetsD);
            }
            return (assetsD);
        }

        private decimal GetReturn(string AssetManagerCode, string MorningstarStrategyId, string sReturnDate)
        {
            SqlCommand cmd = null;
            SqlCommand cmd2 = null;
            string logFuncName = "GetReturn: ";
            decimal ReturnD = 0;

            try
            {
                cmd = new SqlCommand
                {
                    // note that this routine uses mSqlConn3 as 1 and 2 are in use by the calling function
                    Connection = mSqlConn3,
                    CommandText = @"
                        SELECT [SmaStrategyId]
                        FROM [DoverSma].[dbo].[SmaStrategies]
                        WHERE AssetManagerCode = @AssetManagerCode and MorningstarStrategyId = @MorningstarStrategyId
                    "
                };

                cmd.Parameters.Add("@AssetManagerCode", SqlDbType.VarChar);
                cmd.Parameters.Add("@MorningstarStrategyId", SqlDbType.VarChar);
                cmd.Parameters["@AssetManagerCode"].Value = AssetManagerCode;
                cmd.Parameters["@MorningstarStrategyId"].Value = MorningstarStrategyId;

                SqlDataReader dr = null;
                Int32 SmaStrategyId = 0;

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    {
                        SmaStrategyId = Convert.ToInt32(dr["SmaStrategyId"].ToString());
                    }
                }
                else
                {
                    //LogHelper.WriteLine(logFuncName + "No StrategyId");

                }
                dr.Close();

                if (SmaStrategyId > 0)
                {
                    try
                    {
                        cmd2 = new SqlCommand
                        {
                            // note that this routine uses mSqlConn3 as 1 and 2 are in use by the calling function
                            Connection = mSqlConn3,
                            CommandText = @"
                                SELECT[ReturnValueD]
                                FROM [DoverSma].[dbo].[SmaReturns]
                                WHERE 
                                    SmaStrategyId = @SmaStrategyId and 
                                    AssetManagerCode = @AssetManagerCode and
                                    ReturnType = @ReturnType and
                                    ReturnDate = @ReturnDate
                            "
                        };

                        cmd2.Parameters.Add("@SmaStrategyId", SqlDbType.Int);
                        cmd2.Parameters.Add("@AssetManagerCode", SqlDbType.VarChar);
                        cmd2.Parameters.Add("@ReturnType", SqlDbType.VarChar);
                        cmd2.Parameters.Add("@ReturnDate", SqlDbType.Date);

                        cmd2.Parameters["@SmaStrategyId"].Value = SmaStrategyId;
                        cmd2.Parameters["@AssetManagerCode"].Value = AssetManagerCode;
                        cmd2.Parameters["@ReturnType"].Value = "Quarterly";
                        cmd2.Parameters["@ReturnDate"].Value = sReturnDate;

                        dr = null;

                        dr = cmd2.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.Read();
                            {
                                var tmp = dr["ReturnValueD"];
                                if (dr["ReturnValueD"].Equals(DBNull.Value))
                                    ReturnD = 0;
                                else
                                    ReturnD = Convert.ToDecimal(dr["ReturnValueD"].ToString());
                            }
                        }
                        else
                        {
                            //LogHelper.WriteLine(logFuncName + "No Returns");
                        }
                        dr.Close();
                    }
                    catch (SqlException ex)
                    {
                        LogHelper.WriteLine(logFuncName + " " + ex.Message);
                    }
                    finally
                    {
                        //LogHelper.WriteLine(logFuncName + " ReturnD " + ReturnD);
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
                //LogHelper.WriteLine(logFuncName + " ReturnD " + ReturnD);
            }
            return (ReturnD);
        }

        public void CalculateNetFlows()
        {
            SqlCommand cmd = null;
            SqlCommand cmd2 = null;
            string logFuncName = "CalculateNetFlows: ";

            int updateCount = 0;
            int rowCount = 0;
            int assetsCount = 0;
            int grossFlowsCount = 0;
            int redemptionsCount = 0;
            int netFlowsCount = 0;
            int derivedFlowsCount = 0;
            int allValuesZero = 0;
            int calcFinalNetCount = 0;
            int assetsZeroCount = 0;
            int prevAssetsZeroCount = 0;
            int assetsNotZeroCount = 0;
            int prevAssetsNotZeroCount = 0;
            int bothAssetsZeroCount = 0;
            int bothAssetsNotZeroCount = 0;
            int returnValueZeroCount = 0;
            int returnValueNotZeroCount = 0;

            try
            {
                cmd = new SqlCommand
                {
                    Connection = mSqlConn1,
                    CommandText = @"
                        SELECT [SmaOfferings].[SmaOfferingId]
                              ,[SmaOfferingKeyId]
	                          ,[SmaOfferings].[AssetManagerCode]
	                          ,[FlowDate]
                              ,[MorningstarStrategyID]
	                          ,[AssetsD]
	                          ,[GrossFlowsD]
	                          ,[RedemptionsD]
	                          ,[NetFlowsD]
	                          ,[DerivedFlowsD]
                        FROM [DoverSma].[dbo].[SmaOfferings]
                        inner join SmaFlows on SmaOfferings.SmaOfferingId = SmaFlows.SmaOfferingId
                        order by SmaOfferings.AssetManagerCode, SmaOfferings.SmaOfferingId, SmaFlows.SmaFlowId, SmaFlows.FlowDate
                    "
                };

                cmd2 = new SqlCommand
                {
                    Connection = mSqlConn2,
                    CommandText = @"
                        update SmaFlows set DoverDerivedFlowsD = @DoverDerivedFlowsD, FinalNetFlowsD = @FinalNetFlowsD, PerformanceImpactD = @PerformanceImpactD
                        where SmaOfferingId = @SmaOfferingId and AssetManagerCode = @AssetManagerCode and FlowDate = @FlowDate"
                };

                cmd2.Parameters.Add("@SmaOfferingId", SqlDbType.Int);
                cmd2.Parameters.Add("@AssetManagerCode", SqlDbType.VarChar);
                cmd2.Parameters.Add("@FlowDate", SqlDbType.Date);
                cmd2.Parameters.Add("@DoverDerivedFlowsD", SqlDbType.Decimal);
                cmd2.Parameters.Add("@FinalNetFlowsD", SqlDbType.Decimal);
                cmd2.Parameters.Add("@PerformanceImpactD", SqlDbType.Decimal);

                SqlDataReader dr = null;

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    bool update;
                    while (dr.Read())
                    {
                        update = false;
                        rowCount += 1;
                        Int32 SmaOfferingId = Convert.ToInt32(dr["SmaOfferingId"].ToString());
                        string AssetManagerCode = dr["AssetManagerCode"].ToString();
                        string FlowDate = dr["FlowDate"].ToString();
                        string MorningstarStrategyId = dr["MorningstarStrategyId"].ToString();
                        decimal assetsD = ConvertNullDecinalToZero(dr["AssetsD"]);
                        decimal grossFlowsD = ConvertNullDecinalToZero(dr["GrossFlowsD"]);
                        decimal redemptionsD = ConvertNullDecinalToZero(dr["RedemptionsD"]);
                        decimal netFlowsD  = ConvertNullDecinalToZero(dr["NetFlowsD"]);
                        decimal derivedFlowsD = ConvertNullDecinalToZero(dr["DerivedFlowsD"]);
                        
                        cmd2.Parameters["@SmaOfferingId"].Value = SmaOfferingId;
                        cmd2.Parameters["@AssetManagerCode"].Value = AssetManagerCode;
                        cmd2.Parameters["@FlowDate"].Value = FlowDate;
                        cmd2.Parameters["@DoverDerivedFlowsD"].Value = DBNull.Value;
                        cmd2.Parameters["@FinalNetFlowsD"].Value = DBNull.Value;
                        cmd2.Parameters["@PerformanceImpactD"].Value = DBNull.Value;

                        if (assetsD.Equals(0) == false)
                            assetsCount += 1;
                        if (grossFlowsD.Equals(0) == false)
                            grossFlowsCount += 1;
                        if (redemptionsD.Equals(0) == false)
                            redemptionsCount += 1;
                        if (assetsD.Equals(0) && grossFlowsD.Equals(0) && redemptionsD.Equals(0) && netFlowsD.Equals(0) && derivedFlowsD.Equals(0))
                            allValuesZero += 1;

                        if (derivedFlowsD.Equals(0) == false)
                        {
                            cmd2.Parameters["@FinalNetFlowsD"].Value = derivedFlowsD;
                            derivedFlowsCount += 1;
                            //LogHelper.WriteLine(logFuncName + "using Derived Flows," + derivedFlowsD.ToString());
                            update = true;
                        }
                        else if (netFlowsD.Equals(0) == false)
                        { 
                            cmd2.Parameters["@FinalNetFlowsD"].Value = netFlowsD;
                            netFlowsCount += 1;
                            //LogHelper.WriteLine(logFuncName + "using     Net Flows," + netFlowsD.ToString());
                            update = true;
                        }
                        else
                        {
                            calcFinalNetCount += 1;
                            decimal prevQtrAssets = GetPreviousQtrsAssets(SmaOfferingId, AssetManagerCode, FlowDate);
                            decimal performanceImpact = 0M;
                            decimal doverDerivedFlows = 0M;

                            if (assetsD.Equals(0M) == true)
                                assetsZeroCount += 1;
                            else
                                assetsNotZeroCount += 1;
                            if (prevQtrAssets.Equals(0M) == true)
                                prevAssetsZeroCount += 1;
                            else
                                prevAssetsNotZeroCount += 1;
                            if ((assetsD.Equals(0M) == true) && (prevQtrAssets.Equals(0M) == true))
                                bothAssetsZeroCount += 1;
                            else
                                bothAssetsNotZeroCount += 1;

                            if ((assetsD.Equals(0M) == false) && (prevQtrAssets.Equals(0M) == false))
                            {
                                decimal returnValue = GetReturn(AssetManagerCode, MorningstarStrategyId, FlowDate);
                                if (returnValue.Equals(0M) == true)
                                {
                                    returnValueZeroCount += 1;
                                }
                                else
                                {
                                    returnValueNotZeroCount += 1;
                                }

                                if (returnValue.Equals(0M) == false)
                                {
                                    performanceImpact = ((assetsD + prevQtrAssets) / 2) * (returnValue / 100);
                                    doverDerivedFlows = (assetsD - prevQtrAssets) - performanceImpact;
                                    update = true;
                                    cmd2.Parameters["@DoverDerivedFlowsD"].Value = doverDerivedFlows;
                                    cmd2.Parameters["@PerformanceImpactD"].Value = performanceImpact;
                                    cmd2.Parameters["@FinalNetFlowsD"].Value = doverDerivedFlows;
                                }
                            }
                        }
                        if (update.Equals(true))
                        {
                            cmd2.ExecuteNonQuery();
                            updateCount += 1;
                        }
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
                LogHelper.WriteLine(logFuncName + "Rows processed " + rowCount);
                LogHelper.WriteLine(logFuncName + "allValuesZero " + allValuesZero);
                LogHelper.WriteLine(logFuncName + "SomeValuesNotZero " + (rowCount - allValuesZero));
                LogHelper.WriteLine(logFuncName + "assetsCount " + assetsCount);
                LogHelper.WriteLine(logFuncName + "grossFlowsCount " + grossFlowsCount);
                LogHelper.WriteLine(logFuncName + "redemptionsCount " + redemptionsCount);
                LogHelper.WriteLine(logFuncName + "netFlowsCount " + netFlowsCount);
                LogHelper.WriteLine(logFuncName + "derivedFlowsCount " + derivedFlowsCount);
                LogHelper.WriteLine(logFuncName + "calcFinalNetCount " + calcFinalNetCount);
                LogHelper.WriteLine(logFuncName + "assetsZeroCount " + assetsZeroCount);
                LogHelper.WriteLine(logFuncName + "prevAssetsZeroCount " + prevAssetsZeroCount);
                LogHelper.WriteLine(logFuncName + "assetsNotZeroCount " + assetsNotZeroCount);
                LogHelper.WriteLine(logFuncName + "prevAssetsNotZeroCount " + prevAssetsNotZeroCount);
                LogHelper.WriteLine(logFuncName + "bothAssetsZeroCount " + bothAssetsZeroCount);
                LogHelper.WriteLine(logFuncName + "bothAssetsNotZeroCount " + bothAssetsNotZeroCount);
                LogHelper.WriteLine(logFuncName + "returnValueZeroCount " + returnValueZeroCount);
                LogHelper.WriteLine(logFuncName + "returnValueNotZeroCount " + returnValueNotZeroCount);
                LogHelper.WriteLine(logFuncName + "FinalnetFlows Rows Updated " + updateCount);
            }
        }



        #endregion CalculateNetFlowsFunctions

        #region ProcessOfferings

        public void ProcessOfferingsDataSingleRow(string filePath)
        {
            SqlCommand cmd = null;
            string sqlSelect = "";
            string sqlWhere = "";
            string valueParsed = "";
            string colName = "";
            string tableName = " SmaOfferings ";
            string logFuncName = "ProcessOfferingsDataSingleRow: ";
            string assetManagerCode = AssetManagerCode(filePath);
            DeleteOfferingsData(assetManagerCode);
            DeleteFlowsData(assetManagerCode);

            int currentRowCount = 1; // Since csv file has a header set row to 1, data starts in row 2
            int addCount = 0;
            int blankLineCount = 0;
            int duplicateCount = 0;
            int smaOfferingKeyId = 0;

            LogHelper.WriteLine(logFuncName + filePath + " started");

            DataTable dt = ReadCsvIntoTable(filePath);

            cmd = new SqlCommand
            {
                Connection = mSqlConn1,
                CommandText = sqlSelect + sqlWhere
            };

            foreach (DataRow row in dt.Rows)
            {
                bool blankLine = true;
                currentRowCount += 1;
                colName = "AssetManagerCode";
                if (currentRowCount == 2) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd.Parameters["@" + colName].Value = assetManagerCode;

                colName = "SponsorFirm";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                //string sponsorFirm2 = ParseColumn(row, "SponsorFirm2");
                //if (sponsorFirm2.Length > 0)
                //    valueParsed += " - " + sponsorFirm2;
                cmd.Parameters["@" + colName].Value = valueParsed;
                if (valueParsed.Length > 0)
                    blankLine = false;

                colName = "AdvisoryPlatform";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd.Parameters["@" + colName].Value = valueParsed;
                if (valueParsed.Length > 0)
                    blankLine = false;

                colName = "AdvisoryPlatformCode";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd.Parameters["@" + colName].Value = valueParsed;
                if (valueParsed.Length > 0)
                    blankLine = false;

                colName = "SmaStrategy";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd.Parameters["@" + colName].Value = valueParsed;
                if (valueParsed.Length > 0)
                    blankLine = false;

                colName = "SmaProductType";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd.Parameters["@" + colName].Value = valueParsed;
                if (valueParsed.Length > 0)
                    blankLine = false;

                colName = "SmaProductTypeCode";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd.Parameters["@" + colName].Value = valueParsed;
                if (valueParsed.Length > 0)
                    blankLine = false;

                colName = "TampRIAPlatform";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd.Parameters["@" + colName].Value = valueParsed;
                if (valueParsed.Length > 0)
                    blankLine = false;

                colName = "ManagerClass";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd.Parameters["@" + colName].Value = valueParsed;
                if (valueParsed.Length > 0)
                    blankLine = false;

                if (blankLine == false)
                {
                    try
                    {
                        sqlSelect = "select count(*) from" + tableName;
                        sqlWhere = @"where AssetManagerCode = @AssetManagerCode and SponsorFirm = @SponsorFirm  and AdvisoryPlatform = @AdvisoryPlatform  and AdvisoryPlatformCode = @AdvisoryPlatformCode and SmaStrategy = @SmaStrategy and 
                        SmaProductType = @SmaProductType and SmaProductTypeCode = @SmaProductTypeCode and TampRIAPlatform = @TampRIAPlatform and ManagerClass = @ManagerClass";

                        cmd.CommandText = sqlSelect + sqlWhere;
                        int iCount = (int)cmd.ExecuteScalar();

                        smaOfferingKeyId = iCount + 1;
                        colName = "SmaOfferingKeyId";
                        if (addCount == 0) cmd.Parameters.Add("@" + colName, SqlDbType.Int);
                        cmd.Parameters["@" + colName].Value = smaOfferingKeyId;
                        //cmd.Parameters["@" + colName].Value = Convert.ToInt32(smaOfferingKeyId.ToString()); 

                        colName = "SponsorFirmCode";
                        valueParsed = ParseColumn(row, colName);
                        if (addCount == 0) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                        cmd.Parameters["@" + colName].Value = valueParsed;

                        colName = "SponsorFirmId";
                        valueParsed = ParseColumn(row, colName);
                        if (addCount == 0) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                        cmd.Parameters["@" + colName].Value = valueParsed;

                        colName = "DoverSponsorFirmId";
                        valueParsed = ParseColumn(row, colName);
                        if (addCount == 0) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                        cmd.Parameters["@" + colName].Value = valueParsed;

                        colName = "MorningstarStrategyId";
                        valueParsed = ParseColumn(row, colName);
                        if (addCount == 0) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                        cmd.Parameters["@" + colName].Value = valueParsed;

                        colName = "MorningstarClass";
                        valueParsed = ParseColumn(row, colName);
                        if (addCount == 0) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                        cmd.Parameters["@" + colName].Value = valueParsed;

                        colName = "MorningstarClassId";
                        valueParsed = ParseColumn(row, colName);
                        if (addCount == 0) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                        cmd.Parameters["@" + colName].Value = valueParsed;

                        colName = "TotalAccounts";
                        valueParsed = ParseColumn(row, colName);
                        if (addCount == 0) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                        cmd.Parameters["@" + colName].Value = valueParsed;

                        colName = "CsvFileRow";
                        if (addCount == 0) cmd.Parameters.Add("@" + colName, SqlDbType.Int);
                        cmd.Parameters["@" + colName].Value = currentRowCount;

                        if ((assetManagerCode.Equals("nuve") == false) || ((assetManagerCode.Equals("nuve") == true) && (smaOfferingKeyId == 1)))
                        {
                            cmd.CommandText =
                            "insert into " + tableName +
                                "(AssetManagerCode, SponsorFirm, AdvisoryPlatform, AdvisoryPlatformCode, SmaStrategy, SmaProductType, SmaProductTypeCode, TampRIAPlatform, ManagerClass, SmaOfferingKeyId," +
                                " SponsorFirmCode, SponsorFirmId, DoverSponsorFirmId, MorningstarStrategyId, MorningstarClass, MorningstarClassId, TotalAccounts, CsvFileRow) " +
                            "Values (@AssetManagerCode, @SponsorFirm, @AdvisoryPlatform, @AdvisoryPlatformCode, @SmaStrategy, @SmaProductType, @SmaProductTypeCode, @TampRIAPlatform, @ManagerClass, @SmaOfferingKeyId," +
                                    " @SponsorFirmCode, @SponsorFirmId, @DoverSponsorFirmId, @MorningstarStrategyId, @MorningstarClass, @MorningstarClassId, @TotalAccounts, @CsvFileRow)";
                            cmd.ExecuteNonQuery();
                            addCount += 1;
                        }
                        else if ((assetManagerCode.Equals("nuve") == true) && (smaOfferingKeyId > 1))
                        {
                            LogHelper.WriteLine(logFuncName + " skipping nuveen duplicate line number: " + currentRowCount);
                        }
                        if (assetManagerCode.Equals("nuve") == false)
                        {
                            sqlSelect = "select SmaOfferingId from" + tableName;
                            sqlWhere = @"where AssetManagerCode = @AssetManagerCode and SponsorFirm = @SponsorFirm  and AdvisoryPlatform = @AdvisoryPlatform  and SmaStrategy = @SmaStrategy and 
                                        SmaProductType = @SmaProductType and TampRIAPlatform = @TampRIAPlatform and ManagerClass = @ManagerClass and SmaOfferingKeyId = @SmaOfferingKeyId";
                            SqlDataReader dr = null;
                            cmd.CommandText = sqlSelect + sqlWhere;
                            dr = cmd.ExecuteReader();
                            if (dr.HasRows)
                            {
                                if (dr.Read())
                                {
                                    Int32 smaOfferingId = Convert.ToInt32(dr["SmaOfferingId"].ToString());
                                    ProcessFlowsDataForOffering(smaOfferingId, assetManagerCode, row, dt, currentRowCount);
                                }
                            }
                            dr.Close();
                        }
                    }
                    catch (SqlException ex)
                    {
                        LogHelper.WriteLine(logFuncName + ex.Message + " line number: " + currentRowCount);
                    }
                    finally
                    {
                    }
                }
                else
                {
                    blankLineCount += 1;
                }
            }
            LogHelper.WriteLine(logFuncName + "Rows Processed " + currentRowCount);
            LogHelper.WriteLine(logFuncName + "Rows Added " + addCount);
            LogHelper.WriteLine(logFuncName + "BlankLines " + blankLineCount);
            LogHelper.WriteLine(logFuncName + "Duplicates " + duplicateCount);
            LogHelper.WriteLine(logFuncName + filePath + " finished");

            CountOfferingsData(assetManagerCode);
            CountFlowsData(assetManagerCode);
        }

        public void ProcessOfferingsDataUpdates()
        {
            SqlCommand cmd = null;
            string sqlSelect = "";
            string sqlWhere = "";
            string valueParsed = "";
            string colName = "";
            string logFuncName = "ProcessOfferingsDataUpdates: ";
            string filePath = Path.Combine(mFilepath, "DoverDBUpdates20180827.csv");

            int currentRowCount = 1; // Since csv file has a header set row to 1, data starts in row 2
            int addCount = 0;
            int updateCount = 0;

            int blankLineCount = 0;
            int duplicateCount = 0;

            LogHelper.WriteLine(logFuncName + filePath + " started");

            DataTable dt = ReadCsvIntoTable(filePath);

            cmd = new SqlCommand
            {
                Connection = mSqlConn1,
                CommandText = sqlSelect + sqlWhere
            };

            foreach (DataRow row in dt.Rows)
            {
                bool blankLine = true;
                currentRowCount += 1;
                colName = "SmaOfferingId";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2)
                {
                    cmd.Parameters.Add("@" + colName, SqlDbType.Int);
                    cmd.Parameters.Add("@SponsorFirmCode", SqlDbType.VarChar);
                    cmd.Parameters.Add("@DoverSponsorFirmId", SqlDbType.VarChar);
                }
                cmd.Parameters["@" + colName].Value = valueParsed;
                if (valueParsed.Length > 0)
                    blankLine = false;

                colName = "SponsorFirmCode";
                cmd.Parameters["@" + colName].Value = ParseColumn(row, colName);
                colName = "DoverSponsorFirmId";
                cmd.Parameters["@" + colName].Value = ParseColumn(row, colName);

                if (blankLine == false)
                {
                    try
                    {
                        sqlSelect = "select count(*) from SmaOfferings ";
                        sqlWhere = @"where SmaOfferingId = @SmaOfferingId";

                        cmd.CommandText = sqlSelect + sqlWhere;
                        int iCount = (int)cmd.ExecuteScalar();

                        if (iCount > 0)
                        {
                            sqlSelect = "update SmaOfferings set SponsorFirmCode = @SponsorFirmCode, DoverSponsorFirmId = @DoverSponsorFirmId ";
                            sqlWhere = @"where SmaOfferingId = @SmaOfferingId";
                            cmd.CommandText = sqlSelect + sqlWhere;
                            cmd.ExecuteNonQuery();
                            updateCount += 1;
                        }

                    }
                    catch (SqlException ex)
                    {
                        LogHelper.WriteLine(logFuncName + ex.Message + " line number: " + currentRowCount);
                    }
                    finally
                    {
                    }
                }
                else
                {
                    blankLineCount += 1;
                }
            }
            LogHelper.WriteLine(logFuncName + "Rows Processed " + currentRowCount);
            LogHelper.WriteLine(logFuncName + "Rows Added " + addCount);
            LogHelper.WriteLine(logFuncName + "BlankLines " + blankLineCount);
            LogHelper.WriteLine(logFuncName + "Duplicates " + duplicateCount);
            LogHelper.WriteLine(logFuncName + filePath + " finished");
        }

        public void ProcessOfferingsDataUpdatesByColumn( string updateColName, SqlDbType sqlDbType )
        {
            SqlCommand cmd = null;
            string sqlSelect = "";
            string sqlWhere = "";
            string valueParsed = "";
            string colName = "";
            string logFuncName = "ProcessOfferingsDataUpdatesByColumn: ";
            string filePath = Path.Combine(mFilepath, "DoverDBUpdates20180911.csv");

            int currentRowCount = 1; // Since csv file has a header set row to 1, data starts in row 2
            int updateCount = 0;
            int blankLineCount = 0;

            LogHelper.WriteLine(logFuncName + filePath + " started for column: " + updateColName);

            DataTable dt = ReadCsvIntoTable(filePath);

            cmd = new SqlCommand
            {
                Connection = mSqlConn1,
                CommandText = sqlSelect + sqlWhere
            };

            foreach (DataRow row in dt.Rows)
            {
                bool blankLine = true;
                currentRowCount += 1;
                colName = "SmaOfferingId";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2)
                {
                    cmd.Parameters.Add("@" + colName, SqlDbType.Int);
                    cmd.Parameters.Add("@" + updateColName, sqlDbType);
                }
                cmd.Parameters["@" + colName].Value = valueParsed;
                if (valueParsed.Length > 0)
                    blankLine = false;

                string updateValue = ParseColumn(row, updateColName + "Update");

                if( (blankLine == false) && (updateValue.Length > 0) )
                {
                    try
                    {
                        cmd.Parameters["@" + updateColName].Value = updateValue;

                        sqlSelect = "select count(*) from SmaOfferings ";
                        sqlWhere = @"where SmaOfferingId = @SmaOfferingId";

                        cmd.CommandText = sqlSelect + sqlWhere;
                        int iCount = (int)cmd.ExecuteScalar();

                        if (iCount > 0)
                        {
                            sqlSelect = "update SmaOfferings set " + updateColName + " = @" + updateColName + " ";
                            sqlWhere = "where SmaOfferingId = @SmaOfferingId";
                            cmd.CommandText = sqlSelect + sqlWhere;
                            cmd.ExecuteNonQuery();
                            updateCount += 1;
                        }

                    }
                    catch (SqlException ex)
                    {
                        LogHelper.WriteLine(logFuncName + ex.Message + " line number: " + currentRowCount);
                    }
                    finally
                    {
                    }
                }
                else
                {
                    blankLineCount += 1;
                }
            }
            LogHelper.WriteLine(logFuncName + "Rows Processed " + currentRowCount);
            LogHelper.WriteLine(logFuncName + "Rows Updateded " + updateCount);
            LogHelper.WriteLine(logFuncName + "BlankLines " + blankLineCount);
            LogHelper.WriteLine(logFuncName + filePath + " finished");
        }



        // This routine was wriiten to handle Principals multi row format
        public void ProcessOfferingsDataMultiRow(string filePath)
        {
            SqlCommand cmd = null;
            string sqlSelect = "";
            string sqlWhere = "";
            string valueParsed = "";
            string colName = "";
            string tableName = " SmaOfferings ";
            string logFuncName = "ProcessOfferingsDataMultiRow: ";
            // key fields
            string assetManagerCode = AssetManagerCode(filePath);
            string sponsorFirm = "";
            string smaStrategy = "";
            string smaProductType = "";
            string managerClass = "";
            // non-key fields
            string morningstarStrategyId = "";
            string morningstarClass = "";

            int currentRowCount = 1; // Since csv file has a header set row to 1, data starts in row 2
            int addCount = 0;


            LogHelper.WriteLine(logFuncName + filePath + " started");

            DataTable dt = ReadCsvIntoTable(filePath);

            // Principal does not provide the following key fields so they are removed
            //  and AdvisoryPlatform = @AdvisoryPlatform  
            //  and TampRIAPlatform = @TampRIAPlatform
            sqlSelect = "select count(*) from" + tableName;
            sqlWhere = @"where AssetManagerCode = @AssetManagerCode and SponsorFirm = @SponsorFirm  and SmaStrategy = @SmaStrategy and 
                        SmaProductType = @SmaProductType and ManagerClass = @ManagerClass";

            cmd = new SqlCommand
            {
                Connection = mSqlConn1,
                CommandText = sqlSelect + sqlWhere
            };

            foreach (DataRow row in dt.Rows)
            {
                currentRowCount += 1;

                //if (currentRowCount == 855)
                //    currentRowCount = currentRowCount;

                colName = "AssetManagerCode";
                if (currentRowCount == 2) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd.Parameters["@" + colName].Value = assetManagerCode;

                colName = "SponsorFirm";
                valueParsed = ParseColumn(row, colName);
                if ((valueParsed.Length > 0) && (valueParsed.EndsWith("Total") == false))
                {
                    sponsorFirm = valueParsed;
                    // sponsor firm changed so reinitialize the remaining key fields
                    smaStrategy = "";
                    smaProductType = "";
                    managerClass = "";
                }
                if (currentRowCount == 2) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd.Parameters["@" + colName].Value = sponsorFirm;

                colName = "SmaStrategy";
                valueParsed = ParseColumn(row, colName);
                if (valueParsed.Length > 0)
                    smaStrategy = valueParsed;
                if (currentRowCount == 2) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd.Parameters["@" + colName].Value = smaStrategy;

                colName = "SmaProductType";
                valueParsed = ParseColumn(row, colName);
                if (valueParsed.Length > 0)
                    smaProductType = valueParsed;
                if (currentRowCount == 2) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd.Parameters["@" + colName].Value = smaProductType;

                colName = "ManagerClass";
                valueParsed = ParseColumn(row, colName);
                if ((valueParsed.Length > 0) && (valueParsed.EndsWith("Total") == false))
                    managerClass = valueParsed;
                if (currentRowCount == 2) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd.Parameters["@" + colName].Value = managerClass;

                try
                {
                    cmd.CommandText = sqlSelect + sqlWhere;
                    int iCount = (int)cmd.ExecuteScalar();
                    if (iCount == 0)
                    {
                        colName = "MorningstarStrategyId";
                        valueParsed = ParseColumn(row, colName);
                        if (valueParsed.Length > 0)
                            morningstarStrategyId = valueParsed;
                        if (addCount == 0) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                        cmd.Parameters["@" + colName].Value = morningstarStrategyId;

                        colName = "MorningstarClass";
                        valueParsed = ParseColumn(row, colName);
                        if (valueParsed.Length > 0)
                            morningstarClass = valueParsed;
                        if (addCount == 0) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                        cmd.Parameters["@" + colName].Value = morningstarClass;

                        //colName = "TotalAccounts";
                        //valueParsed = ParseColumn(row, colName);
                        //if (addCount == 0) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                        //cmd.Parameters["@" + colName].Value = valueParsed;

                        colName = "CsvFileRow";
                        if (addCount == 0) cmd.Parameters.Add("@" + colName, SqlDbType.Int);
                        cmd.Parameters["@" + colName].Value = currentRowCount;

                        // In addition to the key fields above, Principal does not provide the following non key fields so they are removed
                        //    @SponsorFirmId, @MorningstarClassId, @TotalAccounts,  Note @TotalAccounts is provided but is not on the Offering level but on the Sponsor Firm level
                        cmd.CommandText =
                        "insert into " + tableName +
                            "(AssetManagerCode, SponsorFirm, SmaStrategy, SmaProductType," +
                            " MorningstarStrategyId, MorningstarClass, ManagerClass, CsvFileRow) " +
                        "Values (@AssetManagerCode, @SponsorFirm, @SmaStrategy, @SmaProductType, " +
                                "@MorningstarStrategyId, @MorningstarClass, @ManagerClass, @CsvFileRow)";
                        cmd.ExecuteNonQuery();
                        addCount += 1;
                    }
                    else if (iCount > 0)
                    {
                        LogHelper.WriteLine("----- Skipping Row " + (currentRowCount) + "------");
                        LogHelper.WriteLine("assetManager   = " + assetManagerCode);
                        LogHelper.WriteLine("sponsorFirm    = " + sponsorFirm);
                        LogHelper.WriteLine("smaStrategy    = " + smaStrategy);
                        LogHelper.WriteLine("smaProductType = " + smaProductType);
                        LogHelper.WriteLine("managerClass   = " + managerClass);
                        LogHelper.WriteLine("-----------");
                    }
                }
                catch (SqlException ex)
                {
                    LogHelper.WriteLine(logFuncName + ex.Message + " line number: " + currentRowCount);
                }
                finally
                {
                }
            }
            LogHelper.WriteLine(logFuncName + "Rows Processed " + currentRowCount);
            LogHelper.WriteLine(logFuncName + "Rows Added " + addCount);
            LogHelper.WriteLine(logFuncName + filePath + " finished");
        }

        #endregion ProcessOfferings

        #region ProcessFlows

        private void ProcessFlowsDataForOffering( int smaOfferingId, string assetManagerCode, DataRow row, DataTable dt, int currentRowCount)
        {
            SqlCommand cmd = null;
            string valueParsed = "";
            string colName = "";
            string logFuncName = "ProcessFlowsDataForOffering: ";

            int addCount = 0;


            LogHelper.WriteLine(logFuncName + " started");

            cmd = new SqlCommand
            {
                Connection = mSqlConn2
            };

            if (addCount == 0)
            {
                cmd.Parameters.Add("@AssetManagerCode", SqlDbType.VarChar);
                cmd.Parameters["@AssetManagerCode"].Value = assetManagerCode;
                cmd.Parameters.Add("@SmaOfferingId", SqlDbType.Int);
                cmd.Parameters.Add("@FlowDate", SqlDbType.Date);
            }
            cmd.Parameters["@SmaOfferingId"].Value = smaOfferingId;

            for (int year = 2016; year <= 2018; year++)
            {
                string sYear = year.ToString();
                for (int quarter = 1; quarter <= 4; quarter++)
                {
                    string sQuarter = quarter.ToString();
                    string flowDate = mEndOfQuarterDates[quarter].ToString() + sYear;
                    cmd.Parameters["@FlowDate"].Value = flowDate;

                    foreach (string flowType in flowTypes)
                    {
                        colName = sQuarter + "Q" + " " + sYear + " " + flowType;

                        if (dt.Columns.Contains(colName))
                            {
                            valueParsed = ParseColumn(row, colName);
                            bool insert = false;
                            switch (flowType)
                            {
                                case "a":
                                    colName = "Assets";
                                    break;
                                case "g":
                                    colName = "GrossFlows";
                                    break;
                                case "r":
                                    colName = "Redemptions";
                                    break;
                                case "n":
                                    colName = "NetFlows";
                                    break;
                                case "d":
                                    colName = "DerivedFlows";
                                    insert = true;
                                    break;
                            }
                            if (addCount == 0)
                                cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                            cmd.Parameters["@" + colName].Value = valueParsed;

                            if (insert)
                            {
                                cmd.CommandText =
                                    "insert into SmaFlows " +
                                        "(AssetManagerCode, SmaOfferingId, FlowDate, Assets, GrossFlows, Redemptions, NetFlows, DerivedFlows) " +
                                    "Values (@AssetManagerCode, @SmaOfferingId, @FlowDate, @Assets, @GrossFlows, @Redemptions, @NetFlows, @DerivedFlows)";
                                try
                                {
                                    cmd.ExecuteNonQuery();
                                }
                                catch (SqlException ex)
                                {
                                    LogHelper.WriteLine(logFuncName + ex.Message + " line number: " + currentRowCount);
                                }
                                finally
                                {
                                    addCount += 1;
                                    cmd.Parameters["@Assets"].Value = "";
                                    cmd.Parameters["@GrossFlows"].Value = "";
                                    cmd.Parameters["@Redemptions"].Value = "";
                                    cmd.Parameters["@NetFlows"].Value = "";
                                }
                            }
                        }
                        else
                        {
                            LogHelper.WriteLine("Column Name: " + colName + " not found");
                        }
                    }
                }
            }
        }


        public void ProcessFlowsDataSingleRow(string filePath)
        {
            //SqlCommand cmd1 = null;
            //SqlCommand cmd2 = null;
            //string sqlSelect = "";
            //string sqlWhere = "";
            //string valueParsed = "";
            //string colName = "";
            string logFuncName = "ProcessFlowsDataSingleRow: ";
            string assetManagerCode = AssetManagerCode(filePath);

            //int currentRowCount = 1; // Since csv file has a header set row to 1, data starts in row 2
            //int addCount = 0;
            //int blankLineCount = 0;


            //LogHelper.WriteLine(logFuncName + filePath + " started");
            LogHelper.WriteLine(logFuncName + filePath + " no longer supported");
            return;
        }

        //    DataTable dt = ReadCsvIntoTable(filePath);

        //    sqlSelect = @"select SmaOfferingId from SmaOfferings ";
        //    sqlWhere = @"where AssetManagerCode = @AssetManagerCode and SponsorFirm = @SponsorFirm  and AdvisoryPlatform = @AdvisoryPlatform  and SmaStrategy = @SmaStrategy and " +
        //                "SmaProductType = @SmaProductType and TampRIAPlatform = @TampRIAPlatform";

        //    cmd1 = new SqlCommand
        //    {
        //        Connection = mSqlConn1,
        //        CommandText = sqlSelect + sqlWhere
        //    };

        //    cmd2 = new SqlCommand
        //    {
        //        Connection = mSqlConn2
        //    };


        //    foreach (DataRow row in dt.Rows)
        //    {
        //        bool blankLine = true;
        //        currentRowCount += 1;
        //        colName = "AssetManagerCode";
        //        if (currentRowCount == 2) cmd1.Parameters.Add("@" + colName, SqlDbType.VarChar);
        //        cmd1.Parameters["@" + colName].Value = assetManagerCode;

        //        colName = "SponsorFirm";
        //        valueParsed = ParseColumn(row, colName);
        //        if (currentRowCount == 2) cmd1.Parameters.Add("@" + colName, SqlDbType.VarChar);
        //        cmd1.Parameters["@" + colName].Value = valueParsed;
        //        if (valueParsed.Length > 0)
        //            blankLine = false;

        //        colName = "AdvisoryPlatform";
        //        valueParsed = ParseColumn(row, colName);
        //        if (currentRowCount == 2) cmd1.Parameters.Add("@" + colName, SqlDbType.VarChar);
        //        cmd1.Parameters["@" + colName].Value = valueParsed;
        //        if (valueParsed.Length > 0)
        //            blankLine = false;

        //        colName = "SmaStrategy";
        //        valueParsed = ParseColumn(row, colName);
        //        if (currentRowCount == 2) cmd1.Parameters.Add("@" + colName, SqlDbType.VarChar);
        //        cmd1.Parameters["@" + colName].Value = valueParsed;
        //        if (valueParsed.Length > 0)
        //            blankLine = false;

        //        colName = "SmaProductType";
        //        valueParsed = ParseColumn(row, colName);
        //        if (currentRowCount == 2) cmd1.Parameters.Add("@" + colName, SqlDbType.VarChar);
        //        cmd1.Parameters["@" + colName].Value = valueParsed;
        //        if (valueParsed.Length > 0)
        //            blankLine = false;

        //        colName = "TampRIAPlatform";
        //        valueParsed = ParseColumn(row, colName);
        //        if (currentRowCount == 2) cmd1.Parameters.Add("@" + colName, SqlDbType.VarChar);
        //        cmd1.Parameters["@" + colName].Value = valueParsed;
        //        if (valueParsed.Length > 0)
        //            blankLine = false;
        //        if (blankLine == false)
        //        {
        //            try
        //            {
        //                SqlDataReader dr = null;
        //                cmd1.CommandText = sqlSelect + sqlWhere;

        //                dr = cmd1.ExecuteReader();
        //                if (dr.HasRows)
        //                {
        //                    if (dr.Read())
        //                    {
        //                        Int32 SmaOfferingId = Convert.ToInt32(dr["SmaOfferingId"].ToString());
        //                        if (addCount == 0)
        //                        {
        //                            cmd2.Parameters.Add("@AssetManagerCode", SqlDbType.VarChar);
        //                            cmd2.Parameters["@AssetManagerCode"].Value = assetManagerCode;
        //                            cmd2.Parameters.Add("@SmaOfferingId", SqlDbType.Int);
        //                        }
        //                        cmd2.Parameters["@SmaOfferingId"].Value = SmaOfferingId;

        //                        for (int year = 2016; year <= 2018; year++)
        //                        {
        //                            string sYear = year.ToString();
        //                            for (int quarter = 1; quarter <= 4; quarter++)
        //                            {
        //                                string sQuarter = quarter.ToString();
        //                                string flowDate = mEndOfQuarterDates[quarter].ToString() + sYear;

        //                                if (addCount == 0)
        //                                    cmd2.Parameters.Add("@FlowDate", SqlDbType.Date);
        //                                cmd2.Parameters["@FlowDate"].Value = flowDate;

        //                                foreach (string flowType in flowTypes)
        //                                {
        //                                    colName = sQuarter + "Q" + " " + sYear + " " + flowType;

        //                                    if (dt.Columns.Contains(colName))
        //                                    {
        //                                        valueParsed = ParseColumn(row, colName);
        //                                        bool insert = false;
        //                                        switch (flowType)
        //                                        {
        //                                            case "a":
        //                                                colName = "Assets";
        //                                                break;
        //                                            case "g":
        //                                                colName = "GrossFlows";
        //                                                break;
        //                                            case "r":
        //                                                colName = "Redemptions";
        //                                                break;
        //                                            case "n":
        //                                                colName = "NetFlows";
        //                                                break;
        //                                            case "d":
        //                                                colName = "DerivedFlows";
        //                                                insert = true;
        //                                                break;
        //                                        }
        //                                        if (addCount == 0)
        //                                            cmd2.Parameters.Add("@" + colName, SqlDbType.VarChar);
        //                                        cmd2.Parameters["@" + colName].Value = valueParsed;

        //                                        if (insert)
        //                                        {
        //                                            cmd2.CommandText =
        //                                                "insert into SmaFlows " +
        //                                                 "(AssetManagerCode, SmaOfferingId, FlowDate, Assets, GrossFlows, Redemptions, NetFlows, DerivedFlows) " +
        //                                                "Values (@AssetManagerCode, @SmaOfferingId, @FlowDate, @Assets, @GrossFlows, @Redemptions, @NetFlows, @DerivedFlows)";
        //                                            try
        //                                            {
        //                                                cmd2.ExecuteNonQuery();
        //                                            }
        //                                            catch (SqlException ex)
        //                                            {
        //                                                LogHelper.WriteLine(logFuncName + ex.Message + " line number: " + currentRowCount);
        //                                            }
        //                                            finally
        //                                            {
        //                                                addCount += 1;
        //                                                cmd2.Parameters["@Assets"].Value = "";
        //                                                cmd2.Parameters["@GrossFlows"].Value = "";
        //                                                cmd2.Parameters["@Redemptions"].Value = "";
        //                                                cmd2.Parameters["@NetFlows"].Value = "";
        //                                            }
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        LogHelper.WriteLine("Column Name: " + colName + " not found");
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    LogHelper.WriteLine("----- Offering not found Skipping Row " + (currentRowCount) + "------");

        //                    foreach (DataColumn column in dt.Columns)
        //                    {
        //                        if (column.ColumnName.Equals("AssetManager") || column.ColumnName.Equals("SponsorFirm") || column.ColumnName.Equals("AdvisoryPlatform")
        //                            || column.ColumnName.Equals("SmaStrategy") || column.ColumnName.Equals("SmaProductType") || column.ColumnName.Equals("TampRIAPlatform"))
        //                            LogHelper.WriteLine(column.ColumnName.ToString() + "|" + row[column].ToString());
        //                    }
        //                    LogHelper.WriteLine("-----------");
        //                }
        //                dr.Close();
        //            }
        //            catch (SqlException ex)
        //            {
        //                LogHelper.WriteLine(logFuncName + ex.Message + " line number: " + currentRowCount);
        //            }
        //            finally
        //            {
        //            }
        //        }
        //        else
        //        {
        //            blankLineCount += 1;
        //        }

        //    }
        //    LogHelper.WriteLine(logFuncName + "Rows Processed " + currentRowCount);
        //    LogHelper.WriteLine(logFuncName + "Rows Added " + addCount);
        //    LogHelper.WriteLine(logFuncName + "BlankLines " + blankLineCount);
        //    LogHelper.WriteLine(logFuncName + filePath + " finished");
        //}

        public void ProcessFlowsDataSingleRowNuveen(string filePath)
        {
            SqlCommand cmd1 = null;
            SqlCommand cmd2 = null;
            SqlCommand cmd3 = null;
            string sqlSelect = "";
            string sqlWhere = "";
            //string sqlSelect2 = "";
            //string sqlWhere2 = "";

            string valueParsed = "";
            string colName = "";
            string logFuncName = "ProcessFlowsDataSingleRow: ";
            string assetManagerCode = AssetManagerCode(filePath);

            int currentRowCount = 1; // Since csv file has a header set row to 1, data starts in row 2
            int addCount = 0;
            int updateCount = 0;

            CultureInfo provider = CultureInfo.InvariantCulture;

            LogHelper.WriteLine(logFuncName + filePath + " started");

            DataTable dt = ReadCsvIntoTable(filePath);

            sqlSelect = @"select SmaOfferingId from SmaOfferings ";
            sqlWhere = @"where AssetManagerCode = @AssetManagerCode and SponsorFirm = @SponsorFirm /* and AdvisoryPlatform = @AdvisoryPlatform */  and SmaStrategy = @SmaStrategy and " +
                        "SmaProductType = @SmaProductType /*and TampRIAPlatform = @TampRIAPlatform */";

            cmd1 = new SqlCommand
            {
                Connection = mSqlConn1,
                CommandText = sqlSelect + sqlWhere
            };

            cmd2 = new SqlCommand
            {
                Connection = mSqlConn2
            };

            cmd3 = new SqlCommand
            {
                Connection = mSqlConn3
            };


            foreach (DataRow row in dt.Rows)
            {
                currentRowCount += 1;
                colName = "AssetManagerCode";
                if (currentRowCount == 2) cmd1.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd1.Parameters["@" + colName].Value = assetManagerCode;

                colName = "SponsorFirm";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd1.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd1.Parameters["@" + colName].Value = valueParsed;

                /*
                colName = "AdvisoryPlatform";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd1.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd1.Parameters["@" + colName].Value = valueParsed;
                */
                colName = "SmaStrategy";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd1.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd1.Parameters["@" + colName].Value = valueParsed;

                colName = "SmaProductType";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd1.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd1.Parameters["@" + colName].Value = valueParsed;

                /*
                colName = "TampRIAPlatform";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd1.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd1.Parameters["@" + colName].Value = valueParsed;
                */
                try
                {
                    SqlDataReader dr = null;
                    cmd1.CommandText = sqlSelect + sqlWhere;

                    dr = cmd1.ExecuteReader();
                    if (dr.HasRows)
                    {
                        if (dr.Read())
                        {
                            Int32 SmaOfferingId = Convert.ToInt32(dr["SmaOfferingId"].ToString());
                            if (addCount == 0)
                            {
                                cmd2.Parameters.Add("@AssetManagerCode", SqlDbType.VarChar);
                                cmd2.Parameters["@AssetManagerCode"].Value = assetManagerCode;
                                cmd2.Parameters.Add("@SmaOfferingId", SqlDbType.Int);
                                cmd2.Parameters.Add("@FlowDate", SqlDbType.Date);

                                cmd3.Parameters.Add("@AssetManagerCode", SqlDbType.VarChar);
                                cmd3.Parameters["@AssetManagerCode"].Value = assetManagerCode;
                                cmd3.Parameters.Add("@SmaOfferingId", SqlDbType.Int);
                                cmd3.Parameters.Add("@FlowDate", SqlDbType.Date);

                                cmd2.Parameters.Add("@Assets", SqlDbType.VarChar);
                                cmd2.Parameters.Add("@GrossFlows", SqlDbType.VarChar);
                                cmd2.Parameters.Add("@Redemptions", SqlDbType.VarChar);
                                
                            }
                            cmd2.Parameters["@SmaOfferingId"].Value = SmaOfferingId;
                            cmd3.Parameters["@SmaOfferingId"].Value = SmaOfferingId;

                            colName = "FlowDate";
                            valueParsed = ParseColumn(row, colName);
                            string sFlowDate = "";

                            if(valueParsed.StartsWith("Q1"))
                            {
                                sFlowDate = "03/31/" + valueParsed.Substring(valueParsed.Length - 2);
                            }
                            else if (valueParsed.StartsWith("Q2"))
                            {
                                sFlowDate = "06/30/" + valueParsed.Substring(valueParsed.Length - 2);
                            }
                            else if (valueParsed.StartsWith("Q3"))
                            {
                                sFlowDate = "09/30/" + valueParsed.Substring(valueParsed.Length - 2);
                            }
                            else if (valueParsed.StartsWith("Q4"))
                            {
                                sFlowDate = "12/31/" + valueParsed.Substring(valueParsed.Length - 2);
                            }

                            if (sFlowDate.Length == 8)
                            {
                                try
                                {
                                    string format = "MM/dd/yy";
                                    DateTime result = DateTime.ParseExact(sFlowDate, format, provider);
                                    Console.WriteLine("{0} converts to {1}.", sFlowDate, result.ToString());
                                    cmd2.Parameters["@FlowDate"].Value = result.ToString();
                                    cmd3.Parameters["@FlowDate"].Value = result.ToString();
                                }
                                catch (FormatException)
                                {
                                    Console.WriteLine("{0} is not in the correct format.", valueParsed);
                                }
                            }

                            // Check if a row has been added
                            cmd3.CommandText = @"
                                    select * from SmaFlows where SmaOfferingId = @SmaOfferingId and AssetManagerCode = @AssetManagerCode and FlowDate = @FlowDate
                                    ";
                            bool insert = false;
                            bool update = false;
                            string assets = "";
                            string redemptions = "";
                            string grossFlows = "";
                            string originalAssets = "";
                            string originalGrossFlows = "";
                            string originalRedemptions = "";


                            SqlDataReader dr3 = null;

                            dr3 = cmd3.ExecuteReader();
                            if (dr3.HasRows)
                            {
                                update = true;
                                dr3.Read();
                                originalAssets = dr3["Assets"].ToString();
                                originalGrossFlows = dr3["GrossFlows"].ToString();
                                originalRedemptions = dr3["Redemptions"].ToString();
                            }
                            else
                            {
                                insert = true;
                                cmd2.Parameters["@Assets"].Value = "";
                                cmd2.Parameters["@GrossFlows"].Value = "";
                                cmd2.Parameters["@Redemptions"].Value = "";
                            }
                            dr3.Close();

                            colName = "TradeClass";
                            string tradeClass = ParseColumn(row, colName);

                            string sqlColName = "";

                            colName = "Assets";
                            valueParsed = ParseColumn(row, colName);

                            if (tradeClass.Equals("AUM"))
                            {
                                sqlColName = "Assets";
                                if (originalAssets.Length > 0)
                                {
                                    assets = Convert.ToString(Convert.ToDouble(originalAssets) + Convert.ToDouble(valueParsed));
                                }
                                else
                                {
                                    assets = valueParsed;
                                }
                                redemptions = originalRedemptions;
                                grossFlows = originalGrossFlows;
                            }
                            else if (tradeClass.Equals("R"))
                            {
                                sqlColName = "Redemptions";
                                if (originalRedemptions.Length > 0)
                                {
                                    redemptions = Convert.ToString(Convert.ToDouble(originalRedemptions) + Convert.ToDouble(valueParsed));
                                }
                                else
                                {
                                    redemptions = valueParsed;
                                }
                                assets = originalAssets;
                                grossFlows = originalGrossFlows;
                            }
                            else if (tradeClass.Equals("S"))
                            {
                                sqlColName = "GrossFlows";
                                if (originalGrossFlows.Length > 0)
                                {
                                    grossFlows = Convert.ToString(Convert.ToDouble(originalGrossFlows) + Convert.ToDouble(valueParsed));
                                }
                                else
                                {
                                    grossFlows = valueParsed;
                                }
                                assets = originalAssets;
                                redemptions = originalRedemptions;
                            }

                            if (insert.Equals(true))
                            {
                                cmd2.Parameters["@" + sqlColName].Value = valueParsed;
                                cmd2.CommandText =
                                    "insert into SmaFlows " +
                                        "(AssetManagerCode, SmaOfferingId, FlowDate, Assets, GrossFlows, Redemptions) " +
                                    "Values (@AssetManagerCode, @SmaOfferingId, @FlowDate, @Assets, @GrossFlows, @Redemptions)";
                                try
                                {
                                    cmd2.ExecuteNonQuery();
                                }
                                catch (SqlException ex)
                                {
                                    LogHelper.WriteLine(logFuncName + ex.Message + " line number: " + currentRowCount);
                                }
                                finally
                                {
                                    addCount += 1;
                                }
                            }
                            else if (update.Equals(true))
                            {
                                cmd2.Parameters["@Assets"].Value = assets;
                                cmd2.Parameters["@GrossFlows"].Value = grossFlows;
                                cmd2.Parameters["@Redemptions"].Value = redemptions;
                                cmd2.CommandText = @"
                                        Update SmaFlows
                                        Set Assets = @Assets, GrossFlows = @GrossFlows, Redemptions = @Redemptions
                                        where SmaOfferingId = @SmaOfferingId and AssetManagerCode = @AssetManagerCode and FlowDate = @FlowDate
                                        ";
                                try
                                {
                                    cmd2.ExecuteNonQuery();
                                }
                                catch (SqlException ex)
                                {
                                    LogHelper.WriteLine(logFuncName + ex.Message + " line number: " + currentRowCount);
                                }
                                finally
                                {
                                    updateCount += 1;
                                }

                            }
                        }
                    }
                    else
                    {
                        LogHelper.WriteLine("----- Offering not found Skipping Row " + (currentRowCount) + "------");

                        foreach (DataColumn column in dt.Columns)
                        {
                            if (column.ColumnName.Equals("AssetManager") || column.ColumnName.Equals("SponsorFirm") || column.ColumnName.Equals("AdvisoryPlatform")
                                || column.ColumnName.Equals("SmaStrategy") || column.ColumnName.Equals("SmaProductType") || column.ColumnName.Equals("TampRIAPlatform"))
                                LogHelper.WriteLine(column.ColumnName.ToString() + "|" + row[column].ToString());
                        }
                        LogHelper.WriteLine("-----------");
                    }
                    dr.Close();
                }
                catch (SqlException ex)
                {
                    LogHelper.WriteLine(logFuncName + ex.Message + " line number: " + currentRowCount);
                }
                finally
                {
                }
            }
            LogHelper.WriteLine(logFuncName + "Rows Processed " + currentRowCount);
            LogHelper.WriteLine(logFuncName + "Rows Added " + addCount);
            LogHelper.WriteLine(logFuncName + filePath + " finished");
        }



        public void ProcessFlowsDataMultiRow(string filePath)
        {
            SqlCommand cmd1 = null;
            SqlCommand cmd2 = null;
            SqlCommand cmd3 = null;
            string sqlSelect = "";
            string sqlWhere = "";
            string valueParsed = "";
            string colName = "";
            string logFuncName = "ProcessFlowsDataMultiRow: ";
            // key fields
            string sponsorFirm = "";
            string smaStrategy = "";
            string smaProductType = "";
            string managerClass = "";
            string assetManagerCode = AssetManagerCode(filePath);

            int currentRowCount = 1; // Since csv file has a header set row to 1, data starts in row 2
            int addCount = 0;
            int updateCount = 0;

            LogHelper.WriteLine(logFuncName + filePath + " started");

            DataTable dt = ReadCsvIntoTable(filePath);

            sqlSelect = @"select SmaOfferingId from SmaOfferings ";
            sqlWhere = @"where AssetManagerCode = @AssetManagerCode and SponsorFirm = @SponsorFirm  and SmaStrategy = @SmaStrategy and 
                        SmaProductType = @SmaProductType and ManagerClass = @ManagerClass";

            cmd1 = new SqlCommand
            {
                Connection = mSqlConn1,
                CommandText = sqlSelect + sqlWhere
            };

            cmd2 = new SqlCommand
            {
                Connection = mSqlConn2
            };

            cmd3 = new SqlCommand
            {
                Connection = mSqlConn3
            };

            foreach (DataRow row in dt.Rows)
            {
                currentRowCount += 1;

                colName = "AssetManagerCode";
                if (currentRowCount == 2) cmd1.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd1.Parameters["@" + colName].Value = assetManagerCode;

                colName = "SponsorFirm";
                valueParsed = ParseColumn(row, colName);
                if ((valueParsed.Length > 0) && (valueParsed.EndsWith("Total") == false))
                {
                    sponsorFirm = valueParsed;
                    // sponsor firm changed so reinitialize the remaining key fields
                    smaStrategy = "";
                    smaProductType = "";
                    managerClass = "";
                }
                if (currentRowCount == 2) cmd1.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd1.Parameters["@" + colName].Value = sponsorFirm;

                colName = "SmaStrategy";
                valueParsed = ParseColumn(row, colName);
                if (valueParsed.Length > 0)
                    smaStrategy = valueParsed;
                if (currentRowCount == 2) cmd1.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd1.Parameters["@" + colName].Value = smaStrategy;

                colName = "SmaProductType";
                valueParsed = ParseColumn(row, colName);
                if (valueParsed.Length > 0)
                    smaProductType = valueParsed;
                if (currentRowCount == 2) cmd1.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd1.Parameters["@" + colName].Value = smaProductType;

                colName = "ManagerClass";
                valueParsed = ParseColumn(row, colName);
                if ((valueParsed.Length > 0) && (valueParsed.EndsWith("Total") == false))
                    managerClass = valueParsed;
                if (currentRowCount == 2) cmd1.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd1.Parameters["@" + colName].Value = managerClass;

                try
                {
                    SqlDataReader dr = null;
                    cmd1.CommandText = sqlSelect + sqlWhere;

                    dr = cmd1.ExecuteReader();
                    if (dr.HasRows)
                    {
                        if (dr.Read())
                        {
                            Int32 smaOfferingId = Convert.ToInt32(dr["SmaOfferingId"].ToString());
                            if (addCount == 0)
                            {
                                cmd2.Parameters.Add("@SmaOfferingId", SqlDbType.Int);
                                cmd2.Parameters.Add("@AssetManagerCode", SqlDbType.VarChar);
                            }
                            cmd2.Parameters["@SmaOfferingId"].Value = smaOfferingId;
                            cmd2.Parameters["@AssetManagerCode"].Value = assetManagerCode;

                            colName = "Smry Trns Typ Txt";
                            valueParsed = ParseColumn(row, colName);
                            if ((valueParsed.Equals("Gross Flows")) ||
                                (valueParsed.Equals("Redemptions")) ||
                                (valueParsed.Equals("Net Cash Flows")) ||
                                (valueParsed.Equals("Assets")))
                            {
                                string transTypeValueParsed = valueParsed;
                                for (int year = 2016; year <= 2018; year++)
                                {
                                    string sYear = year.ToString();
                                    int year2 = year - 2000;
                                    string sYearYY = year2.ToString();
                                    for (int quarter = 1; quarter <= 4; quarter++)
                                    {
                                        string sQuarter = quarter.ToString();
                                        string flowDate = mEndOfQuarterDates[quarter].ToString() + sYearYY;

                                        if (addCount == 0)
                                        {
                                            cmd2.Parameters.Add("@FlowDate", SqlDbType.Date);
                                            cmd2.Parameters.Add("@Assets", SqlDbType.VarChar);
                                            cmd2.Parameters.Add("@GrossFlows", SqlDbType.VarChar);
                                            cmd2.Parameters.Add("@Redemptions", SqlDbType.VarChar);
                                            cmd2.Parameters.Add("@NetFlows", SqlDbType.VarChar);
                                        }

                                        DateTime dt2 = DateTime.ParseExact(flowDate, "MM/dd/yy", CultureInfo.InvariantCulture);
                                        //flowDate = dt2.ToString("yyyy-MM-dd");
                                        //cmd2.Parameters["@FlowDate"].Value = flowDate;
                                        cmd2.Parameters["@FlowDate"].Value = dt2.ToString();

                                        cmd2.Parameters["@Assets"].Value = "";
                                        cmd2.Parameters["@GrossFlows"].Value = "";
                                        cmd2.Parameters["@Redemptions"].Value = "";
                                        cmd2.Parameters["@NetFlows"].Value = "";

                                        colName = sQuarter + "Q" + " " + sYear ;

                                        if (dt.Columns.Contains(colName))
                                        {
                                            valueParsed = ParseColumn(row, colName);
                                            string sqlUpdate = "";
                                            switch (transTypeValueParsed)
                                            {
                                                case "Assets":
                                                    colName = "Assets";
                                                    break;
                                                case "Gross Flows":
                                                    colName = "GrossFlows";
                                                    break;
                                                case "Redemptions":
                                                    colName = "Redemptions";
                                                    break;
                                                case "Net Cash Flows":
                                                    colName = "NetFlows";
                                                    break;
                                            }

                                            /*
                                             string sql2 = "UPDATE student SET moneyspent = moneyspent + @spent WHERE id=@id";
                                            SqlCommand myCommand2 = new SqlCommand(sql2, conn);
                                            myCommand2.Parameters.AddWithValue("@spent", 50 )
                                            myCommand2.Parameters.AddWithValue("@id", 1 ) 
                                             */
                                            sqlUpdate = "update SmaFlows set " + colName.ToString() + " = '" + valueParsed.ToString() + "' ";
                                            cmd2.Parameters["@" + colName].Value = valueParsed;

                                            string sqlSelect2 = "select count(*) from [DoverSma].[dbo].[SmaFlows] ";
                                            string sqlWhere2 =  "where SmaOfferingId = '" + smaOfferingId.ToString() + 
                                                                "' and FlowDate = '" + flowDate.ToString() + "'";
                                            cmd3.CommandText = sqlSelect2 + sqlWhere2;
                                            int iCount2 = (int)cmd3.ExecuteScalar();
                                            if (iCount2 == 0)
                                            {
                                                cmd2.CommandText =
                                                "insert into SmaFlows " +
                                                        "(AssetManagerCode, SmaOfferingId, FlowDate, Assets, GrossFlows, Redemptions, NetFlows) " +
                                                    "Values (@AssetManagerCode, @SmaOfferingId, @FlowDate, @Assets, @GrossFlows, @Redemptions, @NetFlows)";
                                            }
                                            else
                                            {
                                                cmd2.CommandText = sqlUpdate + sqlWhere2;
                                            }
                                            try
                                            {
                                                cmd2.ExecuteNonQuery();
                                            }
                                            catch (SqlException ex)
                                            {
                                                LogHelper.WriteLine(logFuncName + ex.Message + " line number: " + currentRowCount);
                                            }
                                            finally
                                            {
                                                if (iCount2 == 0)
                                                {
                                                    addCount += 1;
                                                    cmd2.Parameters["@Assets"].Value = "";
                                                    cmd2.Parameters["@GrossFlows"].Value = "";
                                                    cmd2.Parameters["@Redemptions"].Value = "";
                                                    cmd2.Parameters["@NetFlows"].Value = "";
                                                }
                                                else
                                                {
                                                    updateCount += 1;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            LogHelper.WriteLine("Column Name: " + colName + " not found");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        LogHelper.WriteLine("----- Offering not found Skipping Row " + (currentRowCount) + "------");
                        LogHelper.WriteLine("assetManagerCode   = " + assetManagerCode);
                        LogHelper.WriteLine("sponsorFirm        = " + sponsorFirm);
                        LogHelper.WriteLine("smaStrategy        = " + smaStrategy);
                        LogHelper.WriteLine("smaProductType     = " + smaProductType);
                        LogHelper.WriteLine("managerClass       = " + managerClass);
                        LogHelper.WriteLine("-----------");
                        LogHelper.WriteLine("-----------");
                    }
                    dr.Close();
                }
                catch (SqlException ex)
                {
                    LogHelper.WriteLine(logFuncName + ex.Message + " line number: " + currentRowCount);
                }
                finally
                {
                }
            }
            LogHelper.WriteLine(logFuncName + "Rows Processed " + currentRowCount);
            LogHelper.WriteLine(logFuncName + "Rows Added " + addCount);
            LogHelper.WriteLine(logFuncName + "Rows Updated " + updateCount);
            LogHelper.WriteLine(logFuncName + filePath + " finished");
        }


        #endregion ProcessFlows

        #region ProcessStrategies

        private void ProcessStrategiesData(string filePath) //, ManagerTypes managerType)
        {
            SqlCommand cmd = null;
            string sqlSelect = "";
            string sqlWhere = "";
            string valueParsed = "";
            string colName = "";
            string tableName = " SmaStrategies ";
            string logFuncName = "ProcessStrategiesData: ";
            string format;
            DateTime result;
            CultureInfo provider = CultureInfo.InvariantCulture;
            bool useInceptionDate = false;
            string assetManagerCode = AssetManagerCode(filePath);

            int currentRowCount = 1; // Since csv file has a header set row to 1, data starts in row 2
            int addCount = 0;

            LogHelper.WriteLine(logFuncName + filePath + " started");

            DataTable dt = ReadCsvIntoTable(filePath);

            sqlSelect = "select count(*) from" + tableName;
            sqlWhere = @"where AssetManagerCode = @AssetManagerCode and SmaStrategy = @SmaStrategy";

            cmd = new SqlCommand
            {
                Connection = mSqlConn1,
                CommandText = sqlSelect + sqlWhere
            };

            foreach (DataRow row in dt.Rows)
            {
                currentRowCount += 1;
                colName = "AssetManagerCode";
                if (currentRowCount == 2) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd.Parameters["@" + colName].Value = assetManagerCode;

                colName = "SmaStrategy";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd.Parameters["@" + colName].Value = valueParsed;

                try
                {
                    cmd.CommandText = sqlSelect + sqlWhere;
                    int iCount = (int)cmd.ExecuteScalar();
                    if (iCount == 0)
                    {
                        colName = "MorningstarStrategyId";
                        valueParsed = ParseColumn(row, colName);
                        if (addCount == 0) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                        cmd.Parameters["@" + colName].Value = valueParsed;

                        colName = "MorningstarClass";
                        valueParsed = ParseColumn(row, colName);
                        if (addCount == 0) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                        cmd.Parameters["@" + colName].Value = valueParsed;

                        colName = "MorningstarClassId";
                        valueParsed = ParseColumn(row, colName);
                        if (addCount == 0) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                        cmd.Parameters["@" + colName].Value = valueParsed;

                        colName = "ManagerClass";
                        valueParsed = ParseColumn(row, colName);
                        if (addCount == 0) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                        cmd.Parameters["@" + colName].Value = valueParsed;

                        colName = "InceptionDate";
                        valueParsed = ParseColumn(row, colName);

                        //if ( managerType.Equals(ManagerTypes.Alli))
                        //    format = "M/d/yyyy";
                        //else
                        //    format = "MM/dd/yy";

                        if (valueParsed.Length == 8)
                        {
                            try
                            {
                                useInceptionDate = true;
                                if (addCount == 0) cmd.Parameters.Add("@" + colName, SqlDbType.Date);
                                format = "MM/dd/yy";
                                result = DateTime.ParseExact(valueParsed, format, provider);
                                Console.WriteLine("{0} converts to {1}.", valueParsed, result.ToString());
                                cmd.Parameters["@" + colName].Value = result.ToString();
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("{0} is not in the correct format.", valueParsed);
                            }
                        }


                        if (useInceptionDate)
                        {
                            cmd.CommandText =
                                "insert into " + tableName +
                                    "(AssetManagerCode, SmaStrategy, MorningstarStrategyId, MorningstarClass, MorningstarClassId, ManagerClass, InceptionDate) " +
                                "Values (@AssetManagerCode, @SmaStrategy, @MorningstarStrategyId, @MorningstarClass, @MorningstarClassId, @ManagerClass, @InceptionDate)";
                        }
                        else
                        {
                            cmd.CommandText =
                                "insert into " + tableName +
                                    "(AssetManagerCode, SmaStrategy, MorningstarStrategyId, MorningstarClass, MorningstarClassId, ManagerClass) " +
                                "Values (@AssetManagerCode, @SmaStrategy, @MorningstarStrategyId, @MorningstarClass, @MorningstarClassId, @ManagerClass)";
                        }
                        cmd.ExecuteNonQuery();
                        addCount += 1;
                    }
                    else if (iCount > 0)
                    {
                        LogHelper.WriteLine("----- Skipping Row " + (currentRowCount) + "------");

                        foreach (DataColumn column in dt.Columns)
                        {
                            if (column.ColumnName.Equals("AssetManager") || column.ColumnName.Equals("SmaStrategy"))
                                LogHelper.WriteLine(column.ColumnName.ToString() + "|" + row[column].ToString());
                        }
                        LogHelper.WriteLine("-----------");
                    }
                }
                catch (SqlException ex)
                {
                    LogHelper.WriteLine(logFuncName + ex.Message + " line number: " + currentRowCount);
                }
                finally
                {
                }
            }
            LogHelper.WriteLine(logFuncName + "Rows Processed " + currentRowCount);
            LogHelper.WriteLine(logFuncName + "Rows Added " + addCount);
            LogHelper.WriteLine(logFuncName + filePath + " finished");
        }

        #endregion ProcessStrategies

        #region ProcessReturns

        public void ProcessReturnsData(string filePath)
        {
            SqlCommand cmd1 = null;
            SqlCommand cmd2 = null;
            string sqlSelect = "";
            string sqlWhere = "";
            string valueParsed = "";
            string colName = "";
            string logFuncName = "ProcessReturnsData: ";
            string assetManagerCode = AssetManagerCode(filePath);

            int currentRowCount = 1; // Since csv file has a header set row to 1, data starts in row 2
            int addCount = 0;

            LogHelper.WriteLine(logFuncName + filePath + " started");

            DataTable dt = ReadCsvIntoTable(filePath);

            sqlSelect = @"select SmaStrategyId from SmaStrategies ";
            sqlWhere = @"where AssetManagerCode = @AssetManagerCode and SmaStrategy = @SmaStrategy";

            cmd1 = new SqlCommand
            {
                Connection = mSqlConn1,
                CommandText = sqlSelect + sqlWhere
            };

            cmd2 = new SqlCommand
            {
                Connection = mSqlConn2
            };


            foreach (DataRow row in dt.Rows)
            {
                currentRowCount += 1;
                colName = "AssetManagerCode";
                if (currentRowCount == 2) cmd1.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd1.Parameters["@" + colName].Value = assetManagerCode;

                colName = "SmaStrategy";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd1.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd1.Parameters["@" + colName].Value = valueParsed;

                try
                {
                    SqlDataReader dr = null;
                    cmd1.CommandText = sqlSelect + sqlWhere;

                    dr = cmd1.ExecuteReader();
                    if (dr.HasRows)
                    {
                        if (dr.Read())
                        {
                            Int32 SmaStrategyId = Convert.ToInt32(dr["SmaStrategyId"].ToString());
                            if (addCount == 0)
                            {
                                cmd2.Parameters.Add("@SmaStrategyId", SqlDbType.Int);
                                cmd2.Parameters.Add("@AssetManagerCode", SqlDbType.VarChar);
                                cmd2.Parameters["@AssetManagerCode"].Value = assetManagerCode;
                            }
                            cmd2.Parameters["@SmaStrategyId"].Value = SmaStrategyId;

                            for (int year = 2016; year <= 2018; year++)
                            {
                                string sYear = year.ToString();
                                for (int quarter = 1; quarter <= 4; quarter++)
                                {
                                    for (int returnTypeIndex = 0; returnTypeIndex <= 2; returnTypeIndex++)
                                    {
                                        string sQuarter = quarter.ToString();
                                        string flowDate = mEndOfQuarterDates[quarter].ToString() + sYear;

                                        if (addCount == 0)
                                        {
                                            cmd2.Parameters.Add("@ReturnDate", SqlDbType.Date);
                                        }
                                        cmd2.Parameters["@ReturnDate"].Value = flowDate;

                                        colName = sQuarter + "Q" + " " + sYear + mReturnTypeCol[returnTypeIndex];

                                        if (dt.Columns.Contains(colName))
                                        {
                                            valueParsed = ParseColumn(row, colName);
                                            colName = "ReturnValue";
                                            if (addCount == 0)
                                            {
                                                cmd2.Parameters.Add("@" + colName, SqlDbType.VarChar);
                                                cmd2.Parameters.Add("@ReturnType", SqlDbType.VarChar);
                                            }
                                            cmd2.Parameters["@" + colName].Value = valueParsed;
                                            cmd2.Parameters["@ReturnType"].Value = mReturnTypes[returnTypeIndex];

                                            cmd2.CommandText =
                                                "insert into SmaReturns " +
                                                    "(AssetManagerCode, SmaStrategyId, ReturnType, ReturnDate, ReturnValue) " +
                                                "Values (@AssetManagerCode, @SmaStrategyId, @ReturnType, @ReturnDate, @ReturnValue)";
                                            try
                                            {
                                                cmd2.ExecuteNonQuery();
                                            }
                                            catch (SqlException ex)
                                            {
                                                LogHelper.WriteLine(logFuncName + ex.Message + " line number: " + ex.LineNumber);
                                            }
                                            finally
                                            {
                                                addCount += 1;
                                            }
                                        }
                                        else
                                        {
                                            LogHelper.WriteLine("Column Name: " + colName + " not found");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        LogHelper.WriteLine("----- Strategy not found Skipping Row " + (currentRowCount) + "------");

                        foreach (DataColumn column in dt.Columns)
                        {
                            if (column.ColumnName.Equals("AssetManager") || column.ColumnName.Equals("SmaStrategy") )
                                LogHelper.WriteLine(column.ColumnName.ToString() + "|" + row[column].ToString());
                        }
                        LogHelper.WriteLine("-----------");
                    }
                    dr.Close();
                }
                catch (SqlException ex)
                {
                    LogHelper.WriteLine(logFuncName + ex.Message + " line number: " + currentRowCount);
                }
                finally
                {
                }
            }
            LogHelper.WriteLine(logFuncName + "Rows Processed " + currentRowCount);
            LogHelper.WriteLine(logFuncName + "Rows Added " + addCount);
            LogHelper.WriteLine(logFuncName + filePath + " finished");
        }
        #endregion ProcessReturns
    }
}
