using OpenTK.Graphics.OpenGL;

namespace Console_Project
{
    class GameObjectsController
    {
        public List<(
            GameObject GameObject,
            Dictionary<
                string,
                (dynamic UniformValue, ActiveUniformType UniformType)
            > ShaderUniformValuesInfos
        )> GameObjectsAndTheirShaderUniformsValues = new();
        public readonly ShaderProgram ShaderProgram;

        public GameObjectsController(
            ShaderProgram shaderProgramHandler,
            params (
                GameObject GameObject,
                Dictionary<string, (dynamic UniformValue, ActiveUniformType UniformType)>
            )[] gameObjectsAndTheirShaderUniformsValues
        )
        {
            foreach (var item in gameObjectsAndTheirShaderUniformsValues)
            {
                GameObjectsAndTheirShaderUniformsValues.Add(item);
            }

            ShaderProgram = shaderProgramHandler;
        }

        public void Update()
        {
            ShaderProgram.SetSettedUniforms();
        }

        public void Draw()
        {
            GL.UseProgram(ShaderProgram.ShaderProgramHandler);

            foreach (var gameObjectInfo in GameObjectsAndTheirShaderUniformsValues)
            {
                foreach (var shaderUniformValuesInfo in gameObjectInfo.ShaderUniformValuesInfos)
                {
                    ShaderProgram.SetUniform(
                        shaderUniformValuesInfo.Key,
                        shaderUniformValuesInfo.Value
                    );
                }

                gameObjectInfo.GameObject.Draw();
            }
        }

        ~GameObjectsController()
        {
            Dispose();
        }

        public void Dispose()
        {
            foreach (var gameObjectInfo in GameObjectsAndTheirShaderUniformsValues)
            {
                gameObjectInfo.GameObject.Dispose();
            }

            ShaderProgram.Dispose();
        }
    }
}
