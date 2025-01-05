using System.Windows.Input;

namespace WpfApp.Lib3D
{
    public static class Lib3DCommands
    {
        static Lib3DCommands()
        {
            GenerateVisualCommand = new RoutedUICommand("Generate", "Lib3D.GenerateVisual", typeof(Lib3DCommands));
            ShuffleVisualCommand = new RoutedUICommand("Shuffle", "Lib3D.ShuffleVisual", typeof(Lib3DCommands));
            ClearViewPortCommand = new RoutedUICommand("Clear", "Lib3D.ClearViewPort", typeof(Lib3DCommands));
            ZoomExtentCommand = new RoutedUICommand("Zoom Extent", "Lib3D.ZoomExtent", typeof(Lib3DCommands));
            TogglePointSelectionCommand = new RoutedUICommand("Point Selection", "Lib3D.TogglePointSelection", typeof(Lib3DCommands));
            ToggleRectangleSelectionCommand = new RoutedUICommand("Rectangle Selection", "Lib3D.ToggleRectangleSelection", typeof(Lib3DCommands));
        }

        public static readonly RoutedUICommand GenerateVisualCommand = null!;

        public static readonly RoutedUICommand ShuffleVisualCommand = null!;

        public static readonly RoutedUICommand ClearViewPortCommand = null!;

        public static readonly RoutedUICommand ZoomExtentCommand = null!;

        public static readonly RoutedUICommand TogglePointSelectionCommand = null!;

        public static readonly RoutedUICommand ToggleRectangleSelectionCommand = null!;

    }
}
