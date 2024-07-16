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

namespace ParticleSimulator.Model
{
    public class PhysicsWorld
    {
        public World World { get; private set; }
        public PhysicsWorld(Board board) 
        {
            World = new World(new Vector2(0,0));

            double width = board.Width;
            double height = board.Height;
            // Create boundaries
            CreateBoundary(new Vector2(0, 0), new Vector2((float)width, 0)); // Top
            CreateBoundary(new Vector2((float)width, 0), new Vector2((float)width, (float)height)); // Right
            CreateBoundary(new Vector2(0, (float)height), new Vector2((float)width, (float)height)); // Bottom
            CreateBoundary(new Vector2(0, 0), new Vector2(0, (float)height)); // Left
        }

        private void CreateBoundary(Vector2 start, Vector2 end)
        {
            var edge = BodyFactory.CreateEdge(World, start, end);
            edge.BodyType = BodyType.Static;
        }

        public void Update(float dt)
        {
            World.Step(dt);
        }

    }
}
