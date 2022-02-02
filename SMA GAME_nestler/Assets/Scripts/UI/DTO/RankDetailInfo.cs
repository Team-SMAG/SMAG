using System;
using System.Collections.Generic;

[Serializable]
public class rankDetailInfo
{
    public int resultCode;
    public string resultMessage;
    public rankDetailInfoData resultData;

}

[Serializable]
public class rankDetailInfoData
{
    public int idx;
    public int type;
    public int difficulty;
    public int rank;
    public List<rankDetailUser> rankUserList; 
}

[Serializable]
public class rankDetailUser
{
    public int idx;
    public string nickName;
}