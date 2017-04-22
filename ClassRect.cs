using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace oop1
{
    [Serializable]
    public class Rect:Figure
    {
        protected Rectangle rect;
        public Rect(Color color) : base(color) {}
        public override void Draw(Graphics graphics)
        {
            graphics.DrawRectangle(new Pen(color, penWidth),rect);
           
        }

        public override void UpdateState(Point point)
        {
            if (points.Count == 2)
                points[1] = point;
            else points.Add(point);
            rect = GetRect();
            RecordPoints = !FigureIsReady();
        }

        protected override bool FigureIsReady()
        {
            return points.Count == 2;
        }

        protected Rectangle GetRect()
        {
            Rectangle result;
            int width = Math.Abs(points[0].X-points[1].X);
            int height = Math.Abs(points[0].Y - points[1].Y);
            int left = Math.Min(points[0].X, points[1].X);
            int top = Math.Min(points[0].Y, points[1].Y);        
            result = new Rectangle(left, top, width, height);
            return result;
        }

    }
}
