using OpenTK.Graphics.OpenGL4;

namespace Console_Project
{
    public class Shader
    {
        public static string ShaderDirectory = "./Shaders/";
        int vertexShader,
            fragmentShader,
            programHandler;

        public Shader(string vertexShaderName, string fragmentShaderName)
        {
            string vertexShaderSource = File.ReadAllText(
                Path.Combine(ShaderDirectory, vertexShaderName)
            );
            string fragmentShaderSource = File.ReadAllText(
                Path.Combine(ShaderDirectory, fragmentShaderName)
            );

            vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);

            fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);

            CompileShader(vertexShader);
            CompileShader(fragmentShader);

            programHandler = GL.CreateProgram();
            GL.AttachShader(programHandler, vertexShader);
            GL.AttachShader(programHandler, fragmentShader);
            GL.LinkProgram(programHandler);

            LinkProgram(programHandler);

            Clear();
        }

        public void Use()
        {
            GL.UseProgram(programHandler);
        }

        public void CompileShader(int shader)
        {
            GL.CompileShader(shader);
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                Console.WriteLine(GL.GetShaderInfoLog(shader));
            }
        }

        public void LinkProgram(int programHandler)
        {
            GL.GetProgram(programHandler, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                Console.WriteLine(GL.GetProgramInfoLog(programHandler));
            }
        }

        void Clear()
        {
            GL.DetachShader(programHandler, vertexShader);
            GL.DetachShader(programHandler, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(programHandler, attribName);
        }

        public void Dispose()
        {
            GL.DeleteProgram(programHandler);
            GC.SuppressFinalize(this);
        }

        ~Shader()
        {
            Dispose();
        }
    }
}
