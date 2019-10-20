using Newtonsoft.Json;
using ShortURL.Helpers;
using ShortURL.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using static ShortURL.Models.APIResponseModel;

namespace ShortURL.Controllers
{
    public class ProcessURLController : ApiController
    {
        // GET: api/ProcessURL

        // POST: api/ProcessURL/ShortenURL
        [HttpPost]
        public HttpResponseMessage ShortenURL(URLInfo urlinfo)
        {
            ApiShortURLResponseMessage response = new ApiShortURLResponseMessage();

            try
            {
                Uri uriResult;
                string finalURL = string.Empty;
                bool isURLValid = Uri.TryCreate(urlinfo.LongURL, UriKind.Absolute, out uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                if (isURLValid)
                {
                    urlinfo.ID = GetLongURLID(urlinfo.LongURL);

                    bool isInserted;
                    do
                    {
                        urlinfo.ShortURLCode = GenerateShortID();                        
                        isInserted = UpdateShortURLCode(urlinfo);
                    } while (!isInserted);

                    urlinfo.ShortURL = string.Format("{0}/{1}",ConfigurationManager.AppSettings["redirecturl"].ToString(), urlinfo.ShortURLCode);

                    response.Result = "SUCCESS";
                    response.Responsecode = HttpStatusCode.OK;
                    response.ShortURL = string.Format("{0}/{1}", ConfigurationManager.AppSettings["redirecturl"].ToString(), urlinfo.ShortURLCode);
                    response.Errorcause = "";
                    response.Errordescription = "";

                    JsonConvert.SerializeObject(response);


                }
                else
                {
                    response.Result = "FAIL";
                    response.Responsecode = HttpStatusCode.Forbidden;
                    response.ShortURL = "";
                    response.Errorcause = "Not a valid URL!";
                    response.Errordescription = "Please enter a valid URL";


                }
            }
            catch (Exception ex)
            {
                response.Result = "FAIL";
                response.Responsecode = HttpStatusCode.InternalServerError;
                response.ShortURL = "";
                response.Errorcause = "Internal Server Error";
                response.Errordescription = ex.Message;
            }


            string errorMessage = JsonConvert.SerializeObject(response);

            var httpResponse = new HttpResponseMessage(response.Responsecode);
            httpResponse.Content = new StringContent(errorMessage, System.Text.Encoding.UTF8, "application/json");
            return httpResponse;


        }

        public string GenerateShortID()
        {
            Random random = new Random();
            int length = Convert.ToInt32(ConfigurationManager.AppSettings["shortkeylength"]);
            const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
                              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [HttpGet]
        public string ExpandURL(string u)
        {
            Uri uri = new Uri(u);

            string longURL = GetLongURL(uri.AbsolutePath.TrimStart('/'));

            return longURL;
        }

        [HttpGet]
        public IHttpActionResult RedirectURL(string id)
        {
            string longURL = GetLongURL(id);


            if (string.IsNullOrEmpty(longURL))
            {
                return Redirect(ConfigurationManager.AppSettings["redirecturl"].ToString());
            }
            else
            {
                return Redirect(longURL);
            }


        }


        public long GetLongURLID(string longURL)
        {
            long urlID = 0;
            SQLHelper sql = new SQLHelper();
            using (var Conn = sql.SqlConnection())
            {
                Conn.Open();

                SqlCommand sqlCommand = new SqlCommand("scp_mydb_ins_URLInfo", Conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@LongURL", longURL);

                //SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                //DataSet ds = new DataSet();
                //adapter.Fill(ds);

                urlID = (long)sqlCommand.ExecuteScalar();

                Conn.Close();
                Conn.Dispose();

            }

            return urlID;
        }

        public bool UpdateShortURLCode(URLInfo urlinfo)
        {
            bool isInserted = false;
            SQLHelper sql = new SQLHelper();
            using (var Conn = sql.SqlConnection())
            {
                Conn.Open();

                SqlCommand sqlCommand = new SqlCommand("scp_mydb_upd_URLInfo", Conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@Id", urlinfo.ID);
                sqlCommand.Parameters.AddWithValue("@ShortCode", urlinfo.ShortURLCode);
                sqlCommand.Parameters.AddWithValue("@LongURL", urlinfo.LongURL);

                isInserted = (bool)sqlCommand.ExecuteScalar();

                Conn.Close();
                Conn.Dispose();

            }

            return isInserted;
        }

        public string GetLongURL(string id)
        {
            string longURL = string.Empty;

            SQLHelper sql = new SQLHelper();
            using (var Conn = sql.SqlConnection())
            {
                Conn.Open();

                SqlCommand sqlCommand = new SqlCommand("scp_mydb_get_LongURL", Conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@ShortCode", id);

                //SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                //DataSet ds = new DataSet();
                //adapter.Fill(ds);

                longURL = sqlCommand.ExecuteScalar().ToString();

                Conn.Close();
                Conn.Dispose();
            }

            return longURL;

        }

        public URLInfo GetURLInfo(string shorturl)
        {
            URLInfo info = new URLInfo();

            try
            {
                SQLHelper sql = new SQLHelper();
                using (var Conn = sql.SqlConnection())
                {
                    Conn.Open();

                    SqlCommand sqlCommand = new SqlCommand("scp_mydb_get_URLInfo", Conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    sqlCommand.Parameters.AddWithValue("@ShortCode", shorturl);

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    DataRow row = ds.Tables[0].Rows[0];

                    info.ID = !row.IsNull("ID") ? Convert.ToInt64(row["ID"]) : 0;
                    info.LongURL = !row.IsNull("LongURL") ? row["LongURL"].ToString() : string.Empty;
                    info.ShortURLCode = !row.IsNull("ShortURLCode") ? row["ShortURLCode"].ToString() : string.Empty;
                    if(!string.IsNullOrEmpty(info.ShortURLCode))
                    {
                        info.ShortURL = string.Format("{0}/{1}", ConfigurationManager.AppSettings["redirecturl"].ToString(), info.ShortURLCode);
                    }
                    info.ClicksCount = !row.IsNull("ClicksCount") ? Convert.ToInt32(row["ClicksCount"]) : 0;



                    Conn.Close();
                    Conn.Dispose();
                }
            }
            catch(Exception ex)
            {

            }
            return info;
        }

        
    }
}
