using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace WPF_Chart
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //범례 위치 설정
            chart.LegendLocation = LiveCharts.LegendLocation.Top;

            //세로 눈금 값 설정
            chart.AxisY.Add(new LiveCharts.Wpf.Axis { MinValue=0, MaxValue = 1000 });

            //가로 눈금 값 설정
            chart.AxisX.Add(new LiveCharts.Wpf.Axis { Labels = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12"} });



            //모든 항목 지우기
            chart.Series.Clear();

            //항목 추가
            chart.Series.Add(new LiveCharts.Wpf.LineSeries() 
                                { 
                                    Title = "Sample1", 
                                    Stroke = new SolidColorBrush(Colors.Yellow), 
                                    Values = new LiveCharts.ChartValues<double>(new List<double> { 700, 200, 300, 400, 500, 600, 700, 800, 900, 90, 211, 220 })
                                } 
            );
            chart.Series.Add(new LiveCharts.Wpf.LineSeries() 
                                { 
                                    Title = "Sample2", 
                                    Stroke = new SolidColorBrush(Colors.Red), 
                                    Values = new LiveCharts.ChartValues<double>(new List<double> { 70, 20, 100, 140, 50, 60, 70, 80, 90, 100, 111, 120 }) 
                                }
            );

            chart.Series.Add(new LiveCharts.Wpf.LineSeries()
                                {
                                    Title = "Sample3",
                                    Stroke = new SolidColorBrush(Colors.Green),
                                    Values = new LiveCharts.ChartValues<double>(new List<double> { 70, 20, 100, 140, 50, 60, 70, 80, 90, 100, 111, 120 })
                                }
            );

            chart.Series.Add(new LiveCharts.Wpf.LineSeries()
                                {
                                    Title = "Sample4",
                                    Stroke = new SolidColorBrush(Colors.Blue),
                                    Values = new LiveCharts.ChartValues<double>(new List<double> { 70, 20, 100, 140, 50, 60, 70, 80, 90, 100, 111, 120 })
                                }
            );

        }
    }
}
