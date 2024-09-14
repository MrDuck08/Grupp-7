using UnityEngine;

public class IfWeaponGoBack : MonoBehaviour
{

    [SerializeField] Transform parentTransform;

    float maxDisatnceBetweenPlayerAndSword = 1.3f;

    PlayerSword playerSword;

    private void Start()
    {
        playerSword = FindFirstObjectByType<PlayerSword>();
    }


    private void FixedUpdate()
    {
        //rb.AddForce(transform.right * 10 * Time.deltaTime);

        Vector3 tr = parentTransform.position - transform.position;

        float dis = Vector3.Distance(transform.position, parentTransform.position);

        if (dis != maxDisatnceBetweenPlayerAndSword)
        {
            dis = maxDisatnceBetweenPlayerAndSword;
            transform.position = (transform.position - parentTransform.position).normalized * dis + parentTransform.position;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            playerSword.stayUnsheathed = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            playerSword.stayUnsheathed = true;
        }
    }
}
