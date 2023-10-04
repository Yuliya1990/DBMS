using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для Search.xaml
    /// </summary>
    public partial class Search : Window
    {
        private Models.Table _table;
        private ObservableCollection<object> rowResult = new ObservableCollection<object>();
        private string condition;
        public Search(Models.Table table)
        {
            _table = table;
            InitializeComponent();
            GridResult.AutoGenerateColumns = false;
            GridResult.ItemsSource = rowResult;
            for (int i = 0; i < _table.Attributes.Count; i++)
            {
                DataGridTextColumn column = new DataGridTextColumn();
                column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                column.Header = _table.Attributes[i].Name;
                column.Binding = new Binding("[" + i + "]"); // Привязка к свойству с соответствующим именем
                GridResult.Columns.Add(column);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            rowResult.Clear();
            condition = Condition.Text.ToString();
            try
            {
                var result = _table.SearchRows(condition);
      
                foreach (var row in result)
                {
                    rowResult.Add(row.Records);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            

        }
    }
}
