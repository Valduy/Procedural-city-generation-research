using Engine.Game;
using Engine.Graph;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace Engine.Components
{
    public class RenderComponent : GameComponent
    {
        private float[] _vertices;
        private RenderContext _context;

        public float[] Vertices
        {
            get => _vertices;
            set
            {
                Game.Renderer.Delete(_context);
                _vertices = value;
                _context = Game.Renderer.Setup(_vertices);
            }
        } 

        public readonly Material Material = new()
        {
            Ambient = new Vector3(0.25f, 0.25f, 0.25f),
            Diffuse = new Vector3(0.4f, 0.4f, 0.4f),
            Specular = new Vector3(0.774597f, 0.774597f, 0.774597f),
            Shininess = 0.6f
        };

        public RenderComponent(Game.Game game)
            : base(game)
        {
            _vertices = CreatePlane();
            _context = Game.Renderer.Setup(_vertices);
        }

        public override void RenderUpdate(FrameEventArgs args)
        {
            Game.Renderer.Render(_context, Material, GameObject.Scale, GameObject.Rotation, GameObject.Position);
        }

        public override void Stop()
        {
            Game.Renderer.Delete(_context);
        }

        private static float[] CreatePlane() => new[]
        {
            -0.5f, 0.0f, -0.5f, 0.0f, 0.1f, 0.0f,
            0.5f, 0.0f, -0.5f, 0.0f, 0.1f, 0.0f,
            -0.5f, 0.0f, 0.5f, 0.0f, 0.1f, 0.0f,
            -0.5f, 0.0f, 0.5f, 0.0f, 0.1f, 0.0f,
            0.5f, 0.0f, -0.5f, 0.0f, 0.1f, 0.0f,
            0.5f, 0.0f, 0.5f, 0.0f, 0.1f, 0.0f,
        };
    }
}
