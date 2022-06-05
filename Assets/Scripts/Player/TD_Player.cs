using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TD_Player : MonoBehaviour
{
    public float playerHP;
    public float playerMaxHP;
    public float playerMP;
    public float playerMaxMP;
    public float playerMPRegen;
    //==================================================//
    private float regenTick = 1f;
    private float lastTickTime;

    // Start is called before the first frame update
    void Start()
    {
        playerHP = playerMaxHP;
        playerMP = playerMaxMP / 4;
        lastTickTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > lastTickTime + regenTick)
        {
            if(playerMP + playerMPRegen < playerMaxMP)
            {
                playerMP += playerMPRegen;
            }
            else
            {
                playerMP = playerMaxMP;
            }

            lastTickTime = Time.time;
        }
    }
}
