using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderManagementApproval.Models;
using UpdateStatus = OrderManagementApproval.Models.UpdateStatus;

namespace OrderManagementApproval.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index(long IndentNo)
        {
            ViewBag.IndentNo = "12345";
            return View();
        }


        [HttpPost]
        public IActionResult IndexSubmit(IFormCollection loginCredentials)
        {
            string userName = loginCredentials["UserName"];
            string password = loginCredentials["Password"];
            string indentNo = loginCredentials["IndentNo"];
            ViewBag.IndentNo= indentNo;

            try
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
                    string query = "update IndentApproval set ApprovalStatusID = (select ApprovalStatusID from ApprovalStatus where ApprovalStatus = " + "'" + status + "'" + "), Remarks= " + "'" + textArea + "'" + " where IndentID = " + indentNumber;
                    connection.Open();
                    SqlCommand testCMD = new SqlCommand(query, connection);
                }
                ViewBag.Message = "Indent " + indentNumber + " is " + status + " successfully.";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex;
                return View();
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
