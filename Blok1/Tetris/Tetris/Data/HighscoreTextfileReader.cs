using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace Tetris.Data
{
    public class HighscoreTextfileReader : IHighscoreFetcher
    {
        private string Path;

        public HighscoreTextfileReader(string path = "highscores.txt")
        {
            Path = path;
        }

        public List<HighscoreModel> LoadHighscores()
        {
            if (File.Exists(Path))
            {
                string value = File.ReadAllText(Path);
                return JsonConvert.DeserializeObject<List<HighscoreModel>>(value);
            }
            return new List<HighscoreModel>();
        }

        public void SaveHighscores(List<HighscoreModel> highscores)
        {
            highscores.Sort();
            var top10 = highscores.Take(10);
            File.WriteAllText(Path,JsonConvert.SerializeObject(top10));
        }
    }
}
