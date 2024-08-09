using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Xna.Framework;
using System.Windows.Controls;

namespace ParticleSimulator
{
    public class Barrier
    {
        public Body Body { get; private set; }
        public Rectangle Shape { get; private set; }
        public double Radius { get; private set; }

        public Barrier(World world, double x, double y, float radius)
        {
            Radius = radius;
            Shape = new Rectangle() { Width = radius, Height = radius, Fill = Brushes.Beige };

            Body = BodyFactory.CreateRectangle(world, radius, radius, 1f, new Vector2((float)x, (float)y));
        }

        public void UpdateShapePosition()
        {
            Canvas.SetLeft(Shape, Body.Position.X - Radius/2);
            Canvas.SetTop(Shape, Body.Position.Y - Radius/2);
        }
    }
}
