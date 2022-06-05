using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "TD_Enemy")
        {
            collision.gameObject.GetComponent<TD_EnemyBase>().penetrated = true;
            Destroy(collision.gameObject, 1f);
            TD_GameManager.Instance.DecreaseHP(1);
            TD_GameManager.Instance.enemyCount--;
        }
    }
}
