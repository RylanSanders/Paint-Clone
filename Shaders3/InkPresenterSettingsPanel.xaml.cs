using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Shaders3
{
    /// <summary>
    /// Interaction logic for InkPresenterSettingsPanel.xaml
    /// </summary>
    public partial class InkPresenterSettingsPanel : UserControl
    {
        public InkCanvas _Canvas;
        public ObservableCollection<ColorButtonModel> _ColorButtons;
        public InkPresenterSettingsPanel(InkCanvas canvas)
        {
            _ColorButtons = new ObservableCollection<ColorButtonModel>();
            InitializeComponent();
            _Canvas = canvas;
            _ColorButtons.Add(new ColorButtonModel(Brushes.Green, Colors.Green));
            _ColorButtons.Add(new ColorButtonModel(Brushes.Blue, Colors.Blue));
            _ColorButtons.Add(new ColorButtonModel(Brushes.Gray, Colors.Gray));
            _ColorButtons.Add(new ColorButtonModel(Brushes.Gold, Colors.Gold));
            _ColorButtons.Add(new ColorButtonModel(Brushes.White, Colors.White));
            ColorButtonsListView.ItemsSource = _ColorButtons;
        }
        
        public void ColorButtonClicked(object sender, RoutedEventArgs e)
        {
            var colormodel = ((Button)sender).DataContext as ColorButtonModel;
            if (colormodel != null)
            {
                DrawingAttributes attributes = new DrawingAttributes();
                attributes.Color = colormodel.ButtonColor;
                _Canvas.DefaultDrawingAttributes = attributes;
            }
            
           

        }
        public class ColorButtonModel
        {
            public Brush ButtonColorBrush
            {
                get; set;
            }

            public Color ButtonColor;

            public ColorButtonModel(Brush color, Color buttonColor)
            {
                ButtonColorBrush = color;
                ButtonColor = buttonColor;
            }

            public string ColorString { get { return ButtonColorBrush.ToString(); } }
        }
    }
}
