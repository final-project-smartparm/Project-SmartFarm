using System;
using System.IO.Ports;
using System.Windows;
using System.Windows.Controls;

namespace SFARM.Views
{
    public partial class MyPlantsControl : UserControl
    {
        private SerialPort serialPort;

        public MyPlantsControl()
        {
            InitializeSerialPort();
            this.Unloaded += MyPlantsControl_Unloaded;
        }

        private void InitializeSerialPort()
        {
            try
            {
                serialPort = new SerialPort("COM3", 9600); // COM3 포트는 아두이노가 연결된 포트로 변경해야 합니다.
                serialPort.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing serial port: {ex.Message}");
            }
        }

        private void BrightnessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                int brightness = (int)e.NewValue;
                serialPort.WriteLine(brightness.ToString());
            }
        }

        private void MyPlantsControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }
    }
}
