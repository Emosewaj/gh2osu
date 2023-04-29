using gh2osu.Chart;
using gh2osu.osu;
using gh2osu.osu.Beatmap;

namespace gh2osu
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Logger.Info("Select .chart file to convert");
            OpenFileDialog ofd = new()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false,
                Filter = "chart files (*.chart)|*.chart|All files (*.*)|*.*",
                Title = "Select .chart file to convert",
                InitialDirectory = Application.StartupPath
            };

            if (ofd.ShowDialog() != DialogResult.OK)
            {
                Logger.Error("File reading aborted");
                return;
            }

            Chart.Chart chart;
            using (Stream stream = ofd.OpenFile())
            {
                ChartParser parser = new(stream);
                chart = parser.ParseChart();
            }

            Chart2OsuConverter converter = new(chart);
            BeatmapGroup beatmapGroup = converter.Convert();

            FolderBrowserDialog fbd = new()
            {
                Description = "Select folder to save beatmaps to (a new folder will be created)",
                InitialDirectory = Application.StartupPath,
                RootFolder = Environment.SpecialFolder.MyComputer,
                UseDescriptionForTitle = true,
                ShowNewFolderButton = true
            };

            if (fbd.ShowDialog() != DialogResult.OK)
            {
                Logger.Error("Saving aborted");
                return;
            }

            beatmapGroup.SaveFiles(fbd.SelectedPath);

            Console.WriteLine();
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }
    }
}