using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Console_Project
{
    public class Core : GameWindow
    {
        public readonly static int Vec3AttributeSize = 3;
        OpenGLDrawFigure figure;
        Shader shader;
        Matrix4 rotationMatrixByAxisZ = Matrix4.CreateFromAxisAngle(Vector3.UnitZ, 0.1f);

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
            shader = Shader.GetSimpleOneColorShader(0.5f, 0.8f, 0.6f, 1.0f);
            // TODO: Check OpenTK lessons solution in another folder about uniform and projection
            ShaderController shaderController = new(shader);
            figure = new OpenGLDrawFigure(Figure.TestSquare2, shaderController);
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
            // figure.Figure.Transform(rotationMatrixByAxisZ);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

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
