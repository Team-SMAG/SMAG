using System;
using System.Collections.Generic;

[Serializable]
public class studyResult
{
    public int status;
    public string message;
    public List<studyResultData> data;
}

[Serializable]
public class studyResultData
{
    public string qst_no;    //문제 번호
    public string qst_cd;    //문제 코드
    public string qst_cransr_yn;    //문제에 대한 정오답 여부
    public string qst_expl_scond;    //문제 푼 시간 (millisecond)
    public string row_dt;    //학습 이력 시간

    public override string ToString()
    {
        return qst_no + ", " + qst_cd + ", " + qst_cransr_yn + ", " + qst_expl_scond + ", " + row_dt;
    }
}