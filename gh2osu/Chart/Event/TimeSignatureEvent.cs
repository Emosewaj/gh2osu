namespace gh2osu.Chart.Event
{
    internal class TimeSignatureEvent : AbstractEvent
    {
        public int Numerator { get; set; }
        public int DenominatorExponent = 2;
    }
}
