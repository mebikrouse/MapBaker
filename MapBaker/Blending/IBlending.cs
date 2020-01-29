namespace MapBaker.Blending
{
    public interface IBlending<T>
    {
        T Blend(T target, T overlay);
    }
}