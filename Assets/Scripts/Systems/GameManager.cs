using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] Animator transitionAnim;
    static GameManager instance;
    SceneLoader loader;

    string whoToTurnOfAfterMinigame;

    Vector2 afterMinigamePos;

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
        StartCoroutine(transitionAnimRoutine());
    }

    IEnumerator transitionAnimRoutine()
    {

        //transitionAnim.SetBool("BlockUp", true);
        transitionAnim.SetTrigger("MiniGameStart");

        yield return new WaitForSeconds(1);

        loader.ChangeScene(2);
        yield return new WaitForSeconds(0.5f);
        miniGamehandler = FindAnyObjectByType<MiniGamehandler>();
        transitionAnim.SetBool("FadeOut", false);
        transitionAnim.SetBool("ShowMiniGame", true);
        miniGamehandler.SettingsForMinigame(1);

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
        // transitionAnim.SetBool("StartUp", true);
        Debug.Log("Mini Game COmplete");
        yield return new WaitForSeconds(1f);
        Debug.Log("New scene");
        transitionAnim.SetBool("HideMiniGame", false);
        loader = FindAnyObjectByType<SceneLoader>();
        loader.ChangeScene(1);
        StartCoroutine(RemoveBlockAfterSceneIsLoaded());
    }

    IEnumerator RemoveBlockAfterSceneIsLoaded()
    {
        yield return new WaitForSeconds(0.05f);
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
