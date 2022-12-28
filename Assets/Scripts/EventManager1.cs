using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager1 : MonoBehaviour
{
    public static float GameEventTimer;
    public enum EVENTS
    {
        WATER_FLOWER,
        VACUUMING,
        LAUNDRY,
        GAME_END,
    }

    public Transform[] WayPointsForEvent0;
    public Transform[] WayPointsForEvent1;
    public Transform[] WayPointsForEvent2;
    public GameObject MomPrefab;
    public AudioClip Vacuum;
    private bool[] EventTriggered;
    private bool[] EventDone;
    private GameObject mom;
    private int index;
    private Transform[] transToUse;
    private float GetEventTriggerTime(EVENTS evt)
    {
        switch(evt)
        {
            case EVENTS.WATER_FLOWER:
                return 40;
            case EVENTS.VACUUMING:
                return 90;
            case EVENTS.LAUNDRY:
                return 150;
            case EVENTS.GAME_END:
                return 205;
        }
        return 0;
    }

    private bool GetWillMonEnterRoom(EVENTS evt)
    {
        switch(evt)
        {
            case EVENTS.VACUUMING:
                return true;
            default:
                return false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameEventTimer = 0;
        EventTriggered = new bool[Enum.GetValues( typeof( EVENTS ) ).Length];
        EventDone = new bool[Enum.GetValues( typeof( EVENTS ) ).Length];
        for(int i = 0;i<Enum.GetValues( typeof( EVENTS ) ).Length;i++)
        {
            EventTriggered[i] = false;
            EventDone[i] = false;
        }
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        GameEventTimer += Time.deltaTime;
        if (GameManager.Instance.GameEnd) return;
        for (int i = 0; i < Enum.GetValues( typeof( EVENTS ) ).Length; i++)
        {
            if(EventTriggered[i] && !EventDone[i] && MomPrefab != null)
            {
                float speed = (EVENTS)i == EVENTS.VACUUMING ? 4 : 2;
                mom.transform.position = Vector3.MoveTowards(mom.transform.position, transToUse[index].position, speed * Time.deltaTime);
                mom.transform.LookAt(transToUse[index].position);
                float dis = Vector3.Distance(mom.transform.position, transToUse[index].position);
                if (index != transToUse.Length - 1 || dis>1f) mom.GetComponent<Animator>().SetBool("IsWalk", true);
                else mom.GetComponent<Animator>().SetBool("IsWalk", false);
                if (dis <= 0.5f && index<transToUse.Length -1 )
                {
                    index++;
                }
            }
            if (EventTriggered[i] || EventDone[i]) continue;
            if (GameEventTimer >= GetEventTriggerTime((EVENTS)i))
            {
                HandleEventTriggered((EVENTS)i);
            }
        }  
    }
              
    private void HandleEventTriggered(EVENTS evt)
    {
        print("Event Triggered :" + evt.ToString());
        if (evt == EVENTS.GAME_END)
        {
            GameManager.Instance.HandleGameEndNormal();
            return;
        }
        if (MomPrefab!=null)
        {
            mom = Instantiate(MomPrefab);
            transToUse = WayPointsForEvent0;
            if (evt == EVENTS.VACUUMING)
            {
                transToUse = WayPointsForEvent1;
            }
            else if (evt == EVENTS.LAUNDRY)
            {
                transToUse = WayPointsForEvent2;
            }
            mom.GetComponent<MomEquipment>().HandleEventTriggered(evt);
            mom.transform.position = transToUse[0].position;
        }
       
        index = 0;
        EventTriggered[(int)evt] = true;
        //do some character setup, animation, etc.
        switch(evt)
        {
            case EVENTS.WATER_FLOWER:
                StartCoroutine(WaitAndHandleEventDone(EVENTS.WATER_FLOWER, 15));
                break;
            case EVENTS.VACUUMING:
                StartCoroutine(MomEnterRoomCheck(5,10));
                break;
            case EVENTS.LAUNDRY:
                StartCoroutine(WaitAndHandleEventDone(EVENTS.LAUNDRY, 15));
                break;
        }
    }

    IEnumerator MomEnterRoomCheck(float enterLength,float quitLength)
    {
        yield return new WaitForSeconds(enterLength);
        GameManager.Instance.HandleMomEnterRoomCheck();
        this.GetComponent<AudioSource>().clip = Vacuum;
        this.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(quitLength);
        GameManager.Instance.HandleMomQuitRoom();
        this.GetComponent<AudioSource>().Pause();
        HandleEventDone(EVENTS.VACUUMING);
    }

    IEnumerator WaitAndHandleEventDone(EVENTS evt, float timer)
    {
        yield return new WaitForSeconds(timer);
        HandleEventDone(evt);
       
    }

    private void HandleEventDone(EVENTS evt)
    {
        EventDone[(int)evt] = true;
        if (MomPrefab != null) Destroy(mom);
        print("Event Done :" + evt.ToString());
    }
}
