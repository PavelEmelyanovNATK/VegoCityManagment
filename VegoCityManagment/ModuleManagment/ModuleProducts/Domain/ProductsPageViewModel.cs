using MVVMBaseByNH.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VegoAPI.Domain;
using VegoAPI.Domain.Models;
using VegoCityManagment.ModuleManagment.Domain;
using VegoCityManagment.ModuleManagment.ModuleProducts.Domain.Models;
using VegoCityManagment.ModuleManagment.ModuleProducts.Presentation.Windows;
using VegoCityManagment.Shared.Domain;
namespace VegoCityManagment.ModuleManagment.ModuleProducts.Domain
{
    public class ProductsPageViewModel : ViewModelBase
    {
        private DrawerController _drawerController;
        private readonly ProductsNavController _productsNavController;
        public ProductsNavController ProductsNavController => _productsNavController;

        public ProductsPageViewModel()
        {
            _drawerController = new DrawerController();
            PropertyWasChanged("ProductsNavController");
            _productsNavController = new ProductsNavController();
        }

        public void Setup(DrawerController drawerController)
        {
            _drawerController = drawerController;
            ProductsNavController.NavigateToProductsList(ProductsNavController, _drawerController);
        }
    }
}
