namespace gh2osu.osu.Beatmap
{
    internal class Difficulty
    {
        public string? Version;
        public int CircleSize;

        public List<Element.TimingPoint> TimingPoints = new();
        public List<Element.HitObject> HitObjects = new();
    }
}
