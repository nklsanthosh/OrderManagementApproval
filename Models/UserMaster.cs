using System;
using System.Collections.Generic;

namespace OrderManagementApproval.Models
{
    public partial class UserMaster
    {
        public long UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public long EmployeeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long? ModifiedBy { get; set; }
    }
}
