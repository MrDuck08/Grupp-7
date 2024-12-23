using Unity.VisualScripting;
using UnityEngine;

public class ObsticleSword : ObsticleBase
{

    float chargeSpeed = 0.2f;
    float speed = 50;
    float distanceToGo;
    float timeforBack = 2;
    float acceration;

    bool startChargeUp;
    bool startAttack; 

    Vector3 whereToGo;

    public override void Start()
    {
        base.Start();

        startChargeUp = true;
        startAttack = false;

        distanceToGo = Vector2.Distance(gameObject.transform.position, gameObject.transform.position + gameObject.transform.localScale/2);

        whereToGo = new Vector2(gameObject.transform.position.x, transform.position.y + transform.localScale.y / 2);

        chargeSpeed = distanceToGo * 2 / timeforBack;

        acceration = -chargeSpeed / timeforBack;

    }

    public override void Update()
    {
        base.Update();


        if(startChargeUp)
        {

            chargeSpeed += acceration * Time.deltaTime;

            timeforBack -= Time.deltaTime;

            transform.position = Vector2.MoveTowards(transform.position, whereToGo, chargeSpeed * Time.deltaTime);

            if (transform.position == whereToGo)
            {

                startChargeUp = false;
                startAttack = true;

               

            }

        }
     
        
        if(startAttack)
        {

            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, spawnPoint.transform.position.y - spawnPoint.transform.localScale.y), speed * Time.deltaTime);

        }

    }

}
