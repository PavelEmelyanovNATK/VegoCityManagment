using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VegoCityManagment.ModuleManagment.ModuleProducts.Presentation.Pages;
using VegoCityManagment.Shared.Domain;

namespace VegoCityManagment.ModuleManagment.Domain
{
    public class ManagmentNavController : NavController
    {
        public void NavigateToProductsPage()
            => CurrentPage = new ProductsPage();

        public void NavigateToOrdersPage()
        { }
    }
}
