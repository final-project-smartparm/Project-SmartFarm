using Microsoft.Data.SqlClient;
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
using System.Windows.Threading;

namespace SFARM.Views
{
    /// <summary>
    /// PanelControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PanelLiveInfo : UserControl
    {
        public PanelLiveInfo()
        {
            InitializeComponent();
            // 싱글톤 BluetoothManager 인스턴스 사용
            BluetoothManager bluetoothManager = BluetoothManager.Instance;
            // BluetoothManager의 DataReceived 이벤트를 구독
            bluetoothManager.DataReceived += OnDataReceived;

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        // 데이터 수신 시 UI를 업데이트하는 메소드
        private void OnDataReceived(string data)
        {
            Dispatcher.Invoke(() =>
            {
                // 수신된 데이터를 파싱하고 UI를 업데이트
                string[] dataParts = data.Split(',');

                if (dataParts.Length == 5)
                {
                    string soilMoisturePart = dataParts[0].Split(':')[1].Trim();

                    string tempPart = dataParts[1].Split(':')[1].Trim();
                    string lightPart = dataParts[3].Split(':')[1].Trim();
                    string waterPart = dataParts[4].Split(':')[1].Trim();

                    lblTemperature.Content = tempPart + "˚C";
                    lblSoilMoisture.Content = soilMoisturePart + "%";
                    // lblHumidity.Content = humidPart + "%";
                    lblLight.Content = lightPart + "lux";
                    lblWater.Content = waterPart;
                }

                lblTemperature.UpdateLayout();
                lblLight.UpdateLayout();

                lblWater.UpdateLayout();

            });
        }


        // DB에 식물 측정 값 저장 메서드
        private void Btnsave_Click(object sender, RoutedEventArgs e)
        {
            // 버튼을 누르면 DB에 LiveInfoPanel 정보 저장
            // TODO 시간마다 자동으로 들어가도록 설정은 미구현

            string INSERT_QUERY = @"INSERT INTO dbo.UserPlant
                                           ( USER_NUM
                                           , PLANT_NUM
                                           , PLANT_IDX
                                           , PLANT_TEMP
                                           , PLANT_SOILHUMID
                                           , PLANT_LUX
                                           , WATER_SUPPLY
                                           , PLANT_DATE)
                                    VALUES
                                          (  @USER_NUM
                                           , @PLANT_NUM
                                           , @PLANT_IDX
                                           , @PLANT_TEMP
                                           , @PLANT_SOILHUMID
                                           , @PLANT_LUX
                                           , @WATER_SUPPLY
                                           , @PLANT_DATE)";


            string temp = lblTemperature.Content.ToString();
            var PLANT_TEMP = temp.Split("˚C");

            string solihumid = lblSoilMoisture.Content.ToString();
            var PLANT_SOILHUMID = solihumid.Split("%");

            string light = lblLight.Content.ToString();
            var PLANT_LUX = light.Split("lux");


            using (SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                //if (Id == 0) // INSERT
                cmd.CommandText = INSERT_QUERY;
                //else
                //    cmd.CommandText = Models.Employees.UPDATE_QUERY;

                SqlParameter prmUSER_NUM = new SqlParameter("@USER_NUM", Helpers.UserInfo.USER_NUM);
                cmd.Parameters.Add(prmUSER_NUM);
                SqlParameter prmPLANT_NUM = new SqlParameter("@PLANT_NUM", Helpers.UserPlantList.PLANT_NUM);
                cmd.Parameters.Add(prmPLANT_NUM);
                SqlParameter prmPLANT_IDX = new SqlParameter("@PLANT_IDX", Helpers.SattingPlant.PLANT_IDX);
                cmd.Parameters.Add(prmPLANT_IDX);
                SqlParameter prmPLANT_TEMP = new SqlParameter("@PLANT_TEMP", float.Parse(PLANT_TEMP[0]));
                cmd.Parameters.Add(prmPLANT_TEMP);
                SqlParameter prmPLANT_SOILHUMID = new SqlParameter("@PLANT_SOILHUMID", float.Parse(PLANT_SOILHUMID[0]));
                cmd.Parameters.Add(prmPLANT_SOILHUMID);
                SqlParameter prmPLANT_LUX = new SqlParameter("@PLANT_LUX", float.Parse(PLANT_LUX[0]));
                cmd.Parameters.Add(prmPLANT_LUX);
                SqlParameter prmWATER_SUPPLY = new SqlParameter("@WATER_SUPPLY", Helpers.SattingPlant.WATER_SUPPLY);
                cmd.Parameters.Add(prmWATER_SUPPLY);
                SqlParameter prmPLANT_DATE = new SqlParameter("@PLANT_DATE", DateTime.Now.Date);
                cmd.Parameters.Add(prmPLANT_DATE);

                //SqlParameter prmAddr = new SqlParameter("@PLANT_TEMP", Addr ?? (object)DBNull.Value); // 빈 값을 DB컬럼에 null값으로 입력
                //cmd.Parameters.Add(prmAddr);

                //if (Id != 0) //업데이트면 Id 파라미터가 필요
                //{
                //    SqlParameter prmId = new SqlParameter("@Id", Id);
                //    cmd.Parameters.Add(prmId);
                //}

                var result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("저장성공");
                    //await this._dialogCoordinator.ShowMessageAsync(this, "저장 성공", "저장");
                }
                else
                {
                    MessageBox.Show("저장실패");
                    //await this._dialogCoordinator.ShowMessageAsync(this, "저장 실패", "저장");
                }
            }
        }

    }
}
