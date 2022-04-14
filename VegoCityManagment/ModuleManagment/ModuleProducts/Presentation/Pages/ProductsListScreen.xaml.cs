using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VegoCityManagment.ModuleManagment.Domain;
using VegoCityManagment.ModuleManagment.ModuleProducts.Domain;

namespace VegoCityManagment.ModuleManagment.ModuleProducts.Presentation.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductsListScreen.xaml
    /// </summary>
    public partial class ProductsListScreen : Page
    {
        private readonly ProductsListViewModel _viewModel;
        public ProductsListScreen()
        {
            InitializeComponent();

            _viewModel = (ProductsListViewModel)DataContext;
        }

        public ProductsListScreen(ProductsNavController navController, DrawerController drawerController) : this()
        {
            _viewModel.Setup(drawerController, navController);
        }
    }
}
