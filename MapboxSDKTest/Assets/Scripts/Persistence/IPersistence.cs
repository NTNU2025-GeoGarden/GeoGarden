namespace Persistence
{
    public interface IPersistence
    {
        void LoadData(GameState state);
        void SaveData(ref GameState state);
    }
}
