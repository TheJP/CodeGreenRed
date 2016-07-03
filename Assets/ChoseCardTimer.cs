using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChoseCardTimer : MonoBehaviour
{


    Text text;

    public DraftManager draftManager;

    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

        text.text = "Timeleft: " + string.Format("{0:00}:{1:00}" ,0, draftManager.TimeLeft);

    }

}
