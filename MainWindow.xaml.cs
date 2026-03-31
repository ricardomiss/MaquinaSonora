using MaquinaSonora.Services;
using MaquinaSonora.Utilities;
using MaquinaSonora.ViewModels;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MaquinaSonora
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IMediaService _mediaService;
        private readonly GradientBackgroundService _gradientBackgroundService = new GradientBackgroundService();
        private readonly MainViewModel _viewModel = new MainViewModel();
        private string _selectedImagePath = string.Empty;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += (s, e) => WindowBlur.EnableBlur(this);
            _gradientBackgroundService.Attach(GradientPointsLayer);
            DataContext = _viewModel;
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
                MaxRestoreButton_Click(sender, e);
            else
                DragMove();
        }

        private void MaxRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                MaxRestoreButton.Content = "□";
            }
            else
            {
                WindowState = WindowState.Maximized;
                MaxRestoreButton.Content = "❐";
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
            => WindowState = WindowState.Minimized;

        private void CloseButton_Click(object sender, RoutedEventArgs e)
                    => Close();

        private void SubirImagenButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                Title = "Select an Image"
            };
            if(openFileDialog.ShowDialog() == false) { return; }

            _selectedImagePath = openFileDialog.FileName;
            SetImage();
            var colors = PaletteExtractor.GetDominantPaletteHex(_selectedImagePath);
            _gradientBackgroundService.UpdatePalette(colors, seed: 5);
        }

        private void SetImage()
        {
            coverimg.Source = new BitmapImage(new Uri(_selectedImagePath));
        }
    }
}