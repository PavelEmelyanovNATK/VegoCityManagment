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

namespace VegoCityManagment.ModuleManagment.Presentation.Pages
{
    /// <summary>
    /// Логика взаимодействия для ManagmentPage.xaml
    /// </summary>
    public partial class ManagmentPage : Page
    {
        private readonly ManagmentPageViewModel _viewModel;
        public ManagmentPage()
        {
            InitializeComponent();

            _viewModel = (ManagmentPageViewModel)DataContext;;
        }

        private void ManagmentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            while(ManagmentFrame.CanGoBack)
                ManagmentFrame.RemoveBackEntry();
        }
    }
}
