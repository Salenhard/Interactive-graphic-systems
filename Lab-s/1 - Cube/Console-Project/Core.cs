using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Console_Project
{
    public class Core : GameWindow
    {
        public Core(int width = 800, int height = 640, string title = "Cube")
            : base(
                GameWindowSettings.Default,
                new NativeWindowSettings() { Size = (width, height), Title = title }
            ) { }

        public override void Run()
        {
            base.Run();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(.18f, .18f, .21f, 1f);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            // there are two areas that OpenGL draws to. In essence: One area is displayed, while the other is being rendered to. Then, when you call SwapBuffers, the two are reversed
            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }
    }
}
