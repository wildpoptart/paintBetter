using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq;
using System.Windows.Media.Animation;
using System.Windows.Ink;
using System.Windows.Forms;
using WinFormsColor = System.Drawing.Color;
using WpfColor = System.Windows.Media.Color;

namespace PaintBetter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isFilePanelOpen = false;
        private Storyboard slideInStoryboard = new Storyboard();
        private Storyboard slideOutStoryboard = new Storyboard();
        private const double FilePanelWidth = 250.0;

        // --- Interaction State ---
        private enum DragMode { None, Scrolling, DraggingCanvas }
        private DragMode currentDragMode = DragMode.None;
        private System.Windows.Point dragStartPointInScrollViewer;
        private System.Windows.Point dragStartPointOnCanvas;
        private double canvasStartLeft;
        private double canvasStartTop;

        // For zooming
        private const double ZoomFactor = 1.1;
        private const double MaxZoom = 5.0;
        private const double MinZoom = 0.2;

        // For brush size
        private const double BrushSizeStep = 0.5;
        private const double MaxBrushSize = 50.0;
        private const double MinBrushSize = 1.0;

        public MainWindow()
        {
            InitializeComponent();
            //InitializeAnimations(); // Keep disabled until themes/interaction confirmed

            WidthTextBox.Text = CanvasBorder.Width.ToString();
            HeightTextBox.Text = CanvasBorder.Height.ToString();
            LoadAndApplyTheme();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CenterCanvas();
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Re-center the canvas whenever the window size changes
             // Note: This might have unintended effects if the user has manually dragged the canvas.
             // Consider adding a flag or logic if you want to preserve user-dragged position across resizes.
            CenterCanvas();
        }

        private void CenterCanvas()
        {
            // Ensure the container panel has had a chance to measure
            if (CanvasContainerPanel.ActualWidth <= 0 || CanvasContainerPanel.ActualHeight <= 0)
            {
                return; // Exit if container not ready
            }

            // Calculate centered position based on current container size and canvas size
            double centerLeft = (CanvasContainerPanel.ActualWidth - CanvasBorder.Width * CanvasScaleTransform.ScaleX) / 2.0;
            double centerTop = (CanvasContainerPanel.ActualHeight - CanvasBorder.Height * CanvasScaleTransform.ScaleY) / 2.0;

            // Clamp to ensure canvas doesn't start with negative coordinates
            centerLeft = Math.Max(0, centerLeft);
            centerTop = Math.Max(0, centerTop);

            Canvas.SetLeft(CanvasBorder, centerLeft);
            Canvas.SetTop(CanvasBorder, centerTop);
        }

        private void InitializeAnimations()
        {
            Duration duration = new Duration(TimeSpan.FromSeconds(0.3));
            var ease = new CubicEase { EasingMode = EasingMode.EaseOut };

            // Slide In Animation
            var slideInAnimation = new DoubleAnimation
            {
                To = 0, // Target X position (visible)
                Duration = duration,
                EasingFunction = ease
            };
            Storyboard.SetTarget(slideInAnimation, FilePanelTranslateTransform);
            Storyboard.SetTargetProperty(slideInAnimation, new PropertyPath(TranslateTransform.XProperty));
            slideInStoryboard.Children.Clear();
            slideInStoryboard.Children.Add(slideInAnimation);

            // Slide Out Animation
            var slideOutAnimation = new DoubleAnimation
            {
                To = -FilePanelWidth,
                Duration = duration,
                EasingFunction = ease
            };
             Storyboard.SetTarget(slideOutAnimation, FilePanelTranslateTransform);
            Storyboard.SetTargetProperty(slideOutAnimation, new PropertyPath(TranslateTransform.XProperty));
            slideOutStoryboard.Children.Clear();
            slideOutStoryboard.Children.Add(slideOutAnimation);
        }

        private void LoadAndApplyTheme()
        {
            string savedTheme = Properties.Settings.Default.Theme;
            SwitchTheme(savedTheme);
            UpdateThemeRadioButtons(savedTheme);
        }

        private void SwitchTheme(string themeName)
        {
            var existingDictionaries = System.Windows.Application.Current.Resources.MergedDictionaries // Qualify Application
                .Where(rd => rd.Source != null && rd.Source.OriginalString.StartsWith("Themes/"))
                .ToList();
            
            foreach (var rd in existingDictionaries)
            {
                // Keep Generic.xaml, remove others (like LightTheme.xaml or DarkTheme.xaml)
                if (!rd.Source.OriginalString.Equals("Themes/Generic.xaml", StringComparison.OrdinalIgnoreCase))
                {
                    System.Windows.Application.Current.Resources.MergedDictionaries.Remove(rd); // Qualify Application
                }
            }

            string themeUri = themeName == "Dark" ? "Themes/DarkTheme.xaml" : "Themes/LightTheme.xaml";
            var newThemeDictionary = new ResourceDictionary { Source = new Uri(themeUri, UriKind.Relative) };
            System.Windows.Application.Current.Resources.MergedDictionaries.Add(newThemeDictionary); // Qualify Application

            // Ensure foreground is updated correctly (already set on Window in XAML, but good to be explicit if needed)
            // this.Foreground = (Brush)FindResource("ThemeForegroundBrush"); 
        }

        private void UpdateThemeRadioButtons(string themeName)
        {
             LightThemeRadio.IsChecked = (themeName == "Light");
             DarkThemeRadio.IsChecked = (themeName == "Dark");
        }

        private void FileButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isFilePanelOpen)
            {
                 FilePanelTranslateTransform.X = 0; 
                isFilePanelOpen = true;
            }
        }

        private void CloseFilePanel()
        {
            if (isFilePanelOpen)
            {
                // Consider using animation if InitializeAnimations was enabled
                // slideOutStoryboard.Begin(); 
                FilePanelTranslateTransform.X = -FilePanelWidth; 
                isFilePanelOpen = false;
            }
        }

        private void CloseFilePanelButton_Click(object sender, RoutedEventArgs e)
        {
             CloseFilePanel(); // Call the refactored method
        }

        private void FilePanelSettingsThemeButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedTheme = "Light";
            if (sender == DarkThemeRadio)
            {
                selectedTheme = "Dark";
            }
            SwitchTheme(selectedTheme);
            UpdateThemeRadioButtons(selectedTheme);
            Properties.Settings.Default.Theme = selectedTheme;
            Properties.Settings.Default.Save(); 
        }

        private void FilePanelQuitButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown(); // Fully qualify Application
        }

        private void CanvasScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            bool ctrlDown = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
            bool shiftDown = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);

            if (ctrlDown)
            {
                e.Handled = true; // Handle the event to prevent default scrolling

                // --- Zooming --- 
                System.Windows.Point mousePosInScrollViewer = e.GetPosition(CanvasScrollViewer);
                double currentScale = CanvasScaleTransform.ScaleX; // Assuming uniform scaling

                // Calculate zoom factor and new scale, clamping included
                double zoom = e.Delta > 0 ? ZoomFactor : 1.0 / ZoomFactor;
                double newScale = Math.Clamp(currentScale * zoom, MinZoom, MaxZoom);

                if (currentScale == newScale) return; // No change if clamped

                // Calculate position relative to the CanvasBorder for ScaleTransform center
                 System.Windows.Point mousePosOnBorder = e.GetPosition(CanvasBorder);
                 CanvasScaleTransform.CenterX = mousePosOnBorder.X;
                 CanvasScaleTransform.CenterY = mousePosOnBorder.Y;

                 // Calculate the absolute position of the mouse pointer within the scrollable content before zoom
                 double absoluteX = CanvasScrollViewer.HorizontalOffset + mousePosInScrollViewer.X;
                 double absoluteY = CanvasScrollViewer.VerticalOffset + mousePosInScrollViewer.Y;

                 // Apply the new scale
                 CanvasScaleTransform.ScaleX = newScale;
                 CanvasScaleTransform.ScaleY = newScale;

                 // Calculate where the logical point under the mouse should be *after* scaling
                 double newAbsoluteX = (absoluteX / currentScale) * newScale;
                 double newAbsoluteY = (absoluteY / currentScale) * newScale;

                 // Calculate the required scroll offset to keep the point under the mouse
                 double newOffsetX = newAbsoluteX - mousePosInScrollViewer.X;
                 double newOffsetY = newAbsoluteY - mousePosInScrollViewer.Y;

                 // Scroll to the new offsets
                 CanvasScrollViewer.ScrollToHorizontalOffset(newOffsetX);
                 CanvasScrollViewer.ScrollToVerticalOffset(newOffsetY);
            }
            else if (shiftDown)
            {
                e.Handled = true; // Handle the event
                // --- Brush Size --- 
                DrawingAttributes inkAttributes = DrawingCanvas.DefaultDrawingAttributes;
                double change = e.Delta > 0 ? BrushSizeStep : -BrushSizeStep;
                double newSize = Math.Clamp(inkAttributes.Width + change, MinBrushSize, MaxBrushSize);
                if (inkAttributes.Width != newSize) 
                {
                    inkAttributes.Width = newSize;
                    inkAttributes.Height = newSize;
                }
            }
        }

        private void CanvasScrollViewer_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle && e.ButtonState == MouseButtonState.Pressed)
            {
                dragStartPointInScrollViewer = e.GetPosition(CanvasScrollViewer); 
                dragStartPointOnCanvas = e.GetPosition(CanvasContainerPanel); // Position relative to Canvas panel
                
                // Use hit testing on the container Canvas panel
                var hitElement = CanvasContainerPanel.InputHitTest(dragStartPointOnCanvas) as FrameworkElement;

                if (hitElement == CanvasContainerPanel) // Clicked directly on the Canvas background
                { 
                    // Clicked on background - start DRAGGING mode
                    currentDragMode = DragMode.DraggingCanvas;
                    canvasStartLeft = Canvas.GetLeft(CanvasBorder);
                    canvasStartTop = Canvas.GetTop(CanvasBorder);
                    CanvasScrollViewer.Cursor = System.Windows.Input.Cursors.SizeAll; // Qualify Cursors
                }
                else // Clicked on the CanvasBorder or something inside it
                {   
                    // Clicked on content - start SCROLLING mode
                    currentDragMode = DragMode.Scrolling;
                    CanvasScrollViewer.Cursor = System.Windows.Input.Cursors.ScrollAll; // Qualify Cursors
                }

                e.Handled = true; 
            }
        }

        private void CanvasScrollViewer_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle && currentDragMode != DragMode.None)
            {
                currentDragMode = DragMode.None;
                CanvasScrollViewer.Cursor = System.Windows.Input.Cursors.Arrow; // Qualify Cursors
                e.Handled = true;
            }
        }

        private void CanvasScrollViewer_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            System.Windows.Point currentPointInScrollViewer = e.GetPosition(CanvasScrollViewer);
            double deltaXScrollViewer = currentPointInScrollViewer.X - dragStartPointInScrollViewer.X;
            double deltaYScrollViewer = currentPointInScrollViewer.Y - dragStartPointInScrollViewer.Y;

            if (currentDragMode == DragMode.Scrolling)
            {
                // Scroll the viewport
                CanvasScrollViewer.ScrollToHorizontalOffset(CanvasScrollViewer.HorizontalOffset - deltaXScrollViewer);
                CanvasScrollViewer.ScrollToVerticalOffset(CanvasScrollViewer.VerticalOffset - deltaYScrollViewer);
                // Update the start point for scrolling delta calculation
                 dragStartPointInScrollViewer = currentPointInScrollViewer; 
            }
            else if (currentDragMode == DragMode.DraggingCanvas)
            {
                 // Drag the canvas by changing Canvas.Left/Top
                 // Use the delta relative to the start point on the Canvas panel for positioning
                 System.Windows.Point currentPointOnCanvas = e.GetPosition(CanvasContainerPanel);
                 double deltaXCanvas = currentPointOnCanvas.X - dragStartPointOnCanvas.X;
                 double deltaYCanvas = currentPointOnCanvas.Y - dragStartPointOnCanvas.Y;

                 double newLeft = canvasStartLeft + deltaXCanvas;
                 double newTop = canvasStartTop + deltaYCanvas;

                 Canvas.SetLeft(CanvasBorder, newLeft);
                 Canvas.SetTop(CanvasBorder, newTop);
            }
           
            // Update coordinate display (relative to InkCanvas)
             System.Windows.Point positionInInkCanvas = e.GetPosition(DrawingCanvas);
             CoordinatesText.Text = $"X: {(int)positionInInkCanvas.X}, Y: {(int)positionInInkCanvas.Y}";
        }

        private void DrawingCanvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
             System.Windows.Point positionInInkCanvas = e.GetPosition(DrawingCanvas);
              CoordinatesText.Text = $"X: {(int)positionInInkCanvas.X}, Y: {(int)positionInInkCanvas.Y}";
        }

        private void ResizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(WidthTextBox.Text, out double newWidth) && 
                double.TryParse(HeightTextBox.Text, out double newHeight) &&
                newWidth > 0 && newHeight > 0)
            {
                CanvasBorder.Width = newWidth;
                CanvasBorder.Height = newHeight;
                CenterCanvas();
            }
            else
            {
                System.Windows.MessageBox.Show("Please enter valid positive numbers for width and height.", "Invalid Size", MessageBoxButton.OK, MessageBoxImage.Warning); // Qualify MessageBox
                WidthTextBox.Text = CanvasBorder.Width.ToString();
                HeightTextBox.Text = CanvasBorder.Height.ToString();
            }
        }

        // --- Color Swatch Handling ---

        private void ColorSwatch_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border swatch)
            {
                SolidColorBrush currentSwatchBrush = swatch.Background as SolidColorBrush;

                // Single Click: Set primary color
                if (e.ClickCount == 1)
                {
                    if (currentSwatchBrush != null)
                    {
                        PrimaryColorIndicator.Background = currentSwatchBrush;
                        DrawingCanvas.DefaultDrawingAttributes.Color = currentSwatchBrush.Color;
                        // Optionally: Update secondary color logic here if needed later
                    }
                }
                // Double Click: Open Color Picker
                else if (e.ClickCount == 2)
                {
                    ColorDialog colorDialog = new ColorDialog();
                    colorDialog.AllowFullOpen = true;
                    colorDialog.AnyColor = true;
                    
                    // Initialize with current swatch color OR default to white
                    if (currentSwatchBrush != null)
                    {
                        WpfColor wpfColor = currentSwatchBrush.Color;
                        colorDialog.Color = WinFormsColor.FromArgb(wpfColor.A, wpfColor.R, wpfColor.G, wpfColor.B);
                    }
                    else
                    {
                        // Default color if the swatch background isn't a SolidColorBrush
                        colorDialog.Color = WinFormsColor.White; 
                    }

                    if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        WinFormsColor selectedWinColor = colorDialog.Color;
                        WpfColor selectedWpfColor = WpfColor.FromArgb(selectedWinColor.A, selectedWinColor.R, selectedWinColor.G, selectedWinColor.B);
                        SolidColorBrush newBrush = new SolidColorBrush(selectedWpfColor);

                        swatch.Background = newBrush;

                        // Check if primary indicator needs updating (if it held the OLD swatch color)
                        if (PrimaryColorIndicator.Background is SolidColorBrush primaryIndicatorBrush &&
                            currentSwatchBrush != null && // Ensure we have the old brush color
                            primaryIndicatorBrush.Color == currentSwatchBrush.Color)
                        {
                            PrimaryColorIndicator.Background = newBrush;
                            DrawingCanvas.DefaultDrawingAttributes.Color = selectedWpfColor;
                        }
                        // Optionally: Update secondary indicator logic here if needed later
                    }
                    e.Handled = true; // Prevent single-click logic firing after double-click
                }
            }
        }

        // --- Helper Method for Visual Tree Traversal ---
        private bool IsElementOrDescendantOf(DependencyObject? child, DependencyObject? potentialParent)
        {
            if (child == null || potentialParent == null) return false;
            if (child == potentialParent) return true;

            DependencyObject? parent = VisualTreeHelper.GetParent(child);
            while (parent != null)
            {
                if (parent == potentialParent) return true;
                parent = VisualTreeHelper.GetParent(parent);
            }
            return false;
        }

        // --- Updated Handler for Click-Outside-to-Close ---
        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Only act if the file panel is open
            if (isFilePanelOpen)
            {
                // Get the element that was actually clicked
                if (e.OriginalSource is DependencyObject originalSource)
                {
                    // Check if the click was *outside* the FilePanel AND *not* on the FileButton
                    // Use the new helper method for checking descendants.
                    if (!IsElementOrDescendantOf(originalSource, FilePanel) && 
                        !IsElementOrDescendantOf(originalSource, FileButton)) 
                    {
                        CloseFilePanel();
                        // Optional: Mark the event as handled
                        // e.Handled = true; 
                    }
                }
            }
        }

    }
} 