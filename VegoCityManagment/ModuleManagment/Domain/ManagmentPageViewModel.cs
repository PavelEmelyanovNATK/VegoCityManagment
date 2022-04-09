using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using VegoCityManagment.ModuleManagment.ModuleProducts.Presentation.Pages;
using VegoCityManagment.Shared.Domain;

namespace VegoCityManagment.ModuleManagment.Domain
{
    public class ManagmentPageViewModel : ViewModelBase
    {
        private ManagmentNavController _managmentNavController;

        public Page CurrentPage => _managmentNavController?.CurrentPage;

        public ManagmentPageViewModel()
        {
            _managmentNavController = new ManagmentNavController();
            _managmentNavController.NavigateToProductsPage();
        }

        private Command _goToProductsCommand;
        public Command GoToProductsCommand
            => _goToProductsCommand ??= new Command(
                o =>
                {
                    _managmentNavController.NavigateToProductsPage();
                },
                c => CurrentPage is not ProductsPage);

        private Command _goToOrdersCommand;
        public Command GoToOrdersCommand
            => _goToOrdersCommand ??= new Command(
                o =>
                {
                    _managmentNavController.NavigateToOrdersPage();
                },
                c => false);
    }
}
