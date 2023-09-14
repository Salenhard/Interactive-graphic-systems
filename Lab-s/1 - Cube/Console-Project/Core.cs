using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Console_Project
{

    public class Core : GameWindow
    {
        Shader shader;
        Polygon polygon;
        int vertexBufferObject, vertexArrayObject;

        public Core(int width = 800, int height = 640, string title = "Cube")
            : base(
                GameWindowSettings.Default,
                new NativeWindowSettings() { Size = (width, height), Title = title }
            )
        {
            polygon = new Polygon(new Vector3[] {
                new Vector3( -.5f, .5f, 0f ),
                // new Vector3( .5f, .5f, 0f ),
                new Vector3( .5f, -.5f, 0f ),
                new Vector3( -.5f, -.5f, 0f )
            }
            );
        }


        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(.18f, .18f, .21f, 1f);

            vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            var vertices = polygon.Vertices;
            var count = polygon.VerticesCount;

            GL.BufferData(BufferTarget.ArrayBuffer,
            count * sizeof(float),
            vertices, BufferUsageHint.StaticDraw);

            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);
            var index = 0;
            GL.VertexAttribPointer(index, count, VertexAttribPointerType.Float, false, count * sizeof(float), 0); 
            GL.EnableVertexAttribArray(index); 


            shader = new Shader("shader.vert", "shader.frag");
            shader.Use();
        }

        void Draw()
        {
            shader.Use();
            GL.BindVertexArray(vertexArrayObject);
            // TODO: display square
            GL.DrawArrays(PrimitiveType.Triangles, 0, polygon.VerticesCount);
        }

        public override void Run()
        {
            base.Run();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }

        protected override void OnUnload()
        {
            shader.Dispose();
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            Draw();
            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }
    }

}