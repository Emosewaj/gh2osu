namespace gh2osu.Chart.Section
{
    internal class SongMetadataSection : AbstractSection
    {
        protected override string GetSectionTypeName()
        {
            return "Song";
        }

        protected override void ParseLine(Chart chart, string line)
        {
            line = line.Trim();

            string[] parts = line.Split(" = ");
            string field = parts[0];
            string value = parts[1];

            if (value.StartsWith('"'))
                value = value.Replace("\"", "");

            switch (field)
            {
                case "Name":
                case "Artist":
                case "Charter":
                    var type = chart.GetType();
                    var property = type.GetProperty(field);
                    property.SetValue(chart, value);
                    break;
                case "Offset":
                case "Resolution":
                    int intValue = int.Parse(value);
                    chart.GetType().GetProperty(field).SetValue(chart, intValue);
                    break;
                default:
                    Logger.Warn("Ignoring unknown field " + field);
                    break;
            }
        }
    }
}
