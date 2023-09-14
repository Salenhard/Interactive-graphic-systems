using OpenTK.Mathematics;

namespace Console_Project
{
    public class Triangle
    {
        private float[] _vertices;
        public float[] Vertices => _vertices;

        public Triangle(Vector3 a1, Vector3 a2, Vector3 a3)
        {
            _vertices = new float[]
            {
                a1.X, a1.Y, a1.Z,
                a2.X, a2.Y, a2.Z,
                a3.X, a3.Y, a3.Z
            };
        }
    }

    public class Square
    {
        private Triangle[] _triangles;
        public Triangle[] Trinagles => _triangles;
        public Square(Triangle t1, Triangle t2)
        {
            _triangles = new Triangle[]
            {
                t1, t2
            };
        }
    }

    public class Polygon
    {
        private float[] _vertices;
        public float[] Vertices => _vertices;
        public int VerticesCount => _vertices.Length / 3;

        public Polygon(Vector3[] points)
        {
            _vertices = new float[points.Length * 3];
            int counter = 0;

            foreach (var point in points)
            {
                _vertices[counter] = point.X;
                _vertices[counter + 1] = point.Y;
                _vertices[counter + 2] = point.Z;
                counter += 3;
            }
        }

    }
}