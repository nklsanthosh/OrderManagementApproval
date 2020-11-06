using System;
using System.Collections.Generic;

namespace OrderManagementApproval.Models
{
    public partial class ItemCategory
    {
        public ItemCategory()
        {
            ItemMaster = new HashSet<ItemMaster>();
        }

        public long ItemCategoryId { get; set; }
        public string ItemCategoryName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long? ModifiedBy { get; set; }

        public virtual ICollection<ItemMaster> ItemMaster { get; set; }
    }
}
