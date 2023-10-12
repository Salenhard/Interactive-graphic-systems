using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Console_Project
{
    public partial class OpenGLFigure
    {
        public VertexPositionTexture[] Vertices { get; private set; }
        public float[] VerticesCoordinates { get; private set; }
        public uint[] Indices { get; private set; }
        public bool IsTextured { get; private set; }
        public bool IsColored { get; private set; }

        public OpenGLFigure(VertexPositionTexture[] vertices, uint[] indices)
        {
            Vertices = vertices;
            Indices = indices;
            var len = VertexPositionTexture.VertexInfo.SizeInBytes;
            var j = 0;
            VerticesCoordinates = new float[len];
            for (int i = 0; i < len; i += 5)
            {
                VerticesCoordinates[i] = vertices[j].Position.X;
                VerticesCoordinates[i + 1] = vertices[j].Position.Y;
                VerticesCoordinates[i + 2] = vertices[j].Position.Z;
                VerticesCoordinates[i + 3] = vertices[j].TexCoord.X;
                VerticesCoordinates[i + 4] = vertices[j].TexCoord.Y;

                j++;
            }
        }
    }

    public partial class OpenGLFigure
    {
        public static OpenGLFigure CreateTriangle(Vector3 a1, Vector3 a2, Vector3 a3)
        {
            VertexPositionTexture[] vertices =
            {
                new(a1, Vector2.Normalize(a1.Xy)),
                new(a2, Vector2.Normalize(a2.Xy)),
                new(a3, Vector2.Normalize(a3.Xy))
            };

            return new(vertices, new uint[] { 0, 1, 2 });
        }
    }
}
