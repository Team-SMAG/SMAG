using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ApiTest : MonoBehaviour
{
	//Test Main
    void Start()
    {
     	GetAllStudyInfo("MPID0120060011959");   
	    StartCoroutine(GetStudyResult("MPID0120060011959", "F02_0_1", "mathpd28500398652414182"));    
	    StartCoroutine(GetStudyInfo("MPID0120060011959", "F02_0_1"));
    }

    void Update()
    {
    }

    void GetGameKey(WWWForm form)
    {
	    form.AddField("gameKey", Utils.GAME_KEY_AssignedByWJTB);
    }

    IEnumerator GetAllStudyInfo(string mbr_id)
    {
	    WWWForm form = new WWWForm();
	    GetGameKey(form);
	    form.AddField("mbr_id", mbr_id);

	    using (UnityWebRequest www = UnityWebRequest.Post(Utils.API_TEST_URI_AssignedByWJTB + "getAllStudyInfo", form))
		{
			yield return www.SendWebRequest();

			if (www.result != UnityWebRequest.Result.Success)
			{
				Debug.Log(www.error);
			}
			else
			{
				Debug.Log(www.downloadHandler.text);
			}
		}
    }

    IEnumerator GetStudyInfo(string mbr_id, string stg_cd)
    {
	    WWWForm form = new WWWForm();
	    GetGameKey(form);
	    form.AddField("mbr_id", mbr_id);
	    form.AddField("stg_cd", stg_cd);

	    using (UnityWebRequest www = UnityWebRequest.Post(Utils.API_TEST_URI_AssignedByWJTB + "getStudyInfo", form))
		{
			yield return www.SendWebRequest();

			if (www.result != UnityWebRequest.Result.Success)
			{
				Debug.Log(www.error);
			}
			else
			{
				Debug.Log(www.downloadHandler.text);
			}
		}
    }

    IEnumerator GetStudyResult(string mbr_id, string stg_cd, string sid)
    {
	    WWWForm form = new WWWForm();
	    GetGameKey(form);
	    form.AddField("mbr_id", mbr_id);
	    form.AddField("sid", sid);
	    form.AddField("stg_cd", stg_cd);

	    using (UnityWebRequest www = UnityWebRequest.Post(Utils.API_TEST_URI_AssignedByWJTB + "getStudyResult", form))
		{
			yield return www.SendWebRequest();

			if (www.result != UnityWebRequest.Result.Success)
			{
				Debug.Log(www.error);
			}
			else
			{
				Debug.Log(www.downloadHandler.text);
			}
		}
    }
}