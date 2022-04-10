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

        public void NavigateToManagmentScreen()
            => CurrentPage = new ManagmentPage();
    }
}
