using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.SqlClient;

using LumenWorks.Framework.IO.Csv;

using DoverUtilities;

namespace DoverSmaEngine
{
    public class FileProcessor
    {
        // assets, gross flows, redemptions, net flows, derived flows
        private enum FlowTypes { a, g, r, n, d };
        
        string[] flowTypes = Enum.GetNames(typeof(FlowTypes));

        private SqlConnection mSqlConn1 = null;
        private SqlConnection mSqlConn2 = null;
        private string mConnectionString = "";

        public FileProcessor()
        {
            mConnectionString = @"server=JKERMOND-NEW\SQLEXPRESS2014;database=DoverSma;uid=sa;pwd=M@gichat!";
            mSqlConn1 = new SqlConnection(mConnectionString);
            mSqlConn1.Open();
            mSqlConn2 = new SqlConnection(mConnectionString);
            mSqlConn2.Open();

        }

        #region CsvReader

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

        #endregion

        public void ProcessOfferingsData(string filePath)
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

                        colName = "SmaStrategyId";
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
                                " SponsorFirmId, SmaStrategyId, MorningstarClass, MorningstarClassId, TotalAccounts, CsvFileRow) " +
                            "Values (@AssetManager, @SponsorFirm, @AdvisoryPlatform, @SmaStrategy, @SmaProductType, @TampRIAPlatform, " +
                                    "@SponsorFirmId, @SmaStrategyId, @MorningstarClass, @MorningstarClassId, @TotalAccounts, @CsvFileRow)";
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

        public void ProcessFlowsData(string filePath)
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

                            string[] EndOfQuarterDates = { "", "03/31/", "06/30/", "09/30/", "12/31/" };

                            for ( int year = 2016; year <= 2018; year++)
                            {
                                string sYear = year.ToString();
                                for( int quarter = 1; quarter <= 4; quarter++ )
                                {
                                    string sQuarter = quarter.ToString();
                                    string flowDate = EndOfQuarterDates[quarter].ToString() + sYear;

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
    }
}
