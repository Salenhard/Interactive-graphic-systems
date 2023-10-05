namespace Console_Project
{
    class ShaderDefinitions
    {
        public static readonly string VertexShaderDefault =
            $@"
        #version 410

        layout (location = {VertexAttribute .Position .Index}) in vec3 {VertexAttribute.Position.Name};

        void main(void)
        {{
            gl_Position = vec4({VertexAttribute.Position.Name}, 1.0);
        }}
        ";

        public static readonly string FragmentShaderDefault =
            $@"
        #version 410

        out vec4 fragmentColor;

        void main(void)
        {{
            fragmentColor = vec4(0.3, 0.6, 0.5, 1.0);
        }}
        ";
    }
}
