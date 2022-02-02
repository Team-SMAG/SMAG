using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Image background;
    public Sprite idleImg;
    public Sprite selectedImg;
	public bool select = false;


    
    void Awake()
    {
        background = this.transform.GetChild(0).GetComponent<Image>();
    }
    
    void Start()
    {
    }
    
    void Update()
    {
    }

    public void Selected()
    {
        background.sprite = selectedImg;
		select = true;
    }

    public void DeSelected()
    {
        background.sprite = idleImg;
		select = false;
    }

    public void OnPointerEnter(PointerEventData evenData)
    {
		if (!select)
        	background.sprite = selectedImg;
    }

    public void OnPointerExit(PointerEventData evenData)
    {
		if (!select)
        	background.sprite = idleImg;
    }
}
