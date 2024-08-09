using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using ParticleSimulator.Model;
using Genbox.VelcroPhysics;
using Genbox.VelcroPhysics.Dynamics;
using Microsoft.Xna.Framework;
//using System.Numerics;
using System.Windows;

namespace ParticleSimulator.Model
{
    public class Board
    {
        public PhysicsWorld PhysicsWorld { get; private set; }
        public List<Particle> Particles;
        public List<Barrier> Barriers;
        public double Height;
        public double Width;

        public Board(double height, double width) 
        {
            PhysicsWorld = new PhysicsWorld(this);
            Particles = new List<Particle>();
            Barriers = new List<Barrier>();
            Height = height;
            Width = width;
        }

        public void AddParticle(Particle particle)
        {
            Particles.Add(particle);
        }

        public void AddBarrier(Barrier barrier)
        {
            Barriers.Add(barrier);
            barrier.UpdateShapePosition();
        }

        public void UpdateBoard(double dt)
        {
            PhysicsWorld.Update((float)dt);

            foreach (var particle in Particles)
            {
                particle.UpdateShapePosition();
            }
        }

        public void UpdateBoardSize(double height, double width)
        {
            Height = height;
            Width = width;
            PhysicsWorld = new PhysicsWorld(this); // Recreate boundaries
        }

        public void PullParticlesToCursor(Point pos)
        {
            float mouse_x = (float)pos.X;
            float mouse_y = (float)pos.Y;
            foreach (Particle p in Particles) 
            {
                float x = p.Body.Position.X;
                float y = p.Body.Position.Y;
                Vector2 force = new Vector2((mouse_x - x) * 1000, (mouse_y - y) * 1000);
                p.ApplyImpulse(force);                
            }
        }



    }
}
