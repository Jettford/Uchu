using System.Numerics;
using BepuPhysics;
using BepuPhysics.Collidables;

namespace Uchu.Physics
{
    public class CapsuleBody : PhysicsBody
    {
        public CapsuleBody(PhysicsSimulation simulation, BodyHandle handle) : base(simulation, handle)
        {
        }

        public static CapsuleBody Create(PhysicsSimulation simulation, Vector3 position, Quaternion rotation, Vector2 size)
        {
            var shape = new Capsule(size.X, size.Y);

            shape.ComputeInertia(1, out var inertia);

            var index = simulation.Simulation.Shapes.Add(shape);

            var collidable = new CollidableDescription(index, 0.1f);
            
            var activity = new BodyActivityDescription(0.01f);

            var pose = new RigidPose(position, rotation);
            
            var descriptor = BodyDescription.CreateDynamic(pose, inertia, collidable, activity);

            var handle = simulation.Simulation.Bodies.Add(descriptor);

            var obj = new CapsuleBody(simulation, handle);

            simulation.Register(obj);

            return obj;
        }
    }
}