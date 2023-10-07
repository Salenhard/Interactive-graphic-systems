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
            ShaderProgram temp = ShaderProgram.PerspectiveUniform;

            InitShaderUniforms(temp);

            gameObjectsControllers.Add(new(temp, Figure.TestCube2.ToGameObject()));

            CenterWindow();
        }

        void InitShaderUniforms(ShaderProgram shaderProgram)
        {
            shaderProgram.SetUniform(
                "iColor",
                (new Vector3(.5f, .5f, .5f), ActiveUniformType.FloatVec3)
            );
            shaderProgram.SetUniform(
                "iModel",
                (Matrix4.CreateTranslation(0f, 0f, -5f), ActiveUniformType.FloatMat4)
            );
            shaderProgram.SetUniform(
                "iView",
                (Matrix4.CreateTranslation(0f, 0f, 0f), ActiveUniformType.FloatMat4)
            );
            shaderProgram.SetUniform(
                "iProjection",
                (
                    Matrix4.CreatePerspectiveFieldOfView(
                        MathHelper.DegreesToRadians(45f),
                        1f * WIDTH / HEIGHT,
                        1f,
                        10f
                    ),
                    ActiveUniformType.FloatMat4
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

            foreach (var item in gameObjectsControllers)
            {
                item.Update();
            }
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

        void CheckInputs()
        {
            if (MouseState.IsAnyButtonDown)
            {
                InputsController.InvokeLMBClick(this, EventArgs.Empty);
            }
        }
    }
}
