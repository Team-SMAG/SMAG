using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabPanel : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public List<GameObject> contentsPanels;



    void Start()
    {
        ClickTab(0);
    }

    void Update()
    {
    }

    public void ClickTab(int id)
    {
        for (int i = 0; i < contentsPanels.Count; ++i)
        {
            if (i == id)
            {
                contentsPanels[i].SetActive(true);
                tabButtons[i].Selected();
            }
            else
            {
                contentsPanels[i].SetActive(false);
                tabButtons[i].DeSelected();
            }
        }
    }
}
