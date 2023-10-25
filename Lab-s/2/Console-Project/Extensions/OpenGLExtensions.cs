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
            this GameObject gameObject,
            BufferUsageHint bufferUsageHint = BufferUsageHint.StaticDraw,
            int vertexStride = 3
        )
        {
            var figure = gameObject.Figure;
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
            GL.BindVertexArray(vao);

            var attr = VertexAttributes.Position;

            // POSITION
            GL.EnableVertexAttribArray(attr.Index);
            GL.VertexAttribPointer(
                attr.Index,
                attr.ComponentCount,
                VertexAttribPointerType.Float,
                false,
                VertexPositionTexture.VertexInfo.SizeInBytes,
                attr.Offset
            );

            attr = VertexAttributes.TexCoord;

            // TEXTURE COORDINATES
            GL.EnableVertexAttribArray(attr.Index);
            GL.VertexAttribPointer(
                attr.Index,
                attr.ComponentCount,
                VertexAttribPointerType.Float,
                false,
                VertexPositionTexture.VertexInfo.SizeInBytes,
                attr.Offset
            );

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

        // public static GameObject ToGameObject(
        //     this Figure figure,
        //     BufferUsageHint bufferUsageHint = BufferUsageHint.StaticDraw
        // ) => new(figure, bufferUsageHint);

        public static GameObject ToGameObject(
            this OpenGLFigure figure,
            BufferUsageHint bufferUsageHint = BufferUsageHint.StaticDraw,
            PrimitiveType drawingType = PrimitiveType.Triangles
        ) => new(figure, bufferUsageHint, drawingType);
    };
}
