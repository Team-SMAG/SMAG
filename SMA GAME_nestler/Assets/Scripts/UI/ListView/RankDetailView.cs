using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class RankDetailView : MonoBehaviour
{
    private ScrollRect scrollRect;
    public float space = 50f;
    public GameObject uiPrefabTop;
	public GameObject uiPrefabNormal;
    public List<RectTransform> uiObjects = new List<RectTransform>();
    public rankDetailInfo rankDetailList = new rankDetailInfo();
	public GameObject userInfo;
	public GameObject trackInfo;
    


    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
    }
    
    void Update()
    {
    }

    void OnEnable()
    {
        StartCoroutine(GetUserList());
    }

    void OnDisable()
    {
        foreach (RectTransform rectTransform in uiObjects)
        {
			if (rectTransform != null)
            	Destroy(rectTransform.gameObject);
        }
    }

    IEnumerator GetUserList()
    {
		string trackIdx = trackInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;
		string userIdx = userInfo.transform.GetChild(1).GetComponent<Text>().text;
		string uri = Utils.AWS_SERVER_IP + "rankDetail?trackIdx=" + trackIdx + "&userIdx=" + userIdx + "&size=10";
        
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                rankDetailList = JsonUtility.FromJson<rankDetailInfo>(request.downloadHandler.text);
                int count = 1;
                foreach (rankDetailUser user in rankDetailList.resultData.rankUserList) 
                {
                    GameObject uiPrefab = uiPrefabNormal;
                    if (count < 4)
                    {
                        uiPrefab = uiPrefabTop;
                    }
                    var newUi = Instantiate(uiPrefab, scrollRect.content).GetComponent<RectTransform>();
                    newUi.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = count + "위";
                    newUi.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = user.idx + "";
                    newUi.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = user.nickName;
                    newUi.transform.GetChild(3).GetComponent<Image>().sprite = userInfo.transform.GetChild(0).GetComponent<Image>().sprite;
                    uiObjects.Add(newUi);
                    count++;
                }
            }
        }
    }
}
