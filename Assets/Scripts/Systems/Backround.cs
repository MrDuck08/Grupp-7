using UnityEngine;

public class Backround : MonoBehaviour
{
    [SerializeField] GameObject cam;
    [SerializeField] float parallaxEffect;

    private float length, startpos;


    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        // Gör så att positionen centrelas igen

        //if (temp > startpos + length)
        //{

        //    startpos += length;
        //}
        //else if (temp < startpos - length)
        //{

        //    startpos -= length;
        //}
    }
}
