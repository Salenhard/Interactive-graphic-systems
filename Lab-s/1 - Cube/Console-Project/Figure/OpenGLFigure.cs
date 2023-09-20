using OpenTK.Graphics.OpenGL4;

namespace Console_Project
{
    public class OpenGLDrawFigure
    {
        public static readonly int NotInitializatedElementBufferObjectValue = -72;
        public Figure Figure { get; private set; }
        public int VertexBufferObject { get; private set; }
        public int VertexArrayObject { get; private set; }
        public int ElementBufferObject { get; private set; }
        public Shader Shader { get; private set; }

        public OpenGLDrawFigure(Figure figure, Shader shader)
        {
            Figure = figure;
            ElementBufferObject = NotInitializatedElementBufferObjectValue;
            Shader = shader;
        }

        /// <summary>
        /// Generating buffer from figure as ElementBufferObject and binding that buffer
        /// </summary>
        /// <returns> Genarated buffer </returns>
        public void Init(BufferUsageHint bufferUsageHint = BufferUsageHint.DynamicDraw)
        {
            var vertices = Figure.VerticesCoordinates;
            var indices = Figure.Indices;

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);

            GL.BufferData(
                BufferTarget.ArrayBuffer,
                vertices.Length * sizeof(float),
                vertices,
                bufferUsageHint
            );

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            GL.VertexAttribPointer(
                Shader.GetAttribLocation("pos0"),
                Core.Vec3AttributeSize,
                VertexAttribPointerType.Float,
                false,
                // Because we are draw triangles => 3 verteces
                3 * sizeof(float),
                0
            );
            GL.EnableVertexAttribArray(0);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(
                BufferTarget.ElementArrayBuffer,
                indices.Length * sizeof(uint),
                indices,
                bufferUsageHint
            );

            Shader.Use();
        }

        /// <summary>
        /// Draw figure using ElementBufferObject
        /// </summary>
        public void Draw()
        {
            if (ElementBufferObject == NotInitializatedElementBufferObjectValue)
            {
                throw new Exception("Trying to draw not initializated figure");
            }

            Shader.Use();
            GL.BindVertexArray(VertexArrayObject);
            GL.DrawElements(
                PrimitiveType.Triangles,
                Figure.Indices.Length,
                DrawElementsType.UnsignedInt,
                0
            );
        }

        public void Dispose()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteBuffer(ElementBufferObject);
            GL.DeleteVertexArray(VertexArrayObject);
            Shader.Dispose();
            ElementBufferObject = NotInitializatedElementBufferObjectValue;
        }

        ~OpenGLDrawFigure()
        {
            Dispose();
        }
    }
}
