using OpenTK.Graphics.OpenGL;

namespace Console_Project
{
    class GameObjectsController
    {
        public List<GameObject> GameObjects = new();
        public readonly ShaderProgram ShaderProgram;

        public GameObjectsController(params GameObject[] gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                GameObjects.Add(gameObject);
            }

            ShaderProgram = ShaderProgram.Default;
        }

        public GameObjectsController(
            ShaderProgram shaderProgramHandler,
            params GameObject[] gameObjects
        )
        {
            foreach (var gameObject in gameObjects)
            {
                GameObjects.Add(gameObject);
            }

            ShaderProgram = shaderProgramHandler;
        }

        public void Draw()
        {
            GL.UseProgram(ShaderProgram.ShaderProgramHandler);

            foreach (var gameObject in GameObjects)
            {
                gameObject.Draw();
            }
        }

        ~GameObjectsController()
        {
            Dispose();
        }

        public void Dispose()
        {
            foreach (var gameObject in GameObjects)
            {
                gameObject.Dispose();
            }
        }
    }
}
