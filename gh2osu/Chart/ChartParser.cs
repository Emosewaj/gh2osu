using gh2osu.Chart.Section;

namespace gh2osu.Chart
{
    internal class ChartParser
    {
        private readonly List<string> LoggedTypes = new();

        private readonly StreamReader file;
        private readonly Chart chart;

        public ChartParser(Stream filestream)
        {
            this.file = new StreamReader(filestream);
            chart = new Chart();
        }

        public Chart ParseChart()
        {
            while (!file.EndOfStream)
            {
                string sectionName = file.ReadLine() ?? "";

                if (!sectionName.StartsWith("["))
                    continue;

                AbstractSection? section = null;
                switch (sectionName)
                {
                    case "[Song]":
                        section = new SongMetadataSection();
                        break;

                    case "[SyncTrack]":
                        section = new SyncTrackSection();
                        break;

                    case "[EasySingle]":
                        section = new DifficultySection("Easy");
                        break;

                    case "[MediumSingle]":
                        section = new DifficultySection("Medium");
                        break;

                    case "[HardSingle]":
                        section = new DifficultySection("Hard");
                        break;

                    case "[ExpertSingle]":
                        section = new DifficultySection("Expert");
                        break;

                    default:
                        LogUnknownSectionType(sectionName);
                        break;
                }

                section?.ParseSection(chart, file);
            }

            return chart;
        }

        private void LogUnknownSectionType(string type)
        {
            if (LoggedTypes.Contains(type))
                return;

            LoggedTypes.Add(type);
            Logger.Warn("Ignoring unknown section type: " + type);
        }
    }
}
