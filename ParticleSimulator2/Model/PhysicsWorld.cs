using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;
using Genbox.VelcroPhysics.Collision.Shapes;
using Microsoft.Xna.Framework; // Note: VelcroPhysics uses XNA types
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using ParticleSimulator.Model;
using System.ComponentModel;
using System.Diagnostics;
using Genbox.VelcroPhysics;
using Genbox.VelcroPhysics.Shared;

namespace ParticleSimulator.Model
{
    public class PhysicsWorld : INotifyPropertyChanged
    {
        public World World { get; private set; }
        private float _friction;
        public float Friction
        {
            get => _friction;
            set
            {
                if (_friction != value)
                {
                    _friction = value;
                    OnPropertyChanged(nameof(Friction));
                    PrintState();
                }
            }
        }

        private float _restitution;
        public float Restitution
        {
            get => _restitution;
            set
            {
                if (_restitution != value)
                {
                    _restitution = value;
                    OnPropertyChanged(nameof(Restitution));
                    PrintState();
                }
            }
        }

        private float _airResistance;
        public float AirResistance
        {
            get => _airResistance;
            set
            {
                if (_airResistance != value)
                {
                    _airResistance = value;
                    OnPropertyChanged(nameof(AirResistance));
                    PrintState();
                }
            }
        }

        private float _gravity;
        public float Gravity
        {
            get => _gravity;
            set
            {
                if (_gravity != value)
                {
                    _gravity = value;
                    World.Gravity = new Vector2(value,value);
                    OnPropertyChanged(nameof(Gravity));
                    PrintState();
                }
            }
        }

        // When sliders are updated, raise event and have particle properties updated, and gravity updated.
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
            

        public PhysicsWorld(Board board) 
        {
            World = new World(new Vector2(0,1) * Gravity);
            Settings.MaxTranslation = 500.0f; // Increase the max linear velocity


            double width = board.Width;
            double height = board.Height;
            // Create boundaries
            CreateBoundary(new Vector2(0, 0), new Vector2((float)width, 0)); // Top
            CreateBoundary(new Vector2((float)width, 0), new Vector2((float)width, (float)height)); // Right
            CreateBoundary(new Vector2(0, (float)height), new Vector2((float)width, (float)height)); // Bottom
            CreateBoundary(new Vector2(0, 0), new Vector2(0, (float)height)); // Left

            Friction = 0f;
            Restitution = 1f;
            AirResistance = 0f;
            Gravity = 0f;

            // Debug
            PrintState();
        }

        private void CreateBoundary(Vector2 start, Vector2 end)
        {
            var edge = BodyFactory.CreateEdge(this.World, start, end);
            edge.BodyType = BodyType.Static;
        }

        public void PrintState()
        {
            Debug.WriteLine($"Fr: {Friction} R: {Restitution} AirR: {AirResistance} Gravity: {Gravity}");
        }

        public void Update(float dt)
        {
            World.Step(dt);
        }


    }
}
