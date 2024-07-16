using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using ParticleSimulator.Model;
using ParticleSimulator.View;

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
            previousTick = DateTime.Now.Ticks;
        }

        private void OnRendering(object sender, EventArgs e)
        {
            _view.UpdateParticles(_board.Particles);
            _view.UpdateDebugLabel(DateTime.Now.Ticks);
            _view.UpdateParticleLabel(_board.Particles.FirstOrDefault());

            //foreach (var ball in balls)
            //{
            //    ball.UpdatePosition();
            //    Canvas.SetLeft(ball.Ellipse, ball.Position.X);
            //    Canvas.SetTop(ball.Ellipse, ball.Position.Y);
            //}
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

        private void SpawnParticle(double x, double y)
        {
            double vx = random.NextDouble() * 200 - 100;
            double vy = random.NextDouble() * 200 - 100;
            double ax = random.NextDouble() * 20 - 10;
            double ay = random.NextDouble() * 20 - 10;
            double m = 3;
            Particle p = new Particle(x,y,vx * m,vy * m,ax * m * 20,ay * m * 20);
            _board.AddParticle(p);
            _view.SimulationCanvas.Children.Add(p.Shape);
        }

        public void UpdateScreenSize(double newHeight, double newWidth)
        {
            _board.UpdateBoardSize(newHeight, newWidth);
            _view.UpdateScreenSize(newHeight, newWidth);
        }
    }
}
