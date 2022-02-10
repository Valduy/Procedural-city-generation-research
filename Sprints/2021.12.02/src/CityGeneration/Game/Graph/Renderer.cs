using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Engine.Graph
{
    public class Renderer
    {
        public readonly Camera Camera;
        public readonly Shader Shader;
        public readonly Light Light;

        public Renderer(Camera camera, Shader shader, Light light)
        {
            Camera = camera;
            Shader = shader;
            Light = light;
        }

        public void Init()
        {
            Shader.Use();
            Shader.SetVector3("light.position", Light.Position);
            Shader.SetVector3("light.ambient", Light.Ambient);
            Shader.SetVector3("light.diffuse", Light.Diffuse);
            Shader.SetVector3("light.specular", Light.Specular);
        }

        public RenderContext Setup(float[] vertices)
        {
            var vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            var vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            var positionLocation = Shader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

            var normalLocation = Shader.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation);
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));

            GL.EnableVertexAttribArray(0);
            return new RenderContext(vertexArrayObject, vertexBufferObject, vertices.Length / 6);
        }

        public void Render(RenderContext context, Material material, Vector3 scale, Vector3 rotation, Vector3 position)
        {
            GL.BindVertexArray(context.VertexArrayObject);

            SetupShader(material, scale, rotation, position);
            GL.DrawArrays(PrimitiveType.Triangles, 0, context.Count);

            GL.BindVertexArray(0);
        }

        public void Delete(RenderContext context)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            GL.DeleteBuffer(context.VertexBufferObject);
            GL.DeleteVertexArray(context.VertexArrayObject);
        }

        private void SetupShader(Material material, Vector3 scale, Vector3 rotation, Vector3 position)
        {
            Shader.SetMatrix4("model", GetModelMatrix(scale, rotation, position));
            Shader.SetMatrix4("view", Camera.GetViewMatrix());
            Shader.SetMatrix4("projection", Camera.GetProjectionMatrix());
            Shader.SetVector3("viewPos", Camera.Position);
            Shader.SetVector3("material.ambient", material.Ambient);
            Shader.SetVector3("material.diffuse", material.Diffuse);
            Shader.SetVector3("material.specular", material.Specular);
            Shader.SetFloat("material.shininess", material.Shininess);
        }

        private Matrix4 GetModelMatrix(Vector3 scale, Vector3 rotation, Vector3 position)
        {
            var model = Matrix4.Identity;
            model *= Matrix4.CreateScale(scale);
            model *= Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(rotation));
            model *= Matrix4.CreateTranslation(position);
            return model;
        }
    }
}
