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
using VegoCityManagment.Shared.Domain;

namespace VegoCityManagment.Shared.Components.VegoMessageDialog.Presentation
{
    /// <summary>
    /// Логика взаимодействия для VegoYesNoDialogWindow.xaml
    /// </summary>
    public partial class VegoYesNoDialogWindow : Window
    {
        public VegoYesNoDialogWindow()
        {
            InitializeComponent();

            var vegoWindowViewModel = new VegoWindowViewModel();
            vegoWindowViewModel.CloseAction = this.Close;
            vegoWindowViewModel.MinimizeAction = () => this.WindowState = WindowState.Minimized;
            vegoWindowViewModel.MaximizeAction = () =>
            {
                if (this.WindowState == WindowState.Maximized)
                    this.WindowState = WindowState.Normal;
                else
                    this.WindowState = WindowState.Maximized;
            };

            DataContext = vegoWindowViewModel;
        }

        public static bool? ShowDialog(string message, string title = "")
        {
            var window = new VegoMessageDialogWindow();
            window.Title = title;
            window.MessageTextBlock.Text = message;
            return window.ShowDialog();
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
