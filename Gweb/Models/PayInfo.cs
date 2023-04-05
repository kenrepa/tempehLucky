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
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public decimal Taxes { get; set; }
        public decimal ShippingFee { get; set; }
        public string Description { get; set; }

    }
}
