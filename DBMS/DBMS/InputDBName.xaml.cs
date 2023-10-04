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

namespace DBMS
{
    /// <summary>
    /// Логика взаимодействия для InputDBName.xaml
    /// </summary>
    public partial class InputDBName : Window
    {
        public string DatabaseName { get; private set; }
        public InputDBName()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DatabaseName = NameTextBox.Text;
            if (string.IsNullOrEmpty(DatabaseName))
            {
                MessageBox.Show("Please enter a name for the database.");
                return;
            }
            this.Close();
        }
    }
}
