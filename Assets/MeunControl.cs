using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.SceneManagement;

public class MeunControl : MonoBehaviour
{
    public Button start;
    public Button quit;

    public string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //按下开始
    public void OnStart()
    {
        SceneManager.LoadScene(sceneName);
    }
    //按下退出
    public void OnQuit()
    {
        Application.Quit();
    }
}
