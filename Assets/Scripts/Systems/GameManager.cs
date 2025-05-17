using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] Animator transitionAnim;
    static GameManager instance;
    SceneLoader loader;

    string whoToTurnOfAfterMinigame;

    Vector2 afterMinigamePos;

    [Header("MiniGame")]

    [SerializeField] List<float> timeForMingame = new List<float>();
    int whatnumberMinigame = 0;

    int whatSceneToReturnTo;

    public float currentTimeFroMiniGame;

    public bool minigameHasStarted = false;

    PlayerMovement player;
    MiniGamehandler miniGamehandler;

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

        GameObject tutorialWeakPoint = GameObject.Find("WeakPointTutorial");
        if(tutorialWeakPoint != null)
        {
            tutorialWeakPoint.GetComponent<Animator>().SetBool("Tutorial", true);
        }

        loader = FindAnyObjectByType<SceneLoader>();
        StartCoroutine(StartRoutine());
    }

    IEnumerator StartRoutine()
    {


        transitionAnim.SetBool("FadeIn", true);

        yield return new WaitForSeconds(1);

        transitionAnim.SetBool("FadeIn", false);
    }

    #region Go back and forth minigame

    public void StartMinigame(Vector2 whereToSpawnWhenGoBack, string wallToTurnOff)
    {
        afterMinigamePos = whereToSpawnWhenGoBack;
        whoToTurnOfAfterMinigame = wallToTurnOff;
        loader = FindAnyObjectByType<SceneLoader>();
        whatSceneToReturnTo = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(transitionAnimRoutine());
    }

    IEnumerator transitionAnimRoutine()
    {

        transitionAnim.SetTrigger("MiniGameStart");

        yield return new WaitForSeconds(1);

        loader.ChangeScene(1);
        yield return new WaitForSeconds(0.5f);
        miniGamehandler = FindAnyObjectByType<MiniGamehandler>();
        transitionAnim.SetBool("FadeOut", false);
        transitionAnim.SetBool("ShowMiniGame", true);

        minigameHasStarted = true;

        currentTimeFroMiniGame = timeForMingame[whatnumberMinigame];
        miniGamehandler.SettingsForMinigame(timeForMingame[whatnumberMinigame]);

        yield return new WaitForSeconds(1);
        transitionAnim.SetBool("ShowMiniGame", false);

    }

    IEnumerator InfoToMinigameHandler()
    {
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator MiniGameComplete()
    {

        transitionAnim.SetBool("FadeIn", false);
        transitionAnim.SetBool("HideMiniGame", true);
        whatnumberMinigame++;
        minigameHasStarted = false;

        yield return new WaitForSeconds(1f);

        transitionAnim.SetBool("HideMiniGame", false);
        loader = FindAnyObjectByType<SceneLoader>();


        loader.ChangeScene(whatSceneToReturnTo);
        StartCoroutine(RemoveBlockAfterSceneIsLoaded());
    }

    IEnumerator RemoveBlockAfterSceneIsLoaded()
    {
        yield return new WaitForSeconds(0.5f);
        transitionAnim.SetBool("ShowMiniGame", false);
        transitionAnim.SetBool("BlockDown", true);

        GameObject.Find(whoToTurnOfAfterMinigame).SetActive(false);
        Destroy(GameObject.Find(whoToTurnOfAfterMinigame));


        player = FindAnyObjectByType<PlayerMovement>();

        player.gameObject.transform.position = afterMinigamePos;

        yield return new WaitForSeconds(1);

        transitionAnim.SetBool("BlockDown", false);
        transitionAnim.SetBool("BlockUp", false);

    }

    #endregion
}
