using System;
using System.Collections.Generic;
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

namespace Shapes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Canvas canvas;
        private Polygon polygon;
        private CustomPolygon customPolygon;
        private Ellipse crossHair;
        private List<string> objectiveNames = new List<string>();
        private List<TextBlock> textBlocks = new List<TextBlock>();

        public MainWindow()
        {
            this.MouseMove += this.onMouseMove;

            // Create a canvas sized to fill the window
            this.canvas = new Canvas();

            // Add polygon
            this.polygon = new Polygon();
            this.polygon.Stroke = Brushes.Black;
            this.polygon.Fill = Brushes.Transparent;
            this.polygon.StrokeThickness = 1;
            // this.polygon.Margin = new Thickness(0, 0, 0, 0.5);
            this.polygon.Points = Constants.rectangle;
            // this.polygon.MouseMove += this.onMouseMovePolygon;
            this.canvas.Children.Add(this.polygon);
            // Make sure the polygon is right at the top so the textbox does not overlay it
            Canvas.SetZIndex(this.polygon, (int)99);

            // Add customPolygon
            this.customPolygon = new CustomPolygon(this.polygon);

            // Add crosshair
            Point centroid = Utilities.getPolygonCentroid(this.polygon);
            // this.crossHair = this.addTextBlockToCanvas("X", centroid.X, centroid.Y);
            this.crossHair = new Ellipse();
            this.crossHair.Width = Constants.crossHairDim;
            this.crossHair.Height = Constants.crossHairDim;
            this.crossHair.Fill = Brushes.Red;
            Canvas.SetLeft(this.crossHair, centroid.X - Constants.crossHairDim/2);
            Canvas.SetTop(this.crossHair, centroid.Y - Constants.crossHairDim/2);
            this.canvas.Children.Add(this.crossHair);

            // Populate based on the number of points in the polygon 
            this.populateObjectiveNames();

            this.labelPolygonPoints();

            // Add objective names to text elements
            this.addObjectiveText();

            this.Content = this.canvas;
            this.Title = "Shapes Demo";
            this.Show();

            // mock click in the center of the polygon
        }

        // set up methods during the initialization phase
        private void populateObjectiveNames()
        {
            for (int i = 0; i<this.polygon.Points.Count; i++)
            {
                string objectiveName = "O" + i.ToString();
                this.objectiveNames.Add(objectiveName);
            }
        }

        private TextBlock addTextBlockToCanvas(string text, double x, double y)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.FontSize = Constants.fontSize;
            textBlock.Text = text;
            Canvas.SetTop(textBlock, y);
            Canvas.SetLeft(textBlock, x);
            this.canvas.Children.Add(textBlock);
            return textBlock;
        }

        private void labelPolygonPoints()
        {
            for (int i = 0; i<this.objectiveNames.Count; i++)
            {
                this.addTextBlockToCanvas(this.objectiveNames[i], this.polygon.Points[i].X, this.polygon.Points[i].Y);
            }
        }

        private void addObjectiveText()
        {
            for (int i = 0; i < this.objectiveNames.Count; i++)
            {
                // addTextBlockToCanvas returns the textBlock that was just added
                this.textBlocks.Add(this.addTextBlockToCanvas(this.objectiveNames[i], 10, (i+1)*20));
            }
        }

        private void displayWeightsFromClick(Point clickPoint)
        {
            Canvas.SetLeft(this.crossHair, clickPoint.X - Constants.crossHairDim / 2);
            Canvas.SetTop(this.crossHair, clickPoint.Y - Constants.crossHairDim / 2);

            List<Double> clickDistances = this.getClickDistanceList(this.polygon.Points, clickPoint);
            List<Double> extendedDistances = this.getExtendedDistanceList(this.polygon.Points, clickPoint);
            // extendedDistances.ForEach(Console.WriteLine);

            List<Double> unnormalizedWeights = this.getUnnormalizedWeights(extendedDistances, clickDistances);
            // unnormalizedWeights.ForEach(Console.WriteLine);

            // Need to include this because line thickness of the polygon causes it to "bleed outwards" from the 
            // polygon's points
            if (Utilities.containsNegative(unnormalizedWeights))
            {
                Console.WriteLine("Out of bounds of the polygon");
            }
            else
            {
                List<Double> normalizedWeights = this.normalizeWeights(unnormalizedWeights);
                for (int i = 0; i < normalizedWeights.Count; i++)
                {
                    this.textBlocks[i].Text = this.objectiveNames[i] + ": " + normalizedWeights[i].ToString();
                }
            }
        }

        private void onMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {

                Point clickPoint = e.GetPosition(this);
                Console.WriteLine("the click point is ({0}, {1})", clickPoint.X, clickPoint.Y);

                if (!CustomPolygon.IsInPolygon(this.polygon.Points, clickPoint))
                {
                    Point closestPoint = CustomPolygon.ClosestPointFromPointToPolygon(this.customPolygon, clickPoint);
                    this.displayWeightsFromClick(closestPoint);
                }
                else
                {
                    this.displayWeightsFromClick(clickPoint);
                }

            }
        }

        private void onMouseMovePolygon(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Console.WriteLine(sender.GetType());
                Point clickPoint = e.GetPosition(this);
                Console.WriteLine("the click point is ({0}, {1})", clickPoint.X, clickPoint.Y);

                Canvas.SetLeft(this.crossHair, clickPoint.X - Constants.crossHairDim / 2);
                Canvas.SetTop(this.crossHair, clickPoint.Y - Constants.crossHairDim / 2);

                List<Double> clickDistances = this.getClickDistanceList(this.polygon.Points, clickPoint);
                List<Double> extendedDistances = this.getExtendedDistanceList(this.polygon.Points, clickPoint);
                // extendedDistances.ForEach(Console.WriteLine);

                List<Double> unnormalizedWeights = this.getUnnormalizedWeights(extendedDistances, clickDistances);
                // unnormalizedWeights.ForEach(Console.WriteLine);

                // Need to include this because line thickness of the polygon causes it to "bleed outwards" from the 
                // polygon's points
                if (Utilities.containsNegative(unnormalizedWeights))
                {
                    Console.WriteLine("Out of bounds of the polygon");
                }
                else
                {
                    List<Double> normalizedWeights = this.normalizeWeights(unnormalizedWeights);
                    for (int i = 0; i < normalizedWeights.Count; i++)
                    {
                        this.textBlocks[i].Text = this.objectiveNames[i] + ": " + normalizedWeights[i].ToString();
                    }
                }
            }
        }

        // Calculate distances for algorithm
        private List<Double> getClickDistanceList(PointCollection points, Point clickPoint)
        {
            List<Double> distances = points.Aggregate(
                new List<Double>(),
                (acc, p) => {
                    acc.Add(Utilities.getDistance(p, clickPoint));
                    return acc;
                });
            return distances;
        }


        private List<Double> getExtendedDistanceList(PointCollection points, Point clickPoint)
        {
            List<Double> extendedDistances = new List<Double>();

            for (int i = 0; i < points.Count; i++)
            {
                Point objectivePoint = points[i];

                // get the line from objective point to the clickpoint
                CustomLine clickLine = new CustomLine(objectivePoint, clickPoint);

                Point? intersectionPoint = null;

                // we cycle through the polygon points 2 at a time, finding the polygon line
                // that intersects with the extended clickLine
                for (int j = 0; j < points.Count; j++)
                {
                    Point currentPoint = points[j];
                    Point nextPoint;

                    if ((j + 1) < points.Count)
                    {
                        nextPoint = points[j + 1];
                    }
                    else
                    {
                        // if the point is the last point, we cycle back to get the point in the 0th index
                        nextPoint = points[0];
                    }

                    CustomLine polygonLine = new CustomLine(currentPoint, nextPoint);
                    
                    intersectionPoint = CustomLine.getIntersectionCoordinates(clickLine, polygonLine);

                    // Make sure the intersection point calculated is not the objectivePoint
                    if (intersectionPoint.HasValue &&

                        // this part is trick af because it is floating point calculation. Need some offset
                        (!Utilities.floatsEqual(objectivePoint.X, intersectionPoint.Value.X, Constants.offset) ||
                        !Utilities.floatsEqual(objectivePoint.Y, intersectionPoint.Value.Y, Constants.offset)) 
                        
                        &&

                        CustomLine.withinLineSegment(intersectionPoint.Value, polygonLine))
                    {
                        // if we have discovered the point we break out of the for loop
                        Console.WriteLine("the valid intersection point is ({0}, {1})", intersectionPoint.Value.X, intersectionPoint.Value.Y);
                        break;
                    }
                }

                // Get distance between the polygon corner and the intersecting coordinate
                if (intersectionPoint.HasValue)
                {
                    double extendedDistance = Utilities.getDistance(intersectionPoint.Value, objectivePoint);
                    extendedDistances.Add(extendedDistance);
                }
                else
                {
                    // I should be getting some kind of intersection here if not 
                    throw new Exception("Intersection Point is null even though it should have a coordinate");
                }
            }

            return extendedDistances;
        }

        private List<Double> getUnnormalizedWeights(List<Double> longDist, List<Double> clickDist) {
            List<Double> unnormWeights = new List<Double>(); 
            for (int i = 0; i<longDist.Count; i++)
            {
                unnormWeights.Add(1 - (clickDist[i] / longDist[i]));
            }
            return unnormWeights;
        }

        private List<Double> normalizeWeights(List<Double> weights)
        {
            List<Double> normalizedWeights = new List<Double>();
            double sum = weights.Sum();
            for (int i = 0; i<weights.Count; i++)
            {
                normalizedWeights.Add((weights[i] / sum) * 100);
            }
            return normalizedWeights;
        }

    }
}
