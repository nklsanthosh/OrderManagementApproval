using System;
using System.Collections.Generic;

namespace OrderManagementApproval.Models
{
    public partial class ApprovalStatus
    {
        public long ApprovalStatusId { get; set; }
        public string ApprovalStatus1 { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long? ModifiedBy { get; set; }
    }
}
