namespace Console_Project
{
    public interface IFigure
    {
        float[] Vertices { get; }
        uint[] Indices { get; }
        int VerticesCount { get; }
    }
}
