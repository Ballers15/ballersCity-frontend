namespace Orego.Util
{
    public sealed class Reference<T>
    {
        public T value { get; set; }

        public bool HasValue()
        {
            return !Equals(this.value, default(T));
        }
    }
}