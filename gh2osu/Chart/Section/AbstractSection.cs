namespace gh2osu.Chart.Section
{
    internal abstract class AbstractSection
    {
        private readonly List<string> LoggedEvents = new();

        public Chart ParseSection(Chart chart, StreamReader file)
        {
            Logger.Info("Parsing section of type " + this.GetSectionTypeName());
            bool inSection = false;
            string line;
            int linesParsed = 0;

            while (!file.EndOfStream)
            {
                line = file.ReadLine() ?? "";

                if (line == "{")
                {
                    inSection = true;
                    continue;
                }
                else if(line == "}")
                {
                    inSection = false;
                    break;
                }

                if (!inSection)
                    continue;

                ParseLine(chart, line);
                linesParsed++;
            }

            if (inSection)
                throw new FormatException("No section end found for section " + this.GetType().Name +
                    "! Chart probably corrupted!");

            Logger.Info(linesParsed + " lines parsed");

            return chart;
        }

        protected void LogUnknownEventType(string type)
        {
            if (LoggedEvents.Contains(type))
                return;

            LoggedEvents.Add(type);
            Logger.Warn("Ignoring unknown event type: " + type);
        }

        protected abstract string GetSectionTypeName();
        protected abstract void ParseLine(Chart chart, string line);
    }
}
