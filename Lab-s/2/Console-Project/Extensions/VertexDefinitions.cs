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

    public readonly struct VertexAttributes
    {
        public static readonly VertexAttribute Position = new("Position", 0, 3, 0);
        public static readonly VertexAttribute Color = new("Color", 1, 4, 3 * sizeof(float));
        public static readonly VertexAttribute TexCoord = new("TexCoord", 2, 2, 3 * sizeof(float));
        public static readonly VertexAttribute TexCoordAfterColor =
            new("TexCoord", 2, 2, 7 * sizeof(float));
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

    public readonly struct VertexPosition
    {
        public readonly Vector3 Position;

        public static readonly VertexInfo VertexInfo =
            new(typeof(VertexPosition), VertexAttributes.Position);

        public VertexPosition(Vector3 position)
        {
            Position = position;
        }
    }

    public readonly struct VertexPositionColor
    {
        public readonly Vector3 Position;
        public readonly Color4 Color;

        public static readonly VertexInfo VertexInfo =
            new(typeof(VertexPositionColor), VertexAttributes.Position, VertexAttributes.Color);

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

    public class OpenGLVertex
    {
        public readonly Vector3 Position;
        public readonly Vector4? Color;
        public readonly Vector2? TexCoord;

        public readonly VertexInfo VertexInfo;

        public OpenGLVertex(Vector3 position, Vector4? color = null, Vector2? texCoord = null)
        {
            Position = position;
            Color = color;
            TexCoord = texCoord;

            List<VertexAttribute> vertexAttributes = new() { VertexAttributes.Position };

            var isColored = false;

            if (color is not null)
            {
                vertexAttributes.Add(VertexAttributes.Color);
                isColored = true;
            }
            if (texCoord is not null)
            {
                if (isColored)
                    vertexAttributes.Add(VertexAttributes.TexCoordAfterColor);
                else
                    vertexAttributes.Add(VertexAttributes.TexCoord);
            }

            VertexInfo = new(typeof(OpenGLVertex), vertexAttributes.ToArray());
        }
    }
}
