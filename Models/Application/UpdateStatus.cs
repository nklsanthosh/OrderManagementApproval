using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagementApproval.Models
{
    public class UpdateStatus
    {
        public string Status { get; set; }
        public string TextArea { get; set; }
    }
}
