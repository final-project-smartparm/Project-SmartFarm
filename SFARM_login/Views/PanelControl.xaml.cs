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

namespace SFARM.Views
{
    /// <summary>
    /// PanelControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PanelControl : UserControl
    {
        private BluetoothManager bluetoothManager = BluetoothManager.Instance;


        public PanelControl()
        {
            InitializeComponent();
        }
        public void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            textBox_Water_SupplyTick.Text = Helpers.SattingPlant.WATER_SUPPLY.ToString();
            textBox_SoilHumidTick.Text = Helpers.SattingPlant.SATTINGP_SOILHUMID.ToString();
            textBox_TempTick.Text = Helpers.SattingPlant.SATTINGP_TEMP.ToString();
            textBox_LuxTick.Text = Helpers.SattingPlant.SATTINGP_LUX.ToString();


        }
        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            FanSpeed();
            await Task.Delay(2000);
            Ledcontrol();
            await Task.Delay(2000);
            Tempcontrol();
            await Task.Delay(2000);
            Soilmoisture_control();
            await Task.Delay(2000);
            Watersupply_control();
        }

        private void FanSpeed()
        {
            if (bluetoothManager != null)
            {
                string fanSpeed = GetSelectedFanSpeed();

                switch (fanSpeed)
                {
                    case "약":
                        bluetoothManager.SendData("A");
                        break;
                    case "중":
                        bluetoothManager.SendData("B");
                        break;
                    case "강":
                        bluetoothManager.SendData("C");
                        break;
                    default:
                        MessageBox.Show("No fan speed selected.");
                        break;
                }
            }
        }

        private string GetSelectedFanSpeed()
        {
            if (RadioButtonLow.IsChecked == true)
                return RadioButtonLow.Content.ToString();
            if (RadioButtonMedium.IsChecked == true)
                return RadioButtonMedium.Content.ToString();
            if (RadioButtonHigh.IsChecked == true)
                return RadioButtonHigh.Content.ToString();
            return string.Empty; 
        }

        private void Ledcontrol()
        {
            if (int.TryParse(textBox_LuxTick.Text, out int ledBrightness))
            {
                if (ledBrightness >= 1 && ledBrightness <= 10)
                {
                    // LED 밝기 값을 BluetoothManager를 통해 전송
                    bluetoothManager.SendData(ledBrightness.ToString());
                }
                else
                {
                    MessageBox.Show("입력된 값이 유효하지 않습니다. 1에서 10 사이의 값을 입력하세요.");
                }
            }
            else
            {
                MessageBox.Show("숫자를 입력하세요.");
            }
        }

        private void Tempcontrol()
        {
            double sliderValue = TempSlider.Value;
            int adjustedValue = (int)sliderValue + 100; // 슬라이더 값에 100을 더한 후 정수형으로 변환

            string dataToSend = adjustedValue.ToString(); // 문자열로 변환하여 전송 준비
            bluetoothManager.SendData(dataToSend); // BluetoothManager로 데이터 전송
            
        }

        private void Soilmoisture_control()
        {
            if (int.TryParse(textBox_SoilHumidTick.Text, out int soil_moisture))
            {
                if (soil_moisture >= 30 && soil_moisture <= 80)
                {
                    bluetoothManager.SendData(soil_moisture.ToString());
                }
                else
                {
                    MessageBox.Show("입력된 값이 유효하지 않습니다. 30에서 80 사이의 값을 입력하세요.");
                }
            }
            else
            {
                MessageBox.Show("숫자를 입력하세요.");
            }
        }

        private void Watersupply_control()
        {
            if (int.TryParse(textBox_Water_SupplyTick.Text, out int watersupply))
            {
                if (watersupply >= 0 && watersupply <= 250)
                {
                    watersupply += 200;
                    bluetoothManager.SendData(watersupply.ToString());
                }
                else
                {
                    MessageBox.Show("입력된 값이 유효하지 않습니다. 30에서 80 사이의 값을 입력하세요.");
                }
            }
            else
            {
                MessageBox.Show("숫자를 입력하세요.");
            }
        }
    }
}
