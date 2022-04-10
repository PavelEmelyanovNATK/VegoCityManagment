using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VegoAPI.Domain.Models
{
    public class FilteredProductsRequest
    {
        public int[] CategoriesIds { get; set; }
        public string Filter { get; set; }
        //public int PagesCount { get; set; } = 1;
    }
}
