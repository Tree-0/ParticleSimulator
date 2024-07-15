using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using ParticleSimulator.Model;

namespace ParticleSimulator.Model
{
    public class Board
    {
        public List<Particle> Particles;
        double Height;
        double Width;

        public Board(double height, double width) 
        {
            Particles = new List<Particle>();
            Height = height;
            Width = width;
        }

        public void AddParticle(Particle particle)
        {
            Particles.Add(particle);
        }

        public void UpdateBoard(double dt)
        {
            foreach (Particle particle in Particles)
            {
                HandleBoundaryCollision(particle);
                particle.Update(dt);
            }
        }

        public void UpdateBoardSize(double height, double width)
        {
            Height = height;
            Width = width;
        }

        public void HandleBoundaryCollision(Particle particle)
        {
            if (particle.X - particle.Radius <= 0 || particle.X + particle.Radius >= Width)
            {
                particle.VX = -particle.VX;
            }
            if (particle.Y - particle.Radius <= 0 || particle.Y + particle.Radius >= Height)
            {
                particle.VY = -particle.VY;
            }
        }
    }
}
