using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace VegoCityManagment.ModuleManagment.ModuleProducts.Domain.Models
{
    public class ProductDetailModel
    {
        public int Id { get; set; }
        public Uri ImagePath { get; set; }
        public ImageSource Image
        {
            get
            {
                var converter = new ImageSourceConverter();

                return (ImageSource)converter.ConvertFrom(ImagePath);
            }
        }
        public string Title { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }
    }
}
