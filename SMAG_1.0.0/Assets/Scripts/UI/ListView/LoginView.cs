using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class LoginView : MonoBehaviour
{
    private ScrollRect scrollRect;
    public float space = 50f;
    public GameObject uiPrefab;
    public List<RectTransform> uiObjects = new List<RectTransform>();
    public userInfo userList = new userInfo();
    private Sprite[] profileImgList;
    public GameObject userInfo;
    public GameObject currentCanvas;
    public GameObject nextCanvas;
    private bool check = false;
    


    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        profileImgList = Resources.LoadAll<Sprite>("profile/");
        StartCoroutine(GetUserList());
    }

    void Update()
    {
    }

    void OnEnable()
    {
    }

    void OnDisable()
    {
    }

    IEnumerator GetUserList()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(Utils.AWS_SERVER_IP + "userList"))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                userList = JsonUtility.FromJson<userInfo>(request.downloadHandler.text);
                int count = 0;
                foreach (userInfoData data in userList.resultData)
                {
                    var newUi = Instantiate(uiPrefab, scrollRect.content).GetComponent<RectTransform>();
                    newUi.transform.GetChild(0).GetComponent<Image>().sprite = profileImgList[0];
                    newUi.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = data.idx + "";
                    newUi.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = data.id + "";
                    newUi.transform.GetChild(3).GetComponent<Text>().text = data.nickName;
                    newUi.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = data.age + "";
                    newUi.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = data.gender;
                    newUi.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(delegate()
                    {
                        SelectUser(newUi);
                    });
                    uiObjects.Add(newUi);
                }
            }
        }
    }

    public void SelectUser(RectTransform rectTransform)
    {
        userInfo.transform.GetChild(0).GetComponent<Image>().sprite = rectTransform.transform.GetChild(0).GetComponent<Image>().sprite;
        userInfo.transform.GetChild(1).GetComponent<Text>().text = rectTransform.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;
        userInfo.transform.GetChild(2).GetComponent<Text>().text = rectTransform.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text;
        userInfo.transform.GetChild(3).GetComponent<Text>().text = rectTransform.transform.GetChild(3).GetComponent<Text>().text;
        userInfo.transform.GetChild(4).GetComponent<Text>().text = rectTransform.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text;
        userInfo.transform.GetChild(5).GetComponent<Text>().text = rectTransform.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text;
        nextCanvas.SetActive(true);
        currentCanvas.SetActive(false);
    }
}
