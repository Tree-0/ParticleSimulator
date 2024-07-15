using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using ParticleSimulator.Model;
using ParticleSimulator.View;

namespace ParticleSimulator.Controller
{
    public class SimulationController
    {
        private Board board;
        private MainWindow view;
        private DispatcherTimer timer;
        private Random random;
        private long previousTick;

        public SimulationController(MainWindow view)
        {

            this.view = view; 
            board = new Board(view.SimulationCanvas.ActualHeight, view.SimulationCanvas.ActualWidth);
            random = new Random();

            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromTicks(1);
            timer.Start();

            view.CanvasLeftMouseButtonDown += Canvas_LeftMouseButtonDown;
            previousTick = DateTime.Now.Ticks;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            long currentTick = DateTime.Now.Ticks;
            double dt = (currentTick - previousTick) / (double)TimeSpan.TicksPerSecond;
            previousTick = currentTick;

            board.UpdateBoard(dt);
            view.UpdateParticles(board.Particles);
            view.UpdateDebugLabel(DateTime.Now.Ticks);
            view.UpdateParticleLabel(board.Particles.FirstOrDefault());
        }

        private void Canvas_LeftMouseButtonDown(object? sender, MouseEventArgs e)
        {
            var pos = e.GetPosition(view.SimulationCanvas); // Get position relative to the canvas
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
            board.AddParticle(p);
            view.SimulationCanvas.Children.Add(p.Shape);
        }

        public void UpdateScreenSize(double newHeight, double newWidth)
        {
            board.UpdateBoardSize(newHeight, newWidth);
            view.UpdateScreenSize(newHeight, newWidth);
        }
    }
}
