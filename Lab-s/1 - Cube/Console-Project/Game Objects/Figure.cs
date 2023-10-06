using OpenTK.Mathematics;

namespace Console_Project
{
    public partial class Figure
    {
        public Vector3[] Vertices { get; private set; }
        public float[] VerticesCoordinates { get; private set; }
        public uint[] Indices { get; private set; }

        public Figure(Vector3[] vertices, uint[] indices)
        {
            Vertices = vertices;
            Indices = indices;
            var len = vertices.Length * 3;
            var j = 0;
            VerticesCoordinates = new float[len];
            for (int i = 0; i < len; i += 3)
            {
                VerticesCoordinates[i] = vertices[j].X;
                VerticesCoordinates[i + 1] = vertices[j].Y;
                VerticesCoordinates[i + 2] = vertices[j].Z;
                j++;
            }
        }
    }

    // Creation figure methods
    public partial class Figure
    {
        public static Figure CreateTriangle(Vector3 a1, Vector3 a2, Vector3 a3)
        {
            return new Figure(new Vector3[] { a1, a2, a3 }, new uint[] { 0, 1, 2 });
        }

        public static Figure CreateSquare(Vector3 centerPoint, float sideLength)
        {
            var h = sideLength / 2;
            var x = centerPoint.X;
            var y = centerPoint.Y;
            var z = centerPoint.Z;

            var vertices = new Vector3[]
            {
                new Vector3(x - h, y + h, z),
                new Vector3(x + h, y + h, z),
                new Vector3(x + h, y - h, z),
                new Vector3(x - h, y - h, z)
            };

            return new Figure(vertices, new uint[] { 0, 1, 2, 2, 3, 0 });
        }

        public static Figure CreateCube(Vector3 centerPoint, float sideLength)
        {
            var h = sideLength / 2f;

            var x = centerPoint.X;
            var y = centerPoint.Y;
            var z = centerPoint.Z;

            Vector3[] vertices =
            {
                // A1..A4
                new(x - h, y + h, z - h),
                new(x - h, y + h, z + h),
                new(x + h, y + h, z + h),
                new(x + h, y + h, z - h),
                // B1..B4
                new(x - h, y - h, z - h),
                new(x - h, y - h, z + h),
                new(x + h, y - h, z + h),
                new(x + h, y - h, z - h),
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

    // Test instances
    public partial class Figure
    {
        public static Figure TestTriangle =>
            Figure.CreateTriangle(
                new Vector3(0f, .5f, 0f),
                new Vector3(.5f, -.5f, 0f),
                new Vector3(-.5f, -.5f, 0f)
            );

        public static Figure TestSquare => Figure.CreateSquare(Vector3.Zero, 1f);
        public static Figure TestSquare2 => Figure.CreateSquare(new(0f, 0f, -1f), 1f);
        public static Figure TestCube => Figure.CreateCube(new(0f, 0f, -1f), 1f);
        public static Figure TestCube2 => Figure.CreateCube(new(0f, 0f, -5f), 3f);
    }
}
