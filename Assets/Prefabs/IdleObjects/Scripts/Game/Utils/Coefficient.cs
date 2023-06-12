namespace SinSity.Core
{
    public class Coefficient
    {
        public double value { get; }

        public Coefficient(double value)
        {
            this.value = value;
        }

        public virtual Coefficient Clone()
        {
            return new Coefficient(this.value);
        }
    }

    public class TimeCoefficient : Coefficient
    {
        public long passedTimeSeconds { get; set; }

        public TimeCoefficient(double value, long passedTimeSeconds) : base(value)
        {
            this.passedTimeSeconds = passedTimeSeconds;
        }

        public override Coefficient Clone()
        {
            return new TimeCoefficient(this.value, this.passedTimeSeconds);
        }
    }
}