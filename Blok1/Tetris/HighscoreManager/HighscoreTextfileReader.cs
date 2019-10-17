using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace Tetris.Data
{
    public class HighscoreTextfileReader : IHighscoreFetcher
    {
        /// <summary>
        /// The path of the save file
        /// </summary>
        private readonly string Path;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path">The path of the save file</param>
        public HighscoreTextfileReader(string path = "highscores.txt")
        {
            Path = path;
        }

        /// <summary>
        /// Load the highscores from the save file
        /// </summary>
        /// <returns>a list of highscores</returns>
        public List<HighscoreModel> LoadHighscores()
        {
            List<HighscoreModel> highscores;
            try { 
                string value = File.ReadAllText(Path);
                highscores = JsonConvert.DeserializeObject<List<HighscoreModel>>(value);
            } catch
            {
                highscores = new List<HighscoreModel>();
            }
            return highscores;
        }

        /// <summary>
        /// Save the top 10 highscores to the save file
        /// </summary>
        /// <param name="highscores">the list of highscores to be saved</param>
        public void SaveHighscores(List<HighscoreModel> highscores)
        {
            highscores.Sort();
            var top10 = highscores.Take(10);
            var top10Json = JsonConvert.SerializeObject(top10);
            using (var writer = new StreamWriter(Path))
            {
                writer.WriteLine(top10Json);
            }
        }
    }
}
