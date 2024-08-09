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

namespace ParticleSimulator.Controller
{
    public class SimulationController
    {
        private Board _board;
        private MainWindow _view;
        private DispatcherTimer timer;
        private Random random;
        private long previousTick;

        public SimulationController(MainWindow view)
        {

            this._view = view; 
            _board = new Board(view.SimulationCanvas.ActualHeight, view.SimulationCanvas.ActualWidth);
            random = new Random();

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
        // Update any text, shapes, and movement on screen 
        //
        private void OnRendering(object sender, EventArgs e)
        {
            _view.UpdateParticles(_board.Particles);
            _view.UpdateDebugLabel(DateTime.Now.Ticks);
            _view.UpdateParticleLabel(_board.Particles.FirstOrDefault());

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


            _board.UpdateBoard(dt);
        
        }

        //
        // When left mouse button is clicked, add a particle to the board
        //
        private void Canvas_LeftMouseButtonDown(object? sender, MouseEventArgs e)
        {
            var pos = e.GetPosition(_view.SimulationCanvas); // Get position relative to the canvas
            SpawnParticle(pos.X, pos.Y);
        }

        //
        // When right mouse button is clicked, pull particles towards the cursor
        //
        private void Canvas_RightMouseButtonDown(object? sender, MouseEventArgs e)
        {
            var pos = e.GetPosition(_view.SimulationCanvas);
            _board.PullParticlesToCursor(pos);
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
        // Spawn a particle with randomized velocity, radius, and acceleration.
        //
        private void SpawnParticle(double x, double y)
        {
            float vx = (random.NextSingle() * 500 - 250) * 2;
            float vy = (random.NextSingle() * 500 - 250) * 2;
            float ax = (random.NextSingle() * 200 - 100) * 2;
            float ay = (random.NextSingle() * 200 - 100) * 2;
            float radius = random.NextSingle() * 15 + 5;
            Particle p = new Particle(_board.PhysicsWorld.World, x, y, radius);
            p.SetVelocity(new Vector2(vx, vy) * p.Body.Mass);

            _board.AddParticle(p);
            _view.SimulationCanvas.Children.Add(p.Shape);
        }

        //
        // Spawn a static barrier with fixed radius at the mouse position
        //
        private void SpawnBarrier(double x, double y)
        {
            Barrier b = new Barrier(_board.PhysicsWorld.World, x, y, 40f);
            _board.AddBarrier(b);
            b.UpdateShapePosition();
            _view.SimulationCanvas.Children.Add(b.Shape);
        }

        public void UpdateScreenSize(double newHeight, double newWidth)
        {
            _board.UpdateBoardSize(newHeight, newWidth);
            _view.UpdateScreenSize(newHeight, newWidth);
            foreach (Particle p in _board.Particles) {
                _view.SimulationCanvas.Children.Remove(p.Shape);
            }
        }
    }
}
