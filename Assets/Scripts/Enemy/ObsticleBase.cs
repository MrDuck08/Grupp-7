using UnityEngine;

public class ObsticleBase : MonoBehaviour
{
    MiniGamehandler gameHandler;

    public GameObject spawnPoint;

    public virtual void Start()
    {

        gameHandler = FindAnyObjectByType<MiniGamehandler>();

        Transform spawnPointTransform = gameHandler.arena.transform.Find("SpawnPoint");

        spawnPoint = spawnPointTransform.gameObject;

    }


    public virtual void Update()
    {

        transform.rotation = spawnPoint.transform.rotation;  // G�r s� att de tittar "Ner�t"

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.tag == "ObsticleRespawn")
        {

            // Jag dividerar med 2 f�r d� f�r jag pos kanten p� den f�r jag vet att dens pos �r p� 0
            Vector2 spawnObjectsPos = spawnPoint.transform.position;
            spawnObjectsPos += new Vector2(Random.Range(-spawnPoint.transform.localScale.x / 2, spawnPoint.transform.localScale.x / 2), Random.Range(spawnPoint.transform.localScale.y / 2, -spawnPoint.transform.localScale.y / 2)); // Random Pos Spawn


            Instantiate(gameObject, spawnObjectsPos, Quaternion.identity);
            Destroy(gameObject);

        }

    }

}
