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
    }

    public class VertexAttributes
    {
        public static readonly VertexAttribute Position = new("Position", 0, 3, 0);
        public static readonly VertexAttribute Color = new("Color", 1, 3, 3 * sizeof(float));
        public static readonly VertexAttribute TexCoord = new("TexCoord", 2, 2, 3 * sizeof(float));
        public static readonly VertexAttribute TexCoordAfterColor =
            new("TexCoord", 2, 2, 6 * sizeof(float));
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

    public readonly struct VertexPositionTexture
    {
        public readonly Vector3 Position;
        public readonly Vector2 TexCoord;

        public static readonly VertexInfo VertexInfo =
            new(
                typeof(VertexPositionTexture),
                VertexAttributes.Position,
                VertexAttributes.TexCoord
            );

        public VertexPositionTexture(Vector3 position, Vector2 texCoord)
        {
            Position = position;
            TexCoord = texCoord;
        }
    }
}
