using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

namespace Console_Project
{
    public class Core : GameWindow
    {
        List<GameObjectsController> gameObjectsControllers = new();

        public const int WIDTH = 800;
        public const int HEIGHT = 640;

        public Core(
            string title,
            int width = WIDTH,
            int height = HEIGHT,
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
            gameObjectsControllers.Add(
                new(
                    ShaderProgram.PerspectiveSimple.ShaderProgramHandler,
                    Figure.TestCube2.ToGameObject()
                )
            );

            gameObjectsControllers.Add(
                new(
                    ShaderProgram.Default.ShaderProgramHandler,
                    Figure.CreateSquare(new(-.4f, -.4f, 0.0f), .3f).ToGameObject(),
                    Figure.CreateSquare(new(-.4f, +.4f, 0.0f), .3f).ToGameObject(),
                    Figure.CreateSquare(new(+.4f, -.4f, 0.0f), .3f).ToGameObject(),
                    Figure.CreateSquare(new(+.4f, +.4f, 0.0f), .3f).ToGameObject()
                )
            );

            CenterWindow();
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(.1f, .1f, .15f, 1f);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            foreach (var item in gameObjectsControllers)
            {
                item.Draw();
            }

            SwapBuffers();
        }

        protected override void OnUnload()
        {
            foreach (var item in gameObjectsControllers)
            {
                item.Dispose();
            }
            base.OnUnload();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }
    }
}
