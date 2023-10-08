using OpenTK.Graphics.OpenGL;

namespace Console_Project
{
    public static class OpenGLExtensions
    {
        public static readonly int Ver2AttributeSize = 2,
            Ver3AttributeSize = 3;

        public static (
            int VertexBufferObject,
            int VertexArrayObject,
            int ElementBufferObject
        ) GetAllHandlers(
            this Figure figure,
            BufferUsageHint bufferUsageHint = BufferUsageHint.StaticDraw,
            int vertexStride = 3
        )
        {
            var vertices = figure.VerticesCoordinates;
            var indices = figure.Indices;

            var verticesTypeSize = sizeof(float);
            var indicesTypeSize = sizeof(uint);

            var vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                vertices.Length * verticesTypeSize,
                vertices,
                bufferUsageHint
            );

            var vao = GL.GenVertexArray();
            // NOTE: As we use only position in vertex shader
            var positionAttributeIndex = VertexAttribute.Position.Index;
            GL.BindVertexArray(vao);
            GL.VertexAttribPointer(
                positionAttributeIndex,
                Ver3AttributeSize,
                VertexAttribPointerType.Float,
                false,
                vertexStride * sizeof(float),
                VertexAttribute.Position.Offset
            );
            GL.EnableVertexAttribArray(positionAttributeIndex);

            var ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(
                BufferTarget.ElementArrayBuffer,
                indices.Length * indicesTypeSize,
                indices,
                bufferUsageHint
            );

            return (vbo, vao, ebo);
        }

        public static GameObject ToGameObject(
            this Figure figure,
            BufferUsageHint bufferUsageHint = BufferUsageHint.StaticDraw
        ) => new(figure, bufferUsageHint);
    };
}
