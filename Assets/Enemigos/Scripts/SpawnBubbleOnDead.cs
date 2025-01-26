using System;
using UnityEngine;

namespace Enemigos.Scripts
{
    public class SpawnBubbleOnDead : MonoBehaviour
    {
        public EnemyStats enemyStats;
        public LootBubble lootBubble;
        private EnemyStats _enemy;

        private void Awake()
        {
            _enemy = GetComponent<EnemyStats>();
            _enemy.OnDead += SpawnBubble;
        }

        private void SpawnBubble()
        {
            Instantiate(lootBubble, transform.position, Quaternion.identity);
        }
    }
}