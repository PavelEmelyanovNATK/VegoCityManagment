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

namespace VegoCityManagment.ModuleManagment.ModuleProducts.Presentation.Windows
{
    /// <summary>
    /// Логика взаимодействия для TextFieldWindow.xaml
    /// </summary>
    public partial class TextFieldWindow : Window
    {
        public TextFieldWindow()
        {
            InitializeComponent();
        }

        public new string? ShowDialog()
        {
            if (base.ShowDialog() == true)
                return tbLink.Text;

            return null;
        }

        public string? ShowDialog(string title)
        {
            this.Title = title;
            
            return this.ShowDialog();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
