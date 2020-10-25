using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using OrderManagementApproval.Models;
using UpdateStatus = OrderManagementApproval.Models.UpdateStatus;


namespace OrderManagementApproval.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        const string SessionUserID = "_UserId";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        [Route("{Id?}")]
        [Route("Home")]
        [Route("Home/{Id?}")]
        public IActionResult Index(long Id, string ErrorMessage = "")
        {
            ViewBag.IndentNo = Id;
            ViewBag.ErrorMessage = ErrorMessage;
            return View();
        }

        private bool LogInCheck(string userName, string password, long indentNo)
        {
            bool isAuthenticated = false;
            //TODO: Check the user if it is admin or normal user, (true-Admin, false- Normal user)  
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ToString()))
                {
                    // string query = "select Count(*) from [dbo].[UserMaster] where email= " + "'" + userName + "'" + " and   password = " + "'" + password + "'";

                    string query = "select ApprovalId from IndentApproval where ApprovalID=  (select userid from UserMaster where email= " + "'" + userName + "'" + " and   password = " + "'" + password + "'" + ") and IndentID = " + indentNo;

                    connection.Open();
                    SqlCommand testCMD = new SqlCommand(query, connection);
                    SqlDataReader sdr = testCMD.ExecuteReader();

                    while (sdr.Read())
                    {
                        long userFound = Convert.ToInt64(sdr[0]);
                        if (userFound != 0)
                        {
                            HttpContext.Session.SetString(SessionUserID, userFound.ToString());
                            isAuthenticated = true;
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return isAuthenticated;
        }

        [HttpPost]
        public IActionResult IndexSubmit(IFormCollection loginCredentials)
        {
            string userName = loginCredentials["UserName"];
            string password = loginCredentials["Password"];
            long indentNo = Convert.ToInt64(loginCredentials["IndentNo"]);
            ViewBag.IndentNo = indentNo;

            try
            {
                bool isAuthenticated = LogInCheck(userName, password, indentNo);
                if (isAuthenticated)
                {
                    List<ApprovalStatus> approvalStatusList = new List<ApprovalStatus>();
                    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ToString()))
                    {
                        connection.Open();
                        SqlCommand testCMD = new SqlCommand("select ApprovalStatusID , ApprovalStatus from ApprovalStatus", connection);

                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(testCMD);

                        DataSet dataSet = new DataSet();
                        sqlDataAdapter.Fill(dataSet);

                        int counter = 0;

                        while (counter < dataSet.Tables[0].Rows.Count)
                        {
                            ApprovalStatus approvalStatus = new ApprovalStatus();
                            approvalStatus.Id = Convert.ToInt32(dataSet.Tables[0].Rows[counter]["ApprovalStatusID"]);
                            approvalStatus.Status = Convert.ToString(dataSet.Tables[0].Rows[counter]["ApprovalStatus"]);
                            approvalStatusList.Add(approvalStatus);
                            counter++;
                        }
                    }
                    return View(approvalStatusList);
                }
                else
                {
                    var routeValues = new RouteValueDictionary {
                             { "id", indentNo },
                            { "ErrorMessage",  "Please enter valid Credentials / You are not authorized to apporve IndentNo "+ indentNo }
                            };
                    return RedirectToAction("Index", routeValues);
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult UpdateIndentStatus(IFormCollection updateStatus)
        {
            string indentNumber = updateStatus["IndentNo"];
            string status = updateStatus["UpdateStatus"];
            string textArea = updateStatus["TextArea"];
            try
            {
                List<ApprovalStatus> approvalStatusList = new List<ApprovalStatus>();
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ToString()))
                {
                    long ApprovalId = Convert.ToInt64(HttpContext.Session.GetString(SessionUserID));

                    //string query = "update IndentApproval set ApprovalStatusID = (select ApprovalStatusID from ApprovalStatus where ApprovalStatus = " + "'" + status + "'" + "), Remarks= " + "'" + textArea + "'" + " where IndentID = " + indentNumber;

                    string query = "update IndentApproval set ApprovalStatusID = (select ApprovalStatusID from ApprovalStatus where ApprovalStatus = " + "'" + status + "'" + "), Remarks= " + "'" + textArea + "'" + " , ModifiedBy = " + "'" + ApprovalId + "'" + " , ModifiedDate = +  GETDATE()  where IndentID = " + indentNumber;
                    connection.Open();
                    SqlCommand testCMD = new SqlCommand(query, connection);
                    int a = testCMD.ExecuteNonQuery();
                    if (a != 0)
                    {

                        string query1 = "select email from Employee where EmployeeID = (select createdby from IndentApproval where indentid =" + indentNumber + ")";
                    
                        SqlCommand testCMD1 = new SqlCommand(query1, connection);
                        string mailTo = "";
                        using (SqlDataReader dr = testCMD1.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                mailTo = dr[0].ToString();
                            }
                        }
                        if (mailTo != null)
                        {
                            bool email = SendMail(Convert.ToInt64(indentNumber), status, mailTo);
                            if (email)
                            {
                                ViewBag.Message = "Indent " + indentNumber + " is updated successfully to " + status;
                            }
                            else
                            {
                                ViewBag.Message = "Indent " + indentNumber + " is not updated successfully.";
                            }
                        }

                    }
                    else
                    {
                        ViewBag.Message = "Indent " + indentNumber + " is not updated successfully.";
                    }
                    return View();
                }

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex;
                return View();
            }
        }

        private bool SendMail(long indentNumber, string status, string mailTo)
        {
            bool mailSent = false;
            try
            {
                string message = DateTime.Now + " In SendMail\n";

                using (MailMessage mm = new MailMessage())
                {
                    mm.From = new MailAddress(Convert.ToString(ConfigurationManager.AppSettings["MailFrom"]));

                    mm.To.Add(mailTo);

                    mm.Subject = ConfigurationManager.AppSettings["Subject"];
                    mm.Body = " Your Indent Number" + indentNumber + " has been " + status;
                    mm.IsBodyHtml = false;

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = ConfigurationManager.AppSettings["Host"];
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential(ConfigurationManager.AppSettings["Username"],
                        ConfigurationManager.AppSettings["Password"]);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = int.Parse(ConfigurationManager.AppSettings["Port"]);

                    message = DateTime.Now + " Sending Mail\n";
                    smtp.Send(mm);
                    message = DateTime.Now + " Mail Sent\n";

                    System.Threading.Thread.Sleep(3000);
                    mailSent = true;
                }
                return mailSent;
            }
            catch (Exception ex)
            {
                return mailSent;
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
