using System.Collections;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefabricado del proyectil
    public Transform firePoint;         // Punto desde donde el enemigo dispara el proyectil
    public float fireRate = 2f;         // Tiempo entre disparos (en segundos)
    public float projectileSpeed = 5f;  // Velocidad del proyectil
    public float projectileLifetime = 3f; // Tiempo antes de destruir el proyectil (en segundos)

    public float offset;

    public Vector2 direction= Vector2.right;
    private float nextFireTime = 0f;

    void Start(){
        nextFireTime = offset;
        direction.Normalize();
    }
    void Update()
    {
        // Disparar cuando el tiempo lo permita
        if (Time.time > nextFireTime)
        {
            FireProjectile();
            nextFireTime = Time.time + fireRate;
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
                Vector2 velocity = direction * projectileSpeed; 
                rb.velocity = velocity;  
            }

            // Destruir el proyectil después de un tiempo (projectileLifetime)
            Destroy(projectile, projectileLifetime);
        }
    }
}
