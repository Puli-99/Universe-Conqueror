using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairFollow : MonoBehaviour
{
    [SerializeField] Transform player;
    float newPositionX;

    private void Update()
    {
        CrosshairUpdate();
    }

    void CrosshairUpdate()
    {
        newPositionX = player.position.x;
    }
}
