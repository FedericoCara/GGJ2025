using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCollector : MonoBehaviour
{
    private PlayerStats _playerStats;

    private void Awake()
    {
        _playerStats = GetComponent<PlayerStats>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Key")
        {
            _playerStats.keys++;
            Destroy(col.gameObject);
        }
    }
}
