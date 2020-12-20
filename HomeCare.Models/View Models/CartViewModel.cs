using System;
using System.Collections.Generic;
using System.Text;

namespace HomeCare.Models.View_Models
{
    public class CartViewModel
    {
        public IList<Service> ServiceList { get; set;  }
        public OrderHeader OrderHeader { get; set; }
    }
}
