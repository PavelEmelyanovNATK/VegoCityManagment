using MVVMBaseByNH.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VegoCityManagment.ModuleManagment.ModuleOrders.Domain.Models
{
    public class StatusLVItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsChecked { get; set; }
        public Command OnClick { get; set; }
    }
}
