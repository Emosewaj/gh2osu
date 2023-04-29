using gh2osu.Chart.Event;

namespace gh2osu.Chart
{
    internal class Chart
    {
        public string? Name { get; set; }
        public string? Artist { get; set; }
        public string? Charter { get; set; }
        public int? Offset { get; set; }
        public int? Resolution { get; set; }

        public List<AbstractEvent> SyncTrackEvents = new();
        public Dictionary<string, List<NoteEvent>> difficulties = new();
    }
}
