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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Shaders3
{
    /// <summary>
    /// Interaction logic for LayersPanel.xaml
    /// </summary>
    public partial class LayersPanel : UserControl
    {

        protected ObservableCollection<string> Layers { get; set; }
        private ViewPanel linkedViewPanel;
        public LayersPanel()
        {
            InitializeComponent();
        }

        public void InitializePanel(ViewPanel panel)
        {
            linkedViewPanel = panel;
            Layers = new ObservableCollection<string>(panel.Layers.Select(l=>l.Name));
            LayersListView.ItemsSource = Layers;
        }

        private void LayersPanelItemSelected(object sender, SelectionChangedEventArgs args)
        {
            if (args.AddedItems.Count > 0)
            {
                string layerName  = args.AddedItems[0] as string;
                var topLayer = linkedViewPanel.Layers.Where(l => l.Name == layerName).First();
                linkedViewPanel.Layers.Remove(topLayer);
                linkedViewPanel.Layers.Add(topLayer);
                linkedViewPanel.ReorderLayers();
                linkedViewPanel.ChangeActiveLayer(topLayer);
            }
        }
    }
}
