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
using VegoCityManagment.ModuleManagment.ModuleProducts.Domain;

namespace VegoCityManagment.ModuleManagment.ModuleProducts.Presentation.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddProductScreen.xaml
    /// </summary>
    public partial class AddProductScreen : Page
    {
        private readonly AddProductViewModel _viewModel;
        public AddProductScreen()
        {
            InitializeComponent();

            _viewModel = (AddProductViewModel)DataContext;
        }

        public AddProductScreen(ProductsNavController navController) : this()
        {
            _viewModel.Setup(navController);
        }
    }
}
