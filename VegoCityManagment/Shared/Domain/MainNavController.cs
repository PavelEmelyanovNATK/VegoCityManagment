using MVVMBaseByNH.Domain;
using MVVMBaseByNH.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using VegoCityManagment.ModuleManagment.Presentation.Pages;

namespace VegoCityManagment.Shared.Domain
{
    public class MainNavController : NavController
    {
        public void NavigateToLoginingScreen() { }

        public void NavigateToRegistrationScreen() { }

        public void NavigateToManagmentScreen(NavOptions options = NavOptions.GetFromBackStack)
        {
            if (options == NavOptions.None)
                CurrentPage = new ManagmentPage();
            else
            {
                var page = BackStack.LastOrDefault(p => p is ManagmentPage);

                if(page is null)
                {
                    page = new ManagmentPage();
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
