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
            timer.Interval = TimeSpan.FromTicks(1);
            timer.Start();

            CompositionTarget.Rendering += OnRendering;

            view.CanvasLeftMouseButtonDown += Canvas_LeftMouseButtonDown;
            view.CanvasRightMouseButtonDown += Canvas_RightMouseButtonDown;
            previousTick = DateTime.Now.Ticks;
        }

        private void OnRendering(object sender, EventArgs e)
        {
            _view.UpdateParticles(_board.Particles);
            _view.UpdateDebugLabel(DateTime.Now.Ticks);
            _view.UpdateParticleLabel(_board.Particles.FirstOrDefault());

        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            long currentTick = DateTime.Now.Ticks;
            double dt = (currentTick - previousTick) / (double)TimeSpan.TicksPerSecond;
            previousTick = currentTick;

            _board.UpdateBoard(dt);
        
        }

        private void Canvas_LeftMouseButtonDown(object? sender, MouseEventArgs e)
        {
            var pos = e.GetPosition(_view.SimulationCanvas); // Get position relative to the canvas
            double x = pos.X;
            double y = pos.Y;
            SpawnParticle(x, y);
        }

        private void Canvas_RightMouseButtonDown(object? sender, MouseEventArgs e)
        {
            var pos = e.GetPosition(_view.SimulationCanvas);
            _board.PullParticlesToCursor(pos);
        }

        private void SpawnParticle(double x, double y)
        {
            float vx = random.NextSingle() * 500 - 250;
            float vy = random.NextSingle() * 500 - 250;
            float ax = random.NextSingle() * 200 - 100;
            float ay = random.NextSingle() * 200 - 100;
            float radius = random.NextSingle() * 15 + 5;
            Particle p = new Particle(_board.PhysicsWorld.World, x, y, radius);
            p.ApplyVelocity(new Vector2(vx, vy) * p.Body.Mass);

            _board.AddParticle(p);
            _view.SimulationCanvas.Children.Add(p.Shape);
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
