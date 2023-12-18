using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private int range = 100;
    [SerializeField]
    private int speed = 100;

    public Vector3 target;

    private float ttl;

    private Vector3 direction;
    private Vector3 startPoint;

    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.position;

        if (target.x == startPoint.x || target.z == startPoint.z)
        {
            direction = transform.forward;
        }
        else
        {
            direction = new Vector3(target.x - startPoint.x, 0 , target.z - startPoint.z);
            direction = direction.normalized;
        }

        Debug.Log("Direction X: " + direction.x);
        Debug.Log("Direction Z: " + direction.z);
        ttl = range / speed;
    }

    // Update is called once per frame
    void Update()
    {
        ttl -= Time.deltaTime;

        if (ttl <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }
}
