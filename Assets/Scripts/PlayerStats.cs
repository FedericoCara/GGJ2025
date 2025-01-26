using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerStats : MonoBehaviour
{
    public float life = 100f;

    public float maxLife = 100f;

    public float damage = 10f;
    public bool IsDead { get; private set; }

    public int oxigin = 100;
    public Sprite[] cascoEstados;

    public SpriteRenderer casco;

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
        UpdateCascoState();
    }

    public void UpdateCascoState() {
    if (cascoEstados.Length > 0) {
        float healthPercentage = Mathf.Clamp01(health / maxLife);

        int stateIndex = Mathf.FloorToInt((1 - healthPercentage) * 4); // Cuatro rangos: 0-25%, 25-50%, 50-75%, 75-100%

        stateIndex = Mathf.Clamp(stateIndex, 0, cascoEstados.Length - 1);

        casco.sprite = cascoEstados[stateIndex];
    }
}

}

