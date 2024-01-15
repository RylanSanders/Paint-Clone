using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Shaders3
{
    public interface Layer
    {
        public UIElement Layer { get; }
        public string Name { get; }
    }
}
