using System;
using System.Collections.Generic;

namespace OrderManagementApproval.Models
{
    public partial class RolesMaster
    {
        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long? ModifiedBy { get; set; }
    }
}
