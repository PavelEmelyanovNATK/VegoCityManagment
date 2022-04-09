using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VegoCityManagment.Shared.Domain.Models
{
    public class AddProductRequest
    {
        public string Title { get; set; }
        public int ProductTypeId { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
    }
}
