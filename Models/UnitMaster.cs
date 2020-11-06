using System;
using System.Collections.Generic;

namespace OrderManagementApproval.Models
{
    public partial class UnitMaster
    {
        public long UnitMasterId { get; set; }
        public string Unit { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long? ModifiedBy { get; set; }
    }
}
