using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class RankView : MonoBehaviour
{
    private ScrollRect scrollRect;
    public float space = 50f;
    public GameObject uiPrefab;
    public List<RectTransform> uiObjects = new List<RectTransform>();
    public rankInfo rankList = new rankInfo();
    private Sprite[] difficultyImgList;
    public GameObject trackInfo;
    public GameObject currentCanvas;
    public GameObject nextCanvas;
    


    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        difficultyImgList = Resources.LoadAll<Sprite>("difficulty/");
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
        using (UnityWebRequest request = UnityWebRequest.Get(Utils.AWS_SERVER_IP + "rankList"))
        {
            yield return request.SendWebRequest();
            
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                rankList = JsonUtility.FromJson<rankInfo>(request.downloadHandler.text);
                foreach (rankInfoData data in rankList.resultData)
                {
                    var newUi = Instantiate(uiPrefab, scrollRect.content).GetComponent<RectTransform>();
                    string trackName = "";
                    switch (data.type)
                    {
                        case 1: 
                            trackName = "덧셈트랙";
                            break;
                        case 2:
                            trackName = "뺄셈트랙";
                            break;
                        case 3:
                            trackName = "곱셈트랙";
                            break;
                        case 4:
                            trackName = "나눗셈트랙";
                            break;
                    }

                    string difficultyName = "";
                    switch (data.difficulty)
                    {
                        case 1: 
                            difficultyName = "하";
                            break;
                        case 2:
                            difficultyName = "중";
                            break;
                        case 3:
                            difficultyName = "상";
                            break;
                    }
                    newUi.transform.GetChild(0).GetComponent<Text>().text = trackName;
                    newUi.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = data.idx + "";
                    newUi.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = data.type + "";
                    newUi.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = data.difficulty + "";
                    newUi.transform.GetChild(4).GetComponent<Image>().sprite = difficultyImgList[data.difficulty - 1];
                    newUi.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = difficultyName;
                    newUi.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(delegate()
                    {
                        selectRank(newUi);
                    });
                    
                    int count = 0;
                    foreach (rankUser user in data.rankUserList)
                    {
                        newUi.transform.GetChild(6).GetChild(count).GetChild(0).GetComponent<Text>().text = (count + 1) + "등";
                        newUi.transform.GetChild(6).GetChild(count).GetChild(1).GetComponent<Text>().text = user.nickName;
                        count++;
                        if (count >= 3)
                            break;
                    }
                    uiObjects.Add(newUi);
                }
            }
        }
    }

    public void selectRank(RectTransform g)
    { 
        trackInfo.transform.GetChild(0).GetComponent<Text>().text = g.transform.GetChild(0).GetComponent<Text>().text;//name
        trackInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;//idx
        trackInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = g.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text;//type
        trackInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = g.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text;//difficulty
        trackInfo.transform.GetChild(4).GetComponent<Image>().sprite = g.transform.GetChild(4).GetComponent<Image>().sprite;//image
        trackInfo.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = g.transform.GetChild(4).GetChild(0).GetComponent<Text>().text; //imageName
        
        
        nextCanvas.SetActive(true);
        currentCanvas.SetActive(false);
    }
}
