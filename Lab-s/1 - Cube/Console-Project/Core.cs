using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Console_Project
{
    public class Core : GameWindow
    {
        GameObject gameObject;
        Shader shader;

        public Core(
            string title,
            int width = 800,
            int height = 640,
            VSyncMode vSyncMode = VSyncMode.On
        )
            : base(
                GameWindowSettings.Default,
                new NativeWindowSettings()
                {
                    Size = (width, height),
                    Title = title,
                    Vsync = vSyncMode,
                    Flags = ContextFlags.ForwardCompatible
                }
            )
        {
            shader = new(
                Path.Combine(Shader.ShaderSourcesPath, "shader.vert"),
                Path.Combine(Shader.ShaderSourcesPath, "shader.frag")
            );
            // TODO: Check OpenTK lessons solution in another folder about uniform and projection
            gameObject = new GameObject(Figure.TestSquare2, shader.ShaderProgramHandler);
            CenterWindow();
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(.1f, .1f, .15f, 1f);
            gameObject.Init();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            // figure.Figure.Transform(rotationMatrixByAxisZ);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            gameObject.Draw();

            SwapBuffers();
        }

        protected override void OnUnload()
        {
            gameObject.Dispose();
            base.OnUnload();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }
    }
}
