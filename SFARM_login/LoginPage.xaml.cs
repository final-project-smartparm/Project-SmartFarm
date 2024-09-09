using Microsoft.Data.SqlClient;
using SFARM.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace SFARM
{
    /// <summary>
    /// LoginPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoginPage : Window
    {
        private bool isLogin = false;
        public string LoggedInUserName { get; private set; }


        public bool IsLogin { get { return isLogin; } set { isLogin = value; } }
        public LoginPage()
        {
            InitializeComponent();
            TxtId.Text = string.Empty;
            TxtPass.Password = string.Empty;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            IsLogin = LoginProcess(); // 로그인이 성공하면 True, 실패하면 False 리턴
            if (IsLogin)
            {
                MainWindow mainWindow = new MainWindow(LoggedInUserName);
                this.Close(); // 현재 로그인 창 닫기
                mainWindow.Show(); // 메인 창 표시
            }
            else
            {
                MessageBox.Show("아이디 또는 비밀번호가 잘못되었습니다.", "로그인 실패", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private bool LoginProcess()
        {
            string USER_EMAIL = TxtId.Text;
            string password = TxtPass.Password;

            using (SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))
            {
                conn.Open();

                string query = @"SELECT USER_NUM, USER_EMAIL, USER_PASS, USER_NAME, USER_TELL, USER_MEMO
                                 FROM UserInfo
                                 WHERE USER_EMAIL = @USER_EMAIL 
                                   AND USER_PASS = @USER_PASS";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@USER_EMAIL", USER_EMAIL);
                cmd.Parameters.AddWithValue("@USER_PASS", password);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // 로그인 성공 시 사용자 이름 저장
                    LoggedInUserName = reader["USER_NAME"]?.ToString();

                    Helpers.UserInfo.USER_NUM = Int32.Parse(reader["USER_NUM"].ToString());
                    Helpers.UserInfo.USER_NAME = reader["USER_NAME"]?.ToString();
                    Helpers.UserInfo.USER_EMAIL = reader["USER_EMAIL"]?.ToString();
                    Helpers.UserInfo.USER_TELL = reader["USER_TELL"]?.ToString();
                    Helpers.UserInfo.USER_MEMO = reader["USER_MEMO"]?.ToString();
                    SetUserPalntList();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void SetUserPalntList()
        {
            // 식물 별칭/시작날짜/블루투스 연결
            using (SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))
            {
                conn.Open();

                string query = @"SELECT TOP 1
	                                    USER_NUM
                                      , PLANT_IDX
                                      , PLANT_NUM
                                      , PLANT_NAME
                                      , PLANT_STARTDATE
                                      , BLUETOOTH
                                      , PLANT_CAMERAIP
                                   FROM UserPlantList
                                  WHERE USER_NUM = @userNum
                               ORDER BY PLANT_NUM ASC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userNum", Helpers.UserInfo.USER_NUM);
                

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // 세팅 값 저장
                    Helpers.UserPlantList.PLANT_NUM = Int32.Parse(reader["PLANT_NUM"]?.ToString());
                    Helpers.UserPlantList.PLANT_NAME = reader["PLANT_NAME"]?.ToString();
                    Helpers.UserPlantList.PLANT_STARTDATE = DateTime.Parse(reader["PLANT_STARTDATE"].ToString());
                    Helpers.UserPlantList.BLUETOOTH = reader["BLUETOOTH"].ToString();
                    Helpers.UserPlantList.PLANT_CAMERAIP = reader["PLANT_CAMERAIP"].ToString();
                }

            }
        }
       

        private void TxtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) // Enter 키가 눌렸는지 확인
            {
                BtnLogin_Click(sender, e); // BtnLogin_Click 이벤트 호출
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

