using Microsoft.Data.SqlClient;
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
        public bool IsLogin { get { return isLogin; } set { isLogin = value; } }
        public LoginPage()
        {
            InitializeComponent();
            TxtId.Text = string.Empty;
            TxtPass.Password = string.Empty;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            IsLogin = LoginProcees(); // 로그인이 성공하면 True, 실패하면 False 리턴
            if (IsLogin)
            {
                MainWindow mainWindow = new MainWindow(); // MainWindow 인스턴스 생성
                this.Close(); // 현재 로그인 창 닫기
                mainWindow.Show(); // 메인 창 표시
            }
        }

        private bool LoginProcees()
        {
            string USER_EMAIL = TxtId.Text; // 사용자가 입력한 이메일
            string password = TxtPass.Password; // 사용자가 입력한 비밀번호

            using (SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))
            {
                conn.Open();

                // 1단계: 이메일만 확인
                string queryEmailCheck = @"SELECT COUNT(1) 
                                           FROM UserInfo 
                                           WHERE USER_EMAIL = @USER_EMAIL";
                SqlCommand cmdEmailCheck = new SqlCommand(queryEmailCheck, conn);
                cmdEmailCheck.Parameters.AddWithValue("@USER_EMAIL", USER_EMAIL);

                int emailExists = (int)cmdEmailCheck.ExecuteScalar();

                if (emailExists == 0)
                {
                    MessageBox.Show("아이디 오류", "로그인 실패", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                // 2단계: 이메일과 비밀번호 확인
                string query = @"SELECT COUNT(1)
                                 FROM UserInfo
                                 WHERE USER_EMAIL = @USER_EMAIL 
                                   AND USER_PASS = @USER_PASS";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@USER_EMAIL", USER_EMAIL);
                cmd.Parameters.AddWithValue("@USER_PASS", password);

                int loginSuccess = (int)cmd.ExecuteScalar();

                if (loginSuccess == 1)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("비밀번호 오류", "로그인 실패", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
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
    }
}

