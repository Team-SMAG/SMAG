using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankDetailSettings : MonoBehaviour
{
    public GameObject currentCanvas;
    public GameObject nextCanvas;



    void Start()
    {
    }

    void Update()
    {
    }
    
    public void Onclick()
    { 
        nextCanvas.SetActive(true);
        currentCanvas.SetActive(false);
    }
}
