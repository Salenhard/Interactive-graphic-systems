using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

namespace Console_Project
{
    public class GameCore : GameWindow
    {
        List<GameObjectsController> gameObjectsControllers = new();

        public const int WIDTH = 800;
        public const int HEIGHT = 640;

        readonly Dictionary<string, float> floatValues = new();
        readonly Dictionary<string, bool> boolValues = new();
        readonly Dictionary<string, int> intValues = new();
        readonly System.Diagnostics.Stopwatch timer = System.Diagnostics.Stopwatch.StartNew();
        readonly ShaderProgram mainShader;

        public GameCore(
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
            mainShader = ShaderProgram.PerspectiveUniform;
            InitShaderUniforms(mainShader);

            gameObjectsControllers.Add(
                new(mainShader, Figure.TestCube2.ToGameObject(BufferUsageHint.StreamDraw))
            );

            CenterWindow();
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(.1f, .1f, .15f, 1f);

            floatValues.Add("rotSmall", 0f);
            floatValues.Add("rotMedium", 0f);
            boolValues.Add("RageMode", false);
            intValues.Add("Score", 0);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            CheckInputs();
            UpdateMainShaderValues(boolValues["RageMode"]);

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

            var (w, h) = ((float)WIDTH / e.Width, (float)HEIGHT / e.Height);

            mainShader.SetUniform(
                "iScale",
                (
                    // Matrix4.CreateScale(w, h, 1),
                    Matrix4.CreateScale(w, h, 1),
                    ActiveUniformType.FloatMat4
                ),
                true
            );
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        void CheckInputs()
        {
            if (MouseState.IsAnyButtonDown)
            {
                InputsController.InvokeLMBClick(this, EventArgs.Empty);
            }
            if (MouseState.IsButtonPressed(MouseButton.Button2))
            {
                boolValues["RageMode"] = true;
            }
            if (MouseState.IsButtonReleased(MouseButton.Button2))
            {
                boolValues["RageMode"] = false;
            }
        }

        void InitShaderUniforms(ShaderProgram shaderProgram)
        {
            shaderProgram.SetUniform(
                "iMVP",
                (
                    Matrix4.CreateTranslation(0f, 0f, 0f)
                        * Matrix4.LookAt(new(0f, 0f, 5f), Vector3.Zero, Vector3.UnitY)
                        * Matrix4.CreatePerspectiveFieldOfView(
                            MathHelper.DegreesToRadians(45f),
                            1f * WIDTH / HEIGHT,
                            1f,
                            10f
                        ),
                    ActiveUniformType.FloatMat4
                )
            );
            shaderProgram.SetUniform("iScale", (Matrix4.Identity, ActiveUniformType.FloatMat4));
        }

        void UpdateMainShaderValues(bool isRageMode = false)
        {
            UpdateRotationValues(isRageMode);
            UpdateColorValues(isRageMode);
        }

        void UpdateRotationValues(bool isRageMode = false)
        {
            foreach (var f in floatValues)
            {
                if (f.Value > 360)
                {
                    floatValues[f.Key] = 0f;
                }
            }

            if (isRageMode)
            {
                floatValues["rotSmall"] += MathExtension.BigStep;
                floatValues["rotMedium"] += MathExtension.KiloStep;
            }
            else
            {
                floatValues["rotSmall"] += MathExtension.SmallStep;
                floatValues["rotMedium"] += MathExtension.MediumStep;
            }

            mainShader.SetUniform(
                "iRotationX",
                (Matrix4.CreateRotationX(floatValues["rotMedium"]), ActiveUniformType.FloatMat4),
                true
            );
            mainShader.SetUniform(
                "iRotationY",
                (Matrix4.CreateRotationY(floatValues["rotSmall"]), ActiveUniformType.FloatMat4),
                true
            );
            mainShader.SetUniform(
                "iRotationZ",
                (Matrix4.CreateRotationY(floatValues["rotSmall"]), ActiveUniformType.FloatMat4),
                true
            );
        }

        void UpdateColorValues(bool isRageMode = false)
        {
            var seconds = (float)timer.Elapsed.TotalSeconds * (isRageMode ? 5 : 1);

            mainShader.SetUniform(
                "iResolution",
                (new Vector2(Size.X, Size.Y), ActiveUniformType.FloatVec2),
                true
            );

            mainShader.SetUniform("iTime", (seconds, ActiveUniformType.Float), true);

            if (seconds > 1024_000f)
            {
                timer.Restart();
            }
        }
    }
}
