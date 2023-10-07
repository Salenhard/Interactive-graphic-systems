using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;

namespace Console_Project
{
    class ShaderDefinitions
    {
        public static string GetPerspectiveVertexShader(
            Matrix4 model,
            Matrix4 view,
            Matrix4 projection
        ) =>
            $@"
        #version 420

        in vec3 {VertexAttribute.Position.Name};

        mat4 model = mat4({model.ToFormattedString()});
        mat4 view = mat4({view.ToFormattedString()});
        mat4 projection = mat4({projection.ToFormattedString()});

        void main(void)
        {{
            gl_Position = vec4({VertexAttribute.Position.Name}, 1.0) * model * view * projection;
        }}
        ";

        public static string UniformVertexShader =>
            $@"
        #version 420

        in vec3 {VertexAttribute.Position.Name};

        uniform mat4 iModel;
        uniform mat4 iView;
        uniform mat4 iProjection;

        void main(void)
        {{
            gl_Position = vec4({VertexAttribute.Position.Name}, 1.0) * iModel * iView * iProjection;
        }}
        ";

        public static readonly string VertexShaderDefault =
            $@"
        #version 420

        layout (location = {VertexAttribute .Position .Index}) in vec3 {VertexAttribute.Position.Name};

        void main(void)
        {{
            gl_Position = vec4({VertexAttribute.Position.Name}, 1.0);
        }}
        ";

        public static readonly string FragmentShaderDefault =
            $@"
        #version 420

        out vec4 fragmentColor;

        void main(void)
        {{
            fragmentColor = vec4(0.3, 0.6, 0.5, 1.0);
        }}
        ";

        public static readonly string FragmentNoise =
            $@"
        #version 420

        out vec4 fragColor;

        float random (vec2 st) 
        {{
            return fract(sin(dot(st.xy, vec2(12.9898,78.233))) * 43758.5453123);
        }}

        void main(void)
        {{
            vec2 test = gl_FragCoord.xy;
            fragColor = vec4(random(test), random(test), random(test), 1.0);
        }}
        ";

        public static readonly string FragmentNoiseColored =
            $@"
        #version 420

        out vec4 fragColor;

        float random (vec2 st) 
        {{
            return fract(sin(dot(st.xy, vec2(12.9898,78.233))) * 43758.5453123);
        }}

        void main(void)
        {{
            fragColor = vec4(random(gl_FragCoord.xy), random(gl_FragCoord.yz), random(gl_FragCoord.zx), 1.0);
        }}
        ";

        public static string UniformFragmentShader =>
            $@"
        #version 420

        out vec4 fragColor;

        uniform vec3 iColor;

        void main(void)
        {{
            fragColor = vec4(iColor, 1.0);
        }}
        ";

        public static string UniformHoverFragmentShader =>
            $@"
        #version 420

        out vec4 fragColor;

        uniform vec3 iColor;
        uniform vec3 iHoverColor;
        uniform vec2 iMouseCoordinates;
        uniform vec2 iLeftTopHoverCoord;
        uniform vec2 iRightBottomHoverCoord;

        void main(void)
        {{       
            vec3 color = iColor;

            if (
                iMouseCoordinates.x >= iLeftTopHoverCoord.x 
                && iMouseCoordinates.x <= iRightBottomHoverCoord.x
                && iMouseCoordinates.y >= iLeftTopHoverCoord.y 
                && iMouseCoordinates.y <= iRightBottomHoverCoord.y)
            {{
                color = iHoverColor;
            }}

            fragColor = (color, 1.0);
        }}
        ";
    }

    public readonly struct ShaderUniform
    {
        public readonly string Name;
        public readonly int Location;
        public readonly ActiveUniformType Type;

        public ShaderUniform(string name, int location, ActiveUniformType type)
        {
            Name = name;
            Location = location;
            Type = type;
        }
    }

    public readonly struct ShaderAttribute
    {
        public readonly string Name;
        public readonly int Location;
        public readonly ActiveAttribType Type;

        public ShaderAttribute(string name, int location, ActiveAttribType type)
        {
            Name = name;
            Location = location;
            Type = type;
        }
    }
}
