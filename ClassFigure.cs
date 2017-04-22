using System;
using System.Collections.Generic;
using System.Drawing;

namespace oop1
{
    [Serializable]
    public abstract class Figure
    {
        protected Color color;
        protected int penWidth = 2;
        public bool RecordPoints {get; set; }
        public List<Point> points { get; set; }

        protected Figure(Color color) {
            RecordPoints = false;
            points = new List<Point>();
            this.color = color;
        }

        public abstract void Draw(Graphics graphics);
        public abstract void UpdateState(Point point);
        protected abstract bool FigureIsReady();
    }
}
