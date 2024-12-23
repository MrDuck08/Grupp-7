using UnityEngine;
using System.Collections;

public class MiniGamehandler : MonoBehaviour
{

    public GameObject arena;
    [SerializeField] GameObject basicObsticle;
    [SerializeField] GameObject swordObsticle;

    public GameObject testObject;

    Vector2 spawnObjectsPos = Vector2.zero;
    public Vector2 spawnObsticlePos;

    float timeUntilArenaFlip = 1;
    float timeForPreRotation = 2;
    float timeForFinalRotation = 2;
    float rotation = 0;
    float speedForRotation = -140;
    [SerializeField] float howMuchSpeedForFinalRotation = -140;

    float spawnEnemyTime = 3;

    int maxOfBasicObsticle = 8;
    int maxOfSwordObsticle = 3;
    int howManyTimesRotate;
    int howMuchToRotate = 0;


    bool rotatingArena = false;
    bool preRotation = false;
    bool finalRotation = false;
    bool enemySpawnCooldown = false;

    private void Update()
    {
        //testObject.transform.position = new Vector2(arena.transform.localScale.x / 2, arena.transform.localScale.y / 2 + 1);

        if (!rotatingArena)
        {

            timeUntilArenaFlip -= Time.deltaTime;

            if(timeUntilArenaFlip <= 0)
            {

                preRotation = true;
                rotatingArena = true;

                timeUntilArenaFlip = Random.Range(25, 35);

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

            arena.transform.rotation = Quaternion.Euler(0f, 0f, arena.transform.rotation.z + rotation);

            if (timeForFinalRotation < 0)
            {

                arena.transform.rotation = Quaternion.Euler(0, 0, howMuchToRotate + 10);

               finalRotation = false;

            }
        }

        StartCoroutine(EnemySpawnerCooldownRoutine());

        if (enemySpawnCooldown)
        {
            return;
        }

        StartCoroutine(SpawnEnemyAfterTimeRoutine());

    }


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
            Debug.Log(spawnObjectsPos + " Where To Spawn");

            while (true)
            {

                int whoToSpawn = Random.Range(0, 10);

                //Debug.Log(whoToSpawn);

                if(whoToSpawn <= 7)
                {

                    Instantiate(basicObsticle, spawnObjectsPos, Quaternion.identity);
                    maxOfBasicObsticle--;

                }
                else
                {

                    Instantiate(swordObsticle, spawnObjectsPos, Quaternion.identity);
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

    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.yellow;

        Gizmos.DrawSphere(spawnObsticlePos, 1);

    }

}
