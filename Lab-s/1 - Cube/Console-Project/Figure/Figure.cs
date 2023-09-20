using OpenTK.Mathematics;

namespace Console_Project
{
    public partial class Figure
    {
        public Vector3[] Vertices { get; private set; }
        public float[] VerticesCoordinates { get; private set; }
        public uint[] Indices { get; private set; }
        public event EventHandler? OnTransform;

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

        void Transform(Matrix4 transformMatrix)
        {
            Parallel.ForEach(
                Vertices,
                vertex =>
                {
                    var v = new Vector4(vertex, 1) * transformMatrix;
                    var w = v.W;
                    vertex = new Vector3(v / w);
                }
            );

            OnTransform?.Invoke(this, EventArgs.Empty);
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
    }
}
