using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Gweb.Models
{
    public class PayInfo
    {
        public string ItemName { get; set; }
        public string Currency { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public string SubTotal { get; set; }
        public string Total { get; set; }
        public string Taxes { get; set; }
        public string ShippingFee { get; set; }
        public string Description { get; set; }

    }
}
