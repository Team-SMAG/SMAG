using System;
using System.Collections.Generic;

[Serializable]
public class QuestionResult
{
    public QuestionResultData resultData;
    public int resultCode;
    public string resultMessage;
}

[Serializable]
public class QuestionResultData
{
    public int successCount;
}