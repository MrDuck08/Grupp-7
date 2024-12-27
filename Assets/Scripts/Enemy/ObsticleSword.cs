using Unity.VisualScripting;
using UnityEngine;

public class ObsticleSword : ObsticleBase
{

    float chargeSpeed = 0.2f;
    float timeforBack = 1.7f;
    float speedDown;
    float acceration;

    bool startChargeUp;
    bool startAttack; 

    Vector3 whereToGo;

    Rigidbody2D rb;

    public override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();

        startChargeUp = true;
        startAttack = false;

        chargeSpeed = 300;

        acceration = 100;

    }

    public override void Update()
    {
        base.Update();


        if(startChargeUp)
        {

            chargeSpeed -= acceration * Time.deltaTime;

            timeforBack -= Time.deltaTime;

            rb.linearVelocity = transform.up * chargeSpeed * Time.deltaTime;

            if (timeforBack <= 0)
            {

                startChargeUp = false;
                startAttack = true;

                speedDown = Random.Range(19000 ,20000);

            }

        }
     
        
        if(startAttack)
        {
            
            //transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, spawnPoint.transform.position.y), speed * Time.deltaTime);

            rb.linearVelocity = -transform.up * speedDown * Time.deltaTime;

        }

    }

}
