using System.Reflection.Metadata;
using OpenTK.Graphics.OpenGL4;

namespace Console_Project
{
    public partial class Shader
    {
        readonly int VertexShader,
            FragmentShader;
        public readonly int ShaderProgramHandler;

        public Shader(string vertexShaderSource, string fragmentShaderSource)
        {
            VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, vertexShaderSource);

            FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, fragmentShaderSource);

            CompileShader(VertexShader);
            CompileShader(FragmentShader);

            ShaderProgramHandler = GL.CreateProgram();
            GL.AttachShader(ShaderProgramHandler, VertexShader);
            GL.AttachShader(ShaderProgramHandler, FragmentShader);
            GL.LinkProgram(ShaderProgramHandler);

            LinkProgram(ShaderProgramHandler);

            Clear();
        }

        public void Use()
        {
            GL.UseProgram(ShaderProgramHandler);
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
            GL.DetachShader(ShaderProgramHandler, VertexShader);
            GL.DetachShader(ShaderProgramHandler, FragmentShader);
            GL.DeleteShader(VertexShader);
            GL.DeleteShader(FragmentShader);
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
    }

    public partial class Shader
    {
        private static readonly string nl = System.Environment.NewLine;

        public static Shader GetSimpleOneColorShader(
            float red,
            float green,
            float blue,
            float transparency
        )
        {
            var variableName = "pos0";
            var v =
                "#version 330 core"
                + nl
                + $"layout (location = 0) in vec3 {variableName};"
                + nl
                + "void main()"
                + nl
                + "{"
                + nl
                + $"gl_Position = vec4({variableName}, 1.0);"
                + nl
                + "}"
                + nl;
            var f =
                "#version 330 core"
                + nl
                + "out vec4 FragColor;"
                + nl
                + "void main()"
                + nl
                + "{"
                + nl
                + $"FragColor = vec4({F2S(red)}f, {F2S(green)}f, {F2S(blue)}f, {F2S(transparency)}f);"
                + nl
                + "}"
                + nl;

            // System.Console.WriteLine(v);
            // System.Console.WriteLine(f);

            return new(v, f);
        }

        private static string F2S(float f)
        {
            return f.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Work In Process
        /// </summary>
        public static Shader GetSimpleOneTexturedShader()
        {
            var pos0 = "pos0";
            // var pos1 = "pos1";
            var v =
                "#version 330 core\n"
                + $"layout (location = 0) in vec3 {pos0};\n"
                + "void main()\n"
                + "{\n"
                + $"gl_Position = vec4({pos0}, 1.0);\n"
                + "}\n";
            var f =
                "#version 330 core\n"
                + "out vec4 FragColor;\n"
                + "void main()\n"
                + "{\n"
                + $"FragColor = vec4(0.5f, 0.5f, 0.5f, 1f);"
                + "}\n";

            return new(v, f);
        }
    }
}
