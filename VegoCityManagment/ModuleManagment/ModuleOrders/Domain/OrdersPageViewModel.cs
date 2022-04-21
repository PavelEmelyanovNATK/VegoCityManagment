using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VegoCityManagment.ModuleManagment.Domain;
using VegoCityManagment.Shared.Domain;

namespace VegoCityManagment.ModuleManagment.ModuleOrders.Domain
{
    public class OrdersPageViewModel : ViewModelBase
    {
        private DrawerController _drawerController;
        private readonly OrdersNavController _productsNavController;
        public OrdersNavController OrdersNavController => _productsNavController;

        public OrdersPageViewModel()
        {
            _drawerController = new DrawerController();
            PropertyWasChanged("ProductsNavController");
            _productsNavController = new OrdersNavController();
        }

        public void Setup(DrawerController drawerController)
        {
            _drawerController = drawerController;
            OrdersNavController.NavigateToOrdersList(OrdersNavController, _drawerController);
        }
    }
}
