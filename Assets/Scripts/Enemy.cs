using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject deathVFX;
    [SerializeField] GameObject hitVFX;
    [SerializeField] int scorePerHit = 15;
    [SerializeField] int hitPoints = 5;
    ScoreBoard scoreBoard;
    GameObject parentGO;
   // [SerializeField] AudioSource deathSFX;

    void Start()
    {
        parentGO = GameObject.FindWithTag("DP");
        AddRigidBody();
        scoreBoard = FindObjectOfType<ScoreBoard>();
    }

    void AddRigidBody()
    {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void OnParticleCollision(GameObject other)
    {
        ProcessHit();
        if (hitPoints < 1)
        {
          KillEnemy();
        }
    }

    void ProcessHit()
    {
        GameObject vfx = Instantiate(hitVFX, transform.position , Quaternion.identity);
        vfx.transform.parent = parentGO.transform;
        hitPoints--;
    }

    void KillEnemy()
    {
        scoreBoard.IncreaseScore(scorePerHit);
        GameObject vfx = Instantiate(deathVFX, transform.position, Quaternion.identity);
        vfx.transform.parent = parentGO.transform;
        //deathSFX.Play();                
        Debug.Log("Hitted");
        Destroy(gameObject);
    }
}