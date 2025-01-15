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
            ToggleMeshPointSelectionCommand = new RoutedUICommand("Mesh Point Selection", "Lib3D.ToggleMeshPointSelection", typeof(Lib3DCommands));


            LoadNetworkCommand = new RoutedUICommand("Load Network", "Lib3D.LoadNetwork", typeof(Lib3DCommands));
            NetworkHideTrayCommand = new RoutedUICommand("Hide Tray", "Lib3D.HideTray", typeof(Lib3DCommands));
            NetworkHideJumpCommand = new RoutedUICommand("Hide Jump", "Lib3D.HideJump", typeof(Lib3DCommands));
            NetworkHideDropCommand = new RoutedUICommand("Hide Drop", "Lib3D.HideDrop", typeof(Lib3DCommands));
            NetworkHideTrayNodeCommand = new RoutedUICommand("Hide Tray Node", "Lib3D.HideTrayNode", typeof(Lib3DCommands));
            NetworkHideEquipNodeCommand = new RoutedUICommand("Hide Equipment Node", "Lib3D.HideEquipNode", typeof(Lib3DCommands));
            NetworkClearSelectionCommand = new RoutedUICommand("Clear Selection", "Lib3D.ClearSelection", typeof(Lib3DCommands));
        }

        public static readonly RoutedUICommand GenerateVisualCommand = null!;

        public static readonly RoutedUICommand ShuffleVisualCommand = null!;

        public static readonly RoutedUICommand ClearViewPortCommand = null!;

        public static readonly RoutedUICommand ZoomExtentCommand = null!;

        public static readonly RoutedUICommand TogglePointSelectionCommand = null!;

        public static readonly RoutedUICommand ToggleRectangleSelectionCommand = null!;

        public static readonly RoutedUICommand ToggleMeshPointSelectionCommand = null!;


        public static readonly RoutedUICommand LoadNetworkCommand = null!;

        public static readonly RoutedUICommand NetworkHideTrayCommand = null!;

        public static readonly RoutedUICommand NetworkHideJumpCommand = null!;

        public static readonly RoutedUICommand NetworkHideDropCommand = null!;

        public static readonly RoutedUICommand NetworkHideTrayNodeCommand = null!;

        public static readonly RoutedUICommand NetworkHideEquipNodeCommand = null!;

        public static readonly RoutedUICommand NetworkClearSelectionCommand = null!;

    }
}
