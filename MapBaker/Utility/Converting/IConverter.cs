namespace MapBaker.Utility.Converting
{
    public interface IConverter<in T, out U>
    {
        U Convert(T input);
    }
}