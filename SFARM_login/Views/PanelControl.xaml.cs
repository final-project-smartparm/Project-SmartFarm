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

        private void textBox_TempTick_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

    }
}
