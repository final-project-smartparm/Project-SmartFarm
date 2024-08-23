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
        private string userName; // 로그인된 사용자의 이름

        public MainWindow(string userName)
        {
            InitializeComponent();
            this.userName = userName;
            lblUserName.Content = $"환영합니다, {userName}님!";
            CboPlants.Items.Clear();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ActiveItem.Content = new Views.HomeControl();
            TodayNow.Content = Helpers.Common.TODAY;
            LoadSettings(); // LoadSettings 호출
        }

        private void LoadSettings()
        {
            string connectionString = Helpers.Common.CONNSTRING; // 데이터베이스 연결 문자열

            // 사용자 이름을 기반으로 설정 이름을 찾는 쿼리문
            string query = @"
                SELECT sp.SATTINGP_NAME 
                FROM SattingPlant sp
                WHERE sp.PLANT_IDX IN (
                    SELECT DISTINCT up.PLANT_IDX 
                    FROM UserPlant up
                    JOIN UserInfo ui ON up.USER_NUM = ui.USER_NUM
                    WHERE ui.USER_NAME = @UserName
                )";

            List<string> settings = new List<string>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // 사용자 이름을 매개 변수로 추가
                        cmd.Parameters.AddWithValue("@UserName", userName);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // 쿼리 결과에서 SATTINGP_NAME을 읽어 리스트에 추가
                                string settingName = reader["SATTINGP_NAME"]?.ToString();
                                if (!string.IsNullOrEmpty(settingName))
                                {
                                    settings.Add(settingName);
                                }
                            }
                        }
                    }
                }

                // ComboBox에 데이터 바인딩
                if (settings.Count > 0)
                {
                    CboPlants.ItemsSource = settings;

                    // 처음 시작할 때 첫 번째 항목을 선택
                    CboPlants.SelectedIndex = 0; // 첫 번째 인덱스 항목 선택
                }
                else
                {
                    CboPlants.ItemsSource = new List<string> { "설정 없음" }; // 기본 항목 추가
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"데이터를 로드하는 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

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

        private void BtnPrevious_Click(object sender, RoutedEventArgs e)
        {
            ActiveItem.Content = new Views.PanelLiveChart();

        }

        private void CboPlants_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CboPlants.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedPlant = selectedItem.Content.ToString();
                //LoadPlantData(selectedPlant);
            }
        }

        private void LoadPlantData(string? selectedPlant)
        {
            throw new NotImplementedException();
        }
    }
}