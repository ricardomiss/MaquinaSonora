using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace MaquinaSonora.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Brush _dynamicBackground = (SolidColorBrush)new BrushConverter().ConvertFromString("#2F030712")!;
        public Brush DynamicBackground
        {
            get => _dynamicBackground;
            set { _dynamicBackground = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
