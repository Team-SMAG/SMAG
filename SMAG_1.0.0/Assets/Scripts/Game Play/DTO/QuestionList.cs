using System;
using System.Collections.Generic;

[Serializable]
public class QuestionList
{
    public QuestionData[] resultData;
    public short resultCode;
    public string resultMessage;
}

[Serializable]
public class QuestionData
{
    public short idx;
    public string question;
    public short answer;
    public short[] answerList;
}