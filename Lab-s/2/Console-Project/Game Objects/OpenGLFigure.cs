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
            var stride = 5;
            var len = stride * vertices.Length;
            var j = 0;
            VerticesCoordinates = new float[len];
            for (int i = 0; i < len; i += stride)
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

        public static OpenGLFigure CreateSquare(Vector3 centerPoint, float sideLength)
        {
            var h = sideLength / 2;
            var x = centerPoint.X;
            var y = centerPoint.Y;
            var z = centerPoint.Z;

            VertexPositionTexture[] vertices =
            {
                new(new(x - h, y + h, z), new(0f, 1f)),
                new(new(x + h, y + h, z), new(1f, 1f)),
                new(new(x + h, y - h, z), new(1f, 0f)),
                new(new(x - h, y - h, z), new(0f, 0f))
            };

            return new(vertices, new uint[] { 0, 1, 2, 2, 3, 0 });
        }

        public static OpenGLFigure CreateCube(Vector3 centerPoint, float sideLength)
        {
            var h = sideLength / 2f;

            var x = centerPoint.X;
            var y = centerPoint.Y;
            var z = centerPoint.Z;

            VertexPositionTexture[] vertices =
            {
                // A1..A4
                new(new(x - h, y + h, z - h), new()),
                new(new(x - h, y + h, z + h), new()),
                new(new(x + h, y + h, z + h), new()),
                new(new(x + h, y + h, z - h), new()),
                // B1..B4), new()
                new(new(x - h, y - h, z - h), new()),
                new(new(x - h, y - h, z + h), new()),
                new(new(x + h, y - h, z + h), new()),
                new(new(x + h, y - h, z - h), new()),
            };

            uint[] indices =
            {
                0,
                1,
                2,
                2,
                3,
                0,
                0,
                1,
                5,
                5,
                4,
                0,
                0,
                3,
                7,
                7,
                4,
                0,
                6,
                7,
                3,
                3,
                2,
                6,
                6,
                5,
                4,
                4,
                7,
                6,
                6,
                2,
                1,
                1,
                5,
                6,
            };

            return new(vertices, indices);
        }
    }

    public partial class OpenGLFigure
    {
        public static OpenGLFigure MainCube => OpenGLFigure.CreateCube(new(0f, 0f, 0f), 1f);
    }
}
