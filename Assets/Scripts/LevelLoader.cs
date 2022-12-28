using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        if (Instance = null) Instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadP1Level()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadP2Level()
    {
        SceneManager.LoadScene(2);
    }
    public void LoadTitle()
    {
        SceneManager.LoadScene(0);
    }
}
