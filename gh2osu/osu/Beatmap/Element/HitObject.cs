namespace gh2osu.osu.Beatmap.Element
{
    internal class HitObject
    {
        public static readonly int[] X_VALUES_FOUR_COL = { 64, 192, 320, 448 };
        public static readonly int[] X_VALUES_FIVE_COL = { 51, 153, 256, 358, 460 };

        public int X;
        public readonly int Y = 192;
        public int Time;
        public int Type;
        public readonly int HitSound = 0;
        public int? EndTime;
        public readonly string HitSample = "0:0:0:0";
    }
}
