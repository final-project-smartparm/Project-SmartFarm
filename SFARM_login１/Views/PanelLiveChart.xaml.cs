using Microsoft.Data.SqlClient;
using Syncfusion.Windows.Shared;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace SFARM.Views
{
    /// <summary>
    /// PanelLiveChart.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PanelLiveChart : UserControl
    {
        private ScaleTransform scaleTransform;
        public int PLANT_LUX {  get; set; }
        public int PLANT_TEMP { get; set; }
        public int PLANT_SOILHUMID { get; set; }

        public PanelLiveChart()
        {
            InitializeComponent();
            scaleTransform = new ScaleTransform();
            contentGrid.LayoutTransform = scaleTransform;
            LoadSettings();

        }


        private void LoadSettings()
        {
            string connectionString = Helpers.Common.CONNSTRING; // 데이터베이스 연결 문자열

            // 사용자 이름을 기반으로 설정 이름을 찾는 쿼리문
            string query = @"
                SELECT [PLANT_NUM]
                      ,[PLANT_TEMP]
                      ,[PLANT_SOILHUMID]
                      ,[PLANT_LUX]
                      ,[PLANT_IDX]
                      ,[USER_NUM]
                      ,[PLANT_DATE]
                  FROM [dbo].[UserPlant]
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
                        cmd.Parameters.AddWithValue("@[PLANT_TEMP]", PLANT_TEMP);
                        cmd.Parameters.AddWithValue("@[PLANT_SOILHUMID]", PLANT_SOILHUMID);
                        cmd.Parameters.AddWithValue("@[PLANT_LUX]", PLANT_LUX);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // 쿼리 결과에서 SATTINGP_NAME을 읽어 리스트에 추가
                                
                                
                            }
                        }
                    }
                }
            }
            catch { }
        }
    } 
}
