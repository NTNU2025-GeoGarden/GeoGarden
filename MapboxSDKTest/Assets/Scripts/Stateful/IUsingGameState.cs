namespace Stateful
{
    public interface IUsingGameState
    {
        void LoadData(GameState state);
        void SaveData(ref GameState state);
    }
}
