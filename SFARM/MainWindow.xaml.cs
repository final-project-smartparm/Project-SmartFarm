using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Microsoft.Web.WebView2.Core;

namespace SFARM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ActiveItem.Content = new Views.HomeControl();
            TodayNow.Content = Helpers.Common.TODAY;
            
        }

        //private void InitComboDateFromDB()
        //{
        //    using (SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))
        //    {
        //        conn.Open();
        //        SqlCommand cmd = new SqlCommand(Helpers.Common.CONNSTRING, conn);
        //        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //        DataSet dSet = new DataSet();
        //        adapter.Fill(dSet);
        //        List<string> saveDates = new List<string>();

        //        foreach (DataRow row in dSet.Tables[0].Rows)
        //        {
        //            saveDates.Add(row["Facilities_id"].ToString());
        //        }
        //        CboReqDate.ItemsSource = saveDates;
        //    })
        //}

        private void BtnMnuHome_Click(object sender, RoutedEventArgs e)
        {
            ActiveItem.Content = new Views.HomeControl();

        }

        private void BtnMunMyPlants_Click(object sender, RoutedEventArgs e)
        {
            ActiveItem.Content = new Views.MyPlantsControl();

        }

        private void BtnMyInfo_Click(object sender, RoutedEventArgs e)
        {
            ActiveItem.Content = new Views.MyInfoControl();

        }
    }
}