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
            new(typeof(VertexPosition3D), new VertexAttribute("Position", 0, 3, 0));

        public VertexPosition3D(Vector3 position)
        {
            Position = position;
        }
    }

    public class VertexPositionColor
    {
        public readonly Vector3 Position;
        public readonly Color4 Color;

        public static readonly VertexInfo VertexInfo =
            new(
                typeof(VertexPosition3D),
                new VertexAttribute("Position", 0, 3, 0),
                new VertexAttribute("Color", 1, 4, 3 * sizeof(float))
            );

        public VertexPositionColor(Vector3 position, Color4 color)
        {
            Position = position;
            Color = color;
        }
    }

    public class VertexPositionTexture
    {
        public readonly Vector3 Position;
        public readonly Vector2 TexCoord;

        public static readonly VertexInfo VertexInfo =
            new(
                typeof(VertexPosition3D),
                new VertexAttribute("Position", 0, 3, 0),
                new VertexAttribute("TexCoord", 1, 2, 3 * sizeof(float))
            );

        public VertexPositionTexture(Vector3 position, Vector2 texCoord)
        {
            Position = position;
            TexCoord = texCoord;
        }
    }
}
