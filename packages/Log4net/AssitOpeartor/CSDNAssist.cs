using DGT.Model.CSDNForums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGT.DBOperation.AssitOpeartor
{
    public class CSDNAssist
    {
        static string connectstring = ConfigurationManager.ConnectionStrings["CSDN"].ConnectionString;
        public static List<int> GetThreadIds()
        {
            List<int> threadIdList = new List<int>();
            using (SqlConnection con = new SqlConnection(connectstring))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandText = "SELECT distinct ThreadID FROM [dbo].[ThreadTemp]";
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
        public static List<ForumPost> GetPostIdsPages()
        {
            List<ForumPost> postInfoList = new List<ForumPost>();
            using (SqlConnection con = new SqlConnection(connectstring))
            {
                try
                {                    
                    con.Open();
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandText = @"select distinct ThreadID,PageCount from [dbo].[FirstPostTemp] where PageCount>1";
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ForumPost post = new ForumPost();
                        post.ThreadID = int.Parse(reader["ThreadID"].ToString());
                        post.PageCount = int.Parse(reader["PageCount"].ToString());
                        postInfoList.Add(post);
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
                return postInfoList;
            }
        }
        public static List<string> GetUserIds()
        {
            
            List<string> userIdList = new List<string>();
            using (SqlConnection con = new SqlConnection(connectstring))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandText = @"select distinct postuserid as userid from(
                               select distinct postuserid from [dbo].[FirstPostTemp] union 
                               select distinct postuserid from [dbo].[RestPostTemp] union  
                               select distinct authorid from [dbo].[ThreadTemp] union
                               select distinct lastupdateuserid from [dbo].[ThreadTemp]) s";
//                    cmd.CommandText = @"select distinct postuserid as userid from(
//                               select distinct postuserid from [dbo].[FirstPostTemp] union 
//                               select distinct postuserid from [dbo].[RestPostTemp] union  
//                               select distinct authorid from [dbo].[ThreadTemp] union
//                               select distinct lastupdateuserid from [dbo].[ThreadTemp]) s
//							   where postuserid not in (select userid from [dbo].[UserTemp])";
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        userIdList.Add(reader["userid"].ToString());
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
                return userIdList;
            }
        }
    }
}
