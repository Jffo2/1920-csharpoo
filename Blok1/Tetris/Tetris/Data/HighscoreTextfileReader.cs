﻿using System;
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
            if (File.Exists(Path))
            {
                string value = File.ReadAllText(Path);
                return JsonConvert.DeserializeObject<List<HighscoreModel>>(value);
            }
            return new List<HighscoreModel>();
        }

        /// <summary>
        /// Save the top 10 highscores to the save file
        /// </summary>
        /// <param name="highscores">the list of highscores to be saved</param>
        public void SaveHighscores(List<HighscoreModel> highscores)
        {
            highscores.Sort();
            var top10 = highscores.Take(10);
            File.WriteAllText(Path,JsonConvert.SerializeObject(top10));
        }
    }
}
