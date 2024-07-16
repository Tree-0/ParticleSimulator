using ParticleSimulator.Controller;
using ParticleSimulator.Model;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ParticleSimulator.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MouseButtonEventHandler? CanvasLeftMouseButtonDown;
        private SimulationController controller;

        public MainWindow()
        {
            RenderOptions.ProcessRenderMode = RenderMode.Default; //stuff
            InitializeComponent();
            controller = new SimulationController(this);
        }

        private void Canvas_LeftMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            CanvasLeftMouseButtonDown?.Invoke(sender, e);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            controller.UpdateScreenSize(e.NewSize.Height, e.NewSize.Width);
        }

        public void UpdateParticles(List<Particle> particles)
        {
            // render all particles
            foreach(var particle in particles)
            {
                Canvas.SetLeft(particle.Shape, particle.X - particle.Radius);
                Canvas.SetTop(particle.Shape, particle.Y - particle.Radius);
            }
        }

        public void UpdateDebugLabel(long ticks) { TimeDebugLabel.Content = $"Ticks: {ticks}"; }

        public void UpdateDebugLabel(int seconds) { TimeDebugLabel.Content = $"Seconds: {seconds.ToString()}"; }

        public void UpdateParticleLabel(Particle? p)
        {
            if (p == null) { ParticleDebugLabel.Content = "No particles"; return; }
            ParticleDebugLabel.Content = $"XY: {Math.Round(p.X,5)} {Math.Round(p.Y, 5)} " +
                $"\nVXY: {Math.Round(p.VX, 5)} {Math.Round(p.VY, 5)} " +
                $"\nAXY: {Math.Round(p.AX, 5)} {Math.Round(p.AY,5)}";
        }

        public void UpdateScreenSize(double height, double width)
        {
            ScreenSizeLabel.Content = $"Height: {height}, Width: {width}";
        }

    }
}
