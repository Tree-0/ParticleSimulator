using ParticleSimulator.Controller;
using ParticleSimulator.Model;
using System.Numerics;
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
        public MouseButtonEventHandler? CanvasRightMouseButtonDown;
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

        private void Canvas_RightMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            CanvasRightMouseButtonDown?.Invoke(sender, e);
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
                particle.UpdateShapePosition();
            }
        }

        public void UpdateDebugLabel(long ticks) { TimeDebugLabel.Content = $"Ticks: {ticks}"; }

        public void UpdateDebugLabel(int seconds) { TimeDebugLabel.Content = $"Seconds: {seconds.ToString()}"; }

        public void UpdateParticleLabel(Particle? p)
        {
            if (p == null) { ParticleDebugLabel.Content = "no particles"; return; }
            ParticleDebugLabel.Content = $"X: {p.Body.Position.X}, Y: {p.Body.Position.Y} \n" +
                $"VX: {p.Body.GetLinearVelocityFromWorldPoint(p.Body.Position).X}, VY: {p.Body.GetLinearVelocityFromWorldPoint(p.Body.Position).Y} \n" +
                $"AX AY TODO";
        }

        public void UpdateImpulseLabel(Vector2 impulse)
        {
            ImpulseLabel.Content = $"Most Recent Impulse: X: {impulse.X}, Y: {impulse.Y}";
        }

        public void UpdateScreenSize(double height, double width)
        {
            ScreenSizeLabel.Content = $"Height: {height}, Width: {width}";
        }

    }
}
