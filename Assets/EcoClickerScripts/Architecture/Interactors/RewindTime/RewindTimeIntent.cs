namespace SinSity.Domain
{
    public abstract class RewindTimeIntent
    {
        public int passedSeconds { get; }

        protected RewindTimeIntent(int passedSeconds)
        {
            this.passedSeconds = passedSeconds;
        }
    }
}