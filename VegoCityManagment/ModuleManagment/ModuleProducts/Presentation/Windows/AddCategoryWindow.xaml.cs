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

namespace VegoCityManagment.ModuleManagment.ModuleProducts.Presentation.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddCategoryWindow.xaml
    /// </summary>
    public partial class AddCategoryWindow : Window
    {
        private readonly AddCategoryWindowViewModel _viewModel;
        public AddCategoryWindow()
        {
            InitializeComponent();

            _viewModel = (AddCategoryWindowViewModel)DataContext;

            _viewModel.CloseWindow = this.Close;
        }
    }
}
