using MVVMBaseByNH.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VegoCityManagment.Shared.Domain
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly MainNavController _mainNavController;
        public MainNavController NavController => _mainNavController;

        public MainWindowViewModel()
        {
            _mainNavController = new MainNavController();
            NavController.NavigateToManagmentScreen(NavOptions.None);
        }
    }
}
