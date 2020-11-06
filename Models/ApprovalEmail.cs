using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderManagementApproval.Models
{
    public class ApprovalEmail
    {
        public string Next_Approver { get; set; }
        public string Next_Approver_Name { get; set; }
        public string Raised_By { get; set; }
        public string Raised_By_Name { get; set; }
        public string Approved_By { get; set; }
        public string Approved_By_Name { get; set; }
        public string Approved_Status { get; set; }
        public string Remarks { get; set; }
    }
}
