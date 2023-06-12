namespace VavilichevGD.Architecture
{
    public interface IVersionUpdater<T>
    {
        bool UpdateVersion(ref T data);
    }
}