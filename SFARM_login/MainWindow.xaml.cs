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
using SFARM.Views;
using System.Windows.Forms;
using System.Diagnostics.PerformanceData;

namespace SFARM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string userName; // 로그인된 사용자의 이름
        
        HomeControl homeControl = new HomeControl();
        MyPlantsControl plantsControl = new MyPlantsControl();
        MyInfoControl infoControl = new MyInfoControl();
        PanelLiveChart panelLiveChart = new PanelLiveChart();
        

        public MainWindow(string userName)
        {
            InitializeComponent();
            this.userName = userName;
            lblUserName.Content = $"환영합니다, {userName}님!";
            CboPlants.Items.Clear();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BtnMnuHome_Click(sender, e);
            TodayNow.Content = Helpers.Common.TODAY;
            LoadSettings(); // LoadSettings 호출
        }

        private void LoadSettings()
        {
            string connectionString = Helpers.Common.CONNSTRING; // 데이터베이스 연결 문자열


            
            // 사용자 이름을 기반으로 로그인 유저의 식물 리스트를 찾는 쿼리문
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
                    Helpers.SattingPlant.SATTINGP_NAME = settings[0];
                }

                else
                {
                    CboPlants.ItemsSource = new List<string> { "설정 없음" }; // 기본 항목 추가
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"데이터를 로드하는 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }
        private void CboPlants_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Helpers.SattingPlant.SATTINGP_NAME = CboPlants.SelectedItem.ToString();
                CboChanged();

                var duration = DateTime.Now - Helpers.UserPlantList.PLANT_STARTDATE;
                var countday = duration.Days;

                var lblDday =  Helpers.UserPlantList.PLANT_NAME + " 함께한 날 " + countday + "일째";
                LblDday.Content = lblDday;

                homeControl.UserControl_Loaded(sender, e);
                plantsControl.UserControl_Loaded(sender, e);
            }
            catch (Exception)
            {
            }
        }
        
        private void CboChanged()
        {
            var Satting_Name = Helpers.SattingPlant.SATTINGP_NAME;
            
            // 식물 세팅값
            using (SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))
                {
                    conn.Open();

                    string query = @"SELECT PLANT_IDX
                                          , SATTINGP_NAME
                                          , SATTINGP_TEMP
                                          , SATTINGP_LUX
                                          , SATTINGP_SOILHUMID
                                          , WATER_SUPPLY
                                          , SATTING_TOTALDATE
                                       FROM SattingPlant
                                      WHERE SATTINGP_NAME = @SATTINGP_NAME";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@SATTINGP_NAME", Satting_Name);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // 세팅 값 저장
                        Helpers.SattingPlant.PLANT_IDX = Int32.Parse(reader["PLANT_IDX"]?.ToString());
                        Helpers.SattingPlant.SATTINGP_TEMP = float.Parse(reader["SATTINGP_TEMP"]?.ToString());
                        Helpers.SattingPlant.SATTINGP_LUX = float.Parse(reader["SATTINGP_LUX"]?.ToString());
                        Helpers.SattingPlant.SATTINGP_SOILHUMID = float.Parse(reader["SATTINGP_SOILHUMID"]?.ToString());
                        Helpers.SattingPlant.WATER_SUPPLY = float.Parse(reader["WATER_SUPPLY"]?.ToString());
                        Helpers.SattingPlant.SATTING_TOTALDATE = Int32.Parse(reader["WATER_SUPPLY"]?.ToString());
                    }

                }

            var userNum = Helpers.UserInfo.USER_NUM;
            var plantIdx = Helpers.SattingPlant.PLANT_IDX;


            // 식물 별칭/시작날짜
            using (SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))
            {
                conn.Open();

                string query = @"SELECT PLANT_NUM
                                      , PLANT_NAME
                                      , PLANT_STARTDATE
                                  FROM UserPlantList
                                 WHERE USER_NUM = @userNum
                                   AND PLANT_IDX = @plantIdx";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userNum", userNum);
                cmd.Parameters.AddWithValue("@plantIdx", plantIdx);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // 세팅 값 저장
                    Helpers.UserPlantList.PLANT_NUM = Int32.Parse(reader["PLANT_NUM"]?.ToString());
                    Helpers.UserPlantList.PLANT_NAME = reader["PLANT_NAME"]?.ToString();
                    Helpers.UserPlantList.PLANT_STARTDATE = DateTime.Parse(reader["PLANT_STARTDATE"].ToString());
                }

            }

            // 식물 설명정보
            using (SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))
            {
                conn.Open();

                string query = @"SELECT PLANT_LUX
                                      , PLANT_TEMP
                                      , PLANT_HUMID
                                      , PLANT_SOILHUMID
                                      , PLANT_TEXT
                                  FROM InfoPlant
                                  WHERE PLANT_NAME = @SATTINGP_NAME";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SATTINGP_NAME", Satting_Name);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // 세팅 값 저장
                    //Helpers.SattingPlnt.PLANT_IDX = Int32.Parse(reader["PLANT_IDX"]?.ToString());
                    Helpers.InfoPlant.PLANT_LUX = reader["PLANT_LUX"]?.ToString();
                    Helpers.InfoPlant.PLANT_TEMP = reader["PLANT_TEMP"]?.ToString();
                    Helpers.InfoPlant.PLANT_HUMID = reader["PLANT_HUMID"]?.ToString();
                    Helpers.InfoPlant.PLANT_SOILHUMID = reader["PLANT_SOILHUMID"]?.ToString();
                    Helpers.InfoPlant.PLANT_TEXT = reader["PLANT_TEXT"]?.ToString();
                }
            }

        }

        private void BtnMnuHome_Click(object sender, RoutedEventArgs e)
        {
            ActiveItem.Content = homeControl;

        }

        private void BtnMunMyPlants_Click(object sender, RoutedEventArgs e)
        {
            ActiveItem.Content = plantsControl;

        }

        private void BtnMyInfo_Click(object sender, RoutedEventArgs e)
        {
            ActiveItem.Content = infoControl;

        }

        private void BtnPrevious_Click(object sender, RoutedEventArgs e)
        {
            ActiveItem.Content = panelLiveChart;

        }

    }
}