using System;
using System.Linq;
using System.Drawing;


namespace oop1
{
    [Serializable]
    public class Line:Figure
    {
        public Line(Color color):base(color) {}
        public override void Draw(Graphics graphics) {
            graphics.DrawLine(new Pen(color, penWidth), points[0], points[1]);
        }
        public override void UpdateState(Point point)
        {
            if (points.Count == 2)
                points[1] = point;
            else points.Add(point);
            RecordPoints = !FigureIsReady();
        }

        protected override bool FigureIsReady()
        {
            return (points.Count == 2);
        }
    }
}
