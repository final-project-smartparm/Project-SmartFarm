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
    /// PanelLiveChart.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PanelLiveChart : UserControl
    {
        private ScaleTransform scaleTransform;
        public PanelLiveChart()
        {
            InitializeComponent();
            scaleTransform = new ScaleTransform();
            contentGrid.LayoutTransform = scaleTransform;

            scrollViewer.MouseWheel += ScrollViewer_MouseWheel;

        }
        private void ScrollViewer_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            const double zoomFactor = 0.1;
            if (e.Delta > 0)
            {
                // Zoom in
                scaleTransform.ScaleX += zoomFactor;
                scaleTransform.ScaleY += zoomFactor;
            }
            else
            {
                // Zoom out
                scaleTransform.ScaleX = Math.Max(0.1, scaleTransform.ScaleX - zoomFactor);
                scaleTransform.ScaleY = Math.Max(0.1, scaleTransform.ScaleY - zoomFactor);
            }

            e.Handled = true;
        }
    } 
}
