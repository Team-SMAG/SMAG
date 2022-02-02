using System;
using System.Collections.Generic;

[Serializable]
public class trackInfo
{
    public int resultCode;
    public string resultMessage;
    public List<trackInfoData> resultData;
}

[Serializable]
public class trackInfoData
{
    public int idx;
    public int type;
    public int difficulty;
}