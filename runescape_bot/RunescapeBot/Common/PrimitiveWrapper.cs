namespace RunescapeBot.Common
{
    public class PrimitiveWrapper<T>
    {
        public T Value { get; set; }

        public PrimitiveWrapper(T value)
        {
            this.Value = value;
        }
    }
}
