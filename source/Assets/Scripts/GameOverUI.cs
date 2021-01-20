using UnityEngine;
using UnityEngine.SceneManagement;
using AudioStuff;

public class GameOverUI : MonoBehaviour {

    AudioManager audioManager;

    void Start()
    {
        audioManager = AudioManager.instance;
    }

    public void Quit()
    {
        Debug.Log("Application Quit");
        Application.Quit();
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnHover()
    {
        audioManager.PlaySound("Woosh");
    }

    public void OnClick()
    {
        audioManager.PlaySound("Click");
    }
}
