using System;
using System.Collections.Generic;

namespace OrderManagementApproval.Models
{
    public partial class RoleMapping
    {
        public long RoleMappingId { get; set; }
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long? ModifiedBy { get; set; }
    }
}
