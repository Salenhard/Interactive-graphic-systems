using OpenTK.Graphics.OpenGL;

namespace Console_Project
{
    class GameObjectsController
    {
        public List<GameObject> GameObjects = new();
        public readonly int ShaderProgramHandler;

        public GameObjectsController(params GameObject[] gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                GameObjects.Add(gameObject);
            }

            ShaderProgramHandler = ShaderProgram.Default.ShaderProgramHandler;
        }

        public GameObjectsController(int shaderProgramHandler, params GameObject[] gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                GameObjects.Add(gameObject);
            }

            ShaderProgramHandler = shaderProgramHandler;
        }

        public void Draw()
        {
            GL.UseProgram(ShaderProgramHandler);

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
