using System;
using System.Collections.Generic;
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
    /// Interaction logic for OpaqueInkCanvas.xaml
    /// </summary>
    public partial class OpaqueInkCanvas : InkCanvas
    {
        public OpaqueInkCanvas()
        {
            InitializeComponent();
            //Custom Erase at https://learn.microsoft.com/en-us/dotnet/desktop/wpf/advanced/how-to-erase-ink-on-a-custom-control?view=netframeworkdesktop-4.8
            //EditingMode = InkCanvasEditingMode.EraseByPoint;
        }

        protected override void OnStrokeCollected(InkCanvasStrokeCollectedEventArgs e)
        {
            //TODO Here this should submit a stroke to the associated InkCanvas for normal rendering. The stroke added to the other ink canvas should be transparent but otherwise match
            base.OnStrokeCollected(e);
            Stroke originalStroke = e.Stroke.Clone();
            originalStroke.DrawingAttributes.Color = Colors.Transparent;
            
        }
    }
}
