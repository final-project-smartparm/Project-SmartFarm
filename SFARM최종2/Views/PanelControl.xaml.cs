﻿using InTheHand.Net.Sockets;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SFARM.Views
{
    public partial class PanelControl : UserControl
    {

        // BluetoothManager 인스턴스 가져오기
        private BluetoothManager bluetoothManager = BluetoothManager.Instance;

        public PanelControl()
        {
            InitializeComponent();

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null && bluetoothManager != null)
            {
                string fanSpeed = rb.Content.ToString();
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
                }
            }
        }
        private void TextBox_LEDLightTick_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(textBox_LEDLightTick.Text, out int ledBrightness))
            {
                if (ledBrightness >= 1 && ledBrightness <= 10)
                {
                    // LED 밝기 값을 BluetoothManager를 통해 전송
                    bluetoothManager.SendData($"ledbrigth:{ledBrightness}");
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

        private void TempSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SendTemperatureToArduino();
            //double sliderValue = TempSlider.Value;

            // 아두이노로 전송할 값을 현재 슬라이더 값보다 100 크게 설정하고 문자열로 변환
            // string adjustedValue = (sliderValue + 100.0).ToString();

            // adjustedValue를 아두이노로 전송하는 코드를 추가하세요
            // bluetoothManager.SendData(adjustedValue);
        }

        private void SendTemperatureToArduino()
        {
           
        }
    }
}
