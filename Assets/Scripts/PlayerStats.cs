using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float life = 100f;

    public float damage = 10f;
    private bool isDead = false;

    public float oxigin = 100f;

    public float health = 100f;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         if (health <= 0f && !isDead)
        {
            isDead = true;
        }
        
    }
}
