using System;
using System.Collections.Generic;

namespace OrderManagementApproval.Models
{
    public partial class ItemMaster
    {
        public long ItemMasterId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public string TechnicalSpecification { get; set; }
        public long ItemCategoryId { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long? ModifiedBy { get; set; }

        public virtual ItemCategory ItemCategory { get; set; }
    }
}
