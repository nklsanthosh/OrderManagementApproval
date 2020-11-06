using System;
using System.Collections.Generic;

namespace OrderManagementApproval.Models
{
    public partial class IndentDetails
    {
        public long IndentDetailsId { get; set; }
        public long IndentId { get; set; }
        public long ItemCategoryId { get; set; }
        public long ItemMasterId { get; set; }
        public long UnitId { get; set; }
        public int Quantity { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long? ModifiedBy { get; set; }
    }
}
