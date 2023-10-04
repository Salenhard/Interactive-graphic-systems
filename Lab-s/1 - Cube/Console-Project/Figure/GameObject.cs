using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Console_Project
{
    public class GameObject
    {
        readonly Figure Figure;
        int VertexBufferHandler,
            VertexArrayHandler,
            ElementBufferHandler,
            ShaderProgrammHandler;

        public GameObject(Figure figure, int shaderProgrammHandler)
        {
            Figure = figure;
            ShaderProgrammHandler = shaderProgrammHandler;
        }

        public GameObject(Figure figure)
        {
            Figure = figure;
            ShaderProgrammHandler = Shader.Default.ShaderProgramHandler;
        }

        /// <summary>
        /// Generating buffer from figure as ElementBufferObject and binding that buffer
        /// </summary>
        /// <returns> Generated buffer </returns>
        public void Init(BufferUsageHint bufferUsageHint = BufferUsageHint.StaticDraw)
        {
            var vertices = Figure.VerticesCoordinates;
            var indices = Figure.Indices;

            (VertexBufferHandler, VertexArrayHandler) = OpenGLExtensions.CreateVBOandVAO(
                vertices,
                bufferUsageHint
            );
        }

        /// <summary>
        /// Draw figure using ElementBufferObject
        /// </summary>
        public void Draw()
        {
            GL.UseProgram(ShaderProgrammHandler);
            GL.BindVertexArray(VertexArrayHandler);
            GL.DrawElements(
                PrimitiveType.Triangles,
                Figure.Indices.Length,
                DrawElementsType.UnsignedInt,
                0
            );
        }

        public void Dispose(bool IsDisposeShader = false)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VertexBufferHandler);
            GL.DeleteBuffer(ElementBufferHandler);
            GL.DeleteVertexArray(VertexArrayHandler);
        }

        ~GameObject()
        {
            Dispose(true);
        }
    }
}
