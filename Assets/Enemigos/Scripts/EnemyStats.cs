using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float health = 100f;  // Salud inicial del enemigo
    public float damage = 10f;   // Daño que el enemigo causa
    public float destructionDelay = 0.5f; // Tiempo antes de destruir el objeto tras morir (opcional, para animaciones o efectos)

    private bool isDead = false;

    void Start()
    {
        
    }

    void Update()
    {
        // Verificar si la vida del enemigo es menor o igual a 0
        if (health <= 0f && !isDead)
        {
            isDead = true;
        Destroy(gameObject, destructionDelay);
        }
    }

    // Método para aplicar daño al enemigo
    public void TakeDamage(float damageAmount)
    {
        if (!isDead)
        {
            health -= damageAmount;
            Debug.Log("El enemigo ha recibido " + damageAmount + " de daño. Salud restante: " + health);
        }
    }
}

