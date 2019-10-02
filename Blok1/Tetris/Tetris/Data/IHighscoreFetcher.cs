using System;
using System.Collections.Generic;

public interface IHighscoreFetcher
{
    List<HighscoreModel> LoadHighscores();

    void SaveHighscores(List<HighscoreModel> highscores);
}
