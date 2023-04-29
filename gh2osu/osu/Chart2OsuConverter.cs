using gh2osu.osu.Beatmap;
using gh2osu.osu.Beatmap.Element;
using gh2osu.Chart.Event;

namespace gh2osu.osu
{
    internal class Chart2OsuConverter
    {
        private readonly Chart.Chart chart;

        public Chart2OsuConverter(Chart.Chart chart)
        {
            this.chart = chart;
        }

        public BeatmapGroup Convert()
        {
            Logger.Info("Beginning conversion to osu! beatmap");

            BeatmapGroup group = new()
            {
                Title = chart.Name ?? "",
                Artist = chart.Artist ?? "",
                Charter = chart.Charter ?? "Emosewaj",
                AudioLeadIn = chart.Offset ?? 0
            };

            foreach(KeyValuePair<string, List<NoteEvent>> chartDifficulty in chart.difficulties)
            {
                Logger.Info(string.Format("Converting difficulty {0}", chartDifficulty.Key));

                Difficulty difficulty = new()
                {
                    Version = chartDifficulty.Key
                };

                if (group.Difficulties.Count != 0) { 
                    difficulty.TimingPoints = group.Difficulties.First().TimingPoints;
                } else
                {
                    ConvertTimingPoints(difficulty, chart.SyncTrackEvents);
                }

                ConvertNotes(difficulty, chartDifficulty.Value, chart.SyncTrackEvents);
                
                group.Difficulties.Add(difficulty);
            }

            return group;
        }

        private int GetTotalMilliseconds(int tick, List<AbstractEvent> syncTrackEvents, List<TimingPoint> timingPoints)
        {
            int i;
            for (i = syncTrackEvents.Count - 1; syncTrackEvents[i].Tick >= tick || syncTrackEvents[i] is not BPMChangeEvent; i--) { }

            BPMChangeEvent previousEvent = (BPMChangeEvent)syncTrackEvents[i];

            int k;
            for (k = timingPoints.Count - 1; timingPoints[k].ogTick >= tick; k--) { }

            double prevBpmActual = previousEvent.Tempo / 1000d;
            int prevTick = previousEvent.Tick;

            return timingPoints[k].Time + (int)Math.Round(60d / prevBpmActual * ((tick - prevTick) / (double)(chart.Resolution ?? 0d)) * 1000d);
        }

        private void ConvertTimingPoints(Difficulty difficulty, List<AbstractEvent> syncTrackEvents)
        {
            int currentMeter = 4;
            for (int i = 0; i < syncTrackEvents.Count; i++)
            {
                if (syncTrackEvents[i] is TimeSignatureEvent tsEvent)
                {
                    currentMeter = tsEvent.Numerator;
                    continue;
                }

                BPMChangeEvent @event = (BPMChangeEvent)syncTrackEvents[i];

                TimingPoint timingPoint = new()
                {
                    Meter = currentMeter,
                    ogTick = @event.Tick
                };

                if (@event.Tick == 0)
                {
                    double bpmActual = @event.Tempo / 1000d;

                    timingPoint.Time = 0;
                    timingPoint.BeatLength = 60000 / bpmActual;
                }
                else
                {
                    timingPoint.Time = GetTotalMilliseconds(@event.Tick, syncTrackEvents, difficulty.TimingPoints);
                    timingPoint.BeatLength = 60000 / (@event.Tempo / 1000d);
                }

                difficulty.TimingPoints.Add(timingPoint);
            }

            Logger.Info(string.Format("Converted {0} sync track events", syncTrackEvents.Count));
        }

        private void ConvertNotes(Difficulty difficulty, List<NoteEvent> notes, List<AbstractEvent> syncTrackEvents)
        {
            int maxColumns = 4;
            foreach (NoteEvent note in notes)
            {
                if (note.NoteIndex == 4)
                {
                    maxColumns = 5;
                }
            }

            difficulty.CircleSize = maxColumns;

            foreach (NoteEvent note in notes)
            {
                HitObject hitObject = new();

                if (maxColumns == 4)
                    hitObject.X = HitObject.X_VALUES_FOUR_COL[note.NoteIndex];
                else if (maxColumns == 5)
                    hitObject.X = HitObject.X_VALUES_FIVE_COL[note.NoteIndex];

                hitObject.Type = note.Length == 0 ? 1 : 128;

                if (note.Tick == 0)
                {
                    hitObject.Time = 0;
                }
                else
                {
                    hitObject.Time = GetTotalMilliseconds(note.Tick, syncTrackEvents, difficulty.TimingPoints);
                }

                if(note.Length != 0)
                {
                    hitObject.EndTime = GetTotalMilliseconds(note.Tick + note.Length, syncTrackEvents, difficulty.TimingPoints);
                }

                difficulty.HitObjects.Add(hitObject);
            }

            Logger.Info(string.Format("Converted {0} notes", notes.Count));
        }
    }
}
