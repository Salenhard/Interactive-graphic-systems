using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;

namespace Console_Project
{
    class ShaderDefinitions
    {
        public static string GetPerspectiveVertexShader(Matrix4 MVP) =>
            $@"
        #version 420

        in vec3 {VertexAttributes.Position.Name};

        mat4 MVP = mat4({MVP.ToFormattedString()});

        void main(void)
        {{
            gl_Position = vec4({VertexAttributes.Position.Name}, 1.0) * MVP;
        }}
        ";

        public static string UniformVertexShader =>
            $@"
        #version 420

        layout (location = {VertexAttributes .Position .Index}) in vec3 {VertexAttributes.Position.Name};
        layout (location = {VertexAttributes .TexCoord .Index}) in vec2 {VertexAttributes.TexCoord.Name};

        uniform mat4 iMVP;
        uniform mat4 iTranslation;
        uniform mat4 iRotationX;
        uniform mat4 iRotationY;
        uniform mat4 iRotationZ;
        uniform mat4 iScale;

        out vec2 texCoord;

        void main(void)
        {{
            gl_Position = vec4({VertexAttributes.Position.Name}, 1.0) 
                * iTranslation
                * iRotationX
                * iRotationY
                * iRotationZ
                * iScale
                * iMVP
                ;

            vec2 texCoord = {VertexAttributes.TexCoord.Name};
        }}
        ";

        public static readonly string VertexShaderDefault =
            $@"
        #version 420

        layout (location = {VertexAttributes .Position .Index}) in vec3 {VertexAttributes.Position.Name};

        void main(void)
        {{
            gl_Position = vec4({VertexAttributes.Position.Name}, 1.0);
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
        in vec2 texCoord;

        uniform int iIsUsingInputColor;
        uniform vec3 iColor;
        uniform vec2 iResolution;
        uniform float iTime;

        uniform int iIsUsingTexture;
        uniform sampler2D iTexture;

        void main(void)
        {{
            vec4 col;
    
            if (iIsUsingTexture == 1)
            {{
                col = texture(iTexture, texCoord);
            }}
            else
            {{
                if (iIsUsingInputColor == 1)
                {{
                    col = vec4(iColor, 1.0);
                }}
                else
                {{
                    vec2 uv = gl_FragCoord.xy / iResolution.xy;
                    col = vec4(0.5 + 0.5 * cos(iTime + uv.xyx + vec3(0,2,4)), 1.0);
                }}
            }}

            fragColor = col;
        }}
        ";

        public static string UniformColorableFragmentShader =>
            $@"
        #version 420

        out vec4 fragColor;
        uniform vec4 iColor;

        void main(void)
        {{
            fragColor = iColor;
        }}
        ";

        public static string GetSimpleColoredFragmentShader(Vector4 color) =>
            $@"
        #version 420

        out vec4 fragColor;

        void main(void)
        {{
            fragColor = vec4({color.X.ToFormattedString()}, 
                {color.Y.ToFormattedString()}, 
                {color.Z.ToFormattedString()}, 
                {color.W.ToFormattedString()});
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
