using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Threading;

public class Lane : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    public GameObject notePrefab;
    public Dictionary<string, short> inputs;
    public Dictionary<short, GameObject> buttons;
    List<Note> notes = new List<Note>();
    public List<double> timeStamps = new List<double>();
    int spawnIndex = 0;
    int inputIndex = 0;
    short buttonIndex = 0;
    short questionIndex = 0;    //현재 가장 데드라인에 근접한 문제 블록 인덱스
    public float pressedTime = 0.5f;
    public Sprite originalSprite;
    public Sprite pressedSprite;
	private GameObject trackInfo;
	private GameObject userInfo;
    private GameObject gameManager;    //결과화면 스크립트
	private QuestionInfo qinfo;    //결과화면 관련변수 : 현재 노트에 블록 idx 값이 없어 1로 하드코딩 상태
    private ResultInfo resultInfo;
	private QuestionList questionList;    //문제에 대한 정보를 담은 리스트
    private short questionSize;    //문제 개수



    void Awake()
    {
        trackInfo = GameObject.Find("TrackInfo");
        userInfo = GameObject.Find("UserInfo");
		gameManager = GameObject.Find("Game Manager");
        qinfo = new QuestionInfo();
        resultInfo = new ResultInfo();
		questionList = new QuestionList();

        inputs = new Dictionary<string, short>
        {
            { "A", 0 }, { "a", 0 }, { "ㅁ", 0 },
            { "S", 1 }, { "s", 1 }, { "ㄴ", 0 },
            { "D", 2 }, { "d", 2 }, { "ㅇ", 0 },
            { "F", 3 }, { "f", 3 }, { "ㄹ", 0 },
        };
        
        buttons = new Dictionary<short, GameObject>
        {
            { 0, GameObject.Find("Button 0") }, 
            { 1, GameObject.Find("Button 1") }, 
            { 2, GameObject.Find("Button 2") }, 
            { 3, GameObject.Find("Button 3") }, 
        };

        foreach (var button in buttons)
        {
            button.Value.GetComponent<SpriteRenderer>().sprite = originalSprite;
        }
    }

    void Start()
    {
		StartCoroutine(GetQuestionList());
    }
    
    void Update()
    {
        //Game Victory
        if (inputIndex == timeStamps.Count)
        {  
            GameOver((int)SongManager.GetAudioSourceTime(), inputIndex, true);
        }
        
        if (spawnIndex > 0 && questionList.resultData.Length == questionIndex)
        {
            GameOver((int)SongManager.GetAudioSourceTime(), questionIndex, true);
        }

        //Create Note
        if (spawnIndex < timeStamps.Count)
        {
            if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)
            {
                var note = Instantiate(notePrefab, transform);
                notes.Add(note.GetComponent<Note>());
                note.GetComponent<Note>().assignedTime = (float)timeStamps[spawnIndex];
                note.GetComponent<Note>().expression = questionList.resultData[spawnIndex].question;
                note.GetComponent<Note>().answers = questionList.resultData[spawnIndex].answerList;
				note.GetComponent<Note>().idx = questionList.resultData[spawnIndex].idx;
                
                int difficulty = Int32.Parse(trackInfo.transform.GetChild(2).GetComponent<Text>().text);
                if (difficulty == 1)
                {
                    note.GetComponent<Note>().gameSpeed = 5.0f;
                    SongManager.Instance.marginOfError = 1.6f;
                }
                else if (difficulty == 2)
                {
                    note.GetComponent<Note>().gameSpeed = 4.0f;
                    SongManager.Instance.marginOfError = 1.3f;
                }
                else
                {
                    note.GetComponent<Note>().gameSpeed = 3.0f;
                    SongManager.Instance.marginOfError = 1.0f;
                }
                ++spawnIndex;
            }
        }

        //Note Hit Available State
        if (inputIndex < timeStamps.Count)
        {
            double timeStamp = timeStamps[inputIndex];
            double marginOfError = SongManager.Instance.marginOfError;
            double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

            /* For Windows */
            string nowKeyInput = Input.inputString;
            if (inputs.ContainsKey(nowKeyInput))
            {
                buttonIndex = inputs[nowKeyInput];
                Invoke("PressButton" + buttonIndex.ToString(), 0);
                Invoke("UnpressButton" + buttonIndex.ToString(), pressedTime);

                if (notes.Count > inputIndex && notes[inputIndex].isEnable)
                {
                    //Incorrect Hit
                    if (questionList.resultData[questionIndex].answerList[buttonIndex] != questionList.resultData[questionIndex].answer)
                    {
                        Miss();
                        ++inputIndex;
                        ++questionIndex;
                        return;
                    }

                    //Correct Hit
                    Hit();
                    if (notes.Count > inputIndex)
                    {
                        Destroy(notes[inputIndex].gameObject);
                        ++inputIndex;
                        ++questionIndex;
                        if (questionList.resultData.Length == questionIndex)
                        {
                            GameOver((int)SongManager.GetAudioSourceTime(), questionIndex, true);
                        }
                    }
                }
            }

            /* For Mobile */
            /*
            if (Input.touchCount > 0)
            {
                switch (Input.GetTouch(0).phase)
                {
                    case TouchPhase.Began:
                        Debug.Log(Input.GetTouch(0).position);
                        //여기에 버튼의 안쪽을 클릭했으면, 해당 버튼의 인덱스 가져와서 비교
						
                        break;
                }
            }
            */

            //Miss
            if (audioTime - timeStamp > marginOfError)
            {
                Miss();
                ++inputIndex;
                ++questionIndex;
            }
        }
    }

    public void PressButton0()
    {
        buttons[0].GetComponent<SpriteRenderer>().sprite = pressedSprite;
    }
    public void PressButton1()
    {
        buttons[1].GetComponent<SpriteRenderer>().sprite = pressedSprite;
    }
    public void PressButton2()
    {
        buttons[2].GetComponent<SpriteRenderer>().sprite = pressedSprite;
    }
    public void PressButton3()
    {
        buttons[3].GetComponent<SpriteRenderer>().sprite = pressedSprite;
    }

    public void UnpressButton0()
    {
        buttons[0].GetComponent<SpriteRenderer>().sprite = originalSprite;
    }
    public void UnpressButton1()
    {
        buttons[1].GetComponent<SpriteRenderer>().sprite = originalSprite;
    }
    public void UnpressButton2()
    {
        buttons[2].GetComponent<SpriteRenderer>().sprite = originalSprite;
    }
    public void UnpressButton3()
    {
        buttons[3].GetComponent<SpriteRenderer>().sprite = originalSprite;
    }

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
            }
        }
    }

    private void Hit()
    {
        ScoreManager.Hit();
		string key = notes[questionIndex].GetComponent<Note>().idx.ToString();
		int value;
		value = 1;	
		if (qinfo.questionMap.ContainsKey(key))
			value = qinfo.questionMap[key] + 1;
		else 
            qinfo.questionMap.Add(key, value);
    }

    private void Miss()
    {
        ScoreManager.Miss();
		string key = notes[questionIndex].GetComponent<Note>().idx.ToString();
		int value;
		value = -1;	
		if (qinfo.questionMap.ContainsKey(key))
			value = qinfo.questionMap[key] - 1;
        else
		    qinfo.questionMap.Add(key, value);
        GameOver((int)SongManager.GetAudioSourceTime(), inputIndex, false);
    }

    private void GameOver(int playTime, int collectCount, bool isClear)
    {
        qinfo.trackIdx = int.Parse(trackInfo.transform.GetChild(1).GetComponent<Text>().text);
        qinfo.userIdx = int.Parse(userInfo.transform.GetChild(1).GetComponent<Text>().text);
        
        int type = Int32.Parse(trackInfo.transform.GetChild(1).GetComponent<Text>().text);
        string trackName = "";
        switch (type)
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

        int difficulty = Int32.Parse(trackInfo.transform.GetChild(2).GetComponent<Text>().text);
        string difficultyName = "";
        switch (difficulty)
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

        resultInfo.trackName = trackName + "(" + difficultyName + ")";
        resultInfo.playTime = playTime;
        resultInfo.collectCount = collectCount;
        resultInfo.isClear = isClear;
        string json = JsonConvert.SerializeObject(qinfo);
        gameManager.GetComponent<GameManager>().GetQuestionResult(json, resultInfo);
        gameObject.SetActive(false);
    }

	IEnumerator GetQuestionList()
    {
		string trackIdx = trackInfo.transform.GetChild(0).GetComponent<Text>().text;
		string userIdx = userInfo.transform.GetChild(1).GetComponent<Text>().text;
        
		string uri = Utils.AWS_SERVER_IP + "questionList?trackIdx=" + trackIdx + "&userIdx=" + userIdx;
        Debug.Log(uri);
	    using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                questionList = JsonUtility.FromJson<QuestionList>(request.downloadHandler.text);
            }
        }
    }
}
