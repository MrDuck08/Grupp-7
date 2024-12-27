using UnityEngine;

public class BasicObsticle : ObsticleBase
{

    Rigidbody2D rb2D;

    public override void Start()
    {
        base.Start();

        rb2D = GetComponent<Rigidbody2D>();

    }

    public override void Update()
    {
        base.Update();

        rb2D.linearVelocity = -transform.up * 4000 * Time.deltaTime;

    }
}
