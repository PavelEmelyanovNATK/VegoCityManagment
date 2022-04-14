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
    /// Логика взаимодействия для EditProductScreen.xaml
    /// </summary>
    public partial class EditProductScreen : Page
    {
        private readonly EditProductViewModel _viewModel;

        public EditProductScreen()
        {
            InitializeComponent();

            _viewModel = (EditProductViewModel)DataContext;
        }

        public EditProductScreen(Guid productId, ProductsNavController productsNavController) : this()
        {
            _viewModel.Setup(productId, productsNavController);
        }
    }
}
