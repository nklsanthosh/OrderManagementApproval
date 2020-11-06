using System;
using System.Collections.Generic;

namespace OrderManagementApproval.Models
{
    public partial class LocationCode
    {
        public long LocationCodeId { get; set; }
        public long LocationId { get; set; }
        public string LocationName { get; set; }
    }
}
