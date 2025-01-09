using UnityEngine;

public class BasicObsticle : ObsticleBase
{

    public override void Update()
    {
        base.Update();

        if(gameHandler.rotatingArena == false)
        {

            rb.linearVelocity = -transform.up * 4000 * Time.deltaTime;

        }


    }
}
