using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float life = 100f;

    public float damage = 10f;
    public bool IsDead { get; private set; }

    public int oxigin = 100;

    [SerializeField]
    private float health = 100f;

    public float Health => health;

    public void TakeDamage(float enemyDamage)
    {
        health -= enemyDamage;
        if (health <= 0f && !IsDead)
        {
            IsDead = true;
        }
    }
}
