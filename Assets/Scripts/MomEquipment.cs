using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomEquipment : MonoBehaviour
{
    public GameObject Vacuum;
    public GameObject Kettle;
    public GameObject Clothes;
    // Start is called before the first frame update
    void Start()
    {
        //Vacuum.SetActive(false);
        //Kettle.SetActive(false);
        //Clothes.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HandleEventTriggered(EventManager1.EVENTS evt)
    {
        switch(evt)
        {
            case EventManager1.EVENTS.WATER_FLOWER:
                Kettle.SetActive(true);
                break;
            case EventManager1.EVENTS.VACUUMING:
                Vacuum.SetActive(true);
                break;
            case EventManager1.EVENTS.LAUNDRY:
                Clothes.SetActive(true);
                break;
        }
    }
}
