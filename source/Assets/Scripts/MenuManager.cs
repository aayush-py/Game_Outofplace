
using UnityEngine;
using UnityEngine.SceneManagement;
using AudioStuff;
public class MenuManager : MonoBehaviour {
    private AudioManager audioManager;

    void Start()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No audiomanager found in scene");
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void QuitGame()
    {
        Debug.Log("Bye");
        Application.Quit();
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
