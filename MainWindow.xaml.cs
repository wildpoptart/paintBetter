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

        public MainWindow()
        {
            InitializeComponent();
            //InitializeAnimations(); // Temporarily disable animations

            // Initialize text boxes with current canvas size
            WidthTextBox.Text = CanvasBorder.Width.ToString();
            HeightTextBox.Text = CanvasBorder.Height.ToString();

            // Load and apply saved theme
            //LoadAndApplyTheme(); // Temporarily disable theme loading
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

        private void UpdateThemeRadioButtons(string themeName)
        {
             LightThemeRadio.IsChecked = (themeName == "Light");
             DarkThemeRadio.IsChecked = (themeName == "Dark");
        }

        private void FileButton_Click(object sender, RoutedEventArgs e)
        {
            // Animation will not work now as InitializeAnimations is disabled
            if (!isFilePanelOpen)
            {
                // slideInStoryboard.Begin(); // Keep commented
                // Directly set position to test if panel can show without animations/themes
                 FilePanelTranslateTransform.X = 0; 
                isFilePanelOpen = true;
            }
        }

        private void CloseFilePanelButton_Click(object sender, RoutedEventArgs e)
        {
             // Animation will not work now as InitializeAnimations is disabled
             if (isFilePanelOpen)
            {
                // slideOutStoryboard.Begin(); // Keep commented
                 // Directly set position to test if panel can hide without animations/themes
                 FilePanelTranslateTransform.X = -FilePanelWidth; 
                isFilePanelOpen = false;
            }
        }

        private void FilePanelSettingsThemeButton_Click(object sender, RoutedEventArgs e)
        {
            // Theme switching will not work now as LoadAndApplyTheme/SwitchTheme are disabled
            string selectedTheme = "Light";
            if (sender == DarkThemeRadio)
            {
                selectedTheme = "Dark";
            }
            // SwitchTheme(selectedTheme); // Keep commented
            // UpdateThemeRadioButtons(selectedTheme); // Keep commented
            
            // Still save setting, but UI won't update
            Properties.Settings.Default.Theme = selectedTheme;
            Properties.Settings.Default.Save(); 
        }

        private void FilePanelQuitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            // Get position relative to the InkCanvas itself
            Point position = e.GetPosition(DrawingCanvas);
            CoordinatesText.Text = $"X: {(int)position.X}, Y: {(int)position.Y}";
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