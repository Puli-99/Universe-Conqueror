using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject deathVFX;
    [SerializeField] GameObject hitVFX;
    [SerializeField] int scorePerHit = 15;
    [SerializeField] int hitPoints = 5;
    ScoreBoard scoreBoard;
    GameObject parentGO;
    // [SerializeField] AudioSource deathSFX;
    [SerializeField] ParticleSystem elaserR;
    [SerializeField] ParticleSystem elaserL;
    [SerializeField] AudioSource elaserSfx;

    [SerializeField] Transform weapon;  // Punto donde el enemigo apunta/dispara

    [SerializeField] float range = 15f; // Rango de disparo del enemigo
    [SerializeField] float fireCooldown = 2f; // Tiempo entre disparos

    Transform player;  // Referencia al jugador
    float lastFireTime; // Tiempo desde el último disparo

    void Start()    
    {
        parentGO = GameObject.FindWithTag("DP");
        AddRigidBody();
        scoreBoard = FindObjectOfType<ScoreBoard>();
        elaserSfx = GetComponent<AudioSource>();

        // Encuentra al jugador automáticamente (usando etiqueta "Player")
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
        if (player == null) return; // Si no hay jugador, no hace nada
        AimAtPlayer();
    }

    void AimAtPlayer()
    {
        if (player == null)
        {
            Fire(false);
            Debug.Log("No targets");
            return;
        }

        float targetDistance = Vector3.Distance(transform.position, player.position);
        Debug.Log($"Target distance: {targetDistance}, Range: {range}");

        if (targetDistance < range)
        {
            weapon.LookAt(player);
            Fire(true);
            Debug.Log("I should shoot");
        }
        else
        {
            Fire(false);
            Debug.Log("Can't reach them!");
        }
    }

   /* void TryToFire()
    {
        if (Time.time > lastFireTime + fireCooldown) // Revisa si ya pasó el tiempo de enfriamiento
       {
            Fire();
           // lastFireTime = Time.time; // Actualiza el último tiempo de disparo
        }
    }*/

        // Activa la animación o el sistema de partículas para simular un disparo
        void Fire(bool IsActive)
        {
            var emissionModuleR = elaserR.emission;
            var emissionModuleL = elaserL.emission;
            emissionModuleR.enabled = IsActive;
            emissionModuleL.enabled = IsActive;
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