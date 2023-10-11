using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Console_Project
{
    public partial class ShaderProgram
    {
        public static readonly string ShaderSourcesPath = "./Data/Shaders/";

        public readonly int ShaderProgramHandler;
        public readonly ShaderUniform[] ShaderUniforms;
        public readonly ShaderAttribute[] ShaderAttributes;
        public readonly Dictionary<
            string,
            (dynamic Value, ActiveUniformType Type)
        > ShaderUniformValues;

        public ShaderProgram(string vertexShaderCode, string fragmentShaderCode)
        {
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderCode);

            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderCode);

            CompileShader(vertexShader);
            CompileShader(fragmentShader);

            ShaderProgramHandler = GL.CreateProgram();
            GL.AttachShader(ShaderProgramHandler, vertexShader);
            GL.AttachShader(ShaderProgramHandler, fragmentShader);
            GL.LinkProgram(ShaderProgramHandler);

            LinkProgram(ShaderProgramHandler);

            ShaderAttributes = GetAttributeArray(ShaderProgramHandler);
            ShaderUniforms = GetUniformArray(ShaderProgramHandler);
            ShaderUniformValues = new(ShaderUniforms.Length);

            Clear(vertexShader, fragmentShader);
        }

        static void CompileShader(int shader)
        {
            GL.CompileShader(shader);
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                Console.WriteLine(GL.GetShaderInfoLog(shader));
            }
        }

        static void LinkProgram(int programHandler)
        {
            GL.GetProgram(programHandler, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                throw new Exception(GL.GetProgramInfoLog(programHandler));
            }
        }

        void Clear(int vertexShader, int fragmentShader)
        {
            GL.DetachShader(ShaderProgramHandler, vertexShader);
            GL.DetachShader(ShaderProgramHandler, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        public void Dispose()
        {
            GL.DeleteProgram(ShaderProgramHandler);
            GC.SuppressFinalize(this);
        }

        public void SetSettedUniforms()
        {
            foreach (var settedUniform in ShaderUniformValues)
            {
                SetUniform(settedUniform.Key, settedUniform.Value);
            }
        }

        public void SetUniform(string name, (dynamic value, ActiveUniformType type) valueInfo)
        {
            if (ShaderUniforms.Length < 1)
            {
                throw new IndexOutOfRangeException(nameof(ShaderUniforms));
            }
            else if (!ShaderUniforms.Where(x => x.Name == name).Any())
            {
                throw new ArgumentException(null, nameof(name));
            }

            var location = ShaderUniforms.Where(x => x.Name == name).First().Location;

            var (value, type) = (valueInfo.value, valueInfo.type);

            switch (type)
            {
                case ActiveUniformType.Int:
                    var i = (int)value;
                    GL.Uniform1(location, i);
                    break;
                case ActiveUniformType.Float:
                    var f = (float)value;
                    GL.Uniform1(location, f);
                    break;
                case ActiveUniformType.FloatVec2:
                    var v2 = (Vector2)value;
                    GL.Uniform2(location, ref v2);
                    break;
                case ActiveUniformType.FloatVec3:
                    var v3 = (Vector3)value;
                    GL.Uniform3(location, ref v3);
                    break;
                case ActiveUniformType.FloatVec4:
                    var v4 = (Vector4)value;
                    GL.Uniform4(location, ref v4);
                    break;
                case ActiveUniformType.FloatMat4:
                    var mat4 = (Matrix4)value;
                    GL.UniformMatrix4(location, true, ref mat4);
                    break;
                default:
                    throw new NotImplementedException(
                        $"Class can not handle this type ({nameof(type)}) yet. Add hander"
                    );
            }

            if (!ShaderUniformValues.ContainsKey(name))
            {
                ShaderUniformValues.Add(name, valueInfo);
            }
            else
            {
                ShaderUniformValues[name] = valueInfo;
            }
        }

        ~ShaderProgram()
        {
            Dispose();
        }

        public static ShaderProgram FromFiles(
            string vertexShaderSourceFilePath,
            string fragmentShaderSourceFilePath
        )
        {
            var vertexShaderCode = File.ReadAllText(vertexShaderSourceFilePath);
            var fragmentShaderCode = File.ReadAllText(fragmentShaderSourceFilePath);

            return new(vertexShaderCode, fragmentShaderCode);
        }
    }

    public partial class ShaderProgram
    {
        public static readonly ShaderProgram Default =
            new(ShaderDefinitions.VertexShaderDefault, ShaderDefinitions.FragmentShaderDefault);

        public static readonly ShaderProgram Noise =
            new(ShaderDefinitions.VertexShaderDefault, ShaderDefinitions.FragmentNoise);

        public static readonly ShaderProgram NoiseColored =
            new(ShaderDefinitions.VertexShaderDefault, ShaderDefinitions.FragmentNoiseColored);

        public static readonly ShaderProgram PerspectiveSimple =
            new(
                ShaderDefinitions.GetPerspectiveVertexShader(
                    Matrix4.CreatePerspectiveFieldOfView(
                        MathHelper.DegreesToRadians(90.0f),
                        GameCore.WIDTH / GameCore.HEIGHT,
                        1.0f,
                        10.0f
                    )
                ),
                ShaderDefinitions.FragmentShaderDefault
            );

        public static ShaderProgram OurGameUniformShader =>
            new(ShaderDefinitions.UniformVertexShader, ShaderDefinitions.UniformFragmentShader);

        public static ShaderProgram GetSimpleColoredShader(Vector4 color) =>
            new(
                ShaderDefinitions.VertexShaderDefault,
                ShaderDefinitions.GetSimpleColoredFragmentShader(color)
            );

        public static ShaderUniform[] GetUniformArray(int shaderProgramHandler)
        {
            GL.GetProgram(
                shaderProgramHandler,
                GetProgramParameterName.ActiveUniforms,
                out int uniformCount
            );
            var uniforms = new ShaderUniform[uniformCount];

            for (int i = 0; i < uniformCount; i++)
            {
                GL.GetActiveUniform(
                    shaderProgramHandler,
                    i,
                    256,
                    out _,
                    out _,
                    out var type,
                    out var name
                );
                var location = GL.GetUniformLocation(shaderProgramHandler, name);
                uniforms[i] = new ShaderUniform(name, location, type);
            }

            return uniforms;
        }

        public static ShaderAttribute[] GetAttributeArray(int shaderProgramHandler)
        {
            GL.GetProgram(
                shaderProgramHandler,
                GetProgramParameterName.ActiveAttributes,
                out int attributeCount
            );
            var attributes = new ShaderAttribute[attributeCount];

            for (int i = 0; i < attributeCount; i++)
            {
                GL.GetActiveAttrib(
                    shaderProgramHandler,
                    i,
                    256,
                    out _,
                    out _,
                    out var type,
                    out var name
                );
                var location = GL.GetAttribLocation(shaderProgramHandler, name);
                attributes[i] = new ShaderAttribute(name, location, type);
            }

            return attributes;
        }
    }
}
