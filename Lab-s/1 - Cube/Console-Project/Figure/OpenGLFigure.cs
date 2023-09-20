using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Console_Project
{
    public class OpenGLDrawFigure
    {
        public static readonly int NotInitializatedElementBufferObjectValue = -72;
        public Figure Figure { get; private set; }
        public int VertexBufferObject { get; private set; }
        public int VertexArrayObject { get; private set; }
        public int ElementBufferObject { get; private set; }
        public ShaderController ShaderController { get; private set; }

        public OpenGLDrawFigure(Figure figure, ShaderController shaderController)
        {
            Figure = figure;
            ElementBufferObject = NotInitializatedElementBufferObjectValue;
            ShaderController = shaderController;
            Figure.OnTransformStarted += OnTransformStarted!;
            Figure.OnTransformCompleted += OnTransformCompleted!;
        }

        /// <summary>
        /// Generating buffer from figure as ElementBufferObject and binding that buffer
        /// </summary>
        /// <returns> Genarated buffer </returns>
        public void Init(BufferUsageHint bufferUsageHint = BufferUsageHint.DynamicDraw)
        {
            var vertices = Figure.VerticesCoordinates;
            var indices = Figure.Indices;

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);

            GL.BufferData(
                BufferTarget.ArrayBuffer,
                vertices.Length * sizeof(float),
                vertices,
                bufferUsageHint
            );

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            GL.VertexAttribPointer(
                ShaderController.Shader.GetAttribLocation("pos0"),
                Core.Vec3AttributeSize,
                VertexAttribPointerType.Float,
                false,
                // Because we are draw triangles => 3 verteces
                Core.Vec3AttributeSize * sizeof(float),
                0
            );
            GL.EnableVertexAttribArray(0);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(
                BufferTarget.ElementArrayBuffer,
                indices.Length * sizeof(uint),
                indices,
                bufferUsageHint
            );

            ShaderController.Shader.Use();
        }

        /// <summary>
        /// Draw figure using ElementBufferObject
        /// </summary>
        public void Draw()
        {
            if (ElementBufferObject == NotInitializatedElementBufferObjectValue)
            {
                return;
            }

            ShaderController.Shader.Use();
            GL.BindVertexArray(VertexArrayObject);

            ShaderController.Calculate();

            GL.DrawElements(
                PrimitiveType.Triangles,
                Figure.Indices.Length,
                DrawElementsType.UnsignedInt,
                0
            );
        }

        private void OnTransformStarted(object sender, EventArgs e)
        {
            Dispose();
        }

        private void OnTransformCompleted(object sender, EventArgs e)
        {
            Init();
        }

        public void Dispose(bool IsDisposeShader = false)
        {
            ElementBufferObject = NotInitializatedElementBufferObjectValue;
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteBuffer(ElementBufferObject);
            GL.DeleteVertexArray(VertexArrayObject);
        }

        ~OpenGLDrawFigure()
        {
            Dispose(true);
        }
    }
}
