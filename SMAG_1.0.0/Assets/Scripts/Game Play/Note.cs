using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    double timeInstantiated;
    public float assignedTime;
    public bool isEnable = false;
    public string expression = "";
    public short[] answers;
    public short idx;
    public double gameSpeed = 4.0f;



    void Start()
    {
        
        if (SongManager.GetAudioSourceTime() < 0) 
        {
            timeInstantiated = 0.5;    //hard coding value
            return;
        }

        timeInstantiated = SongManager.GetAudioSourceTime();
    }
    
    void Update()
    {
        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * gameSpeed));
        
        //Delete Note
        if (t > 1)
        {
            Destroy(gameObject);
            return;
        }
        
        transform.localPosition = Vector3.Lerp(Vector3.up * SongManager.Instance.noteSpawnY, Vector3.up * SongManager.Instance.noteDespawnY, t); 
        GetComponent<SpriteRenderer>().enabled = true;
        transform.GetChild(0).GetComponent<TextMesh>().text = expression;
        transform.GetChild(1).GetComponent<TextMesh>().text = answers[0].ToString();
        transform.GetChild(2).GetComponent<TextMesh>().text = answers[1].ToString();
        transform.GetChild(3).GetComponent<TextMesh>().text = answers[2].ToString();
        transform.GetChild(4).GetComponent<TextMesh>().text = answers[3].ToString();
        transform.GetChild(5).GetComponent<TextMesh>().text = idx.ToString();
    }

    void OnBecameVisible()
    {
        isEnable = true;
    }
}
