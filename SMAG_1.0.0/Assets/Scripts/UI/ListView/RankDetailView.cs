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
    [SerializeField] 
    public Sprite[] numberImg;
	public GameObject uiPrefab;
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
                int count = 0;
                foreach (rankDetailUser user in rankDetailList.resultData.rankUserList) 
                {
                    var newUi = Instantiate(uiPrefab, scrollRect.content).GetComponent<RectTransform>();
                    newUi.transform.GetChild(0).GetComponent<Text>().text = (count+1) + "등";
                    if (count < 3)
                    {
                        newUi.transform.GetComponent<Image>().sprite = numberImg[count];
                        newUi.transform.GetChild(0).GetComponent<Text>().text = "";
                    }
                    newUi.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = user.idx + "";
                    newUi.transform.GetChild(2).GetComponent<Text>().text = user.nickName;
                    uiObjects.Add(newUi);
                    count++;
                }
            }
        }
    }
}
