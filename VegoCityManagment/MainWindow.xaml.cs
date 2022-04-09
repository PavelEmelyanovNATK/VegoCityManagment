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
using VegoCityManagment.Shared.Domain;
using VegoCityManagment.Shared.Domain.Models;

namespace VegoCityManagment
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainNavController _mainNavController;

        public MainWindow()
        {
            InitializeComponent();

            _mainNavController = new MainNavController();
            
            var pageBinding = new Binding()
            {
                Source = _mainNavController,
                Path = new PropertyPath("CurrentPage"),
                Mode = BindingMode.OneWay
            };

            MainFrame.SetBinding(ContentProperty, pageBinding);

            _mainNavController.NavigateToManagmentScreen();
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}
