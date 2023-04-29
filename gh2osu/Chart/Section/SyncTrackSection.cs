using gh2osu.Chart.Event;

namespace gh2osu.Chart.Section
{
    internal class SyncTrackSection : AbstractSection
    {
        protected override string GetSectionTypeName()
        {
            return "SyncTrack";
        }

        protected override void ParseLine(Chart chart, string line)
        {
            string[] parts = line.Split(" = ");

            int tick = int.Parse(parts[0]);
            string[] values = parts[1].Split(' ');

            switch(values[0])
            {
                case "TS":
                    { 
                        TimeSignatureEvent @event = new();
                        @event.Tick = tick;
                        @event.Numerator = int.Parse(values[1]);
                        if (values.Length > 2)
                            @event.DenominatorExponent = int.Parse(values[2]);
                        chart.SyncTrackEvents.Add(@event);
                        break;
                    }
                case "B":
                    { 
                        BPMChangeEvent @event = new();
                        @event.Tick = tick;
                        @event.Tempo = int.Parse(values[1]);
                        chart.SyncTrackEvents.Add(@event);
                        break;
                    }

                default:
                    LogUnknownEventType(values[0]);
                    break;
            }
        }
    }
}
