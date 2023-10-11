using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Console_Project
{
    public class GameObject
    {
        readonly Figure Figure;
        int VertexBufferHandler,
            VertexArrayHandler,
            ElementBufferHandler;

        public GameObject(
            Figure figure,
            int shaderProgrammHandler,
            BufferUsageHint bufferUsageHint = BufferUsageHint.StaticDraw
        )
        {
            Figure = figure;
            Init(bufferUsageHint);
        }

        public GameObject(
            Figure figure,
            BufferUsageHint bufferUsageHint = BufferUsageHint.StaticDraw
        )
        {
            Figure = figure;
            Init(bufferUsageHint);
        }

        /// <summary>
        /// Generating buffer from figure as ElementBufferObject and binding that buffer
        /// </summary>
        /// <returns> Genarated buffer </returns>
        public void Init(BufferUsageHint bufferUsageHint = BufferUsageHint.StaticDraw)
        {
            (VertexBufferHandler, VertexArrayHandler, ElementBufferHandler) =
                Figure.GetAllHandlers();
        }

        /// <summary>
        /// Draw figure using ElementBufferObject
        /// </summary>
        public void Draw()
        {
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
