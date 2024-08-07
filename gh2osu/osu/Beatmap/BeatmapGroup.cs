﻿using gh2osu.osu.Beatmap.Element;

namespace gh2osu.osu.Beatmap
{
    internal class BeatmapGroup
    {
        public string Title = string.Empty;
        public string Artist = string.Empty;
        public string? Charter;
        public int AudioLeadIn;

        public List<Difficulty> Difficulties = new();

        public void SaveFiles(string selectedPath)
        {
            if (!Directory.Exists(selectedPath))
            {
                Logger.Error("Selected folder does not exist! Saving aborted");
                return;
            }

            string artist = Artist != string.Empty ? Artist : "Unknown Artist";
            string title = Title != string.Empty ? Title : "Unknown Title";

            if (artist == "Unknown Artist" || title == "Unknown Title")
                Logger.Warn("Artist or Title were empty and were changed to either \"Unknown Artist\" or \"Unknown Title\" respectively!");

            string targetFolder = ReplaceInvalidPathChars(string.Format("{0} - {1}", artist, title)).Trim();
            string targetPath = string.Format(@"{0}\{1}", selectedPath, targetFolder).Trim();

            if (Directory.Exists(targetPath))
            {
                Logger.Error(string.Format("Directory {0} already exists! Saving aborted", targetPath));
                return;
            }

            Directory.CreateDirectory(targetPath);
            Logger.Info(string.Format("Writing to folder {0}", targetPath));

            foreach (Difficulty difficulty in Difficulties)
            {
                string targetFileName = ReplaceInvalidFileNameChars(string.Format("{0} - {1} [{2}].osu", artist, title, difficulty.Version)).Trim();

                Logger.Info(string.Format("Writing {0}...", targetFileName));

                using StreamWriter writer = new(string.Format(@"{0}\{1}", targetPath, targetFileName), false);
                writer.WriteLine("osu file format v14");
                writer.WriteLine();

                writer.WriteLine("[General]");
                writer.WriteLine("AudioFilename: audio.mp3");
                writer.WriteLine(string.Format("AudioLeadIn: {0}", AudioLeadIn));
                writer.WriteLine("PreviewTime: -1");
                writer.WriteLine("Countdown: 0");
                writer.WriteLine("SampleSet: Normal");
                writer.WriteLine("StackLeniency: 0.7");
                writer.WriteLine("Mode: 3");
                writer.WriteLine("LetterboxInBreaks: 0");
                writer.WriteLine("SpecialStyle: 0");
                writer.WriteLine("WidescreenStoryboard: 0");
                writer.WriteLine();

                writer.WriteLine("[Metadata]");
                writer.WriteLine(string.Format("Title:{0}", Title));
                writer.WriteLine(string.Format("TitleUnicode:{0}", Title));
                writer.WriteLine(string.Format("Artist:{0}", Artist));
                writer.WriteLine(string.Format("ArtistUnicode:{0}", Artist));
                writer.WriteLine(string.Format("Creator:{0}", Charter));
                writer.WriteLine(string.Format("Version:{0}", difficulty.Version));
                writer.WriteLine("Source:");
                writer.WriteLine("Tags:");
                writer.WriteLine("BeatmapID:0");
                writer.WriteLine("BeatmapSetID:-1");
                writer.WriteLine();

                // TODO: Tweak these
                writer.WriteLine("[Difficulty]");
                writer.WriteLine("HPDrainRate:5");
                writer.WriteLine(string.Format("CircleSize:{0}", difficulty.CircleSize));
                writer.WriteLine("OverallDifficulty:5");
                writer.WriteLine("ApproachRate:5");
                writer.WriteLine("SliderMultiplier:1.4");
                writer.WriteLine("SliderTickRate:1");
                writer.WriteLine();

                // Timing points
                writer.WriteLine("[TimingPoints]");
                foreach (TimingPoint timingPoint in difficulty.TimingPoints)
                {
                    writer.WriteLine(
                        string.Format(
                            "{0},{1},{2},{3},{4},{5},{6},{7}",
                            timingPoint.Time,
                            timingPoint.BeatLength,
                            timingPoint.Meter,
                            timingPoint.SampleSet,
                            timingPoint.SampleIndex,
                            timingPoint.Volume,
                            timingPoint.Uninherited,
                            timingPoint.Effects
                        )
                    );
                }
                writer.WriteLine();

                // Hit objects
                writer.WriteLine("[HitObjects]");
                foreach (HitObject hitObject in difficulty.HitObjects)
                {
                    if (hitObject.EndTime == null)
                    {
                        writer.WriteLine(
                            string.Format(
                                    "{0},{1},{2},{3},{4}",
                                    hitObject.X,
                                    hitObject.Y,
                                    hitObject.Time,
                                    hitObject.Type,
                                    hitObject.HitSound
                            )
                        );
                    }
                    else
                    {
                        writer.WriteLine(
                            string.Format(
                                "{0},{1},{2},{3},{4},{5}:{6}",
                                hitObject.X,
                                hitObject.Y,
                                hitObject.Time,
                                hitObject.Type,
                                hitObject.HitSound,
                                hitObject.EndTime,
                                hitObject.HitSample
                            )
                        );
                    }
                }
                writer.WriteLine();
            }

            Logger.Info("Finished!");
        }

        private string ReplaceInvalidPathChars(string path)
        {
            return string.Join("_", path.Split(Path.GetInvalidPathChars()));
        }

        private string ReplaceInvalidFileNameChars(string filename)
        {
            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        }
    }
}
