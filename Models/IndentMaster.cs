using System;
using System.Collections.Generic;

namespace OrderManagementApproval.Models
{
    public partial class IndentMaster
    {
        public long IndentId { get; set; }
        public DateTime Date { get; set; }
        public string Remarks { get; set; }
        public long LocationCodeId { get; set; }
        public long RaisedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long? ModifiedBy { get; set; }
    }
}
