using OpenTK.Graphics.OpenGL;

namespace Console_Project
{
    public class GameObject
    {
        // public readonly Figure Figure;
        public readonly OpenGLFigure Figure;
        int VertexBufferHandler,
            VertexArrayHandler,
            ElementBufferHandler;

        public GameObject(
            // Figure figure,
            OpenGLFigure figure,
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
            (VertexBufferHandler, VertexArrayHandler, ElementBufferHandler) = this.GetAllHandlers(
                bufferUsageHint
            );
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
