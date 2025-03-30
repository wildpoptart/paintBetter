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

namespace PaintBetter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Initialize text boxes with current canvas size
            WidthTextBox.Text = CanvasBorder.Width.ToString();
            HeightTextBox.Text = CanvasBorder.Height.ToString();

            // Load and apply saved theme
            LoadAndApplyTheme();
        }

        private void LoadAndApplyTheme()
        {
            string savedTheme = Properties.Settings.Default.Theme;
            SwitchTheme(savedTheme);
            UpdateThemeMenuChecks(savedTheme);
        }

        private void SwitchTheme(string themeName)
        {
            // Clear existing theme dictionaries (if any)
            var existingDictionaries = Application.Current.Resources.MergedDictionaries
                .Where(rd => rd.Source != null && rd.Source.OriginalString.StartsWith("Themes/"))
                .ToList(); // Use System.Linq
            
            foreach (var rd in existingDictionaries)
            {
                // Keep Generic.xaml, remove others (like LightTheme.xaml or DarkTheme.xaml)
                if (!rd.Source.OriginalString.Equals("Themes/Generic.xaml", StringComparison.OrdinalIgnoreCase))
                {
                    Application.Current.Resources.MergedDictionaries.Remove(rd);
                }
            }

            // Determine the URI for the new theme dictionary
            string themeUri = themeName == "Dark" ? "Themes/DarkTheme.xaml" : "Themes/LightTheme.xaml";

            // Load and add the new theme dictionary
            var newThemeDictionary = new ResourceDictionary { Source = new Uri(themeUri, UriKind.Relative) };
            Application.Current.Resources.MergedDictionaries.Add(newThemeDictionary);

            // Note: The old ApplyTheme method setting backgrounds directly is no longer needed
        }

        private void UpdateThemeMenuChecks(string themeName)
        {
             LightThemeMenuItem.IsChecked = (themeName == "Light");
             DarkThemeMenuItem.IsChecked = (themeName == "Dark");
        }

        private void ThemeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem clickedItem = sender as MenuItem;
            if (clickedItem == null) return; // Add null check here

            string selectedTheme = "Light"; // Default

            if (clickedItem == LightThemeMenuItem)
            {
                selectedTheme = "Light";
            }
            else if (clickedItem == DarkThemeMenuItem)
            {
                selectedTheme = "Dark";
            }

            // Switch the theme dictionary
            SwitchTheme(selectedTheme);

            // Update menu checks
            UpdateThemeMenuChecks(selectedTheme);

            // Save the selected theme
            Properties.Settings.Default.Theme = selectedTheme;
            Properties.Settings.Default.Save();
        }

        private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            // Get position relative to the InkCanvas itself
            Point position = e.GetPosition(DrawingCanvas);
            CoordinatesText.Text = $"X: {(int)position.X}, Y: {(int)position.Y}";
        }

        private void QuitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ResizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(WidthTextBox.Text, out double newWidth) && 
                double.TryParse(HeightTextBox.Text, out double newHeight) &&
                newWidth > 0 && newHeight > 0)
            {
                CanvasBorder.Width = newWidth;
                CanvasBorder.Height = newHeight;
            }
            else
            {
                MessageBox.Show("Please enter valid positive numbers for width and height.", "Invalid Size", MessageBoxButton.OK, MessageBoxImage.Warning);
                // Optional: Reset text boxes to current size if input is invalid
                WidthTextBox.Text = CanvasBorder.Width.ToString();
                HeightTextBox.Text = CanvasBorder.Height.ToString();
            }
        }
    }
} 