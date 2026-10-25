using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayTutorial()
    {
        SceneManager.LoadScene("Tutorial1");
    }

    public void PlayLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void PlayLevel2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void PlayLevel3()
    {
        SceneManager.LoadScene("Level3");
    }

    public void PlayLevel4()
    {
        SceneManager.LoadScene("Level4");
    }

    public void PlayLevel5()
    {
        SceneManager.LoadScene("Level5");
    }

    public void PlayLevel6()
    {
        SceneManager.LoadScene("Level6");
    }

    public void PlayLevelEnd()
    {
        SceneManager.LoadScene("End");
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
    }

    public void QuitGame()
    {
        //Debug.Log("QUIT!");
        Application.Quit();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
