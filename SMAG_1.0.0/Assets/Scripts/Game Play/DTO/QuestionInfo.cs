using System;
using System.Collections.Generic;

[Serializable]
public class QuestionInfo
{
    public int trackIdx { get; set; }
    public int userIdx { get; set; }
    public Dictionary<string,int> questionMap { get; set; }

    public QuestionInfo()
    {
        questionMap = new Dictionary<string, int>();
    }
}