using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Xps;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace ParticleSimulator.Model
{
    public class Particle
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double VX { get; set; }
        public double VY { get; set; }
        public double AX { get; set; }
        public double AY { get; set; }
        public double Radius { get; }
        public Ellipse Shape { get; private set; }

        public Particle(double x = 0, double y = 0, double vx = 0, double vy = 0, double ax = 0, double ay = 0, double radius = 5)
        {
            X = x;
            Y = y;
            VX = vx;
            VY = vy;
            AX = ax;
            AY = ay;
            Radius = radius;
            Shape = new Ellipse() { Width = radius * 2, Height = radius * 2, Fill = Brushes.White };

            Canvas.SetLeft(Shape, x - Radius);
            Canvas.SetTop(Shape, y - Radius);
        }

        public void Update(double dt)
        {
            UpdateVelocity(dt);
            UpdatePosition(dt);
        }

        public void UpdatePosition(double dt)
        {
            X += VX * dt;
            Y += VY * dt;
        }

        public void UpdateVelocity(double dt)
        {
            VX += AX * dt;
            VY += AY * dt;
        }




    }
}
