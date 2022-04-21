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
using VegoCityManagment.ModuleManagment.ModuleOrders.Domain;

namespace VegoCityManagment.ModuleManagment.ModuleOrders.Presentation.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrdersListScreen.xaml
    /// </summary>
    public partial class OrdersListScreen : Page
    {
        private readonly OrdersListViewModel _viewModel;
        public OrdersListScreen()
        {
            InitializeComponent();

            _viewModel = (OrdersListViewModel)DataContext;
        }

        public OrdersListScreen(DrawerController drawerController, OrdersNavController navController) : this()
        {
            _viewModel.Setup(drawerController, navController);
        }
    }
}
