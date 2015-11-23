using DGT.Model.UserVoice;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGT.DBOperation.AssitOpeartor
{
    public static class UserVoiceAssist
    {
        static string connectstring = ConfigurationManager.ConnectionStrings["UserVoice"].ConnectionString;
        public static List<int> GetForumnsBySubDomain(string subdomainname)
        {

            List<int> forums = new List<int>();

            using (SqlConnection con = new SqlConnection(connectstring))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandText = "select distinct Id from ForumStagging where SubDomain='" + subdomainname + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        forums.Add(int.Parse(reader["Id"].ToString()));
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
                return forums;
            }
        }
        public static List<SuggestionModel> GetSuggestionsBySubDomain(string subdomainname)
        {
            List<SuggestionModel> lists = new List<SuggestionModel>();
            using (SqlConnection con = new SqlConnection(connectstring))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandText = "select distinct ForumId,Id from SuggestionStagging where commentscount>0 and SubDomain='" + subdomainname + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        SuggestionModel sugg = new SuggestionModel();
                        sugg.ForumId = int.Parse(reader["ForumId"].ToString());
                        sugg.Id = int.Parse(reader["Id"].ToString());
                        lists.Add(sugg);
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
            }
            return lists;
        }

        public static List<UserModel> GetUsersBySubDomain(string subdomainname)
        {
            List<UserModel> users = new List<UserModel>();
            using (SqlConnection con = new SqlConnection(connectstring))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandText = "select distinct CreatorId as Id from SuggestionStagging where CreatorId>0 and SubDomain='" + subdomainname + "'" +
                                                        "union " +
                                                        "select distinct  CreatorId as Id from CommentStagging where CreatorId>0 and SubDomain='" + subdomainname + "'";               
                                     
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        UserModel item = new UserModel();
                        item.Id = int.Parse(reader["Id"].ToString());
                        users.Add(item);
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
                return users;
            }
        }

        public static List<UserModel> GetLostUsersBySubDomain(string subdomainname)
        {
            List<UserModel> users = new List<UserModel>();
            using (SqlConnection con = new SqlConnection(connectstring))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = con.CreateCommand();                    
                    cmd.CommandText = @"select Id from (
                                                        select distinct CreatorId as Id from SuggestionStagging where CreatorId>0 and SubDomain='" + subdomainname + "'" +
                                                        "union select distinct  CreatorId as Id from CommentStagging where CreatorId>0 and SubDomain='" + subdomainname + "'" +
                                                        ") s where Id not in (select Id from [dbo].[UserStagging] where subdomain='" + subdomainname + "')";
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        UserModel item = new UserModel();
                        item.Id = int.Parse(reader["Id"].ToString());
                        users.Add(item);
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
                return users;
            }
        }
    }
}
