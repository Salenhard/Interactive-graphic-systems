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

        float test = 0f;

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
            ShaderProgram temp = ShaderProgram.PerspectiveUniform;

            Test(temp);

            gameObjectsControllers.Add(new(temp, Figure.TestCube2.ToGameObject()));

            CenterWindow();
        }

        void Test(ShaderProgram shaderProgram)
        {
            shaderProgram.SetUniform("iColor", new Vector3(.5f, .5f, .5f));
            shaderProgram.SetUniform(
                "iModel",
                Matrix4.CreateTranslation(0f, 0f, -5f) * Matrix4.CreateRotationZ(test)
            );

            test += .01f;

            shaderProgram.SetUniform("iView", Matrix4.CreateTranslation(0f, 0f, 0f));
            shaderProgram.SetUniform(
                "iProjection",
                Matrix4.CreatePerspectiveFieldOfView(
                    MathHelper.DegreesToRadians(45f),
                    1f * WIDTH / HEIGHT,
                    1f,
                    10f
                )
            );
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(.1f, .1f, .15f, 1f);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            CheckInputs();

            Test(gameObjectsControllers[0].ShaderProgram);
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

        async void CheckInputs()
        {
            if (MouseState.IsAnyButtonDown)
            {
                InputsController.InvokeLMBClick(this, EventArgs.Empty);
            }
        }
    }
}
