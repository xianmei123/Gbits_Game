using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIControl : MonoBehaviour
{

    public GameObject panel;

    public bool isPause;

    public string scene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        if (isPause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public  void OnPause()
    {
        if (isPause == false)
        {
            panel.SetActive(true);
            isPause = true;
        }
        else
        {
            panel.SetActive(false);
        }

    }

    public void PressReturn()
    {
        panel.SetActive(false);

        isPause =false;
    }

    public void PressTurnToMainMeun()
    {
        SceneManager.LoadScene(scene);
    }

}
