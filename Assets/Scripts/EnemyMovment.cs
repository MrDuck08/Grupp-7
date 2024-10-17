using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    public float speed;
    public float lineOfSite;
    public float attackRange;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        //float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        //if (distanceFromPlayer < lineOfSite && distanceFromPlayer > attackRange)
        //{
        //    transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
        //}
        //else if (distanceFromPlayer <= attackRange)
        //{

        //}
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSite);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
