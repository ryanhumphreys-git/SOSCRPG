namespace SOSCSRPG.Models
{
    public class GameState
    {
        public Player Player { get; set; }
        public int XCoordinate { get; init; }
        public int YCoordinate { get; init; }

        public GameState(Player player, int xCoordinate, int yCoordinate)
        {
            Player = player;
            XCoordinate = xCoordinate;
            YCoordinate = yCoordinate;
        }
    }
}
