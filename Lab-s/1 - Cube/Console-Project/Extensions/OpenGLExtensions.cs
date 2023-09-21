using OpenTK.Graphics.OpenGL;

namespace Console_Project
{
    public class OpenGLExtensions
    {
        public static readonly int Ver2AttributeSize = 2,
            Ver3AttributeSize = 3;

        /// <summary>
        /// Creating Vertex Buffer Object
        /// </summary>
        static int CreateVBO(
            float[] vertices,
            int verticesTypeSize,
            BufferUsageHint bufferUsageHint = BufferUsageHint.StaticDraw
        )
        {
            var vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                vertices.Length * verticesTypeSize,
                vertices,
                bufferUsageHint
            );
            return vbo;
        }

        /// <summary>
        /// Create Vertex Array Object
        /// </summary>
        /// <param name="vertexStride"> How many numbers one vertex have </param>
        public static (int VertexBufferObject, int VertexArrayObject) CreateVBOandVAO(
            float[] vertices,
            BufferUsageHint bufferUsageHint = BufferUsageHint.StaticDraw,
            int verticesTypeSize = sizeof(float),
            // TODO: Why is there zero in index? And what is that zero-index means?
            int vertexStride = 3
        )
        {
            var vbo = CreateVBO(vertices, verticesTypeSize, bufferUsageHint); // vbo = Gen, Bind and Buffer
            var vao = GL.GenVertexArray(); // VertexArrayHandler = GL.GenVertexArray();
            // TODO: [0] Why this zero? What's that means?
            var zero = 0;
            GL.BindVertexArray(vao);
            GL.VertexAttribPointer(
                zero,
                Ver3AttributeSize,
                VertexAttribPointerType.Float,
                false,
                vertexStride * sizeof(float),
                0
            );
            // TODO: [0] ...
            GL.EnableVertexAttribArray(zero);

            return (vbo, vao);
        }

        public static int CreateEBO(
            uint[] indices,
            BufferUsageHint bufferUsageHint = BufferUsageHint.StaticDraw,
            int indicesTypeSize = sizeof(uint)
        )
        {
            var ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(
                BufferTarget.ElementArrayBuffer,
                indices.Length * indicesTypeSize,
                indices,
                bufferUsageHint
            );
            return ebo;
        }
    };
}
