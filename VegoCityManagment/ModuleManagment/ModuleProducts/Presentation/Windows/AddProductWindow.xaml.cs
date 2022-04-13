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
using System.Windows.Shapes;
using VegoCityManagment.ModuleManagment.ModuleProducts.Domain;

namespace VegoCityManagment.ModuleProducts.Presentation.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        private readonly AddProductWindowViewModel _viewModel;
        public AddProductWindow()
        {
            InitializeComponent();

            _viewModel = (AddProductWindowViewModel)DataContext;

            _viewModel.CloseWindow = this.Close;
        }

        private void Description_TextChanged(object sender, TextChangedEventArgs e)
        {
            _viewModel.ProductDescription = ((TextBox)sender).Text;
        }
    }
}
