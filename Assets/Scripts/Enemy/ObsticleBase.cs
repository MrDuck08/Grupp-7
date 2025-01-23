using UnityEngine;

public class ObsticleBase : MonoBehaviour
{
    public MiniGamehandler gameHandler;

    public GameObject spawnPoint;

    public Rigidbody2D rb;
    BoxCollider2D boxCollider;
    Collider2D collider2D;

    bool doneOnce = false;

    public virtual void Start()
    {

        gameHandler = FindAnyObjectByType<MiniGamehandler>();

        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();

        transform.rotation = spawnPoint.transform.rotation;  // G�r s� att de tittar "Ner�t"

    }


    public virtual void Update()
    {

        if (gameHandler.rotatingArena)
        {

            rb.linearVelocity = Vector3.zero;

            transform.rotation = spawnPoint.transform.rotation;

            transform.parent = gameHandler.arena.transform;

            collider2D.enabled = false;

            doneOnce = false;

        }
        else
        {
            if (!doneOnce) // Så man inte behöver repetera samma sak i onödan
            {

                transform.parent = null;
                collider2D.enabled = true;

                doneOnce = true;
            }
        }

    }

    public virtual void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.tag == "ObsticleRespawn")
        {

            // Jag dividerar med 2 f�r d� f�r jag pos kanten p� den f�r jag vet att dens pos �r p� 0
            Vector2 spawnObjectsPos = spawnPoint.transform.position;
        
            
            if(spawnPoint.transform.localEulerAngles.z == 90 || spawnPoint.transform.localEulerAngles.z == 270)
            {
                // Den är på sidan, så då byter jag x och y
                spawnObjectsPos += new Vector2(Random.Range(-spawnPoint.transform.localScale.y / 2, spawnPoint.transform.localScale.y / 2), Random.Range(spawnPoint.transform.localScale.x / 2, -spawnPoint.transform.localScale.x / 2)); // Random Pos Spawn

            }
            else
            {
                // Den är rak
                spawnObjectsPos += new Vector2(Random.Range(-spawnPoint.transform.localScale.x / 2, spawnPoint.transform.localScale.x / 2), Random.Range(spawnPoint.transform.localScale.y / 2, -spawnPoint.transform.localScale.y / 2)); // Random Pos Spawn

            }

            Instantiate(gameObject, spawnObjectsPos, Quaternion.identity);
            Destroy(gameObject);

        }

    }

}
