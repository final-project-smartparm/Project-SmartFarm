using LiveCharts.Defaults;
using LiveCharts.Wpf.Charts.Base;
using LiveCharts.Wpf;
using LiveCharts;
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
using System.Windows.Forms;
using UserControl = System.Windows.Controls.UserControl;
using Syncfusion.Windows.Controls;
using Syncfusion.Windows.Shared;
using System.Diagnostics;

namespace SFARM.Views
{
    /// <summary>
    /// PanelLiveChart.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PanelLiveChart : UserControl
    {
        private DispatcherTimer _dataUpdateTimer;

        private DateTime preDate;
        private DateTime endDate;

        private bool DateSet;

        public PanelLiveChart()
        {
            InitializeComponent();
        }

        public void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = this;
            DateSet = false;
            InitializeCharts();
            StartDataUpdateTimer();
            chart.AxisX[0].LabelFormatter = x => DateLabelFormatter(x);
            DatePickerSet();

        }

        private void InitializeCharts()
        {
            // 실제 측정 값 차트 설정
            chart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Soil Humidity",
                    Values = new ChartValues<ObservablePoint>(), // 데이터가 추가됩니다.
                    PointGeometrySize = 5
                },
                new LineSeries
                {
                    Title = "Temperature",
                    Values = new ChartValues<ObservablePoint>(), // 데이터가 추가됩니다.
                    PointGeometrySize = 5
                },
                new LineSeries
                {
                    Title = "Lux",
                    Values = new ChartValues<ObservablePoint>(), // 데이터가 추가됩니다.
                    PointGeometrySize = 5
                },
                new LineSeries
                {
                    Title = "Database Temperature",
                    Values = new ChartValues<ObservablePoint>(), // 데이터가 추가됩니다.
                    PointGeometrySize = 5
                },
                new LineSeries
                {
                    Title = "Database Humidity",
                    Values = new ChartValues<ObservablePoint>(), // 데이터가 추가됩니다.
                    PointGeometrySize = 5
                }
            };
        }

        // TODO 날짜 범위 설정 후 그래프 재출력
        private void DatePickerSet()
        {
            string preQuery = @"SELECT TOP 1
                                       [PLANT_DATE]
                                  FROM [dbo].[UserPlant]
                                 WHERE USER_NUM = @USER_NUM
                                   AND PLANT_NUM = @PLANT_NUM
                              ORDER BY PLANT_DATE ASC";

            string endQuery = @"SELECT TOP 1
                                       [PLANT_DATE]
                                  FROM [dbo].[UserPlant]
                                 WHERE USER_NUM = @USER_NUM
                                   AND PLANT_NUM = @PLANT_NUM
                              ORDER BY PLANT_DATE DESC";


            using (SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(preQuery, conn);
                cmd.Parameters.AddWithValue("@USER_NUM", Helpers.UserInfo.USER_NUM);
                cmd.Parameters.AddWithValue("@PLANT_NUM", Helpers.UserPlantList.PLANT_NUM);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {

                    // 로그인 성공 시 사용자 이름 저장
                    this.preDate = (reader["PLANT_DATE"].ToDateTime()).Date;
                    
                }
            }

            using (SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(endQuery, conn);
                cmd.Parameters.AddWithValue("@USER_NUM", Helpers.UserInfo.USER_NUM);
                cmd.Parameters.AddWithValue("@PLANT_NUM", Helpers.UserPlantList.PLANT_NUM);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    this.endDate = (reader["PLANT_DATE"].ToDateTime()).Date;
                }
            }

            // 초기 설정
            DatePickerPre.DisplayDateStart = preDate;
            DatePickerPre.DisplayDateEnd = endDate;

            DatePickerEnd.DisplayDateStart = preDate;
            DatePickerEnd.DisplayDateEnd= endDate;

            DatePickerEnd.IsEnabled = false;
        }

        public void LoadData(string dataType)
        {
            string connectionString = Helpers.Common.CONNSTRING;

            // 첫 번째 쿼리: test 테이블에서 데이터 가져오기
            string query1 = "SELECT [temp], [humid], [Pdate] FROM [dbo].[test]";

            string query2 = @"
            SELECT PLANT_DATE, PLANT_TEMP, PLANT_SOILHUMID, PLANT_LUX
            FROM UserPlant
            WHERE USER_NUM = @UserNum
              AND PLANT_NUM = @PlantNum
         ORDER BY PLANT_DATE ASC
            ";

            string query3 = @"
            SELECT PLANT_DATE, PLANT_TEMP, PLANT_SOILHUMID, PLANT_LUX
            FROM UserPlant
            WHERE USER_NUM = @UserNum
              AND PLANT_NUM = @PlantNum
              AND PLANT_DATE BETWEEN CONVERT(DATE, @preDate) AND CONVERT(DATE, @endDate)
         ORDER BY PLANT_DATE ASC
            ";
            Dispatcher.Invoke(() =>
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        if (dataType == "Database")
                        {
                            // 첫 번째 쿼리 실행 (Database Temperature and Humidity)
                            using (SqlCommand cmd1 = new SqlCommand(query1, conn))
                            {

                                using (SqlDataReader reader = cmd1.ExecuteReader())
                                {
                                    var databaseTemperatureValues = new ChartValues<ObservablePoint>();
                                    var databaseHumidityValues = new ChartValues<ObservablePoint>();

                                    while (reader.Read())
                                    {
                                        try
                                        {
                                            int temp = reader.GetInt32(0);
                                            int humid = reader.GetInt32(1);
                                            DateTime date = reader.GetDateTime(2);

                                            double xValue = (date - new DateTime(2024, 7, 13)).TotalSeconds;

                                            databaseTemperatureValues.Add(new ObservablePoint(xValue, temp));
                                            databaseHumidityValues.Add(new ObservablePoint(xValue, humid));
                                        }
                                        catch (Exception ex)
                                        {
                                            //MessageBox.Show($"데이터 형식 오류: {ex.Message}", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                                        }
                                    }

                                    // 차트에 데이터 추가
                                    ((LineSeries)chart.Series[3]).Values = databaseTemperatureValues;
                                    ((LineSeries)chart.Series[4]).Values = databaseHumidityValues;
                                }
                            }
                        }
                        else
                        {
                            if(this.DateSet) // true ===> 기간 설정 조회
                            {
                                using (SqlCommand cmd2 = new SqlCommand(query3, conn))
                                {
                                    cmd2.Parameters.AddWithValue("@UserNum", Helpers.UserInfo.USER_NUM);
                                    cmd2.Parameters.AddWithValue("@PlantNum", Helpers.UserPlantList.PLANT_NUM);
                                    cmd2.Parameters.AddWithValue("@preDate", DatePickerPre.Text);
                                    cmd2.Parameters.AddWithValue("@endDate", DatePickerEnd.Text);

                                    using (SqlDataReader reader = cmd2.ExecuteReader())
                                    {
                                        var temperatureValues = new ChartValues<ObservablePoint>();
                                        var soilHumidityValues = new ChartValues<ObservablePoint>();
                                        var luxValues = new ChartValues<ObservablePoint>();

                                        while (reader.Read())
                                        {
                                            try
                                            {
                                                DateTime date = reader.GetDateTime(0);
                                                double temp = reader.GetDouble(1);
                                                double soilHumid = reader.GetDouble(2);
                                                double lux = reader.GetDouble(3);

                                                double xValue = (date - new DateTime(2024, 7, 13)).TotalSeconds;

                                                if (dataType == "LUX")
                                                {
                                                    luxValues.Add(new ObservablePoint(xValue, lux));
                                                }
                                                else if (dataType == "Humidity")
                                                {
                                                    soilHumidityValues.Add(new ObservablePoint(xValue, soilHumid));
                                                }
                                                else if (dataType == "Temperature")
                                                {
                                                    temperatureValues.Add(new ObservablePoint(xValue, temp));
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                System.Windows.MessageBox.Show($"데이터 형식 오류: {ex.Message}", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                                            }
                                        }

                                        // 차트에 데이터 추가
                                        if (dataType == "LUX")
                                        {
                                            ((LineSeries)chart.Series[2]).Values = luxValues;
                                        }
                                        else if (dataType == "Humidity")
                                        {
                                            ((LineSeries)chart.Series[0]).Values = soilHumidityValues;
                                        }
                                        else if (dataType == "Temperature")
                                        {
                                            ((LineSeries)chart.Series[1]).Values = temperatureValues;
                                        }
                                    }
                                }

                            }
                            else
                            {

                                // 두 번째 쿼리 실행 (UserPlant 테이블에서 데이터 가져오기)
                                using (SqlCommand cmd2 = new SqlCommand(query2, conn))
                                {
                                    cmd2.Parameters.AddWithValue("@UserNum", Helpers.UserInfo.USER_NUM);
                                    cmd2.Parameters.AddWithValue("@PlantNum", Helpers.UserPlantList.PLANT_NUM);

                                    using (SqlDataReader reader = cmd2.ExecuteReader())
                                    {
                                        var temperatureValues = new ChartValues<ObservablePoint>();
                                        var soilHumidityValues = new ChartValues<ObservablePoint>();
                                        var luxValues = new ChartValues<ObservablePoint>();

                                        while (reader.Read())
                                        {
                                            try
                                            {
                                                DateTime date = reader.GetDateTime(0);
                                                double temp = reader.GetDouble(1);
                                                double soilHumid = reader.GetDouble(2);
                                                double lux = reader.GetDouble(3);

                                                double xValue = (date - new DateTime(2024, 7, 13)).TotalSeconds;

                                                if (dataType == "LUX")
                                                {
                                                    luxValues.Add(new ObservablePoint(xValue, lux));
                                                }
                                                else if (dataType == "Humidity")
                                                {
                                                    soilHumidityValues.Add(new ObservablePoint(xValue, soilHumid));
                                                }
                                                else if (dataType == "Temperature")
                                                {
                                                    temperatureValues.Add(new ObservablePoint(xValue, temp));
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                System.Windows.MessageBox.Show($"데이터 형식 오류: {ex.Message}", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                                            }
                                        }

                                        // 차트에 데이터 추가
                                        if (dataType == "LUX")
                                        {
                                            ((LineSeries)chart.Series[2]).Values = luxValues;
                                        }
                                        else if (dataType == "Humidity")
                                        {
                                            ((LineSeries)chart.Series[0]).Values = soilHumidityValues;
                                        }
                                        else if (dataType == "Temperature")
                                        {
                                            ((LineSeries)chart.Series[1]).Values = temperatureValues;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"데이터베이스 오류: {ex.Message}", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

        private void ClearChart()
        {
            // 모든 시리즈의 값을 초기화하여 차트를 클리어합니다.
            foreach (var series in chart.Series)
            {
                series.Values.Clear();
            }
            chart.DataContext = null;
        }

        private void StartDataUpdateTimer()
        {
            _dataUpdateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(0.1) // 0.1초마다 데이터 업데이트
            };
            _dataUpdateTimer.Tick += (sender, args) =>
            {
                LoadData("LUX"); // 기본적으로 "LUX" 데이터를 로드하도록 설정
                _dataUpdateTimer.Stop(); // 타이머 정지
            };
            _dataUpdateTimer.Start();

            BtnLUX.IsEnabled = false;
            BtnHumid.IsEnabled = BtnTemp.IsEnabled = true;

        }

        public string DateLabelFormatter(double xValue)
        {
            // 기준 날짜를 설정합니다.
            DateTime baseDate = new DateTime(2024, 7, 13);

            // xValue를 DateTime으로 변환합니다.
            DateTime date = baseDate.AddSeconds(xValue);

            // yyyy/MM/dd 형식으로 변환하여 반환합니다.
            return date.ToString("yyyy/MM/dd");
        }

        // '조도' 버튼 클릭 시 실행될 메서드
        private void BtnLUX_Click(object sender, RoutedEventArgs e)
        {
            ClearChart();  // 차트 초기화
            LoadData("LUX");

            BtnLUX.IsEnabled = false;
            BtnHumid.IsEnabled = BtnTemp.IsEnabled = true;
        }

        // '습도' 버튼 클릭 시 실행될 메서드
        private void BtnHumid_Click(object sender, RoutedEventArgs e)
        {
            ClearChart();  // 차트 초기화
            LoadData("Humidity");

            BtnHumid.IsEnabled = false;
            BtnLUX.IsEnabled = BtnTemp.IsEnabled = true;
        }

        // '온도' 버튼 클릭 시 실행될 메서드
        private void BtnTemp_Click(object sender, RoutedEventArgs e)
        {
            ClearChart();  // 차트 초기화
            LoadData("Temperature");

            BtnTemp.IsEnabled = false;
            BtnLUX.IsEnabled = BtnHumid.IsEnabled = true;
        }


        private void DatePickerPre_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePickerEnd.DisplayDateStart = DatePickerPre.Text.ToDateTime();
            this.preDate = DatePickerPre.Text.ToDateTime();
            DatePickerEnd.IsEnabled = true;
        }

        private void DatePickerEnd_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            this.endDate = DatePickerEnd.Text.ToDateTime();
        }

        private void BtnSearchDate_Click(object sender, RoutedEventArgs e)
        {
            this.DateSet = true;
            ClearChart();
            LoadData("LUX");
        }
    }
}
