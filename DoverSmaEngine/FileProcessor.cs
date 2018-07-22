using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private string mFilepath = @"C:\A_Dover\Dev\Vifs";
        
        // (OAFF_) Offering and Flows Files 
        // (SARF_) Strategies and Returns Files
        private string mOAFF_legg = @"OAFF_legg.csv";
        private string mSARF_legg = @"SARF_legg.csv";
        private string mOAFF_prin = @"OAFF_prin.csv";
        private string mSARF_prin = @"SARF_prin.csv";
        private string mOAFF_alli = @"OAFF_alli.csv";
        private string mSARF_alli = @"SARF_alli.csv";
        private string mOAFF_dela = @"OAFF_dela.csv";
        private string mSARF_dela = @"SARF_dela.csv";
        private string mOAFF_gwnk = @"OAFF_gwnk.csv";
        private string mSARF_gwnk = @"SARF_gwnk.csv";
        private string mOAFF_bran = @"OAFF_bran.csv";
        private string mSARF_bran = @"SARF_bran.csv";
        private string mOAFF_cong = @"OAFF_cong.csv";
        private string mSARF_cong = @"SARF_cong.csv";
        private string mOAFF_fran = @"OAFF_fran.csv";
        private string mSARF_fran = @"SARF_fran.csv";
        private string mOAFF_inve = @"OAFF_inve.csv";
        private string mSARF_inve = @"SARF_inve.csv";
        private string mOAFF_laza = @"OAFF_laza.csv";
        private string mSARF_laza = @"SARF_laza.csv";
        private string mOAFF_anch = @"OAFF_anch.csv";
        private string mSARF_anch = @"SARF_anch.csv";
        private string mOAFF_nuve = @"OAFF_nuve.csv";
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
                case "Legg":
                    ProcessOfferingsDataSingleRow(Path.Combine(mFilepath, mOAFF_legg));
                    break;
                case "Principal":
                    ProcessOfferingsDataMultiRow(Path.Combine(mFilepath, mOAFF_prin));
                    break;
                case "Allianz":
                    ProcessOfferingsDataSingleRow(Path.Combine(mFilepath, mOAFF_alli));
                    break;
                case "Delaware":
                    ProcessOfferingsDataSingleRow(Path.Combine(mFilepath, mOAFF_dela));
                    break;
                case "GW&K":
                    ProcessOfferingsDataSingleRow(Path.Combine(mFilepath, mOAFF_gwnk));
                    break;
                case "Brandes":
                    ProcessOfferingsDataSingleRow(Path.Combine(mFilepath, mOAFF_bran));
                    break;
                case "Congress":
                    ProcessOfferingsDataSingleRow(Path.Combine(mFilepath, mOAFF_cong));
                    break;
                case "Franklin Templeton":
                    ProcessOfferingsDataSingleRow(Path.Combine(mFilepath, mOAFF_fran));
                    break;
                case "Invesco":
                    ProcessOfferingsDataSingleRow(Path.Combine(mFilepath, mOAFF_inve));
                    break;
                case "Lazard":
                    ProcessOfferingsDataSingleRow(Path.Combine(mFilepath, mOAFF_laza));
                    break;
                case "Anchor":
                    ProcessOfferingsDataSingleRow(Path.Combine(mFilepath, mOAFF_anch));
                    break;
                case "Nuveen":
                    ProcessOfferingsDataSingleRow(Path.Combine(mFilepath, mOAFF_nuve));
                    break;
            }
        }

        public void ProcessManagerFlows(string Manager)
        {
            switch (Manager)
            {
                case "Legg":
                    ProcessFlowsDataSingleRow(Path.Combine(mFilepath, mOAFF_legg));
                    break;
                case "Principal":
                    ProcessFlowsDataMultiRow(Path.Combine(mFilepath, mOAFF_prin));
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
                case "Anchor":
                    ProcessFlowsDataSingleRow(Path.Combine(mFilepath, mOAFF_anch));
                    break;
                case "Nuveen":
                    ProcessFlowsDataSingleRowNuveen(Path.Combine(mFilepath, mOAFF_nuve));
                    break;
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

            }
        }
        #endregion ProcessManager

        private string AssetManagerCode( string filePath)
        {
            int startPos = filePath.IndexOf(".csv") - 4;
            return(filePath.Substring(startPos, 4));
        }

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

            int currentRowCount = 1; // Since csv file has a header set row to 1, data starts in row 2
            int addCount = 0;

            LogHelper.WriteLine(logFuncName + filePath + " started");

            DataTable dt = ReadCsvIntoTable(filePath);

            sqlSelect = "select count(*) from" + tableName;
            sqlWhere = @"where AssetManagerCode = @AssetManagerCode and SponsorFirm = @SponsorFirm  and AdvisoryPlatform = @AdvisoryPlatform  and SmaStrategy = @SmaStrategy and 
                        SmaProductType = @SmaProductType and TampRIAPlatform = @TampRIAPlatform and ManagerClass = @ManagerClass";

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

                colName = "SponsorFirm";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                string sponsorFirm2 = ParseColumn(row, "SponsorFirm2");
                if (sponsorFirm2.Length > 0)
                    valueParsed += " - " + sponsorFirm2;
                cmd.Parameters["@" + colName].Value = valueParsed;

                colName = "AdvisoryPlatform";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd.Parameters["@" + colName].Value = valueParsed;

                colName = "SmaStrategy";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd.Parameters["@" + colName].Value = valueParsed;

                colName = "SmaProductType";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd.Parameters["@" + colName].Value = valueParsed;

                colName = "TampRIAPlatform";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd.Parameters["@" + colName].Value = valueParsed;

                colName = "ManagerClass";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd.Parameters["@" + colName].Value = valueParsed;

                try
                {
                    cmd.CommandText = sqlSelect + sqlWhere;
                    int iCount = (int)cmd.ExecuteScalar();
                    if (iCount == 0)
                    {
                        colName = "SponsorFirmId";
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

                        cmd.CommandText =
                            "insert into " + tableName +
                                "(AssetManagerCode, SponsorFirm, AdvisoryPlatform, SmaStrategy, SmaProductType, TampRIAPlatform, ManagerClass," +
                                " SponsorFirmId, MorningstarStrategyId, MorningstarClass, MorningstarClassId, TotalAccounts, CsvFileRow) " +
                            "Values (@AssetManagerCode, @SponsorFirm, @AdvisoryPlatform, @SmaStrategy, @SmaProductType, @TampRIAPlatform, @ManagerClass," +
                                    " @SponsorFirmId, @MorningstarStrategyId, @MorningstarClass, @MorningstarClassId, @TotalAccounts, @CsvFileRow)";
                        cmd.ExecuteNonQuery();
                        addCount += 1;
                    }
                    else if (iCount > 0)
                    {
                        LogHelper.WriteLine("----- Skipping Row " + (currentRowCount) + "------");

                        foreach (DataColumn column in dt.Columns)
                        {
                            if (column.ColumnName.Equals("AssetManager") || column.ColumnName.Equals("SponsorFirm") || column.ColumnName.Equals("AdvisoryPlatform") 
                                || column.ColumnName.Equals("SmaStrategy") || column.ColumnName.Equals("SmaProductType"))
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
        public void ProcessFlowsDataSingleRow(string filePath)
        {
            SqlCommand cmd1 = null;
            SqlCommand cmd2 = null;
            string sqlSelect = "";
            string sqlWhere = "";
            string valueParsed = "";
            string colName = "";
            string logFuncName = "ProcessFlowsDataSingleRow: ";
            string assetManagerCode = AssetManagerCode(filePath);

            int currentRowCount = 1; // Since csv file has a header set row to 1, data starts in row 2
            int addCount = 0;

            LogHelper.WriteLine(logFuncName + filePath + " started");

            DataTable dt = ReadCsvIntoTable(filePath);

            sqlSelect = @"select SmaOfferingId from SmaOfferings ";
            sqlWhere = @"where AssetManagerCode = @AssetManagerCode and SponsorFirm = @SponsorFirm  and AdvisoryPlatform = @AdvisoryPlatform  and SmaStrategy = @SmaStrategy and " +
                        "SmaProductType = @SmaProductType and TampRIAPlatform = @TampRIAPlatform";

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

                colName = "SponsorFirm";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd1.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd1.Parameters["@" + colName].Value = valueParsed;

                colName = "AdvisoryPlatform";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd1.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd1.Parameters["@" + colName].Value = valueParsed;

                colName = "SmaStrategy";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd1.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd1.Parameters["@" + colName].Value = valueParsed;

                colName = "SmaProductType";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd1.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd1.Parameters["@" + colName].Value = valueParsed;

                colName = "TampRIAPlatform";
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
                            Int32 SmaOfferingId = Convert.ToInt32( dr["SmaOfferingId"].ToString());
                            if (addCount == 0)
                            {
                                cmd2.Parameters.Add("@AssetManagerCode", SqlDbType.VarChar);
                                cmd2.Parameters["@AssetManagerCode"].Value = assetManagerCode;
                                cmd2.Parameters.Add("@SmaOfferingId", SqlDbType.Int);
                            }
                            cmd2.Parameters["@SmaOfferingId"].Value = SmaOfferingId;

                            for ( int year = 2016; year <= 2018; year++)
                            {
                                string sYear = year.ToString();
                                for( int quarter = 1; quarter <= 4; quarter++ )
                                {
                                    string sQuarter = quarter.ToString();
                                    string flowDate = mEndOfQuarterDates[quarter].ToString() + sYear;

                                    if (addCount == 0)
                                        cmd2.Parameters.Add("@FlowDate", SqlDbType.Date);
                                    cmd2.Parameters["@FlowDate"].Value = flowDate;

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
                                                cmd2.Parameters.Add("@" + colName, SqlDbType.VarChar);
                                            cmd2.Parameters["@" + colName].Value = valueParsed;

                                            if (insert)
                                            {
                                                cmd2.CommandText =
                                                    "insert into SmaFlows " +
                                                     "(AssetManagerCode, SmaOfferingId, FlowDate, Assets, GrossFlows, Redemptions, NetFlows, DerivedFlows) " +
                                                    "Values (@AssetManagerCode, @SmaOfferingId, @FlowDate, @Assets, @GrossFlows, @Redemptions, @NetFlows, @DerivedFlows)";
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
                                                    cmd2.Parameters["@Assets"].Value = "";
                                                    cmd2.Parameters["@GrossFlows"].Value = "";
                                                    cmd2.Parameters["@Redemptions"].Value = "";
                                                    cmd2.Parameters["@NetFlows"].Value = "";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            LogHelper.WriteLine( "Column Name: " + colName + " not found");
                                        }
                                    }
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

        public void ProcessFlowsDataSingleRowNuveen(string filePath)
        {
            SqlCommand cmd1 = null;
            SqlCommand cmd2 = null;
            string sqlSelect = "";
            string sqlWhere = "";
            string valueParsed = "";
            string colName = "";
            string logFuncName = "ProcessFlowsDataSingleRow: ";
            string assetManagerCode = AssetManagerCode(filePath);

            int currentRowCount = 1; // Since csv file has a header set row to 1, data starts in row 2
            int addCount = 0;
            CultureInfo provider = CultureInfo.InvariantCulture;

            LogHelper.WriteLine(logFuncName + filePath + " started");

            DataTable dt = ReadCsvIntoTable(filePath);

            sqlSelect = @"select SmaOfferingId from SmaOfferings ";
            sqlWhere = @"where AssetManagerCode = @AssetManagerCode and SponsorFirm = @SponsorFirm  and AdvisoryPlatform = @AdvisoryPlatform  and SmaStrategy = @SmaStrategy and " +
                        "SmaProductType = @SmaProductType and TampRIAPlatform = @TampRIAPlatform";

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

                colName = "SponsorFirm";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd1.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd1.Parameters["@" + colName].Value = valueParsed;

                colName = "AdvisoryPlatform";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd1.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd1.Parameters["@" + colName].Value = valueParsed;

                colName = "SmaStrategy";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd1.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd1.Parameters["@" + colName].Value = valueParsed;

                colName = "SmaProductType";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd1.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd1.Parameters["@" + colName].Value = valueParsed;

                colName = "TampRIAPlatform";
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
                            Int32 SmaOfferingId = Convert.ToInt32(dr["SmaOfferingId"].ToString());
                            if (addCount == 0)
                            {
                                cmd2.Parameters.Add("@AssetManagerCode", SqlDbType.VarChar);
                                cmd2.Parameters["@AssetManagerCode"].Value = assetManagerCode;
                                cmd2.Parameters.Add("@SmaOfferingId", SqlDbType.Int);
                                cmd2.Parameters.Add("@FlowDate", SqlDbType.Date);
                                cmd2.Parameters.Add("@Assets", SqlDbType.VarChar);
                            }
                            cmd2.Parameters["@SmaOfferingId"].Value = SmaOfferingId;

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
                                }
                                catch (FormatException)
                                {
                                    Console.WriteLine("{0} is not in the correct format.", valueParsed);
                                }
                            }

                            colName = "Assets";
                            valueParsed = ParseColumn(row, colName);
                            cmd2.Parameters["@Assets"].Value = valueParsed;

                            cmd2.CommandText =
                                "insert into SmaFlows " +
                                    "(AssetManagerCode, SmaOfferingId, FlowDate, Assets) " +
                                "Values (@AssetManagerCode, @SmaOfferingId, @FlowDate, @Assets)";
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
