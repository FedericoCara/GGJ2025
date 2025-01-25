using System.Collections;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefabricado del proyectil
    public Transform firePoint;         // Punto desde donde el enemigo dispara el proyectil
    public float fireRate = 2f;         // Tiempo entre disparos (en segundos)
    public float projectileSpeed = 5f;  // Velocidad del proyectil

    private float nextFireTime = 0f;

    void Update()
    {
        // Disparar cuando el tiempo lo permita
        if (Time.time > nextFireTime)
        {
            FireProjectile();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void FireProjectile()
{
    if (projectilePrefab != null && firePoint != null)
    {
        // Crear el proyectil en el punto de disparo
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        
        // Obtener el Rigidbody2D del proyectil y asignar su velocidad
        Rigidbody2D rb = projectile.GetComponentInChildren<Rigidbody2D>();
        if (rb != null)
        {
            // Asignar la velocidad en el eje X según la dirección del enemigo y la velocidad del proyectil
            Vector2 velocity = new Vector2((transform.localScale.x > 0 ? 1 : -1) * projectileSpeed, 0f); 
            rb.velocity = velocity;  
        }
    }
}

}

