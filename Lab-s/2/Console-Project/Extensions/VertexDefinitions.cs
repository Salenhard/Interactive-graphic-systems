using OpenTK.Mathematics;

namespace Console_Project
{
    public readonly struct VertexAttribute
    {
        public readonly string Name;
        public readonly int Index,
            ComponentCount,
            Offset;

        public VertexAttribute(string name, int index, int componentCount, int offset)
        {
            Name = name;
            Index = index;
            ComponentCount = componentCount;
            Offset = offset;
        }

        public static readonly Dictionary<
            int,
            (string Name, int Index)
        > VertexAttributesDictionary =
            new()
            {
                { 0, ("Position", 0) },
                { 1, ("Color", 1) },
                { 2, ("TexCoord", 2) }
            };

        public static readonly VertexAttribute Position =
            new(VertexAttributesDictionary[0].Name, VertexAttributesDictionary[0].Index, 3, 0);
        public static readonly VertexAttribute Color =
            new(
                VertexAttributesDictionary[1].Name,
                VertexAttributesDictionary[1].Index,
                4,
                3 * sizeof(float)
            );
        public static readonly VertexAttribute TexCoord =
            new(
                VertexAttributesDictionary[2].Name,
                VertexAttributesDictionary[2].Index,
                2,
                3 * sizeof(float)
            );
    }

    public sealed class VertexInfo
    {
        public readonly Type Type;
        public readonly int SizeInBytes;
        public readonly VertexAttribute[] VertexAttributes;

        public VertexInfo(Type type, params VertexAttribute[] vertexAttributes)
        {
            Type = type;
            SizeInBytes = 0;
            VertexAttributes = vertexAttributes;

            foreach (var attribute in vertexAttributes)
            {
                SizeInBytes += attribute.ComponentCount * sizeof(float);
            }
        }
    }

    public readonly struct VertexPosition3D
    {
        public readonly Vector3 Position;

        public static readonly VertexInfo VertexInfo =
            new(typeof(VertexPosition3D), VertexAttribute.Position);

        public VertexPosition3D(Vector3 position)
        {
            Position = position;
        }
    }

    public readonly struct VertexPositionColor
    {
        public readonly Vector3 Position;
        public readonly Color4 Color;

        public static readonly VertexInfo VertexInfo =
            new(typeof(VertexPositionColor), VertexAttribute.Position, VertexAttribute.Color);

        public VertexPositionColor(Vector3 position, Color4 color)
        {
            Position = position;
            Color = color;
        }
    }

    public readonly struct VertexPositionTexture
    {
        public readonly Vector3 Position;
        public readonly Vector2 TexCoord;

        public static readonly VertexInfo VertexInfo =
            new(typeof(VertexPositionTexture), VertexAttribute.Position, VertexAttribute.TexCoord);

        public VertexPositionTexture(Vector3 position, Vector2 texCoord)
        {
            Position = position;
            TexCoord = texCoord;
        }
    }
}
