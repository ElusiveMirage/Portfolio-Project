using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingSpikes : MonoBehaviour
{
    [SerializeField] private bool trapTriggered;
    [SerializeField] private Vector3 startPoint;
    [SerializeField] private Vector3 targetPoint;
    [SerializeField] private float trapSpeed;
    [SerializeField] private float triggerInterval;
    [SerializeField] private float lastTriggerTime;
    [SerializeField] private float trapDamage;

    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.position;
        lastTriggerTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(!trapTriggered && Time.time > lastTriggerTime + triggerInterval)
        {
            transform.position += Vector3.down * trapSpeed * Time.deltaTime;

            if(transform.position.y <= targetPoint.y)
            {
                trapTriggered = true;
            }
        }
        
        if(trapTriggered)
        {
            transform.position -= Vector3.down * trapSpeed * Time.deltaTime;

            if (transform.position.y >= startPoint.y)
            {
                trapTriggered = false;
                lastTriggerTime = Time.time;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<IDamageable>().InflictDamage(trapDamage);
        }
    }
}
