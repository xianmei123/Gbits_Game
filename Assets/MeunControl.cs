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

    //���¿�ʼ
    public void OnStart()
    {
        SceneManager.LoadScene(sceneName);
    }
    //�����˳�
    public void OnQuit()
    {
        Application.Quit();
    }
}
