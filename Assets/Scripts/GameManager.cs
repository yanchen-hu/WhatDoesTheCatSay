using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float P1MonitorRestartWaitingTime = 10f;
    public float P2MoveSpeed = 5;
    public bool GameEnd;
    public GameObject GameText;
    public GameObject GameEndImage;
    public float DistractTime;
    public float DistractCoolDownTime;
    public float SteerTime;
    public GameObject RoomDoor;
    private bool isMomInTheRoom;
    public enum PLAYER_CONTROLLER
    {
        NONE,
        P1,
        P2,
    }
    public PLAYER_CONTROLLER CurrentPlayer;
    public bool IsMonitorOn;
    // Start is called before the first frame update
    void Start()
    {
        if(GameText!=null) GameText.SetActive(false);
        if (GameEndImage != null) GameEndImage.SetActive(false);
         GameEnd = false;
        isMomInTheRoom = false;
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        if(SceneManager.GetActiveScene().name == "P1Scene")
        {
            CurrentPlayer = PLAYER_CONTROLLER.P1;
            IsMonitorOn = true;
        }
        else if(SceneManager.GetActiveScene().name == "P2Scene")
        {
            CurrentPlayer = PLAYER_CONTROLLER.P2;
            IsMonitorOn = false;
        }
        else
        {
            CurrentPlayer = PLAYER_CONTROLLER.NONE;
        }
    }

    public void HandleMomEnterRoomCheck()
    {
        print("Mom entered room");
        if(RoomDoor!=null)
        {
            RoomDoor.SetActive(false);
        }
        isMomInTheRoom = true;
        
    }
    public void HandleMomQuitRoom()
    {
        print("Mom quit room");
        if (RoomDoor != null)
        {
            RoomDoor.SetActive(true);
        }
        isMomInTheRoom = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameEnd) return;
        if(isMomInTheRoom)
        {
            if (IsMonitorOn)
            {
                print("game end");
                GameText.SetActive(true);
                GameText.GetComponent<Text>().text = "YOU ARE CAUGHT";
                GameEndImage.SetActive(true);
                StartCoroutine(reloadTitleScene(3));
                GameEnd = true;
            }
        }
    }

    public void HandleGameEndNormal()
    {
        GameEnd = true;
        GameText.SetActive(true);
        GameText.GetComponent<Text>().text = CurrentPlayer == PLAYER_CONTROLLER.P1?"NOW, TELL YOUR PARTNER ABOUT THE STORY...":"NOW, YOUR PARTNER SHOULD TELL YOU A STORY...";
        GameEndImage.SetActive(true);
    }
    IEnumerator reloadTitleScene(float sec)
    {
        yield return new WaitForSeconds(sec);
        this.GetComponent<LevelLoader>().LoadTitle();
    }
}
