using System.Windows.Input;

namespace WpfApp.Lib3D
{
    public static class Lib3DCommands
    {
        static Lib3DCommands()
        {
            RefreshVisualCommand = new RoutedUICommand("Refresh", "Lib3D.RefreshVisual", typeof(Lib3DCommands));
            ReShuffleVisualCommand = new RoutedUICommand("Reshuffle", "Lib3D.ReShuffleVisual", typeof(Lib3DCommands));
            ClearViewPortCommand = new RoutedUICommand("Clear", "Lib3D.ClearViewPort", typeof(Lib3DCommands));
        }

        public static readonly RoutedUICommand RefreshVisualCommand = null!;

        public static readonly RoutedUICommand ReShuffleVisualCommand = null!;

        public static readonly RoutedUICommand ClearViewPortCommand = null!;

    }
}
