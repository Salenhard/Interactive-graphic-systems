using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

namespace Console_Project
{
    public class GameCore : GameWindow
    {
        #region Constants
        public const int WIDTH = 800;
        public const int HEIGHT = 640;
        #endregion

        #region Variables
        Dictionary<string, GameObjectsController> gameObjectsControllers = new();
        readonly Dictionary<string, float> floatValues = new();
        readonly Dictionary<string, bool> boolValues = new();
        readonly Dictionary<string, int> intValues = new();
        readonly System.Diagnostics.Stopwatch timer = System.Diagnostics.Stopwatch.StartNew();
        ShaderProgram mainShader = null!;
        StateSystem stateSystem = null!;
        #endregion

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
            Init();

            CenterWindow();
        }

        #region Overrides functions
        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(.5f, .6f, .6f, 1f);

            floatValues.Add("RotationSlow", 0f);
            floatValues.Add("RotationMedium", 0f);
            floatValues.Add("Score", 1f);
            floatValues.Add("ScoreDecreasing", 0.01f);
            boolValues.Add("IsRageMode", false);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            OnUpdate();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            foreach (var item in gameObjectsControllers)
            {
                item.Value.Draw();
            }

            SwapBuffers();
        }

        protected override void OnUnload()
        {
            foreach (var item in gameObjectsControllers)
            {
                item.Value.Dispose();
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
        #endregion

        #region Facade
        void OnUpdate()
        {
            CheckInputs();
            UpdateMainShaderValues(boolValues["IsRageMode"]);

            foreach (var item in gameObjectsControllers)
            {
                item.Value.Update();
            }
        }

        void Init()
        {
            mainShader = ShaderProgram.PerspectiveUniform;

            (string UniformName,
                dynamic UniformValue, 
                ActiveUniformType UniformType)[] mainShaderUniformValues = {
                    ("iMVP",
                    Matrix4.CreateTranslation(0f, 0f, 0f)
                        * Matrix4.LookAt(new(0f, 0f, 5f), Vector3.Zero, Vector3.UnitY)
                        * Matrix4.CreatePerspectiveFieldOfView(
                            MathHelper.DegreesToRadians(45f),
                            1f * WIDTH / HEIGHT,
                            1f,
                            10f
                        ),
                    ActiveUniformType.FloatMat4),
                    ("iScale", Matrix4.Identity, ActiveUniformType.FloatMat4)
                };

            (string UniformName,
                dynamic UniformValue, 
                ActiveUniformType UniformType)[] hintLooseDisplayShaderUniformValues = {
                    ("iColor", new Vector4(0f, 1f, 0f, 0f), ActiveUniformType.FloatVec4)
                };

            (string UniformName,
                dynamic UniformValue, 
                ActiveUniformType UniformType)[] hintWinDisplayShaderUniformValues = {
                    ("iColor", new Vector4(1f, 1f, 1f, 0f), ActiveUniformType.FloatVec4)
                };

            InitShaderUniforms(mainShader, mainShaderUniformValues);

            gameObjectsControllers.Add("Main", new(mainShader, Figure.TestCube2.ToGameObject(BufferUsageHint.StreamDraw)));
            gameObjectsControllers.Add("HintLooseDisplay", new(ShaderProgram.Colorable, 
                Figure.CreateSquare(Vector3.Zero, 2f).ToGameObject()));
            // gameObjectsControllers.Add("HintWinDisplay", new(ShaderProgram.Colorable, 
            //     Figure.CreateSquare(Vector3.Zero, 2f).ToGameObject()));

            InitShaderUniforms(gameObjectsControllers["HintLooseDisplay"].ShaderProgram, 
                hintLooseDisplayShaderUniformValues);
            // InitShaderUniforms(gameObjectsControllers["HintWinDisplay"].ShaderProgram, 
            //     hintWinDisplayShaderUniformValues);
            
            stateSystem = new StateSystem(3);
        }
        #endregion

        #region Necessary logic function
        void InitShaderUniforms(ShaderProgram shaderProgram, (string UniformName, dynamic UniformValue, ActiveUniformType UniformType)[] uniformsInfo)
        {
            foreach (var uniformInfo in uniformsInfo)
            {
                shaderProgram.SetUniform(
                    uniformInfo.UniformName,
                    (
                        uniformInfo.UniformValue,
                        uniformInfo.UniformType
                    )
                );
            }
        }

        void CheckInputs()
        {
            if (MouseState.IsButtonPressed(MouseButton.Button1))
            {
                InputsController.InvokeLMBClick(this, EventArgs.Empty);
            }
            if (MouseState.IsButtonPressed(MouseButton.Button2))
            {
                boolValues["IsRageMode"] = true;
            }
            if (MouseState.IsButtonReleased(MouseButton.Button2))
            {
                boolValues["IsRageMode"] = false;
            }
        }

        void UpdateMainShaderValues(bool isRageMode = false)
        {
            UpdateRotationValues(isRageMode);
            UpdateColorValues(isRageMode);
            UpdateHintDisplayTransparancy();
        }

        void UpdateHintDisplayTransparancy()
        {
            
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
                floatValues["RotationSlow"] += MathExtension.BigStep;
                floatValues["RotationMedium"] += MathExtension.KiloStep;
            }
            else
            {
                floatValues["RotationSlow"] += MathExtension.SmallStep;
                floatValues["RotationMedium"] += MathExtension.MediumStep;
            }

            mainShader.SetUniform(
                "iRotationX",
                (Matrix4.CreateRotationX(floatValues["RotationMedium"]), ActiveUniformType.FloatMat4),
                true
            );
            mainShader.SetUniform(
                "iRotationY",
                (Matrix4.CreateRotationY(floatValues["RotationSlow"]), ActiveUniformType.FloatMat4),
                true
            );
            mainShader.SetUniform(
                "iRotationZ",
                (Matrix4.CreateRotationY(floatValues["RotationSlow"]), ActiveUniformType.FloatMat4),
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
        #endregion
    }
}
