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
        public event MouseButtonEventHandler? CanvasLeftMouseButtonDown;
        public event MouseButtonEventHandler? CanvasRightMouseButtonDown;
        public event KeyEventHandler? CanvasSpaceBarDown;

        private SimulationController _controller;

        public MainWindow()
        {
            RenderOptions.ProcessRenderMode = RenderMode.Default; //stuff
            InitializeComponent();
            _controller = new SimulationController(this);

            this.DataContext = _controller.Board.PhysicsWorld;


        }

        private void Canvas_LeftMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            CanvasLeftMouseButtonDown?.Invoke(sender, e);
        }

        private void Canvas_RightMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            CanvasRightMouseButtonDown?.Invoke(sender, e);
        }

        private void Canvas_SpaceBarDown(object sender, KeyEventArgs e)
        {
            CanvasSpaceBarDown?.Invoke(sender, e);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _controller.UpdateScreenSize(e.NewSize.Height, e.NewSize.Width);
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
                $"AX: AY: TODO";
        }

        public void UpdateImpulseLabel(Vector2 impulse)
        {
            ImpulseLabel.Content = $"Most Recent Impulse: X: {impulse.X}, Y: {impulse.Y}";
        }

        public void UpdateScreenSize(double height, double width)
        {
            ScreenSizeLabel.Content = $"Height: {height}, Width: {width}";
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            _controller.RemoveAllPhysicsObjects();
        }


        /// <summary>
        /// Because sliders didn't work, these buttons will set properties of the physics world instead...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        private void FrictionTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (float.TryParse(FrictionTextBox.Text, out float newFriction))
            {
                _controller.Board.PhysicsWorld.Friction = newFriction;
            }
        }

        private void RestitutionTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (float.TryParse(RestitutionTextBox.Text, out float newRestitution))
            {
                _controller.Board.PhysicsWorld.Restitution = newRestitution;
            }
        }

        private void AirResistanceTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (float.TryParse(AirResistanceTextBox.Text, out float newAirResistance))
            {
                _controller.Board.PhysicsWorld.AirResistance = newAirResistance;
            }
        }

        private void GravityTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (float.TryParse(GravityTextBox.Text, out float newGravity))
            {
                _controller.Board.PhysicsWorld.Gravity = newGravity;
            }
        }

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Kill keyboard focus
                Keyboard.ClearFocus();
                // Kill logical focus
                SimulationCanvas.Focus();

            }
        }



    }
}
