using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGT.DBOperation.AssitOpeartor
{
    public class RSDNAssist
    {
        static string connectstring = ConfigurationManager.ConnectionStrings["RSDN"].ConnectionString;
        public static List<string> GetAllGroupNames()
        {
            List<string> groupNameList = new List<string>();
            using (SqlConnection con = new SqlConnection(connectstring))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandText = "select GroupID,GroupName from [dbo].[QueueGidTable]";
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        groupNameList.Add(reader["GroupName"].ToString());
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
                return groupNameList;
            }

        }
        public static int GetGroupIdByName(string groupName)
        {
            int groupId = 0;
            using (SqlConnection con = new SqlConnection(connectstring))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandText = "select GroupID from [dbo].[QueueGidTable] where GroupName='"+groupName+"'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                       groupId=int.Parse(reader["GroupID"].ToString());
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
                return groupId;
            }
        }
        public static List<int> GetThreadIds(string tablename)
        {
            List<int> threadIdList = new List<int>();
            using (SqlConnection con = new SqlConnection(connectstring))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandText = string.Format("select distinct ThreadID from {0}",tablename);
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
        
        public static List<string> GetUserLinks(string tablename)
        {
            List<string> userLinkList = new List<string>();
            using (SqlConnection con = new SqlConnection(connectstring))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = con.CreateCommand();
                   // cmd.CommandText = "select distinct PostUserLink from [dbo].[PostTemp1] where PostUserLink is not null";
                    cmd.CommandText = string.Format("select distinct PostUserLink from {0} where PostUserLink is not null",tablename);
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
