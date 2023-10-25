using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using StbImageSharp;

namespace Console_Project
{
    public class GameCore : GameWindow
    {
        #region Constants
        public const int WIDTH = 800;
        public const int HEIGHT = 640;
        public const string GameTitle = "Fast clicky!";

        private const string DataPath = "../../../Data/Textures/";
        #endregion

        #region Variables
        GameObjectsController gameObjectsController = null!;
        readonly Dictionary<string, float> floatValues = new();
        readonly Dictionary<string, bool> boolValues = new();
        readonly Dictionary<string, Vector3> vector3Values = new();
        readonly System.Diagnostics.Stopwatch timer = System.Diagnostics.Stopwatch.StartNew();
        #endregion

        public GameCore(int width = WIDTH, int height = HEIGHT, VSyncMode vSyncMode = VSyncMode.On)
            : base(
                GameWindowSettings.Default,
                new NativeWindowSettings()
                {
                    Size = (width, height),
                    Title = GameTitle,
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
            GL.ClearColor(.5f, .6f, .6f, 1f);
            GL.Enable(EnableCap.DepthTest);
            // StbImage.stbi_set_flip_vertically_on_load(1);
            // GL.TexImage2D(
            //     TextureTarget.Texture2D,
            //     0,
            //     PixelInternalFormat.Rgba,
            //     image.Width,
            //     image.Height,
            //     0,
            //     PixelFormat.Rgba,
            //     PixelType.UnsignedByte,
            //     image.Data
            // );
            base.OnLoad();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            Update();
            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            if (!boolValues["IsPause"])
            {
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                gameObjectsController.Draw();

                SwapBuffers();
                base.OnRenderFrame(args);
            }
        }

        protected override void OnUnload()
        {
            gameObjectsController.Dispose();
            base.OnUnload();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            var (w, h) = ((float)WIDTH / e.Width, (float)HEIGHT / e.Height);

            foreach (var cube in gameObjectsController.GameObjectsAndTheirShaderUniformsValues)
            {
                cube.ShaderUniformValuesInfos["iResolution"] = (
                    new Vector2(Size.X, Size.Y),
                    ActiveUniformType.FloatVec2
                );

                cube.ShaderUniformValuesInfos["iScale"] = (
                    Matrix4.CreateScale(w, h, 1),
                    ActiveUniformType.FloatMat4
                );
            }

            GL.Viewport(0, 0, e.Width, e.Height);
        }
        #endregion

        #region Facade
        void Init()
        {
            floatValues.Add("RotationX", 0f);
            floatValues.Add("RotationYandZ", 0f);
            floatValues.Add("RotationSlowStep", MathExtension.SmallStep);
            floatValues.Add("RotationMediumStep", MathExtension.BigStep);
            floatValues.Add("Score", -1f);
            floatValues.Add("ScoreDecreasing", MathExtension.SmallStep);
            floatValues.Add("ScoreEncreasing", MathExtension.BigStep * 1.5f);
            boolValues.Add("IsRageMode", false);
            boolValues.Add("IsPause", false);
            vector3Values.Add("ScoreBarNormalColor", new Vector3(0f, 0.28f, 0.67f));
            vector3Values.Add("ScoreBarRageColor", new Vector3(0.82f, 0.1f, 0.1f));
            vector3Values.Add("ScoreBarColor", vector3Values["ScoreBarNormalColor"]);

            // TODO: Add Loose/Win "screen" display
            // var texture = StbImageSharp

            Restart();

            var mat4Type = ActiveUniformType.FloatMat4;
            var identity = (Matrix4.Identity, mat4Type);
            //ImageResult pauseScreen = ImageResult.FromStream(File.OpenRead(Path.Combine(DataPath, "pauseScreen.png")), ColorComponents.RedGreenBlueAlpha);
            //ImageResult loseScreen = ImageResult.FromStream(File.OpenRead(Path.Combine(DataPath, "loseScreen.png")), ColorComponents.RedGreenBlueAlpha);
            //ImageResult winScreen = ImageResult.FromStream(File.OpenRead(Path.Combine(DataPath, "winScreen.png")), ColorComponents.RedGreenBlueAlpha);

            Dictionary<
                string,
                (dynamic UniformValue, ActiveUniformType UniformType)
            > mainObjectUniformValues =
                    new()
                    {
                        {
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
                                mat4Type
                            )
                        },
                        { "iScale", identity },
                        { "iIsUsingInputColor", (0, ActiveUniformType.Int) },
                        { "iTranslation", identity }
                    },
                    scoreBarUniformValues =
                    new()
                    {
                        {
                            "iTranslation",
                            (Matrix4.CreateTranslation(floatValues["Score"], -1.75f, 0f), mat4Type)
                        },
                        { "iMVP", identity },
                        { "iRotationX", identity },
                        { "iRotationY", identity },
                        { "iRotationZ", identity },
                        { "iIsUsingInputColor", (1, ActiveUniformType.Int) },
                        { "iColor", (vector3Values["ScoreBarColor"], ActiveUniformType.FloatVec3) }
                    };
            gameObjectsController = new(
                ShaderProgram.OurGameUniformShader,
                (
                    OpenGLFigure.MainCube
                        .ToGameObject(BufferUsageHint.StreamDraw,
                            PrimitiveType.Lines),
                    mainObjectUniformValues
                ),
                (
                    OpenGLFigure
                        .MainCube
                        .ToGameObject(BufferUsageHint.StreamDraw),
                    mainObjectUniformValues
                ),
                (
                   OpenGLFigure
                        .CreateSquare(Vector3.Zero, 2f)
                        .ToGameObject(BufferUsageHint.StreamDraw),
                    scoreBarUniformValues
                )
                );
        }
        #endregion

        #region Necessary logic function
        void CheckInputs()
        {
            if (MouseState.IsButtonPressed(MouseButton.Button1))
            {
                floatValues["Score"] += floatValues["ScoreEncreasing"];
            }
            if (MouseState.IsButtonPressed(MouseButton.Button2))
            {
                Rage_UnRage();
            }
            if (KeyboardState.IsKeyPressed(Keys.R))
            {
                Restart();
            }
            if (KeyboardState.IsKeyPressed(Keys.P | Keys.Space))
            {
                Pause_Unpause();
            }
        }

        void Update()
        {
            CheckInputs();

            if (!boolValues["IsPause"])
            {
                UpdateScore();
                UpdateMainShaderValues();

                gameObjectsController.Update();
            }
        }

        void UpdateMainShaderValues()
        {
            UpdateRotationValues();
            UpdateColorValues();
        }

        void UpdateRotationValues()
        {
            foreach (var f in floatValues)
            {
                if (f.Value > 360)
                {
                    floatValues[f.Key] = 0f;
                }
            }

            var cube1 = gameObjectsController.GameObjectsAndTheirShaderUniformsValues[1];

            floatValues["RotationX"] += floatValues["RotationMediumStep"];
            floatValues["RotationYandZ"] += floatValues["RotationSlowStep"];

            var slowRotationMatrix = Matrix4.CreateRotationY(floatValues["RotationYandZ"]);
            var mat4Type = ActiveUniformType.FloatMat4;

            cube1.ShaderUniformValuesInfos["iRotationX"] = (
                    Matrix4.CreateRotationX(floatValues["RotationX"]),
                    mat4Type
                );
            cube1.ShaderUniformValuesInfos["iRotationY"] = (slowRotationMatrix, mat4Type);
            cube1.ShaderUniformValuesInfos["iRotationZ"] = (slowRotationMatrix, mat4Type);
        }

        void UpdateColorValues()
        {
            var seconds = (float)timer.Elapsed.TotalSeconds * (boolValues["IsRageMode"] ? 5 : 1);

            foreach (var item in gameObjectsController.GameObjectsAndTheirShaderUniformsValues)
            {
                item.ShaderUniformValuesInfos["iTime"] = (seconds, ActiveUniformType.Float);
                item.ShaderUniformValuesInfos["iColor"] = (
                    vector3Values["ScoreBarColor"],
                    ActiveUniformType.FloatVec3
                );
            }

            if (seconds > 1024_000f)
            {
                timer.Restart();
            }
        }

        void UpdateScore()
        {
            var cubeInside = gameObjectsController.GameObjectsAndTheirShaderUniformsValues[1];
            var scoreBar = gameObjectsController.GameObjectsAndTheirShaderUniformsValues[2];

            floatValues["Score"] -= floatValues["ScoreDecreasing"];

            cubeInside.ShaderUniformValuesInfos["iScale"] = (
                Matrix4.CreateScale(floatValues["Score"]),
                ActiveUniformType.FloatMat4
            );

            scoreBar.ShaderUniformValuesInfos["iTranslation"] = (
                Matrix4.CreateTranslation(floatValues["Score"], -1.75f, 0f),
                ActiveUniformType.FloatMat4
            );

            CheckGameState();
        }

        void CheckGameState()
        {
            if (floatValues["Score"] >= 0f)
            {
                System.Console.WriteLine("You win");
                boolValues["IsPause"] = true;
            }
            else if (floatValues["Score"] <= -2f)
            {
                System.Console.WriteLine("You Loose");
                boolValues["IsPause"] = true;
            }
        }

        void Restart()
        {
            boolValues["IsPause"] = false;
            // Pause_Unpause();
            boolValues["IsRageMode"] = true;
            Rage_UnRage();
            floatValues["Score"] = -1f;
            floatValues["RotationX"] = 0f;
            floatValues["RotationYandZ"] = 0f;
            timer.Restart();
        }

        void Pause_Unpause()
        {
            boolValues["IsPause"] = !boolValues["IsPause"];

            if (boolValues["IsPause"])
            {
                timer.Stop();
                //var pauseObject = gameObjectsController.GameObjectsAndTheirShaderUniformsValues[2];
                //pauseObject.ShaderUniformValuesInfos["iTexture"] = 
            }
            else
            {
                timer.Start();
            }
        }

        void Rage_UnRage()
        {
            boolValues["IsRageMode"] = !boolValues["IsRageMode"];

            floatValues["ScoreEncreasing"] = MathExtension.BigStep;

            if (boolValues["IsRageMode"])
            {
                floatValues["RotationSlowStep"] = MathExtension.BigStep;
                floatValues["RotationMediumStep"] = MathExtension.KiloStep;
                floatValues["ScoreDecreasing"] = MathExtension.SmallStep * 4;
                floatValues["ScoreEncreasing"] *= 4f;
                vector3Values["ScoreBarColor"] = vector3Values["ScoreBarRageColor"];
            }
            else
            {
                floatValues["RotationSlowStep"] = MathExtension.SmallStep;
                floatValues["RotationMediumStep"] = MathExtension.MediumStep;
                floatValues["ScoreDecreasing"] = MathExtension.SmallStep;
                floatValues["ScoreEncreasing"] *= 2f;
                vector3Values["ScoreBarColor"] = vector3Values["ScoreBarNormalColor"];
            }
        }
        #endregion
    }
}
