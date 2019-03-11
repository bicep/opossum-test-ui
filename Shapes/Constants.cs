using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Shapes
{
    static class Constants
    {
        public const double offset = 0.01;
        public const int fontSize = 12;
        public static PointCollection rectangle = 
            new PointCollection() { new Point(150, 150), new Point(250, 150), new Point(250, 250), new Point(150, 250) };
        public static double crossHairDim = 10;
    }
}
