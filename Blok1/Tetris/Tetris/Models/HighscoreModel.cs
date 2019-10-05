using System;

public class HighscoreModel : IComparable<HighscoreModel>
{
	public string Name { get; set; }
    public int Score { get; set; }

    public int CompareTo(HighscoreModel other)
    {
        return Score > other.Score ? -1 : Score == other.Score ? 0 : 1;
    }
}
