using UnityEngine;

public class WeaponBase : MonoBehaviour
{

    public WeaponState weaponType = WeaponState.Total;
    public bool stopAttacking = false;

    //EnemyHealth enemyHealth;

    public virtual void Start()
    {
        
        //enemyHealth = FindAnyObjectByType<EnemyHealth>();

    }


    public virtual void Update()
    {


    }

    public virtual void FixedUpdate()
    {
        
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Weak point")
    //    {
    //        enemyHealth.TakeDamage(2);
    //    }
    //    if (collision.gameObject.tag == "Enemy")
    //    {
    //        enemyHealth.TakeDamage(1);
    //    }
    //    if (collision.gameObject.tag == "EnemyAttack")
    //    {
    //        enemyHealth.TakeDamage(1);
    //    }
    //}

}
