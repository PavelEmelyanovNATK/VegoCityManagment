using MVVMBaseByNH.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VegoCityManagment.ModuleManagment.ModuleOrders.Presentation.Pages;
using VegoCityManagment.ModuleManagment.ModuleProducts.Presentation.Pages;
using VegoCityManagment.Shared.Domain;

namespace VegoCityManagment.ModuleManagment.Domain
{
    public class ManagmentPageViewModel : ViewModelBase
    {
        private readonly ManagmentNavController _managmentNavController;
        private readonly DrawerController _drawerController;
        public ManagmentNavController ManagmentNavController => _managmentNavController;
        public DrawerController DrawerController => _drawerController;

        public ManagmentPageViewModel()
        {
            _managmentNavController = new ManagmentNavController();
            _drawerController = new DrawerController();

            ManagmentNavController.NavigateToProductsPage(DrawerController);
        }

        private Command _goToProductsCommand;
        public Command GoToProductsCommand
            => _goToProductsCommand ??= new Command(
                _ =>
                {
                    ManagmentNavController.NavigateToProductsPage(DrawerController);
                    //DrawerController.CloseDrawer();
                },
                _ => ManagmentNavController.CurrentPage is not ProductsPage);

        private Command _goToOrdersCommand;
        public Command GoToOrdersCommand
            => _goToOrdersCommand ??= new Command(
                _ =>
                {
                    ManagmentNavController.NavigateToOrdersPage();
                    //DrawerController.CloseDrawer();
                },
                _ => ManagmentNavController.CurrentPage is not OrdersPage);
    }
}
