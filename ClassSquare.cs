using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace oop1
{
    [Serializable]
    public class Square:Rect
    {
        public Square(Color color):base(color){}
        public override void Draw(Graphics graphics)
        {
            base.Draw(graphics);
        }

        public override void UpdateState(Point point)
        {
            base.UpdateState(point);
            FixSquare();    
        }

        private void FixSquare()
        {
            if(points[1].Y < points[0].Y)
            {
                int top = rect.Top + rect.Height - rect.Width;
                var square = new Rectangle(rect.Left,top,rect.Width,rect.Height);
                rect = square;
            }
            rect.Height = rect.Width;
        }

        protected override bool FigureIsReady()
        {
            return base.FigureIsReady();
        }
    }
}
