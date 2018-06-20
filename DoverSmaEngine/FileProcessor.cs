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
        
        string[] flowTypes = Enum.GetNames(typeof(FlowTypes));

        private SqlConnection mSqlConn1 = null;
        private SqlConnection mSqlConn2 = null;
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
//        private string mOAFF_ = @"OAFF_.csv";
//        private string mSARF_ = @"SARF_.csv";

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

        }

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
                    value = dr[column].ToString();
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
                    //ProcessOfferingsDataMultiRow1(Path.Combine(mFilepath, mOAFF_prin));
                    break;
                case "Allianz":
                    //ProcessOfferingsDataSingleLine(Path.Combine(mFilepath, mOAFF_alli));
                    break;
                case "Delaware":
                    break;
                case "GWNK":
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
                    break;
                case "Allianz":
                    break;
                case "Delaware":
                    break;
                case "GWNK":
                    break;
            }
        }

        public void ProcessManagerStrategies(string Manager)
        {
            switch (Manager)
            {
                case "Legg":
                    ProcessStrategiesData(Path.Combine(mFilepath, mOAFF_legg));
                    break;
                case "Principal":
                    break;
                case "Allianz":
                    break;
                case "Delaware":
                    break;
                case "GWNK":
                    break;
            }
        }

        public void ProcessManagerReturns(string Manager)
        {
            switch (Manager)
            {
                case "Legg":
                    ProcessReturnsData(Path.Combine(mFilepath, mOAFF_legg));
                    break;
                case "Principal":
                    break;
                case "Allianz":
                    break;
                case "Delaware":
                    break;
                case "GWNK":
                    break;
            }
        }
        #endregion ProcessManager

        #region ProcessOfferings

        public void ProcessOfferingsDataSingleRow(string filePath)
        {
            SqlCommand cmd = null;
            string sqlSelect = "";
            string sqlWhere = "";
            string valueParsed = "";
            string colName = "";
            string tableName = " SmaOfferings ";
            string logFuncName = "ProcessOfferingsData: ";


            int currentRowCount = 1; // Since csv file has a header set row to 1, data starts in row 2
            int addCount = 0;

            LogHelper.WriteLine(logFuncName + filePath + " started");

            DataTable dt = ReadCsvIntoTable(filePath);

            sqlSelect = "select count(*) from" + tableName;
            sqlWhere = @"where AssetManager = @AssetManager and SponsorFirm = @SponsorFirm  and AdvisoryPlatform = @AdvisoryPlatform  and SmaStrategy = @SmaStrategy and 
                        SmaProductType = @SmaProductType and TampRIAPlatform = @TampRIAPlatform";

            cmd = new SqlCommand
            {
                Connection = mSqlConn1,
                CommandText = sqlSelect + sqlWhere
            };

            foreach (DataRow row in dt.Rows)
            {
                currentRowCount += 1;
                colName = "AssetManager";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd.Parameters["@" + colName].Value = valueParsed;

                colName = "SponsorFirm";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
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

                        colName = "ManagerClass";
                        valueParsed = ParseColumn(row, colName);
                        if (addCount == 0) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                        cmd.Parameters["@" + colName].Value = valueParsed;

                        colName = "TotalAccounts";
                        valueParsed = ParseColumn(row, colName);
                        if (addCount == 0) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                        cmd.Parameters["@" + colName].Value = valueParsed;

                        colName = "CsvFileRow";
                        valueParsed = ParseColumn(row, colName);
                        if (addCount == 0) cmd.Parameters.Add("@" + colName, SqlDbType.Int);
                        cmd.Parameters["@" + colName].Value = currentRowCount; 

                        cmd.CommandText =
                            "insert into " + tableName +
                                "(AssetManager, SponsorFirm, AdvisoryPlatform, SmaStrategy, SmaProductType, TampRIAPlatform," +
                                " SponsorFirmId, MorningstarStrategyId, MorningstarClass, MorningstarClassId, TotalAccounts, CsvFileRow) " +
                            "Values (@AssetManager, @SponsorFirm, @AdvisoryPlatform, @SmaStrategy, @SmaProductType, @TampRIAPlatform, " +
                                    "@SponsorFirmId, @MorningstarStrategyId, @MorningstarClass, @MorningstarClassId, @TotalAccounts, @CsvFileRow)";
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
                    LogHelper.WriteLine(logFuncName + ex.Message + " line number: " + ex.LineNumber);
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
            string logFuncName = "ProcessFlowsData: ";

            int currentRowCount = 1; // Since csv file has a header set row to 1, data starts in row 2
            int addCount = 0;

            LogHelper.WriteLine(logFuncName + filePath + " started");

            DataTable dt = ReadCsvIntoTable(filePath);

            sqlSelect = @"select SmaOfferingId from SmaOfferings ";
            sqlWhere = @"where AssetManager = @AssetManager and SponsorFirm = @SponsorFirm  and AdvisoryPlatform = @AdvisoryPlatform  and SmaStrategy = @SmaStrategy and " +
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
                colName = "AssetManager";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd1.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd1.Parameters["@" + colName].Value = valueParsed;

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
                                cmd2.Parameters.Add("@SmaOfferingId", SqlDbType.Int);
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
                                                     "(SmaOfferingId, FlowDate, Assets, GrossFlows, Redemptions, NetFlows, DerivedFlows) " +
                                                    "Values (@SmaOfferingId, @FlowDate, @Assets, @GrossFlows, @Redemptions, @NetFlows, @DerivedFlows)";
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
                    LogHelper.WriteLine(logFuncName + ex.Message + " line number: " + ex.LineNumber);
                }
                finally
                {
                }
            }
            LogHelper.WriteLine(logFuncName + "Rows Processed " + currentRowCount);
            LogHelper.WriteLine(logFuncName + "Rows Added " + addCount);
            LogHelper.WriteLine(logFuncName + filePath + " finished");
        }

        #endregion ProcessFlows

        #region ProcessStrategies

        public void ProcessStrategiesData(string filePath)
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


            int currentRowCount = 1; // Since csv file has a header set row to 1, data starts in row 2
            int addCount = 0;

            LogHelper.WriteLine(logFuncName + filePath + " started");

            DataTable dt = ReadCsvIntoTable(filePath);

            sqlSelect = "select count(*) from" + tableName;
            sqlWhere = @"where AssetManager = @AssetManager and SmaStrategy = @SmaStrategy";

            cmd = new SqlCommand
            {
                Connection = mSqlConn1,
                CommandText = sqlSelect + sqlWhere
            };

            foreach (DataRow row in dt.Rows)
            {
                currentRowCount += 1;
                colName = "AssetManager";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd.Parameters["@" + colName].Value = valueParsed;

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
                        if (addCount == 0) cmd.Parameters.Add("@" + colName, SqlDbType.Date);

                        format = "MM/dd/yy";
                        try
                        {
                            result = DateTime.ParseExact(valueParsed, format, provider);
                            Console.WriteLine("{0} converts to {1}.", valueParsed, result.ToString());
                            cmd.Parameters["@" + colName].Value = result.ToString();
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("{0} is not in the correct format.", valueParsed);
                        }

                        cmd.Parameters["@" + colName].Value = valueParsed;


                        cmd.CommandText =
                            "insert into " + tableName +
                                "(AssetManager, SmaStrategy, MorningstarStrategyId, MorningstarClass, MorningstarClassId, ManagerClass, InceptionDate) " +
                            "Values (@AssetManager, @SmaStrategy, @MorningstarStrategyId, @MorningstarClass, @MorningstarClassId, @ManagerClass, @InceptionDate)";
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
                    LogHelper.WriteLine(logFuncName + ex.Message + " line number: " + ex.LineNumber);
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

            int currentRowCount = 1; // Since csv file has a header set row to 1, data starts in row 2
            int addCount = 0;

            LogHelper.WriteLine(logFuncName + filePath + " started");

            DataTable dt = ReadCsvIntoTable(filePath);

            sqlSelect = @"select SmaStrategyId from SmaStrategies ";
            sqlWhere = @"where AssetManager = @AssetManager and SmaStrategy = @SmaStrategy";

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
                colName = "AssetManager";
                valueParsed = ParseColumn(row, colName);
                if (currentRowCount == 2) cmd1.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd1.Parameters["@" + colName].Value = valueParsed;

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
                                cmd2.Parameters.Add("@SmaStrategyId", SqlDbType.Int);
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
                                            cmd2.Parameters.Add("@ReturnDate", SqlDbType.Date);
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
                                                    "(SmaStrategyId, ReturnType, ReturnDate, ReturnValue) " +
                                                "Values (@SmaStrategyId, @ReturnType, @ReturnDate, @ReturnValue)";
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
                    LogHelper.WriteLine(logFuncName + ex.Message + " line number: " + ex.LineNumber);
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
