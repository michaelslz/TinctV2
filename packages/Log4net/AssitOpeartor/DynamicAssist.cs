using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGT.DBOperation.AssitOpeartor
{
    public class DynamicAssist
    {
        static string connectstring = ConfigurationManager.ConnectionStrings["Dynamic"].ConnectionString;
        public static List<int> GetThreadIds()
        {
            List<int> threadidlist = new List<int>();           
            using (SqlConnection con = new SqlConnection(connectstring))
            {
                try
                {
                    string query = "select distinct ID from TempThreads";
                    con.Open();
                    SqlCommand comm = new SqlCommand(query, con);
                    var reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        threadidlist.Add(int.Parse(reader["ID"].ToString()));
                    }

                }
                catch(Exception e)
                {
                    throw e;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return threadidlist;
        }
        public static List<int> GetUserIds( )
        {
            List<int> useridlist = new List<int>();
            using (SqlConnection con = new SqlConnection(connectstring))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandText = @"select distinct AuthorID from TempReplies (nolock)
                                        union
                                        select distinct AuthorID from TempThreads (nolock)";
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        useridlist.Add(int.Parse(reader["AuthorID"].ToString()));
                    }

                }
                catch (SqlException e)
                {
                    throw e;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                return useridlist;
            }
        }
    }
}
