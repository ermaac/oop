using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace oop1
{
    [Serializable]
    public class Circle:Square
    {
        public Circle(Color color):base(color){}

        public override void Draw(Graphics graphics)
        {
            graphics.DrawEllipse(new Pen(color, penWidth), rect);
        }
        public override void UpdateState(Point point)
        {
            base.UpdateState(point);
        }

        protected override bool FigureIsReady()
        {
            return base.FigureIsReady();
        }
    }
}
