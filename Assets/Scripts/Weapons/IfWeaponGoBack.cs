using System.Collections;
using UnityEngine;

public class IfWeaponGoBack : MonoBehaviour
{

    [SerializeField] Transform parentTransform;

    [SerializeField] Camera cam;

    float maxDisatnceBetweenPlayerAndSword = 1.3f;

    PlayerSword playerSword;
    PlayerAxe playerAxe;
    PlayerWeaponBase playerWeaponBase;

    Rigidbody2D rb;


    private void Start()
    {
        playerSword = FindFirstObjectByType<PlayerSword>();
        playerAxe = FindFirstObjectByType<PlayerAxe>();
        playerWeaponBase = FindFirstObjectByType<PlayerWeaponBase>();

        rb = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {

        float dis = Vector3.Distance(transform.position, parentTransform.position);

        if (dis != maxDisatnceBetweenPlayerAndSword)
        {
            dis = maxDisatnceBetweenPlayerAndSword;
        }

        transform.position = (transform.position - parentTransform.position).normalized * dis + parentTransform.position;

        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        Vector2 lookDirection = mousePos - parentTransform.GetComponent<Rigidbody2D>().position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        parentTransform.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            if (playerWeaponBase.swordActive)
            {
                playerSword = FindFirstObjectByType<PlayerSword>();

                playerSword.stayUnsheathed = false;
            }

            if(playerWeaponBase.axeActive)
            {
                playerAxe = FindFirstObjectByType<PlayerAxe>();

                playerAxe.stayUnsheathed = false;
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (playerWeaponBase.swordActive)
            {
                playerSword = FindFirstObjectByType<PlayerSword>();

                playerSword.stayUnsheathed = true;
            }

            if (playerWeaponBase.axeActive)
            {
                playerAxe = FindFirstObjectByType<PlayerAxe>();

                playerAxe.stayUnsheathed = true;
            }
        }
    }
}
