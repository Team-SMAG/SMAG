using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class TrackView : MonoBehaviour
{
    private ScrollRect scrollRect;
    public float space = 50f;
    public GameObject uiPrefab;
    public List<RectTransform> uiObjects = new List<RectTransform>();
    public trackInfo trackList = new trackInfo();
    private Sprite[] difficultyImgList;
    public GameObject trackInfo;
    


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
        foreach(RectTransform rectTransform in uiObjects)
        {
			if (rectTransform != null)
            	Destroy(rectTransform.gameObject);
        }
    }

    /*
        float y = 0f;
        for (int i = 0; i < uiObjects.Count; i++)
        {
            uiObjects[i].anchoredPosition = new Vector2(0f, -y);
            y += uiObjects[i].sizeDelta.y + space;
        }
        scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, y);
    */

    IEnumerator GetUserList()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(Utils.AWS_SERVER_IP + "trackList"))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                trackList = JsonUtility.FromJson<trackInfo>(request.downloadHandler.text);
                foreach (trackInfoData data in trackList.resultData)
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
                        OnClick(newUi);
                    });
                    
                    uiObjects.Add(newUi);
                }
            }
        }
    }
    
    public void OnClick(RectTransform rectTransform)
    {
        SelectTrack(rectTransform);
        /*
        var objs = FindObjectOfType<DontDestroyObject>();
        if (objs.Length == 1)
        {
            DontDestroyOnLoad(trackInfo.transform.root.gameObject);
        }
        else
        {
            Destroy(trackInfo.transform.root.gameObject);
        } 
        */
        //DontDestroyOnLoad(trackInfo.transform.root.gameObject);
        SceneManager.LoadScene(Utils.SCENE_GAME_PLAY);
    }
    
    public void SelectTrack(RectTransform rectTransform)
    {
        trackInfo.transform.GetChild(0).GetComponent<Text>().text = rectTransform.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;
        trackInfo.transform.GetChild(1).GetComponent<Text>().text = rectTransform.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text;
        trackInfo.transform.GetChild(2).GetComponent<Text>().text = rectTransform.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text;
    }
}
