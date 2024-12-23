using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject optionsMenu;

    bool optionsActive = false;
    bool pause = false;

    SceneLoader sceneLoader;

    private void Start()
    {
        pauseMenu.SetActive(false);

        sceneLoader = FindFirstObjectByType<SceneLoader>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {

            if (optionsActive)
            {

                optionsActive = false;
                optionsMenu.SetActive(false);

                pause = true;
                pauseMenu.SetActive(true);

                return;

            }

            if (!pause)
            {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
                pause = true;
            }
            else
            {
                Time.timeScale = 1;
                pauseMenu.SetActive(false);
                pause = false;
            }
        }
    }

    public void Home()
    {
        sceneLoader.ChangeScene(0);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        pause = false;
    }

    #region Options

    public void Options()
    {
      
        optionsActive = true;
        optionsMenu.SetActive(true);

        pause = false;
        pauseMenu.SetActive(false);

    }

    #endregion

    public void goBackToPauseMenu()
    {

        optionsMenu.SetActive(false);
        optionsActive = false; //Lägg till om mer menyer


        pauseMenu.SetActive(true);
        pause = true;

    }

}
