using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace MaquinaSonora.Services
{
    public sealed class GradientBackgroundService
    {
        private Canvas? _host;
        private IReadOnlyList<string> _colors = Array.Empty<string>();
        private int _seed = 1;

        public void Attach(Canvas host)
        {
            Detach();

            _host = host;
            _host.Loaded += Host_Loaded;
            _host.SizeChanged += Host_SizeChanged;
            Rebuild();
        }

        private void Detach()
        {
            if (_host == null) return;
            _host.Loaded -= Host_Loaded;
            _host.SizeChanged -= Host_SizeChanged;
            _host.Children.Clear();
            _host = null;
        }

        public void UpdatePalette(IReadOnlyList<string> colors, int seed = 1)
        {
            _colors = colors;
            _seed = seed;
            Rebuild();
        }

        private void Host_Loaded(object sender, RoutedEventArgs e) => Rebuild();
        private void Host_SizeChanged(object sender, SizeChangedEventArgs e) => Rebuild();

        private void Rebuild()
        {
            if (_host == null) return;

            double w = _host.RenderSize.Width;
            double h = _host.RenderSize.Height;

            if (w <= 0 || h <= 0)
            {
                _host.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(Rebuild));
                return;
            }

            _host.Children.Clear();
            if(_colors.Count == 0) return;
            
            MultiPointGradient(w, h);
        }

        private void MultiPointGradient(double w, double h)
        {
            var rnd = new Random(_seed);

            for (int i = 0; i < _colors.Count; i++)
            {
                var color = (Color)ColorConverter.ConvertFromString(_colors[i]);

                var blob = new Border
                {
                    Width = w * 1.2,
                    Height = h * 1.2,
                    Opacity = 0.55,
                    Background = new RadialGradientBrush
                    {
                        Center = new Point(0.5, 0.5),
                        GradientOrigin = new Point(0.5, 0.5),
                        RadiusX = 0.5,
                        RadiusY = 0.5,
                        GradientStops = new GradientStopCollection
                        {
                            new GradientStop(color, 0.0),
                            new GradientStop(Color.FromArgb(0, color.R, color.G, color.B), 1.0)
                        }
                    }
                };

                double x = (rnd.NextDouble() - 0.5) * w;
                double y = (rnd.NextDouble() - 0.5) * h;

                Canvas.SetLeft(blob, x);
                Canvas.SetTop(blob, y);

                _host!.Children.Add(blob);
            }
        }
    }
}
