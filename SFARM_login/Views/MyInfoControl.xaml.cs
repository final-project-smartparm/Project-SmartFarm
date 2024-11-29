using Microsoft.Data.SqlClient;
using SFARM.Helpers;
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

namespace SFARM.Views
{
    /// <summary>
    /// MyInfoControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MyInfoControl : UserControl
    {
        public MyInfoControl()
        {
            InitializeComponent();
            TxtUserName.Text = Helpers.UserInfo.USER_NAME;
            TxtUserEmail.Text = Helpers.UserInfo.USER_EMAIL;
            TxtUserTell.Text = Helpers.UserInfo.USER_TELL;
            TxtMemo.Text = Helpers.UserInfo.USER_MEMO;

            BtnMemoSave.Visibility = Visibility.Hidden;
        }


        private void BtnMemoSave_Click(object sender, RoutedEventArgs e)
        {
            Helpers.UserInfo.USER_MEMO = TxtMemo.Text;

            SaveMemo(Helpers.UserInfo.USER_MEMO);
        }

        public void SaveMemo(string textMemo)
        {
            //MessageBox.Show("저장버튼 동작!");

            //string UPDATE_QUERY = @"UPDATE [dbo].[Employees]
            //                           SET [EmpName] = @EmpName
            //                         WHERE Id = @Id";

            string UPDATE_QUERY = @"UPDATE [dbo].[UserInfo]
                                       SET USER_MEMO = @USER_MEMO
                                     WHERE USER_NUM = @USER_NUM";
            
            using (SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                cmd.CommandText = UPDATE_QUERY;

                SqlParameter prmUSER_MEMO = new SqlParameter("@USER_MEMO", textMemo);
                cmd.Parameters.Add(prmUSER_MEMO);
                SqlParameter prmUSER_NUM = new SqlParameter("@USER_NUM", Helpers.UserInfo.USER_NUM);
                cmd.Parameters.Add(prmUSER_NUM);

                var result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("저장성공");
                    BtnMemoSave.Visibility = Visibility.Hidden;
                }
            }
        }

        private void TxtMemo_TextChanged(object sender, TextChangedEventArgs e)
        {
            BtnMemoSave.Visibility = Visibility.Visible;
        }
    }
}
