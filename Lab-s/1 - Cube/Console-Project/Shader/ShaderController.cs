using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Console_Project
{
    public partial class ShaderController
    {
        public Shader Shader { get; private set; }

        public Matrix4 model,
            view,
            projection;

        // public int modelLocation = OpenGLDrawFigure.NotInitializatedElementBufferObjectValue,
        //     viewLocation = OpenGLDrawFigure.NotInitializatedElementBufferObjectValue,
        //     projectionLocation = OpenGLDrawFigure.NotInitializatedElementBufferObjectValue;

        public ShaderController(Shader shader)
        {
            Shader = shader;
        }

        public ShaderController(Shader shader, Matrix4 model, Matrix4 view, Matrix4 projection)
        {
            Shader = shader;
            // modelLocation = GL.GetUniformLocation(Shader.ShaderProgramHandler, "model");
            // viewLocation = GL.GetUniformLocation(Shader.ShaderProgramHandler, "view");
            // projectionLocation = GL.GetUniformLocation(Shader.ShaderProgramHandler, "projection");
        }

        public void ChangeMatrix(MatrixTypes type, Matrix4 newMatrix)
        {
            // if (modelLocation == OpenGLDrawFigure.NotInitializatedElementBufferObjectValue)
            // {
            //     return;
            // }

            // switch (type)
            // {
            //     case MatrixTypes.Model:
            //         model = newMatrix;
            //         break;

            //     case MatrixTypes.View:
            //         view = newMatrix;
            //         break;

            //     case MatrixTypes.Projection:
            //         projection = newMatrix;
            //         break;

            //     default:
            //         return;
            // }

            // Calculate();
        }

        public void Calculate()
        {
            // if (modelLocation == OpenGLDrawFigure.NotInitializatedElementBufferObjectValue)
            // {
            //     return;
            // }

            // GL.UniformMatrix4(modelLocation, true, ref model);
            // GL.UniformMatrix4(viewLocation, true, ref view);
            // GL.UniformMatrix4(projectionLocation, true, ref projection);
        }
    }

    public partial class ShaderController
    {
        public enum MatrixTypes
        {
            Model,
            View,
            Projection
        }
    }
}
