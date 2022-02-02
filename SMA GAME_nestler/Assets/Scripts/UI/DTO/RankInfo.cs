using System;
using System.Collections.Generic;

[Serializable]
public class rankInfo
{
    public int resultCode;
    public string resultMessage;
    public List<rankInfoData> resultData;

}

[Serializable]
public class rankInfoData
{
    public int idx;
    public int type;
    public int difficulty;
    public List<rankUser> rankUserList; 
}

[Serializable]
public class rankUser
{
    public int idx;
    public string nickName;
}
