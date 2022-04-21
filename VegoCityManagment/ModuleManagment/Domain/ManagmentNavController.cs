using MVVMBaseByNH.Domain;
using MVVMBaseByNH.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VegoCityManagment.ModuleManagment.ModuleOrders.Presentation.Pages;
using VegoCityManagment.ModuleManagment.ModuleProducts.Presentation.Pages;
using VegoCityManagment.Shared.Domain;

namespace VegoCityManagment.ModuleManagment.Domain
{
    public class ManagmentNavController : NavController
    {
        public void NavigateToProductsPage(
            DrawerController drawerController = null, 
            NavOptions options = NavOptions.GetFromBackStack
            )
        {
            if (options == NavOptions.None)
                CurrentPage = new ProductsPage(drawerController);
            else
            {
                var page = BackStack.LastOrDefault(p => p is ProductsPage);

                if (page is null)
                {
                    page = new ProductsPage(drawerController);
                    BackStack.Add(page);
                }
                else
                {
                    BackStack.Remove(page);
                    BackStack.Add(page);
                }

                CurrentPage = BackStack.Last();
            }
        }

        public void NavigateToOrdersPage(
            DrawerController drawerController = null,
            NavOptions options = NavOptions.GetFromBackStack
            )
        {
            if (options == NavOptions.None)
                CurrentPage = new OrdersPage(drawerController);
            else
            {
                var page = BackStack.LastOrDefault(p => p is OrdersPage);

                if (page is null)
                {
                    page = new OrdersPage(drawerController);
                    BackStack.Add(page);
                }
                else
                {
                    BackStack.Remove(page);
                    BackStack.Add(page);
                }

                CurrentPage = BackStack.Last();
            }
        }
    }
}
