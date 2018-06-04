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
        private SqlConnection mSqlConn = null;
        private string mConnectionString = "";

        public FileProcessor()
        {
            mConnectionString = @"server=JKERMOND-NEW\SQLEXPRESS2014;database=DoverSma;uid=sa;pwd=M@gichat!";
            mSqlConn = new SqlConnection(mConnectionString);
            mSqlConn.Open();
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

        public void ProcessOfferingsFile(string filePath)
        {
            SqlCommand cmd = null;
            string sqlSelect = "";
            string sqlWhere = "";
            string valueParsed = "";
            string colName = "";
            string tableName = " SmaOfferings ";

            int currentRowCount = 1; // Since csv file has a header set row to 1, data starts in row 2
            int addCount = 0;

            LogHelper.WriteLine("ProcessOfferingsFile: " + filePath + " started");

            DataTable dt = ReadCsvIntoTable(filePath);

            sqlSelect = "select count(*) from" + tableName;
            sqlWhere = "where AssetManager = @AssetManager and SponsorFirm = @SponsorFirm  and AdvisoryPlatform = @AdvisoryPlatform  and SmaStrategy = @SmaStrategy and SmaProductType = @SmaProductType";

            cmd = new SqlCommand
            {
                Connection = mSqlConn,
                CommandText = sqlSelect + sqlWhere
            };

            foreach (DataRow dr in dt.Rows)
            {
                currentRowCount += 1;
                colName = "AssetManager";
                valueParsed = ParseColumn(dr, colName);
                if (currentRowCount == 2) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd.Parameters["@" + colName].Value = valueParsed;

                colName = "SponsorFirm";
                valueParsed = ParseColumn(dr, colName);
                if (currentRowCount == 2) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd.Parameters["@" + colName].Value = valueParsed;

                colName = "AdvisoryPlatform";
                valueParsed = ParseColumn(dr, colName);
                if (currentRowCount == 2) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd.Parameters["@" + colName].Value = valueParsed;

                colName = "SmaStrategy";
                valueParsed = ParseColumn(dr, colName);
                if (currentRowCount == 2) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd.Parameters["@" + colName].Value = valueParsed;

                colName = "SmaProductType";
                valueParsed = ParseColumn(dr, colName);
                if (currentRowCount == 2) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd.Parameters["@" + colName].Value = valueParsed;

                colName = "TampRIAPlatform";
                valueParsed = ParseColumn(dr, colName);
                if (currentRowCount == 2) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                cmd.Parameters["@" + colName].Value = valueParsed;

                try
                {
                    cmd.CommandText = sqlSelect + sqlWhere;
                    int iCount = (int)cmd.ExecuteScalar();
                    if (iCount == 0)
                    {
                        colName = "SponsorFirmId";
                        valueParsed = ParseColumn(dr, colName);
                        if (addCount == 0) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                        cmd.Parameters["@" + colName].Value = valueParsed;

                        colName = "SmaStrategyId";
                        valueParsed = ParseColumn(dr, colName);
                        if (addCount == 0) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                        cmd.Parameters["@" + colName].Value = valueParsed;

                        colName = "MorningstarClass";
                        valueParsed = ParseColumn(dr, colName);
                        if (addCount == 0) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                        cmd.Parameters["@" + colName].Value = valueParsed;

                        colName = "MorningstarClassId";
                        valueParsed = ParseColumn(dr, colName);
                        if (addCount == 0) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                        cmd.Parameters["@" + colName].Value = valueParsed;

                        colName = "ManagerClass";
                        valueParsed = ParseColumn(dr, colName);
                        if (addCount == 0) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                        cmd.Parameters["@" + colName].Value = valueParsed;

                        colName = "TotalAccounts";
                        valueParsed = ParseColumn(dr, colName);
                        if (addCount == 0) cmd.Parameters.Add("@" + colName, SqlDbType.VarChar);
                        cmd.Parameters["@" + colName].Value = valueParsed;

                        colName = "CsvFileRow";
                        valueParsed = ParseColumn(dr, colName);
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
                                LogHelper.WriteLine(column.ColumnName.ToString() + "|" + dr[column].ToString());
                        }
                        LogHelper.WriteLine("-----------");
                    }
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                finally
                {
                }
            }
            LogHelper.WriteLine("ProcessOfferingsFile: Rows Processed " + currentRowCount);
            LogHelper.WriteLine("ProcessOfferingsFile: Rows Added " + addCount);
            LogHelper.WriteLine("ProcessOfferingsFile: " + filePath + " finished");
        }
    }
}
