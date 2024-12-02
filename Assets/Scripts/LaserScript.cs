using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LaserScript : MonoBehaviour
{
    [SerializeField] Transform weapon;
    [SerializeField] float angleAdjust = 90f;
    Transform player;


    [SerializeField] float range = 15f;

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Theres no GO with 'Player' Tag.");
        }
    }
    private void Update()
    {
        AimWeapon();
    }

    void AimWeapon()
    {
        if (player == null)
        {
            Debug.Log("No targets");
            return;
        }

        float targetDistance = Vector3.Distance(transform.position, player.position);        

        if (targetDistance < range)
        {
            Vector3 directionToPlayer = (player.position - weapon.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

            // Corrige la rotación (ajusta los valores según sea necesario)
            lookRotation *= Quaternion.Euler(angleAdjust, 0f, 0f);

            weapon.rotation = lookRotation;

            Debug.Log("I should shoot");
        }
        else
        {
            Debug.Log("Can't reach them!");
        }
    }
}