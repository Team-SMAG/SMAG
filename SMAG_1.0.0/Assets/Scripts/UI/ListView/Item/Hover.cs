using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Hover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject popupWindowObject;
    [SerializeField] private Sprite onCursorImg;
    [SerializeField] private Sprite offCursorImg;
    private Image backgroundImg;
    public AudioSource audioSource;


    void Start()
    {
        backgroundImg = gameObject.GetComponent<Image>();
    }

    void Update()
    {   
    }

    public void OnPointerEnter(PointerEventData evenData)
    {
        backgroundImg.sprite = onCursorImg;
    }

    public void OnPointerExit(PointerEventData evenData)
    {
        backgroundImg.sprite = offCursorImg;
    }

    public void OnClick()
    {
        audioSource.Play();
    }
}
