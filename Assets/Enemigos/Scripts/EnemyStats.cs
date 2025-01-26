using System;
using System.Collections;
using System.Collections.Generic;
using Enemigos.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyStats : MonoBehaviour
{
    public float health = 100f;  // Salud inicial del enemigo
    public float damage = 10f;   // Daño que el enemigo causa
    public float destructionDelay = 0.5f; // Tiempo antes de destruir el objeto tras morir (opcional, para animaciones o efectos)
    public SpriteRenderer enemySprite;
    public SpriteRenderer bubbledSprite;
    public BubbledConfiguration bubbledConfig;
    public event Action OnDead;

    private bool isDead;
    private Collider2D _collider2D;
    private Animator _animator;
    private float _maxHealth;
    private float _timeToDissappearLeft;
    private EnemyShooter _enemyShooter;

    private void Awake()
    {
        _collider2D = GetComponent<Collider2D>();
        _animator = GetComponentInParent<Animator>();
        _enemyShooter = GetComponent<EnemyShooter>();
        _maxHealth = health;
    }

    private void Update()
    {
        if (isDead)
        {
            _timeToDissappearLeft -= Time.deltaTime;
            if(bubbledSprite != null)
                bubbledSprite.color = new Color(1, 1, 1, _timeToDissappearLeft/bubbledConfig.timeToDissappear);

            if(_timeToDissappearLeft<=0)
                Destroy(gameObject);
            return;
        }
        health = Mathf.Clamp(health+bubbledConfig.healthPerSecond*Time.deltaTime, 0, _maxHealth);
        UpdateBubbled();
    }

    // Método para aplicar daño al enemigo
    public void TakeDamage(float damageAmount)
    {
        if (!isDead)
        {
            health -= damageAmount;
            UpdateBubbled();
            Debug.Log("El enemigo ha recibido " + damageAmount + " de daño. Salud restante: " + health);
            if(health<=0)
            {
                isDead = true;
                _collider2D.enabled = false;
                _timeToDissappearLeft = bubbledConfig.timeToDissappear;
                enemySprite.enabled = false;
                OnDead?.Invoke();
            }
        }
    }


    private void UpdateBubbled()
    {
        float maxHealthPercentage = health / _maxHealth;
        var alpha = maxHealthPercentage >= 1 ? 0 :
            maxHealthPercentage >= 0.75f ? bubbledConfig.alphas[0] :
            maxHealthPercentage >= 0.5f ? bubbledConfig.alphas[1] :
            maxHealthPercentage >= 0.25f ? bubbledConfig.alphas[2] :
            maxHealthPercentage > 0f ? bubbledConfig.alphas[3] :
            bubbledConfig.alphas[4];
            
        if (bubbledSprite != null)
        bubbledSprite.color = new Color(1, 1, 1,alpha);
        
        if(_animator != null)
            _animator.enabled = maxHealthPercentage>=1;
        if (_enemyShooter != null)
            _enemyShooter.enabled = maxHealthPercentage >= 1;
    }

    public bool IsMaxHealth()
    {
        return health >= _maxHealth;
    }

    public bool IsBubbled => health / _maxHealth < 1;
}

