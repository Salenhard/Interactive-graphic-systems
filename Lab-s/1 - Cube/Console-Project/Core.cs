using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Console_Project
{
    public class Core : GameWindow
    {
        public readonly static int Vec3AttributeSize = 3;
        readonly OpenGLDrawFigure figure;
        readonly Shader shader;

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
            shader = new Shader("shader.vert", "shader.frag");
            figure = new OpenGLDrawFigure(Square.TestSquare, shader);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(.1f, .1f, .15f, 1f);
            figure.Init();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            figure.Draw();

            SwapBuffers();
        }

        protected override void OnUnload()
        {
            figure.Dispose();
            base.OnUnload();
        }
    }
}
