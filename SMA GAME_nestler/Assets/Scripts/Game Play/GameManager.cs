using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
	public GameObject mainMenu;
	public GameObject pause_canvas;
	private QuestionResult questionResult;



    void Awake()
    {
	    Screen.sleepTimeout = SleepTimeout.NeverSleep;
	    Screen.SetResolution(720, 1280, true);
    }

    void Start()
    {
	    mainMenu = GameObject.Find("Main Camera");
	    mainMenu.SetActive(false);
		mainMenu.GetComponent<AudioListener>().enabled = false;
    }

    void Update()
    {
		/*
  		if (Input.GetButtonDown("Cancel"))
		{
			Debug.Log("Pause");
			pause_canvas.SetActive(isPause);
			isPause = !isPause;
			Time.timeScale = 0f;
			if (isPause)
		 		Time.timeScale = 0.5f;
		 	SceneManager.LoadScene("Main Menu");
		}
		*/
    }

    public void OnClick()
    {
	    mainMenu.SetActive(true);
	    SceneManager.LoadScene("Main Menu");
    }

    public void GetQuestionResult(string json, ResultInfo resultInfo)
    {
	    StartCoroutine(SendQuestionInfo(json, resultInfo));
	    pause_canvas.SetActive(true);
    }

    IEnumerator SendQuestionInfo(string json, ResultInfo resultInfo)
    {
	    using (UnityWebRequest request = UnityWebRequest.Post(Utils.AWS_SERVER_IP + "questionResult", json))
	    {
		    byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
		    request.uploadHandler = new UploadHandlerRaw(jsonToSend);
		    request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
		    request.SetRequestHeader("Content-Type", "application/json");
		    yield return request.SendWebRequest();

		    if (request.result != UnityWebRequest.Result.Success)
		    {
			    Debug.Log(request.error);
		    }
		    else
		    {   
			    questionResult = JsonUtility.FromJson<QuestionResult>(request.downloadHandler.text);
			    
				//트랙 정보
			    pause_canvas.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text += resultInfo.trackName;
			    //플레이 시간
			    pause_canvas.transform.GetChild(1).GetChild(3).GetComponent<TextMeshProUGUI>().text += resultInfo.playTime + "초";
			    //맞춘 개수
			    pause_canvas.transform.GetChild(1).GetChild(5).GetComponent<TextMeshProUGUI>().text += resultInfo.collectCount + "";
			    //클리어 여부
			    pause_canvas.transform.GetChild(1).GetChild(7).GetComponent<TextMeshProUGUI>().text += resultInfo.isClear + "";
			    //클리어 횟수
			    pause_canvas.transform.GetChild(1).GetChild(9).GetComponent<TextMeshProUGUI>().text += questionResult.resultData.successCount + "";

				Debug.Log(request.downloadHandler.text);
			    Debug.Log(questionResult.resultData.successCount);
			    Debug.Log(questionResult.resultCode);
			    Debug.Log(questionResult.resultMessage);
		    }
	    }
    }
}
