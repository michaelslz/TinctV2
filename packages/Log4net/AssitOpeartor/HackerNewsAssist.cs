using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGT.DBOperation.AssitOpeartor
{
    public class HackerNewsAssist
    {
        static string connectstring = ConfigurationManager.ConnectionStrings["HackerNews"].ConnectionString;
        public static List<string> GetCommentLinks()
        {

            List<string> commentLinkList = new List<string>();
            using (SqlConnection con = new SqlConnection(connectstring))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandText = "SELECT distinct CommentLink FROM [dbo].[NewsTemp]";
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                       commentLinkList.Add(reader["CommentLink"].ToString());
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
                return commentLinkList;
            }
        }


        public static void DealWithHackerUser()
        {
            DBOpertor.ExcuteSQlCmd("truncate table [dbo].[UserTemp]", connectstring);
            DBOpertor.ExcuteSQlCmd("exec [dbo].[InsertUserToTemp]", connectstring);
            DBOpertor.ExcuteSQlCmd("exec [dbo].[MergeUser]", connectstring);
        }
    }
}
