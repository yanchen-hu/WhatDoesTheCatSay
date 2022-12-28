using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelfBlink : MonoBehaviour
{
    private bool shouldDecrese;
    // Start is called before the first frame update
    void Start()
    {
        shouldDecrese = false;
    }

    // Update is called once per frame
    void Update()
    {
        Color color = this.GetComponent<Image>().color;
        if (!shouldDecrese)
        {
            color.a += Time.deltaTime;
            if (color.a >= 1.0f)
            {
                shouldDecrese = true;
            }
        }
        else
        {
            color.a -= Time.deltaTime;
            if(color.a <=0)
            {
                shouldDecrese = false;
            }
        }
        this.GetComponent<Image>().color = color;
    }
}
