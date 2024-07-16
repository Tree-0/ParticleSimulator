using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Xps;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;
using Genbox.VelcroPhysics.Collision.Shapes;
using Microsoft.Xna.Framework;

namespace ParticleSimulator.Model
{
    public class Particle
    {
        public Body Body { get; private set; }
        public Ellipse Shape { get; private set; }
        public double Radius { get; private set; }

        public Particle(World world, double x, double y, float radius)
        {
            Radius = radius;
            Shape = new Ellipse() { Width = radius * 2, Height = radius * 2, Fill = Brushes.White };

            Body = BodyFactory.CreateCircle(world, (float)radius, 1f, new Vector2((float)x, (float)y));
            Body.BodyType = BodyType.Dynamic;
            Body.Mass = radius * radius * 3.14f; // proportional to area
            Body.Restitution = 1f; // Bounciness
            Body.Friction = 0f; // Friction
            Body.LinearDamping = 0f; // Air Resistance
        }

        public void UpdateShapePosition()
        {
            Canvas.SetLeft(Shape, Body.Position.X - Radius);
            Canvas.SetTop(Shape, Body.Position.Y - Radius);
        }

        public void ApplyVelocity(Vector2 force)
        {
            Body.LinearVelocity = force;
        }

        public void ApplyImpulse(Vector2 force)
        {
            Body.ApplyLinearImpulse(force);
        }
    }
}

