using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VegoCityManagment.ModuleManagment.ModuleOrders.Domain.Models
{
    public class OrderLVItem
    {
        public Guid Id { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string ClientName { get; set; }
        public string Status { get; set; }
    }
}
