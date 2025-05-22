using UnityEngine;
using System.Collections;

public class MiniGamehandler : MonoBehaviour
{

    GameManager manager;

    public GameObject arena;
    [SerializeField] GameObject basicObsticle;
    [SerializeField] GameObject swordObsticle;
    [SerializeField] GameObject spawnGameobjectTransform;
    [SerializeField] GameObject enemySpawnObject;

    Vector2 spawnObjectsPos = Vector2.zero;
    public Vector2 spawnObsticlePos;

    #region Rotating Arena

    float timeUntilArenaFlip = 19;
    float timeForPreRotation;
    float maxTimeForPreRotation = 2;
    float timeForFinalRotation;
    float rotation = 0;
    float speedForRotation = -140;
    float howMuchToRotate = 0;
    [SerializeField] float howMuchSpeedForFinalRotation = -140;

    int howManyTimesRotate;

    public bool rotatingArena = false;
    bool preRotation = false;
    bool finalRotation = false;

    #endregion

    #region Spawn enemys

    float spawnEnemyTime = 3;

    int maxOfBasicObsticle = 4;
    int maxOfSwordObsticle = 3;

    bool enemySpawnCooldown = false;

    #endregion

    float timeUntilMiniGameComplete = 2;

    private void Start()
    {
        timeForPreRotation = maxTimeForPreRotation;

        manager = FindAnyObjectByType<GameManager>();

        if (manager.minigameHasStarted)
        {
            timeUntilMiniGameComplete = manager.currentTimeFroMiniGame;

            StartCoroutine(TimeUntilMinigameFinished());

        }

    }

    private void Update()
    {

        enemySpawnObject.transform.position = spawnGameobjectTransform.transform.position;
        enemySpawnObject.transform.rotation = spawnGameobjectTransform.transform.rotation;
        
        #region Rotate Arena

        if (!rotatingArena)
        {

            timeUntilArenaFlip -= Time.deltaTime;

            if (timeUntilArenaFlip <= 0)
            {

                rotatingArena = true;

                timeUntilArenaFlip = Random.Range(17, 25);

                speedForRotation = 10 / timeForPreRotation;

                preRotation = true;
            }

        }

        if (preRotation) // Rotera Arena
        {

            rotation += speedForRotation * Time.deltaTime;

            timeForPreRotation -= Time.deltaTime;

            arena.transform.rotation = Quaternion.Euler(0f, 0f, rotation);

            if (timeForPreRotation <= 0)
            {

                timeForPreRotation = maxTimeForPreRotation;

                preRotation = false;

                howManyTimesRotate = Random.Range(1, 4); // 1, 2 eller 3

                howMuchToRotate = 90 * -howManyTimesRotate - 10;

                speedForRotation = howMuchSpeedForFinalRotation;
                timeForFinalRotation = howMuchToRotate / speedForRotation;

                finalRotation = true;

            }

        }

        if (finalRotation)
        {

            rotation += speedForRotation * Time.deltaTime;

            timeForFinalRotation -= Time.deltaTime;

            arena.transform.rotation = Quaternion.Euler(0f, 0f, rotation);

            if (timeForFinalRotation < 0)
            {

                #region Correct Rotation

                float zRotation = arena.transform.eulerAngles.z;

                if (zRotation < 95 && zRotation > 85) // Den är ca 90
                {
                    arena.transform.rotation = Quaternion.Euler(0, 0, 90);
                }
                if (zRotation > 175 && zRotation < 185) // Den är ca 180
                {
                    arena.transform.rotation = Quaternion.Euler(0, 0, 180);
                }
                if (zRotation > 265 && zRotation < 275) // Den är ca -90(270 i Euler)
                {
                    arena.transform.rotation = Quaternion.Euler(0, 0, -90);
                }
                if (zRotation > 355 && zRotation < 365) // Den är ca 0(360 i Euler)
                {
                    arena.transform.rotation = Quaternion.Euler(0, 0, 0);
                }

                #endregion

                rotation = arena.transform.eulerAngles.z;

                finalRotation = false;
                rotatingArena = false;

            }
        }

        #endregion

        if (rotatingArena)
        {

            return;

        }

        StartCoroutine(EnemySpawnerCooldownRoutine());

        if (enemySpawnCooldown)
        {
            return;
        }

        StartCoroutine(SpawnEnemyAfterTimeRoutine());

    }

    #region Enemy Spawn

    void EnemySpawnerFunction()
    {

        if (maxOfBasicObsticle == 0 && maxOfSwordObsticle == 0)
        {

            return;

        }

        if (!enemySpawnCooldown)
        {
            spawnObjectsPos = enemySpawnObject.transform.position;
            spawnObjectsPos += new Vector2(Random.Range(-enemySpawnObject.transform.localScale.x / 2, enemySpawnObject.transform.localScale.x / 2), Random.Range(enemySpawnObject.transform.localScale.y / 2, -enemySpawnObject.transform.localScale.y / 2)); // Random Pos Spawn

            bool somethingWasSpawned = false;
            int NotToManyTimes = 0;

            while (NotToManyTimes < 12)
            {

                int whoToSpawn = Random.Range(0, 10);

                if (whoToSpawn <= 7 && maxOfBasicObsticle != 0)
                {

                    somethingWasSpawned = true;
                    GameObject whoISpawned = Instantiate(basicObsticle, spawnObjectsPos, Quaternion.identity);
                    whoISpawned.GetComponent<ObsticleBase>().spawnPoint = enemySpawnObject;
                    maxOfBasicObsticle--;

                }
                if (whoToSpawn > 7 && maxOfSwordObsticle != 0)
                {

                    somethingWasSpawned = true;
                    GameObject whoISpawned = Instantiate(swordObsticle, spawnObjectsPos, Quaternion.identity);
                    whoISpawned.GetComponent<ObsticleBase>().spawnPoint = enemySpawnObject;
                    maxOfSwordObsticle--;

                }

                if (somethingWasSpawned)
                {
                    break;
                }

                NotToManyTimes++;


            }

            enemySpawnCooldown = true;


        }

    }

    IEnumerator EnemySpawnerCooldownRoutine()
    {
        if (enemySpawnCooldown)
        {
            yield return new WaitForSeconds(spawnEnemyTime);

            enemySpawnCooldown = false;
        }
    }

    IEnumerator SpawnEnemyAfterTimeRoutine()
    {
        yield return new WaitForSeconds(spawnEnemyTime);

        EnemySpawnerFunction();
    }

    #endregion

    IEnumerator TimeUntilMinigameFinished()
    {
 
        yield return new WaitForSeconds(timeUntilMiniGameComplete);

        rotatingArena = true;

        manager = FindAnyObjectByType<GameManager>();

        StartCoroutine(manager.MiniGameComplete());

    }

    public void SettingsForMinigame(float timeForMinigame)
    {

        timeUntilMiniGameComplete = timeForMinigame;

        StartCoroutine(TimeUntilMinigameFinished());

    }

}
