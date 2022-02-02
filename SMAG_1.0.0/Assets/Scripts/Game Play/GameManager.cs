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
	    SetResolution();
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

    /* 해상도 설정하는 함수 */
    public void SetResolution()
    {
	    int setWidth = 878; // 사용자 설정 너비
	    int setHeight = 1440; // 사용자 설정 높이

	    int deviceWidth = Screen.width; // 기기 너비 저장
	    int deviceHeight = Screen.height; // 기기 높이 저장
	    Screen.sleepTimeout = SleepTimeout.NeverSleep;
	    Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution 함수 제대로 사용하기

	    if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
	    {
		    float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
		    Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
	    }
	    else // 게임의 해상도 비가 더 큰 경우
	    {
		    float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
		    Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
	    }
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
			    pause_canvas.transform.GetChild(1).GetChild(1).GetComponent<Text>().text += resultInfo.trackName;
			    //플레이 시간
			    pause_canvas.transform.GetChild(1).GetChild(3).GetComponent<Text>().text += resultInfo.playTime + "초";
			    //맞춘 개수
			    pause_canvas.transform.GetChild(1).GetChild(5).GetComponent<Text>().text += resultInfo.collectCount + "";
			    //클리어 여부
			    pause_canvas.transform.GetChild(1).GetChild(7).GetComponent<Text>().text += resultInfo.isClear + "";
			    //클리어 횟수
			    pause_canvas.transform.GetChild(1).GetChild(9).GetComponent<Text>().text += questionResult.resultData.successCount + "";

				Debug.Log(request.downloadHandler.text);
			    Debug.Log(questionResult.resultData.successCount);
			    Debug.Log(questionResult.resultCode);
			    Debug.Log(questionResult.resultMessage);
		    }
	    }
    }
}
