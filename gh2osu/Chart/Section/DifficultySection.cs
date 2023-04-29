using gh2osu.Chart.Event;

namespace gh2osu.Chart.Section
{
    internal class DifficultySection : AbstractSection
    {
        public string Difficulty;

        public DifficultySection(string difficulty)
        {
            this.Difficulty = difficulty;
        }

        protected override string GetSectionTypeName()
        {
            return "Difficulty (" + Difficulty + ")";
        }

        protected override void ParseLine(Chart chart, string line)
        {
            if (!chart.difficulties.ContainsKey(Difficulty))
                chart.difficulties.Add(Difficulty, new());

            string[] parts = line.Split(" = ");

            int tick = int.Parse(parts[0]);
            string[] values = parts[1].Split(' ');

            switch (values[0])
            {
                case "N":
                    {
                        int noteIndex = int.Parse(values[1]);
                        if (noteIndex > 4)
                        {
                            Logger.Warn("Ignoring invalid/unknown note index " + noteIndex);
                            break;
                        }

                        NoteEvent @event = new()
                        {
                            Tick = tick,
                            NoteIndex = int.Parse(values[1]),
                            Length = int.Parse(values[2])
                        };
                        chart.difficulties[Difficulty].Add(@event);
                        break;
                    }

                default:
                    LogUnknownEventType(values[0]);
                    break;
            }
        }
    }
}
