using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject MenuCanvas;
    [SerializeField] GameObject controllerCanvas;

    bool MenuIsActive;
    bool controllerCanvasActive;

    private void Start()
    {
        MenuCanvas.SetActive(false);
        controllerCanvas.SetActive(false);
    }

    private void Update()
    {
        TutorialAnywhereTrue();
    }

    public void startAgain()
    {
        MenuCanvas.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void controllerButton()
    {
        MenuCanvas.SetActive(false);
        controllerCanvas.SetActive(true);
        controllerCanvasActive = true;
    }

    public void BackToMenu()
    {
        controllerCanvas.SetActive(false);
        MenuCanvas.SetActive(true);
        controllerCanvasActive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            MenuCanvas.SetActive(true);

            MenuIsActive = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            MenuCanvas.SetActive(false);

            MenuIsActive = false;
        }
    }

    void TutorialAnywhereTrue()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {

            if(!MenuIsActive)
            {
                MenuCanvas.SetActive(true);

                MenuIsActive = true;

                Time.timeScale = 0f;

                return;
            }

            if(MenuIsActive) 
            { 
               MenuCanvas.SetActive(false);

                MenuIsActive = false;

                Time.timeScale = 1f;
            }

            if (!MenuIsActive && controllerCanvasActive)
            {
                controllerCanvas.SetActive(false);
                MenuCanvas.SetActive(true);

                controllerCanvasActive = false;
                MenuIsActive = true;

                Time.timeScale = 0f;
            }
        }
    }
}
