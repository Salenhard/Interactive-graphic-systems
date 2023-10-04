using OpenTK.Graphics.OpenGL;

namespace Console_Project
{
    public class Shader
    {
        public static readonly string ShaderSourcesPath = "./Data/Shaders/";

        public readonly int ShaderProgramHandler;

        public Shader(string vertexShaderSourceFilePath, string fragmentShaderSourceFilePath)
        {
            // TODO: Fix error: gl_Position is not accessible in this profile
            var vertexShaderSource = File.ReadAllText(vertexShaderSourceFilePath);
            var fragmentShaderSource = File.ReadAllText(fragmentShaderSourceFilePath);

            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);

            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);

            CompileShader(vertexShader);
            CompileShader(fragmentShader);

            ShaderProgramHandler = GL.CreateProgram();
            GL.AttachShader(ShaderProgramHandler, vertexShader);
            GL.AttachShader(ShaderProgramHandler, fragmentShader);
            GL.LinkProgram(ShaderProgramHandler);

            LinkProgram(ShaderProgramHandler);

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
                Console.WriteLine(GL.GetProgramInfoLog(programHandler));
            }
        }

        void Clear(int vertexShader, int fragmentShader)
        {
            GL.DetachShader(ShaderProgramHandler, vertexShader);
            GL.DetachShader(ShaderProgramHandler, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(ShaderProgramHandler, attribName);
        }

        public void Dispose()
        {
            GL.DeleteProgram(ShaderProgramHandler);
            GC.SuppressFinalize(this);
        }

        ~Shader()
        {
            Dispose();
        }

        public static Shader Default =
            new(
                Path.Combine(Shader.ShaderSourcesPath, "shader.vert"),
                Path.Combine(Shader.ShaderSourcesPath, "shader.frag")
            );
    }
}
