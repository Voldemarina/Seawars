namespace Seawars.Interfaces.Services
{
    public interface IConnection
    {
        string CreateGame();
        string JoinGame(string Id);
    }
}