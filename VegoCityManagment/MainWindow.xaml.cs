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

namespace VegoCityManagment
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            _viewModel = (MainWindowViewModel)DataContext;

            _viewModel.CloseAction = this.Close;
            _viewModel.MinimizeAction = () => this.WindowState = WindowState.Minimized;
            _viewModel.MaximizeAction = () =>
            {
                if(this.WindowState == WindowState.Maximized)
                    this.WindowState = WindowState.Normal;
                else
                    this.WindowState = WindowState.Maximized;
            }; 
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            while(MainFrame.CanGoBack)
                MainFrame.RemoveBackEntry();
        }
    }
}
