using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DGT.Comon.Log;

namespace DGT.DBOperation
{
    public static class DBOpertor
    {



        public static bool ExcuteSQlCmd(string strText, string ConnectionString)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                try
                {
                    con.Open();

                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = strText;

                    cmd.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    LogtoDB.Intance.Info(e, strText);
                    throw e;
                }
            }
            return true;
        }

        public static bool ExcuteSQlCmd(string strText, SqlParameter[] sqlparams, string ConnectionString)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                try
                {
                    con.Open();

                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(sqlparams);
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = strText;

                    cmd.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    LogtoDB.Intance.Error(e, string.Join(";", strText, sqlparams));
                    throw e;
                }
            }
            return true;
        }

        public static bool BulkInserttoDB(DataTable table, string destinctionName, string ConnectionString)
        {
            bool isSuccess = false;


            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                SqlTransaction tran = con.BeginTransaction();

                try
                {
                    SqlBulkCopy bc = new SqlBulkCopy(con, SqlBulkCopyOptions.KeepNulls, tran);
                    bc.BulkCopyTimeout = 0;

                    //bc.BulkCopyTimeout = 5000000;

                    bc.BatchSize = 1000;
                    bc.DestinationTableName = destinctionName;
                    int columnsCount = table.Columns.Count;

                    for (int i = 0; i < columnsCount; i++)
                    {
                        bc.ColumnMappings.Add(new SqlBulkCopyColumnMapping(i, i));
                    }

                    bc.WriteToServer(table);
                    tran.Commit();
                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    LogtoDB.Intance.Error(ex,destinctionName);
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return isSuccess;

        }

        public static bool BulkInserttoDB(DataTable table, string destinctionName, bool hasIndentity, string ConnectionString)
        {
            bool isSuccess = false;

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                SqlTransaction tran = con.BeginTransaction();

                try
                {
                    SqlBulkCopy bc = new SqlBulkCopy(con, SqlBulkCopyOptions.KeepNulls, tran);
                    bc.BulkCopyTimeout = 0;

                    //bc.BulkCopyTimeout = 5000000;

                    bc.BatchSize = 1000;
                    bc.DestinationTableName = destinctionName;
                    int columnsCount = table.Columns.Count;
                    if (hasIndentity)
                    {
                        for (int i = 0; i < columnsCount; i++)
                        {
                            bc.ColumnMappings.Add(new SqlBulkCopyColumnMapping(i, i + 1));
                        }
                    }
                    else
                    {
                        for (int i = 0; i < columnsCount; i++)
                        {
                            bc.ColumnMappings.Add(new SqlBulkCopyColumnMapping(i, i));
                        }
                    }
                    bc.WriteToServer(table);
                    tran.Commit();
                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    LogtoDB.Intance.Error(ex,destinctionName);
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return isSuccess;
        }





    }
}
