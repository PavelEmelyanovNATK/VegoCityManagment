using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VegoCityManagment.Shared.Domain;

namespace VegoCityManagment.ModuleManagment.ModuleProducts.Domain.Models
{
    public class ProductLVItem
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
        public double Price { get; set; }
        public Command OnDoubleClickCommand { get; set; }
    }
}
