using Doublsb.Dialog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserControl : MonoBehaviour
{
    //parameter for P1
    public DialogManager DM;
    public GameObject Screen;
    public GameObject ProcessingText;
    private Coroutine coroutine;
    // Start is called before the first frame update
    void Start()
    {
    }

    IEnumerator WaitAndOpenMonitor(float sec)
    {
        ProcessingText.SetActive(true);
        yield return new WaitForSeconds(sec);
        DM.ShowWindowFromHide();
        ProcessingText.SetActive(false);
        coroutine = null;
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.CurrentPlayer == GameManager.PLAYER_CONTROLLER.P1 && !GameManager.Instance.GameEnd)
        {
            if(Input.GetMouseButtonDown(0) && coroutine ==  null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))//发射射线(射线，射线碰撞信息，射线长度，射线会检测的层级)
                {
                    if (hit.collider.tag == "POWER")
                    {
                        if(GameManager.Instance.IsMonitorOn)
                        {
                            DM.HideWindowForNow();
                            GameManager.Instance.IsMonitorOn = false;
                            Screen.SetActive(false);
                        }
                        else
                        {
                            GameManager.Instance.IsMonitorOn = true;
                            Screen.SetActive(true);
                            coroutine = StartCoroutine(WaitAndOpenMonitor(GameManager.Instance.P1MonitorRestartWaitingTime));
                        }
                    }
                }
            }
        }
    }
}
