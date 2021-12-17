using Engine.Graph;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace Engine.Game
{
    public class Game
    {
        public const string Title = "City generator";
        public const int WindowWidth = 800;
        public const int WindowHeight = 600;

        public readonly Infrastructure.Engine Engine = new();
        public readonly Window Window = new(WindowWidth, WindowHeight, Title);
        public readonly Camera Camera;

        public readonly Light Light = new()
        {
            Ambient = new Vector3(0.2f, 0.2f, 0.2f),
            Diffuse = new Vector3(0.5f, 0.5f, 0.5f),
            Specular = new Vector3(1.0f, 1.0f, 1.0f),
        };

        public readonly Renderer Renderer;
        
        private readonly Shader Shader = new(
            "../../../../Game/Shaders/shader.vert",
            "../../../../Game/Shaders/shader.frag");

        public Game()
        {
            Camera = new Camera(Vector3.Zero, Window.Size.X / (float)Window.Size.Y);
            Renderer = new Renderer(Camera, Shader, Light);

            Window.Load += OnWindowLoaded;
            Window.UpdateFrame += OnWindowUpdateFrame;
            Window.RenderFrame += OnWindowRenderFrame;
            Window.Unload += OnWindowUnloaded;
            Window.Resize += OnWindowResized;
        }

        public void Run()
        {
            GL.ClearColor(0, 0, 0, 1);
            Renderer.Init();
            Window.Run();
        }

        private void OnWindowLoaded()
        {
            GL.Enable(EnableCap.DepthTest);
            Window.CursorGrabbed = true;
            Engine.Start();
        }

        private void OnWindowUpdateFrame(FrameEventArgs args)
        {
            Engine.GameUpdate(args);
        }

        private void OnWindowRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Engine.RenderUpdate(args);
            Window.SwapBuffers();
        }

        private void OnWindowUnloaded()
        {
            Engine.Stop();
            GL.UseProgram(0);
            GL.DeleteProgram(Shader.Handle);
        }

        private void OnWindowResized(ResizeEventArgs args)
        {
            GL.Viewport(0, 0, Window.Size.X, Window.Size.Y);
            Camera.AspectRatio = Window.Size.X / (float) Window.Size.Y;
        }
    }
}
