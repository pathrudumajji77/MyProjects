using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ShortURL.Helpers
{
    public class SQLHelper
    {
        public SQLHelper()
        {

        }
        public SqlConnection SqlConnection()
        {
            try
            {
                string sConstr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
                SqlConnection Conn = new SqlConnection(sConstr);
                return Conn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}