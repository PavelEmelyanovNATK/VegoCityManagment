using MVVMBaseByNH.Domain;
using MVVMBaseByNH.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VegoCityManagment.ModuleManagment.Domain;
using VegoCityManagment.ModuleManagment.ModuleProducts.Presentation.Pages;

namespace VegoCityManagment.ModuleManagment.ModuleProducts.Domain
{
    public class ProductsNavController : NavController
    {
        public void NavigateToProductsList(
            ProductsNavController productsNavController,
            DrawerController drawerController = null,
            NavOptions options = NavOptions.GetFromBackStack)
        {
            if (options == NavOptions.None)
                CurrentPage = new ProductsListScreen(productsNavController, drawerController);
            else
            {
                var page = BackStack.LastOrDefault(p => p is ProductsListScreen);

                if (page is null)
                {
                    page = new ProductsListScreen(productsNavController, drawerController);
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

        public void NavigateToAddProductScreen(
            ProductsNavController productsNavController, 
            NavOptions options = NavOptions.GetFromBackStack)
        {
            if (options == NavOptions.None)
                CurrentPage = new AddProductScreen(productsNavController);
            else
            {
                var page = BackStack.LastOrDefault(p => p is AddProductScreen);

                if (page is null)
                {
                    page = new AddProductScreen(productsNavController);
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

        public void NavigateToEditProductScreen(
            Guid productId,
            ProductsNavController productsNavController,
            NavOptions options = NavOptions.GetFromBackStack)
        {
            if (options == NavOptions.None)
                CurrentPage = new EditProductScreen(productId, productsNavController);
            else
            {
                var page = BackStack.LastOrDefault(p => p is AddProductScreen);

                if (page is null)
                {
                    page = new EditProductScreen(productId, productsNavController);
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
