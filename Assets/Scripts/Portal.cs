using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal portalConnected;
    public int keysRequired;
    public Transform exitPoint;
    public TMP_Text keysRequiredTxt;

    private void Start()
    {
        keysRequiredTxt.text = keysRequired.ToString();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            var player = col.GetComponent<PlayerStats>();
            if (player.keys >= keysRequired)
            {
                player.transform.position = portalConnected.exitPoint.position;
            }
        }
    }
}
