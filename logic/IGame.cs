namespace BlazorApp1;

interface IGame
{
    public string? this [int row, int col] { get; }
    public string? Winner { get; }
    public string? NextPlayer { get; }
    public void Set(int row, int col);
}

interface IGameV
{
    string? Winner { get; }
    void PlacePiece(int column);
    string[,] Field { get; }

}