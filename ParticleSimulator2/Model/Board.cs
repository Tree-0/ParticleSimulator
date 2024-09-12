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
using System.Diagnostics;
using System.ComponentModel;

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
            PhysicsWorld.PropertyChanged += OnPhysicsPropertyChanged; // when properties in view are changed, update particles

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
            // Apply the physics world properties to the particles before stepping the world
            foreach (var particle in Particles)
            {
                foreach (var fixture in particle.Body.FixtureList)
                {
                    fixture.Friction = PhysicsWorld.Friction;
                    fixture.Restitution = PhysicsWorld.Restitution;
                }
                particle.Body.LinearDamping = PhysicsWorld.AirResistance;
            }

            // Step the physics world after applying the properties
            PhysicsWorld.Update((float)dt);

            // Update particle positions
            foreach (var particle in Particles)
            {
                particle.UpdateShapePosition();
            }
        }

        public void OnPhysicsPropertyChanged(Object sender, PropertyChangedEventArgs e)
        {
            Debug.WriteLine($"OnPhysicsPropertyChanged Called for {e.PropertyName}");
            // Ensure there are particles and that the first particle has fixtures
            if (Particles.Any() && Particles.First().Body.FixtureList.Any())
            {
                Debug.WriteLine($"Particle friction before update: {Particles.First().Body.FixtureList.First().Friction}");
            }
            else
            {
                Debug.WriteLine("No fixtures found in the first particle.");
            }

            switch (e.PropertyName)
            {
                case "Friction":
                    //Debug.WriteLine("Friction Case Entered");
                    foreach (Particle p in Particles)
                    {
                        Debug.WriteLine($"Number of fixtures: {p.Body.FixtureList.Count}");
                        foreach (var fixture in p.Body.FixtureList)
                        {
                            Debug.WriteLine("in fixtureList for loop");
                            fixture.Friction = PhysicsWorld.Friction;
                        }
                    }
                    break;
                case "Restitution":
                    foreach (Particle p in Particles)
                    {
                        foreach (var fixture in p.Body.FixtureList)
                        {
                            fixture.Restitution = PhysicsWorld.Restitution;
                        }
                    }
                    break;
                case "AirResistance":
                    foreach (Particle p in Particles)
                    {
                        p.Body.LinearDamping = PhysicsWorld.AirResistance;
                    }
                    break;
                case "Gravity":
                    this.PhysicsWorld.World.Gravity = new Vector2(0, 1) * PhysicsWorld.Gravity;
                    break;
            }

            if (Particles.Any() && Particles.First().Body.FixtureList.Any())
            {
                Debug.WriteLine($"Particle friction after update: {Particles.First().Body.FixtureList.First().Friction}");
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
                Vector2 force = new Vector2((mouse_x - x), (mouse_y - y));
                force.Normalize();
                force *= 100000f;

                //Debug.WriteLine($"Before Impulse: {p.Body.LinearVelocity}");
                p.ApplyImpulse(force);
                //Debug.WriteLine($"After Impulse: {p.Body.LinearVelocity}");

            }
        }

        public void TestPropertyChange()
        {
            var testParticle = Particles.FirstOrDefault();
            if (testParticle != null)
            {
                foreach (var fixture in testParticle.Body.FixtureList)
                {
                    fixture.Friction = 0.9f; // Set to a noticeable value
                    fixture.Restitution = 0.1f; // Set to a noticeable value
                }
                testParticle.Body.LinearDamping = 0.5f;

                PhysicsWorld.World.Gravity = new Vector2(0, 1) * 100; // Set to a noticeable value

                // Log the properties
                //Debug.WriteLine($"Test Friction: {testParticle.Body.FixtureList.First().Friction}");
                //Debug.WriteLine($"Test Restitution: {testParticle.Body.FixtureList.First().Restitution}");
                //Debug.WriteLine($"Test LinearDamping: {testParticle.Body.LinearDamping}");
                //Debug.WriteLine($"Test Gravity: {PhysicsWorld.World.Gravity.ToString}");
            }
        }


    }
}
