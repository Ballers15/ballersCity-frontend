namespace SinSity.Domain
{
    public class RewindTimeIntentGameStarted : RewindTimeIntent
    {
        public RewindTimeIntentGameStarted(int passedSeconds) : base(passedSeconds)
        {
        }
    }
}