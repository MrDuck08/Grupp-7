using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    static GameManager instance;
    SceneLoader loader;

    string whoToTurnOfAfterMinigame;

    Vector2 afterMinigamePos;

    PlayerMovement player;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        

        loader = FindAnyObjectByType<SceneLoader>();

    }

    #region Go back and forth minigame

    public void StartMinigame(Vector2 whereToSpawnWhenGoBack, string wallToTurnOff)
    {

        afterMinigamePos = whereToSpawnWhenGoBack;
        whoToTurnOfAfterMinigame = wallToTurnOff;
        loader = FindAnyObjectByType<SceneLoader>();
        loader.ChangeScene(2);
    }

    public void MiniGameComplete()
    {
        loader = FindAnyObjectByType<SceneLoader>();
        loader.ChangeScene(1);
        StartCoroutine(RemoveBlockAfterSceneIsLoaded());


    }

    IEnumerator RemoveBlockAfterSceneIsLoaded()
    {

        yield return new WaitForSeconds(0.01f);

        player = FindAnyObjectByType<PlayerMovement>();

        player.gameObject.transform.position = afterMinigamePos;

        GameObject.Find(whoToTurnOfAfterMinigame).SetActive(false);


    }

    #endregion
}
