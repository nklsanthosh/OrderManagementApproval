using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderManagementApproval.Models
{
    public class GridIndent
    {
        public int SlNo { get; set; }
        public string ItemCategoryName { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public string Technical_Specifications { get; set; }
        public int Quantity { get; set; }
        public string Units { get; set; }
        public string Remarks { get; set; }
    }
}
