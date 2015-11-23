using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGT.DBOperation.AssitOpeartor
{
    public class AzureAssist
    {
        static string connectstring = ConfigurationManager.ConnectionStrings["Azure"].ConnectionString;
        public static List<int> GetThreadIds()
        {
            List<int> threadIdList = new List<int>();
            using (SqlConnection con = new SqlConnection(connectstring))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandText = "select distinct ThreadID from [dbo].[ThreadTemp]";
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        threadIdList.Add(int.Parse(reader["ThreadID"].ToString()));
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
                return threadIdList;
            }
        }
        public static List<string> GetUserLinks()
        {
            List<string> userLinkList = new List<string>();
            using (SqlConnection con = new SqlConnection(connectstring))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandText = "select distinct PostUserLink from [dbo].[PostTemp]";
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        userLinkList.Add(reader["PostUserLink"].ToString());
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
                return userLinkList;
            }
        }
    }
}
