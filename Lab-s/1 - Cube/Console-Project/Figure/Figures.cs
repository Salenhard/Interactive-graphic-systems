using OpenTK.Mathematics;

namespace Console_Project
{
    public class Triangle : IFigure
    {
        public static readonly Triangle TestTriangle =
            new(new Vector3(-.5f, -.5f, 0f), new Vector3(.5f, -.5f, 0f), new Vector3(0f, .5f, 0f));
        public static readonly int VerticesCount = 3;
        public static readonly uint[] _indexes = new uint[] { 0, 1, 2 };
        public float[] Vertices { get; private set; }
        public uint[] Indices => _indexes;

        int IFigure.VerticesCount => VerticesCount;

        public Triangle(Vector3 a1, Vector3 a2, Vector3 a3)
        {
            Vertices = new float[] { a1.X, a1.Y, a1.Z, a2.X, a2.Y, a2.Z, a3.X, a3.Y, a3.Z };
        }
    }

    public class Square : IFigure
    {
        public static readonly Square TestSquare = new(Vector3.Zero, 1f);
        public static readonly int VerticesCount = 4;
        public static readonly uint[] indexes = new uint[] { 0, 1, 2, 2, 3, 0 };
        public float[] Vertices { get; private set; }
        public uint[] Indices => indexes;

        int IFigure.VerticesCount => VerticesCount;

        public Square(Vector3 centerPoint, float sideLength)
        {
            var h = sideLength / 2;
            var x = centerPoint.X;
            var y = centerPoint.Y;
            var z = centerPoint.Z;
            Vertices = new float[]
            {
                x - h,
                y + h,
                z,
                x + h,
                y + h,
                z,
                x + h,
                y - h,
                z,
                x - h,
                y - h,
                z
            };
        }
    }
}
