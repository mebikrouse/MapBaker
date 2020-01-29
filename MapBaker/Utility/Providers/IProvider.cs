namespace MapBaker.Utility.Providers
{
    public interface IProvider<out T>
    {
        T GetItem();
    }
}