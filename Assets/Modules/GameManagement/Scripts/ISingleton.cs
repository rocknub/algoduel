public interface ISingleton
{

    public void InitializeSingleton();

    public void ClearSingleton();

}

public enum SingletonInitializationStatus
{
    None, Initialized, Initializing
}