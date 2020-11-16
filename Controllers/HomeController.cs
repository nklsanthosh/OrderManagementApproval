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
        const string ApprovalStatusId = "_ApprovalStatusId";
        ApprovalEmail approvalEmail = new ApprovalEmail();
        private string ApproverName = "";
        private string CreatedEmail = "";
        private string POCreatorEmail = "";
        private long indentNo;

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
                    connection.Open();
                    SqlCommand testCMD = new SqlCommand("indent_approval_check", connection);
                    testCMD.CommandType = CommandType.StoredProcedure;

                    testCMD.Parameters.Add(new SqlParameter("@IndentNo", System.Data.SqlDbType.BigInt, 50) { Value = indentNo });
                    testCMD.Parameters.Add(new SqlParameter("@Email", System.Data.SqlDbType.VarChar, 50) { Value = userName });
                    testCMD.Parameters.Add(new SqlParameter("@Password", System.Data.SqlDbType.VarChar, 50) { Value = password });

                    SqlDataAdapter approvalDA = new SqlDataAdapter(testCMD);
                    DataSet approvalInfo = new DataSet();
                    approvalDA.Fill(approvalInfo);

                    if (approvalInfo.Tables[0].Rows.Count > 0)
                    {
                        long userFound = Convert.ToInt64(approvalInfo.Tables[0].Rows[0]["ApproverId"]);
                        long AppStatusId = Convert.ToInt64(approvalInfo.Tables[0].Rows[0]["ApprovalStatusId"]);
                        if (AppStatusId != 0)
                        {
                            HttpContext.Session.SetString(SessionUserID, userFound.ToString());
                            HttpContext.Session.SetString(ApprovalStatusId, AppStatusId.ToString());
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


        public IActionResult RenderMenu()
        {
            SaveIndent saveIndent = new SaveIndent();
            ViewBag.IndentNo = indentNo;
            return PartialView("_MenuBar", saveIndent.GridIndents);
        }

        [Route("Home/GetIndentDetails/{Id}")]
        public SaveIndent GetIndentDetails(long Id)
        {
            SaveIndent saveIndent = GetIndent(Id);
            return saveIndent;
        }

        private SaveIndent GetIndent(long Id)
        {
            SaveIndent saveIndent = new SaveIndent();
            long ApprovalId = Convert.ToInt64(HttpContext.Session.GetString(SessionUserID));
            try
            {
                List<GridIndent> gridIndents = new List<GridIndent>();

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ToString()))
                {
                    connection.Open();
                    SqlCommand testCMD = new SqlCommand("GetIndent", connection);
                    testCMD.CommandType = CommandType.StoredProcedure;
                    testCMD.Parameters.Add(new SqlParameter("@IndentID", System.Data.SqlDbType.BigInt, 50) { Value = Id });
                    testCMD.Parameters.Add(new SqlParameter("@UserID", System.Data.SqlDbType.VarChar, 300) { Value = ApprovalId });
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(testCMD);

                    DataSet dataSet = new DataSet();
                    sqlDataAdapter.Fill(dataSet);
                    int counter = 0;
                    while (counter < dataSet.Tables[0].Rows.Count)
                    {
                        saveIndent.Date = Convert.ToDateTime(dataSet.Tables[0].Rows[counter]["Date"]);
                        saveIndent.LocationCode = Convert.ToInt64(dataSet.Tables[0].Rows[counter]["LocationCode"]);
                        saveIndent.IndentRemarks = Convert.ToString(dataSet.Tables[0].Rows[counter]["Remarks"]);
                        saveIndent.ApproverName = Convert.ToString(dataSet.Tables[0].Rows[counter]["Approver"]);
                        saveIndent.ApprovalID = Convert.ToInt64(dataSet.Tables[0].Rows[counter]["Approver ID"]);
                        saveIndent.ApprovalStatus = Convert.ToString(dataSet.Tables[0].Rows[counter]["ApprovalStatus"]);

                        GridIndent gridIndent = new GridIndent();
                        gridIndent.SlNo = counter + 1;
                        gridIndent.ItemCategoryName = Convert.ToString(dataSet.Tables[0].Rows[counter]["ItemCategoryName"]);
                        gridIndent.ItemName = Convert.ToString(dataSet.Tables[0].Rows[counter]["ItemName"]);
                        gridIndent.ItemCode = Convert.ToString(dataSet.Tables[0].Rows[counter]["ItemCode"]);
                        gridIndent.Units = Convert.ToString(dataSet.Tables[0].Rows[counter]["Unit"]);
                        gridIndent.Description = Convert.ToString(dataSet.Tables[0].Rows[counter]["Description"]);
                        gridIndent.Technical_Specifications = Convert.ToString(dataSet.Tables[0].Rows[counter]["TechnicalSpecification"]);
                        gridIndent.Quantity = Convert.ToInt32(dataSet.Tables[0].Rows[counter]["Quantity"]);
                        gridIndent.Remarks = Convert.ToString(dataSet.Tables[0].Rows[counter]["Item Remarks"]);
                        gridIndents.Add(gridIndent);

                        saveIndent.Email = Convert.ToString(dataSet.Tables[0].Rows[counter]["Email"]);
                        counter++;
                    }
                    dataSet.Dispose();
                }
                saveIndent.GridIndents = gridIndents;
                return saveIndent;
            }
            catch (Exception ex)
            {
                ////log.Error("Error while fetching Indent information: " + ex.StackTrace);
                return saveIndent;
            }
        }

        [HttpPost]
        public IActionResult IndexSubmit(IFormCollection loginCredentials)
        {
            string userName = loginCredentials["UserName"];
            string password = loginCredentials["Password"];
            indentNo = Convert.ToInt64(loginCredentials["IndentNo"]);
            ViewBag.IndentNo = indentNo;

            try
            {
                bool isAuthenticated = LogInCheck(userName, password, indentNo);
                if (isAuthenticated)
                {
                    List<ModelApprovalStatus> approvalStatusList = new List<ModelApprovalStatus>();
                    long ApprovalStatusId = Convert.ToInt64(HttpContext.Session.GetString("_ApprovalStatusId"));
                    string ApprovalStatus = "";
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
                            ModelApprovalStatus approvalStatus = new ModelApprovalStatus();
                            approvalStatus.Id = Convert.ToInt32(dataSet.Tables[0].Rows[counter]["ApprovalStatusID"]);
                            approvalStatus.Status = Convert.ToString(dataSet.Tables[0].Rows[counter]["ApprovalStatus"]);
                            if (approvalStatus.Id == ApprovalStatusId)
                            {
                                ApprovalStatus = approvalStatus.Status;
                            }
                            approvalStatusList.Add(approvalStatus);
                            counter++;
                        }
                    }
                    ViewBag.ApprovalStatus = ApprovalStatus;
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
                //List<ModelApprovalStatus> approvalStatusList = new List<ModelApprovalStatus>();
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ToString()))
                {
                    long ApprovalId = Convert.ToInt64(HttpContext.Session.GetString(SessionUserID));

                    connection.Open();
                    SqlCommand testCMD = new SqlCommand("IndentApprovalStatus", connection);
                    testCMD.CommandType = CommandType.StoredProcedure;

                    testCMD.Parameters.Add(new SqlParameter("@IndentNumber", System.Data.SqlDbType.BigInt, 50) { Value = indentNumber });
                    testCMD.Parameters.Add(new SqlParameter("@ApproverId", System.Data.SqlDbType.BigInt, 50) { Value = ApprovalId });
                    testCMD.Parameters.Add(new SqlParameter("@Status", System.Data.SqlDbType.VarChar, 20) { Value = status });
                    testCMD.Parameters.Add(new SqlParameter("@Remarks", System.Data.SqlDbType.VarChar, 100) { Value = textArea });

                    SqlDataAdapter approvalDA = new SqlDataAdapter(testCMD);
                    DataSet approvalInfo = new DataSet();
                    approvalDA.Fill(approvalInfo);

                    if (approvalInfo.Tables[0].Rows.Count > 0)
                    {
                        ApproverName = approvalInfo.Tables[0].Rows[0]["ApproverName"].ToString();
                        CreatedEmail = approvalInfo.Tables[0].Rows[0]["CreatedEmail"].ToString();
                        POCreatorEmail = approvalInfo.Tables[0].Rows[0]["POCreatorEmail"].ToString();
                    }
                    if (textArea == null || textArea == "")
                    {
                        ViewBag.Message = "Indent " + indentNumber + " is updated successfully to " + status + " by " + ApproverName;
                    }

                    else
                    {
                        ViewBag.Message = "Indent " + indentNumber + " is updated successfully to " + status + "With Remarks" + textArea + " by " + ApproverName;
                    }

                    SendMail(Convert.ToInt64(indentNumber), status, textArea, ApproverName);

                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex;
                return View();
            }
        }

        private bool SendMail(long indentNumber, string status, string textArea, string ApproverName)
        {
            bool mailSent = false;
            try
            {
                string message = DateTime.Now + " In SendMail\n";

                using (MailMessage mm = new MailMessage())
                {
                    mm.From = new MailAddress(Convert.ToString(ConfigurationManager.AppSettings["MailFrom"]));
                    mm.To.Add(CreatedEmail);
                    mm.Subject = "Indent Number - " + indentNumber;

                    if (textArea == null || textArea == "")
                    {
                        mm.Body = " Your Indent Number " + indentNumber + " is " + status + " by " + ApproverName;
                    }

                    else
                    {
                        mm.Body = " Your Indent Number " + indentNumber + " is " + status + " by " + ApproverName + " with Remarks: " + textArea;
                    }

                    mm.IsBodyHtml = false;

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = ConfigurationManager.AppSettings["Host"];
                    smtp.EnableSsl = false;
                    NetworkCredential NetworkCred = new NetworkCredential(ConfigurationManager.AppSettings["Username"],
                        ConfigurationManager.AppSettings["Password"]);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = int.Parse(ConfigurationManager.AppSettings["Port"]);

                    message = DateTime.Now + " Sending Mail\n";
                    smtp.Send(mm);
                    message = DateTime.Now + " Mail Sent\n";

                    System.Threading.Thread.Sleep(3000);

                    if (status == "Approved")
                    {
                        mailSent = SendPOMail(indentNumber, status, textArea, ApproverName, POCreatorEmail);
                    }
                    return mailSent;
                }
            }
            catch (Exception ex)
            {
                return mailSent;
            }
        }

        private bool SendPOMail(long indentNumber, string status, string textArea, string ApproverName, string POCreatorEmail)
        {
            bool mailSent = false;
            try
            {
                string message = DateTime.Now + " In SendMail\n";

                using (MailMessage mm = new MailMessage())
                {
                    mm.From = new MailAddress(Convert.ToString(ConfigurationManager.AppSettings["MailFrom"]));
                    mm.To.Add(POCreatorEmail);
                    mm.Subject = "Indent Number - " + indentNumber + " PO Creation";

                    if (textArea == null || textArea == "")
                    {
                        mm.Body = " Your Indent Number " + indentNumber + " is " + status + " by " + ApproverName + " and ready for PO Creation";
                    }

                    else
                    {
                        mm.Body = " Your Indent Number " + indentNumber + " is " + status + " by " + ApproverName + " with Remarks: " + textArea + " and ready for PO Creation";
                    }

                    mm.IsBodyHtml = false;

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = ConfigurationManager.AppSettings["Host"];
                    smtp.EnableSsl = false;
                    NetworkCredential NetworkCred = new NetworkCredential(ConfigurationManager.AppSettings["Username"],
                        ConfigurationManager.AppSettings["Password"]);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = int.Parse(ConfigurationManager.AppSettings["Port"]);

                    message = DateTime.Now + " Sending Mail\n";
                    smtp.Send(mm);
                    message = DateTime.Now + " Mail Sent\n";

                    System.Threading.Thread.Sleep(3000);
                    mailSent = true;
                    return mailSent;
                }
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
