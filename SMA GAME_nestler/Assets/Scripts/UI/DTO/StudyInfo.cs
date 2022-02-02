using System;
using System.Collections.Generic;

[Serializable]
public class studyInfo
{
    public int status;
    public string message;
    public List<studyInfoData> data;
}

[Serializable]
public class studyInfoData
{
    public string sid;    //학습 이력 코드
    public string sthma_nm;    //소주제가 포함된 주제 이름
    public string step_nm;    //소주제가 포함된 단계 이름
    public string stg_nm;    //소주제 이름
    public string dtl_cn;    //소주제에 대한 세부 설명
    public string irn_prgs_sts_cd;    //학습 성취도

    public override string ToString()
    {
        return sid + ", " + sthma_nm + ", " + step_nm + ", " + stg_nm + ", " + dtl_cn + ", " + irn_prgs_sts_cd;
    }
}