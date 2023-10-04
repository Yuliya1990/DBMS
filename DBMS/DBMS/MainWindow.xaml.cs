using DBMS.Models;
using Newtonsoft.Json;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DBMS
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataBase Db;
        private ObservableCollection<Models.Attribute> attributes;
        private ObservableCollection<Models.Table> tables;
        private Models.Table openedTable;
        private readonly ObservableCollection<object[]> _tableRows = new ObservableCollection<object[]>();
        private int EditedRowIndex;


        public MainWindow()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Converters = { new RowConverter() }
            };

            InitializeComponent();
            ViewTableContainer.Visibility = Visibility.Collapsed;
            DbNameLabel.Visibility = Visibility.Collapsed;
            AddTableBtn.Visibility = Visibility.Collapsed;
            TableListBox.Visibility = Visibility.Collapsed;
            CreateTableContainer.Visibility = Visibility.Collapsed;
            Grid.AutoGenerateColumns = false;
            SaveRow.Visibility = Visibility.Collapsed;

        }

        private void NewProject_Click(object sender, RoutedEventArgs e)
        {
            InputDBName InputDBName = new InputDBName();
            InputDBName.ShowDialog();
            string databaseName = InputDBName.DatabaseName;
            if (!string.IsNullOrEmpty(databaseName))
            {
                Db = new DataBase(databaseName);


                DataBaseName.Text = databaseName;
                DbNameLabel.Visibility = Visibility.Visible;
                AddTableBtn.Visibility = Visibility.Visible;
                TableListBox.Visibility = Visibility.Visible;

                tables = new ObservableCollection<Models.Table>();
                TableListBox.ItemsSource = tables;
            }
        }

        private void AddTableButton_Click(object sender, RoutedEventArgs e)
        {
            ViewTableContainer.Visibility = Visibility.Collapsed;
            CreateTableContainer.Visibility= Visibility.Visible;

            attributes = new ObservableCollection<Models.Attribute>();
            CreateTableGrid.ItemsSource = attributes;
        }

        private void TableButton_Click (object sender, RoutedEventArgs e)
        {

        }

        private void TableListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Models.Table selectedItem = (Models.Table)TableListBox.SelectedItem;
            if (selectedItem == null)
                ViewTableContainer.Visibility = Visibility.Collapsed;
            else
            {
                ViewTableContainer.Visibility = Visibility.Visible;
                OpenTable(selectedItem);
            }
        }

        private void AddRowCreateTableClick(object sender, RoutedEventArgs e)
        {
            Models.Attribute attribute = new Models.Attribute();
            attributes.Add(attribute);
        }

        private void CreateTable_Click(object sender, RoutedEventArgs e)
        {
            string newTableName = TableName.Text;
            try
            {
                var table = Db.AddTable(newTableName, attributes);
                tables.Add(table);

                CreateTableContainer.Visibility = Visibility.Collapsed;
                ViewTableContainer.Visibility = Visibility.Visible;

                OpenTable(table);
                attributes.Clear();
                TableName.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OpenTable(Models.Table table)
        {
            Grid.Columns.Clear();
            _tableRows.Clear();

            for (int i=0; i< table.Attributes.Count; i++)
            {
                DataGridTextColumn column = new DataGridTextColumn();
                column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                column.Header = table.Attributes[i].Name;
                column.Binding = new Binding("[" + i + "]"); // Привязка к свойству с соответствующим именем
                Grid.Columns.Add(column);
            }

            if (table.Rows.Count > 0)
            {
                foreach(var row in table.Rows)
                {
                    _tableRows.Add(row.Records.ToArray());
                }
            }

            Grid.ItemsSource = _tableRows;

            openedTable = table;
        }

        private void AddRowViewTableClick(object sender, RoutedEventArgs e)
        { 

            if (SaveRow.Visibility == Visibility.Visible)
                MessageBox.Show("You have to save previos row");
            else
            {
                var newRow = new object[openedTable.Attributes.Count()];
                _tableRows.Add(newRow);
                SaveRow.Visibility = Visibility.Visible;
            }
        }

        private void DeleteRowViewTableClick(object sender, RoutedEventArgs e)
        {
            openedTable.Rows.RemoveAt(_tableRows.Count - 1);
            _tableRows.RemoveAt(_tableRows.Count - 1);
        }

        private void DeleteRowCreateTableClick(object sender, RoutedEventArgs e)
        {
            attributes.RemoveAt(attributes.Count - 1);
            
        }

        private void CreateTableGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            // Устанавливаем ширину для каждой колонки в 50% от доступной ширины
            e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }


        private void DeleteTableClick(object sender, RoutedEventArgs e)
        {
            Db.Tables.Remove(openedTable);
            tables.Remove(openedTable);
            ViewTableContainer.Visibility = Visibility.Collapsed;

        }

        private void AddRowClick(object sender, RoutedEventArgs e)
        {
            try
            {
                int lastRowIndex = _tableRows.Count - 1;
                Row row = new Row(_tableRows[lastRowIndex].ToList());
                openedTable.AddRow(row);
                SaveRow.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            /*if (Grid.SelectedItem != null)
            {
                var selectedRow = Grid.SelectedItem as object[];
                if (selectedRow != null)
                {
                    // Update the corresponding row in your table object
                    int rowIndex = _tableRows.IndexOf(selectedRow);
                }
            }
            */
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var cellContent = e.EditingElement as TextBox;
                if (cellContent != null)
                {
                    // Update the binding source when the cell loses focus (commit edit)
                    cellContent.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
                }
            }



        }

        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Grid.CurrentCell != null)
                {
                    var cellContent = Grid.CurrentCell.Column.GetCellContent(Grid.CurrentItem);
                    if (cellContent is TextBox textBox)
                    {
                        // Update the binding source explicitly
                        textBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
                    }
                }
                // Handle Enter key press to save the edited row
                if (Grid.SelectedItem != null)
                {
                    var selectedRow = Grid.SelectedItem as object[];
                    if (selectedRow != null)
                    {
                        // Update the corresponding row in your table object
                        int rowIndex = _tableRows.IndexOf(selectedRow);
                        if (rowIndex >= 0 && rowIndex < openedTable.Rows.Count)
                        {
                            var updatedRow = new List<object>();
                            for (int i = 0; i < selectedRow.Length; i++)
                            {
                                updatedRow.Add(selectedRow[i]);
                            }


                            try
                            {
                                openedTable.EditRow(rowIndex, updatedRow);
                            }
                            catch(Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }

                        }
                    }
                }

                // Prevent further handling of the Enter key press
                e.Handled = true;
            }
        }

        private void SearchClick(object sender, RoutedEventArgs e)
        {
            Search search = new Search(openedTable);
            search.Show();
        }

        private void OpenDb(DataBase database)
        {
            DataBaseName.Text = database.Name;
            DbNameLabel.Visibility = Visibility.Visible;
            AddTableBtn.Visibility = Visibility.Visible;
            TableListBox.Visibility = Visibility.Visible;

            tables = new ObservableCollection<Models.Table>();
            foreach (var table in database.Tables)
            {
                tables.Add(table);
            }
            TableListBox.ItemsSource = tables;
        }

        private void OpenDatabaseBtn_OnClick(object sender, RoutedEventArgs e)
        {
            OpenDbWindow openDbWindow = new OpenDbWindow();
            openDbWindow.ShowDialog();
            var path = openDbWindow.path.Text;
            try
            {
                var database = DatabaseJsonConverter.GetDatabaseFrom(path);
                OpenDb(database);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
           
        }

        private void SaveDatabaseBtn_OnClick(object sender, RoutedEventArgs e)
        {
            SaveDb saveWindow = new SaveDb();
            saveWindow.ShowDialog();
            var path = saveWindow.path.Text;
                 try
                 {
                     DatabaseJsonConverter.SaveDatabaseTo(path, Db);
                 }
                 catch (ArgumentException exception)
                 {
                     MessageBox.Show(exception.Message);
                 }

             
        }



    }



}
