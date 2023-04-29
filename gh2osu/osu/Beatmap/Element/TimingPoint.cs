namespace gh2osu.osu.Beatmap.Element
{
    internal class TimingPoint
    {
        // needed to calculate delta time
        public int ogTick;
        // in ms
        public int Time;
        // duration of beat in ms
        public double BeatLength;
        // amount of beats in a measure
        public int Meter;
        public readonly int SampleSet = 0;
        public readonly int SampleIndex = 0;
        public readonly int Volume = 80;
        public readonly int Uninherited = 1;
        public readonly int Effects = 0;
    }
}
