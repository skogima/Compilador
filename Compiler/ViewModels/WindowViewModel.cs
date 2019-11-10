using System.Windows;

namespace Compiler
{
    public class WindowViewModel : BaseViewModel
    {
        public double WindowWidth { get; set; } = 860;

        public double ColumnWidth { get; set; }

        private const double margin = 30;

        public WindowViewModel(Window window)
        {
            window.SizeChanged += (sender, e) =>
            {
                OnNotifyPropertyChanged(nameof(WindowWidth));
                ColumnWidth = (window.ActualWidth / 3) - margin;
            };
        }
    }
}
