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
using Win7.Sqlite;

namespace Win7.Test
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
       
        public MainWindow()
        {
            InitializeComponent();
            DataList.ItemsSource = new  List<Win7User>();
        }

        private void Button_Click_0(object sender, RoutedEventArgs e)
        {
            var dataList = DataList.ItemsSource as List<Win7User>;
            var select = SqliteDataObject.Update(new Win7User() { Name = "Name" + DateTime.Now.Ticks, Password = "12345678" });
            dataList.Add(select);
            DataList.ItemsSource = dataList;
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DataList.ItemsSource = SqliteDataObject.Select<Win7User>(null);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var dataList = DataList.ItemsSource as List<Win7User>;
            SqliteDataObject.Update(DataList.SelectedItem as Win7User);
            DataList.ItemsSource = dataList;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var dataList = DataList.ItemsSource as List<Win7User>;
            var select = DataList.SelectedItem as Win7User;
            if (SqliteDataObject.Delete(select))
            {
                dataList.Remove(select);
            }
            DataList.ItemsSource = dataList;
        }
    }
}
