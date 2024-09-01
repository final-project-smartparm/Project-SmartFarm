using LiveCharts.Wpf;
using LiveCharts;
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
using System.Windows.Threading;
using LiveCharts.Defaults;
using LiveCharts.Maps;

namespace SFARM.Views
{
    /// <summary>
    /// PanelLiveChart2.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PanelLiveChart : UserControl
    {
        private DispatcherTimer _dataUpdateTimer;

        public PanelLiveChart()
        {
            InitializeComponent();
            DataContext = this;
            InitializeCharts();
            StartDataUpdateTimer();

            chart.AxisX[0].LabelFormatter = x => DateLabelFormatter(x);
            chart1.AxisX[0].LabelFormatter = x => DateLabelFormatter(x);


        }

        private void InitializeCharts()
        {
            // 실제 측정 값 차트 설정
            chart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Temperature",
                    Values = new ChartValues<ObservablePoint>(), // 데이터가 추가됩니다.
                    PointGeometrySize = 5
                },
                new LineSeries
                {
                    Title = "Soil Humidity",
                    Values = new ChartValues<ObservablePoint>(), // 데이터가 추가됩니다.
                    PointGeometrySize = 5
                },
                new LineSeries
                {
                    Title = "Lux",
                    Values = new ChartValues<ObservablePoint>(), // 데이터가 추가됩니다.
                    PointGeometrySize = 5
                }
            };

            // 데이터베이스 값 차트 설정
            chart1.Series = new SeriesCollection
            {
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

        //public void LoadData()
        //{
        //    string connectionString = Helpers.Common.CONNSTRING;
        //    string query = "SELECT [temp], [humid], [Pdate] FROM [dbo].[test]";

        //    Dispatcher.Invoke(() =>
        //    {
        //        try
        //        {
        //            using (SqlConnection conn = new SqlConnection(connectionString))
        //            {
        //                conn.Open();
        //                using (SqlCommand cmd = new SqlCommand(query, conn))
        //                {
        //                    using (SqlDataReader reader = cmd.ExecuteReader())
        //                    {
        //                        var temperatureValues = new ChartValues<ObservablePoint>();
        //                        var humidityValues = new ChartValues<ObservablePoint>();

        //                        while (reader.Read())
        //                        {
        //                            try
        //                            {
        //                                int temp = reader.GetInt32(0); // 온도
        //                                int humid = reader.GetInt32(1); // 습도
        //                                DateTime date = reader.GetDateTime(2); // 날짜

        //                                // 날짜를 X축 값으로 변환
        //                                double xValue = (date - new DateTime(1970, 1, 1)).TotalSeconds;


        //                                // 데이터베이스 차트 데이터 추가
        //                                ((LineSeries)chart1.Series[0]).Values.Add(new ObservablePoint(xValue, temp));
        //                                ((LineSeries)chart1.Series[1]).Values.Add(new ObservablePoint(xValue, humid));
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                MessageBox.Show($"Data format error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //                            }
        //                        }

        //                        // 실제 측정 값 차트에 데이터 추가 (임시 데이터 예제)
        //                        ((LineSeries)chart.Series[0]).Values = temperatureValues;
        //                        ((LineSeries)chart.Series[1]).Values = humidityValues;
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    });
        //}
        public void LoadData()
        {
            string connectionString = Helpers.Common.CONNSTRING;
            string query = @"
                select PLANT_DATE, PLANT_TEMP, PLANT_SOILHUMID , PLANT_LUX
                from UserPlant
                WHERE USER_NUM = (
                    select USER_NUM
                    from UserInfo
                    where USER_EMAIL = 'email@address.com'
                )
                and PLANT_IDX = 1";

            Dispatcher.Invoke(() =>
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                var temperatureValues = new ChartValues<ObservablePoint>();
                                var soilHumidityValues = new ChartValues<ObservablePoint>();
                                var luxValues = new ChartValues<ObservablePoint>();

                                while (reader.Read())
                                {
                                    try
                                    {
                                        DateTime date = reader.GetDateTime(0); // PLANT_DATE
                                        double temp = reader.GetDouble(1); // PLANT_TEMP
                                        double soilHumid = reader.GetDouble(2); // PLANT_SOILHUMID
                                        double lux = reader.GetDouble(3); // PLANT_LUX

                                        // 날짜를 X축 값으로 변환
                                        double xValue = (date - new DateTime(1970, 1, 1)).TotalSeconds;

                                        // 차트 데이터 추가
                                        temperatureValues.Add(new ObservablePoint(xValue, temp));
                                        soilHumidityValues.Add(new ObservablePoint(xValue, soilHumid));
                                        luxValues.Add(new ObservablePoint(xValue, lux));
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show($"Data format error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                }


                                // 실제 측정 값 차트에 데이터 추가
                                ((LineSeries)chart.Series[0]).Values = temperatureValues;
                                ((LineSeries)chart.Series[1]).Values = soilHumidityValues;
                                ((LineSeries)chart.Series[2]).Values = luxValues;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

        private void StartDataUpdateTimer()
        {
            _dataUpdateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(0.1) // 0.1초마다 데이터 업데이트
            };
            _dataUpdateTimer.Tick += (sender, args) =>
            {
                LoadData(); // 데이터 로드
                _dataUpdateTimer.Stop(); // 타이머 정지
            };
            _dataUpdateTimer.Start();
        }

        public string DateLabelFormatter(double xValue)
        {
            // 기준 날짜를 설정합니다.
            DateTime baseDate = new DateTime(1990, 1, 1);

            // xValue를 DateTime으로 변환합니다.
            DateTime date = baseDate.AddSeconds(xValue);

            // yyyy/MM/dd 형식으로 변환하여 반환합니다.
            return date.ToString("yyyy/MM/dd");
        }
    }
}