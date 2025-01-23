using Unity.VisualScripting;
using UnityEngine;

public class BasicObsticle : ObsticleBase
{

     float speed = 500;

    public override void Update()
    {
        base.Update();




    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (gameHandler.rotatingArena == false)
        {

            rb.linearVelocity = -transform.up * speed * Time.deltaTime;

        }

    }
}
