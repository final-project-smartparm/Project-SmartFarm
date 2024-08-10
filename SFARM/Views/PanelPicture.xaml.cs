using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class PanelPicture : UserControl
    {
        private string imagesDirectory = @"C:\SFAMCapture";

        public PanelPicture()
        {
            InitializeComponent();
            LoadRandomIamge();
            
        }

        private void LoadRandomIamge()
        {
            try
            {
                var images = Directory.GetFiles(imagesDirectory, "*.*")
                                      .Where(file => file.ToLower().EndsWith(".jpg") ||
                                                     file.ToLower().EndsWith(".jpeg") ||
                                                     file.ToLower().EndsWith(".png") ||
                                                     file.ToLower().EndsWith(".bmp") ||
                                                     file.ToLower().EndsWith(".gif"))
                                      .ToArray();

                if (images.Length > 0)
                {
                    Random random = new Random();

                    for (int i = 1; i <= 6; i++)
                    {
                        int randomIndex = random.Next(images.Length);
                        string randomImagePath = images[randomIndex];

                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(randomImagePath, UriKind.Absolute);
                        bitmap.EndInit();

                        ImageBrush imageBrush = new ImageBrush();
                        imageBrush.ImageSource = bitmap;

                        Ellipse ellipse = (Ellipse)FindName($"ImageEllipse{i}");
                        ellipse.Fill = imageBrush;
                    }
                }
                else
                {
                    MessageBox.Show("No images found in the specified directory.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }

        }
    }
}
