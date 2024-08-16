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
            if (IsLogin) GetWindow(this).Close(); // 현재 로그인창이 닫혀야 Main창이 열림
        }


        private bool LoginProcees()
        {

            string USER_EMAIL = TxtId.Text; // 현재 폼에서 DB로 넘기는 값
            string password = TxtPass.Password;

            string chkUserId = string.Empty; // DB에서 넘어오는 값
            string chkPassword = string.Empty;

            using (SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))
            {
                conn.Open();

                string query = $@"SELECT USER_EMAIL 
                                   , USER_PASS
                                FROM UserInfo
                               WHERE USER_EMAIL = @USER_EMAIL 
                                 AND USER_PASS = @password"; // @userId와 @password는 쿼리문 외부에서 변수 값을 안전하게 주입함
                SqlCommand cmd = new SqlCommand(query, conn);

                //@userId, @password 파라미터 할당
                SqlParameter prmUSER_EMAIL = new SqlParameter("@USER_EMAIL", USER_EMAIL);
                SqlParameter prmUSER_PASS = new SqlParameter("@USER_PASS", password);

                cmd.Parameters.Add(prmUSER_EMAIL);
                cmd.Parameters.Add(prmUSER_PASS);

                SqlDataReader reader = cmd.ExecuteReader(); // 아이디와 패스워드가 넘어옴

                if (reader.Read())
                {
                    chkUserId = reader["USER_EMAIL"] != null ? reader["prmUSER_EMAIL"].ToString() : "-"; // reader이 가리키는 쿼리에 속성 ["??"] 값이 있으면 (not null) 값을 string으로 변환
                    chkPassword = reader["prmUSER_PASS"] != null ? reader["prmUSER_PASS"].ToString() : "-"; //Password가 0이면 -로 변경
                                                                                                            //Helpers.Common.LoginId = chkUserId;

                    return true;
                }
                else
                    return false;
                //using을 사용하면 conn.Close()가 필요없음.
            }

        }
    }
}

