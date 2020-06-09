using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenStudy()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings", LoadSceneMode.Single);
    }

    public void OpenAssemble()
    {
        SceneManager.LoadScene("AssembleStation", LoadSceneMode.Single);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

}
