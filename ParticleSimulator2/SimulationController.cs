using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Xps;
using ParticleSimulator.Model;
using ParticleSimulator.View;
using Microsoft.Xna.Framework;
using Genbox.VelcroPhysics.Dynamics;
using System.Windows;

namespace ParticleSimulator.Controller
{
    public class SimulationController
    {
        public Board Board;
        private MainWindow _view;
        private DispatcherTimer timer;
        private Random random;
        private long previousTick;

        public SimulationController(MainWindow view)
        {

            this._view = view; 
            Board = new Board(view.SimulationCanvas.ActualHeight, view.SimulationCanvas.ActualWidth);
            random = new Random();

            // Attach to the SizeChanged event
            view.SimulationCanvas.SizeChanged += OnCanvasSizeChanged;

            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(16.67);
            timer.Start();

            CompositionTarget.Rendering += OnRendering;

            view.CanvasLeftMouseButtonDown += Canvas_LeftMouseButtonDown;
            view.CanvasRightMouseButtonDown += Canvas_RightMouseButtonDown;
            view.KeyDown += Canvas_SpaceBarDown;
            previousTick = DateTime.Now.Ticks;
        }


        //
        // Remove all children from model and view
        //
        public void RemoveAllPhysicsObjects()
        {
            // remove the graphics and the reference lists of bodies from the view and Board
            foreach (Barrier b in this.Board.Barriers)
            {
                _view.SimulationCanvas.Children.Remove(b.Shape);
            }
            this.Board.Barriers.Clear();

            // remove the graphics and the reference lists of bodies from the view and Board
            foreach (Particle p in this.Board.Particles)
            {
                _view.SimulationCanvas.Children.Remove(p.Shape);
            }
            this.Board.Particles.Clear();

            // recreate the whole damn board
            //Board = new Board(_view.SimulationCanvas.ActualHeight, _view.SimulationCanvas.ActualWidth);
        }

        //
        // Update any text, shapes, and movement on screen 
        //
        private void OnRendering(object sender, EventArgs e)
        {
            _view.UpdateParticles(Board.Particles);
            _view.UpdateDebugLabel(DateTime.Now.Ticks);
            _view.UpdateParticleLabel(Board.Particles.FirstOrDefault());
            _view.UpdateMagnitudeLabel(Board.Particles.FirstOrDefault());
        }

        //
        // Update the board at a rate of 120fps
        //
        private void Timer_Tick(object? sender, EventArgs e)
        {
            //long currentTick = DateTime.Now.Ticks;
            //double dt = (currentTick - previousTick) / (double)TimeSpan.TicksPerSecond;
            //previousTick = currentTick;
            double dt = 1.0f / 120.0f;


            Board.UpdateBoard(dt);
        
        }

        //
        // When left mouse button is clicked, add a particle to the board
        //
        private void Canvas_LeftMouseButtonDown(object? sender, MouseEventArgs e)
        {
            var pos = e.GetPosition(_view.SimulationCanvas); // Get position relative to the canvas

            // random properties
            float vx = (random.NextSingle() * 10 - 5);
            float vy = (random.NextSingle() * 10 - 5);
            float radius = random.NextSingle() * 15 + 5;
            SpawnParticle(pos.X, pos.Y, vx, vy, radius);
            
        }

        //
        // When right mouse button is clicked, pull particles towards the cursor
        //
        private void Canvas_RightMouseButtonDown(object? sender, MouseEventArgs e)
        {
            var pos = e.GetPosition(_view.SimulationCanvas);
            Board.PullParticlesToCursor(pos);
        }

        //
        // When the space bar is pressed, add a static barrier at the mouse position.
        // Can collide with particles. 
        //
        private void Canvas_SpaceBarDown(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                var pos = Mouse.GetPosition(_view.SimulationCanvas);
                SpawnBarrier(pos.X, pos.Y);
            }
        }

        //
        // When the f key is pressed, spawn ???
        //


        //
        // Spawn a particle with randomized velocity, radius, and acceleration.
        //
        private void SpawnParticle(double x, double y, float vx=0, float vy=-10, float radius=10)
        {
            Particle p = new Particle(Board.PhysicsWorld, x, y, radius);
            p.SetVelocity(new Vector2(vx, vy) * p.Body.Mass);

            Board.AddParticle(p);
            _view.SimulationCanvas.Children.Add(p.Shape);
        }

        //
        // Spawn a static barrier with fixed radius at the mouse position
        //
        private void SpawnBarrier(double x, double y)
        {
            Barrier b = new Barrier(Board.PhysicsWorld.World, x, y, 40f);

            Board.AddBarrier(b);
            _view.SimulationCanvas.Children.Add(b.Shape);
        }

        //
        // Handle Canvas SizeChanged event
        //
        private void OnCanvasSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateScreenSize(e.NewSize.Height, e.NewSize.Width);

            // Refresh particles
            foreach (var particle in Board.Particles)
            {
                particle.UpdateShapePosition();
            }
        }

        //
        // When the screen is resized, make sure that the border for the physics world updates as well.
        //
        public void UpdateScreenSize(double newHeight, double newWidth)
        {
            RemoveAllPhysicsObjects();
            Board.UpdateBoardSize(newHeight, newWidth);

            // change text
            _view.UpdateScreenSizeLabel(newHeight, newWidth);


        }
    }
}
