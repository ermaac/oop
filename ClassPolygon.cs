using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop1
{
    [Serializable]
    public class Polygon:Line
    {
        const int eps = 10;

        public Polygon(Color color):base(color){}

        public override void Draw(Graphics graphics)
        {
            for (int i = 1; i < points.Count; i++)
                graphics.DrawLine(new Pen(color, penWidth), points[i-1], points[i]);
        }

        public override void UpdateState(Point point)
        {
            if (points.Count == 1)
                points.Add(point);
            else points[points.Count - 1] = point;
            RecordPoints = !FigureIsReady();
        }

        private bool CountDelta(Point first, Point second)
        {
            int deltaX = Math.Abs(first.X - second.X);
            int deltaY = Math.Abs(first.Y - second.Y);
            return (deltaX<eps && deltaY<eps);
        }

        protected override bool FigureIsReady()
        {
            if (CountDelta(points.First(), points.Last()))
                points[points.Count-1] = points.First();
            return (points[0] == points.Last()) && points.Count >=3;
        }
    }
}
