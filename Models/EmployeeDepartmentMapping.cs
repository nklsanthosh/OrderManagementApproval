using System;
using System.Collections.Generic;

namespace OrderManagementApproval.Models
{
    public partial class EmployeeDepartmentMapping
    {
        public long EmployeeDepartmentMappingId { get; set; }
        public long EmployeeId { get; set; }
        public long DepartmentId { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long? ModifiedBy { get; set; }
    }
}
