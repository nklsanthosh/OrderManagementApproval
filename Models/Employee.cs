using System;
using System.Collections.Generic;

namespace OrderManagementApproval.Models
{
    public partial class Employee
    {
        public long EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public long? DesignationId { get; set; }
        public long StatusId { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long? ModifiedBy { get; set; }
        public long ReportsTo { get; set; }
    }
}
