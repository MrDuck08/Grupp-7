using UnityEngine;
using System.Collections;

public class MiniGamehandler : MonoBehaviour
{

    public GameObject arena;
    [SerializeField] GameObject basicObsticle;
    [SerializeField] GameObject swordObsticle;
    [SerializeField] GameObject spawnGameobject;
    [SerializeField] GameObject testObject;

    Vector2 spawnObjectsPos = Vector2.zero;
    public Vector2 spawnObsticlePos;

    float timeUntilArenaFlip = 1;
    float timeForPreRotation;
    float maxTimeForPreRotation = 2;
    float timeForFinalRotation;
    float rotation = 0;
    float speedForRotation = -140;
    [SerializeField] float howMuchSpeedForFinalRotation = -140;

    float spawnEnemyTime = 3;

    int maxOfBasicObsticle = 6;
    int maxOfSwordObsticle = 3;
    int howManyTimesRotate;
    float howMuchToRotate = 0;


    bool rotatingArena = false;
    bool preRotation = false;
    bool finalRotation = false;
    bool enemySpawnCooldown = false;

    private void Start()
    {
        timeForPreRotation = maxTimeForPreRotation;

    }

    private void Update()
    {

        testObject.transform.position = spawnGameobject.transform.position;
        testObject.transform.rotation = spawnGameobject.transform.rotation;

        #region Rotate Arena

        if (!rotatingArena)
        {

            timeUntilArenaFlip -= Time.deltaTime;

            if(timeUntilArenaFlip <= 0)
            {

                preRotation = true;
                rotatingArena = true;

                timeUntilArenaFlip = Random.Range(25, 35);
                timeUntilArenaFlip = 3;

                speedForRotation = 10 / timeForPreRotation;

            }

        }

        if(preRotation) // Rotera Arena
        {

            rotation += speedForRotation * Time.deltaTime;

            timeForPreRotation -= Time.deltaTime;

            arena.transform.rotation = Quaternion.Euler(0f, 0f, arena.transform.rotation.z + rotation);

            if(timeForPreRotation <= 0)
            {

                timeForPreRotation = maxTimeForPreRotation;

                preRotation = false;

                howManyTimesRotate = Random.Range(1, 4); // 1, 2 eller 3

                howMuchToRotate = 90 * -howManyTimesRotate - 10;

                speedForRotation = howMuchSpeedForFinalRotation;
                timeForFinalRotation = howMuchToRotate / speedForRotation;

                howMuchToRotate += arena.transform.rotation.z;

                finalRotation = true;

            }

        }

        if (finalRotation)
        {

            rotation += speedForRotation * Time.deltaTime;

            timeForFinalRotation -= Time.deltaTime;

            arena.transform.rotation = Quaternion.Euler(0f, 0f, arena.transform.rotation.z + rotation);

            if (timeForFinalRotation < 0)
            {
                rotation = arena.transform.rotation.z;
                arena.transform.rotation = Quaternion.Euler(0, 0, howMuchToRotate + 10);

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

        if(maxOfBasicObsticle == 0 && maxOfSwordObsticle == 0)
        {

            return;

        }

        if (!enemySpawnCooldown)
        {
            spawnObjectsPos = testObject.transform.position;
            spawnObjectsPos += new Vector2(Random.Range(-testObject.transform.localScale.x / 2, testObject.transform.localScale.x / 2), Random.Range(testObject.transform.localScale.y / 2, -testObject.transform.localScale.y / 2)); // Random Pos Spawn

            while (true)
            {

                int whoToSpawn = Random.Range(0, 10);

                break;

                if (whoToSpawn <= 7)
                {

                    GameObject whoISpawned = Instantiate(basicObsticle, spawnObjectsPos, Quaternion.identity);
                    whoISpawned.GetComponent<ObsticleBase>().spawnPoint = testObject;
                    maxOfBasicObsticle--;

                }
                else
                {

                    GameObject whoISpawned = Instantiate(swordObsticle, spawnObjectsPos, Quaternion.identity);
                    whoISpawned.GetComponent<ObsticleBase>().spawnPoint = testObject;
                    maxOfSwordObsticle--;

                }

                break;

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

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.blue;

        //Gizmos.DrawSphere(spawnObsticlePos, 0.5f);
        Gizmos.DrawSphere(testObject.transform.position, 0.5f);

    }
}
