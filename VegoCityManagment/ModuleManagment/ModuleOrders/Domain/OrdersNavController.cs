using MVVMBaseByNH.Domain;
using MVVMBaseByNH.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VegoCityManagment.ModuleManagment.Domain;
using VegoCityManagment.ModuleManagment.ModuleOrders.Presentation.Pages;

namespace VegoCityManagment.ModuleManagment.ModuleOrders.Domain
{
    public class OrdersNavController : NavController
    {
        public void NavigateToOrdersList(
            OrdersNavController ordersNavController,
            DrawerController drawerController = null,
            NavOptions options = NavOptions.GetFromBackStack)
        {
            if (options == NavOptions.None)
                CurrentPage = new OrdersListScreen(drawerController, ordersNavController);
            else
            {
                var page = BackStack.LastOrDefault(p => p is OrdersListScreen);

                if (page is null)
                {
                    page = new OrdersListScreen(drawerController, ordersNavController);
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
