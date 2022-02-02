using System;
using System.Collections.Generic;

[Serializable]
public class userInfo
{
    public int resultCode;
    public string resultMessage;
    public List<userInfoData> resultData;
}

[Serializable]
public class userInfoData
{
    public int idx;
    public string id;
    public string nickName;
    public int age;
    public string gender;
}