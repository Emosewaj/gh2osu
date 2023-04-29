namespace gh2osu.Chart.Event
{
    internal class BPMChangeEvent : AbstractEvent
    {
        // raw BPM (120000 = 120 BPM)
        public int Tempo { get; set; }
    }
}
