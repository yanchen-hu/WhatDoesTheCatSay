using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Doublsb.Dialog;

public class StoryManager : MonoBehaviour
{
    public DialogManager dialogManager;
    //private const string filename = "WhatDoesTheCatSay_Data/story.txt";
    private List<DialogData> dialogTexts;

    private void LoadText()
    {
        StreamReader stm = new StreamReader(Application.dataPath+"/story.txt");

        while(!stm.EndOfStream)
        {
            string line = stm.ReadLine();
            dialogTexts.Add(new DialogData(line,"Author",null,false));
        }
        stm.Close();
    }
    // Start is called before the first frame update
    void Start()
    {
        dialogTexts = new List<DialogData>();
        LoadText();
        dialogManager.Show(dialogTexts);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
