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
    [SerializeField] ParticleSystem elaserR2;
    [SerializeField] ParticleSystem elaserL2;
    [SerializeField] AudioSource elaserSfx;
    [SerializeField] float range;
    [SerializeField] Transform weapon;  // Punto donde el enemigo apunta/dispara


    bool isFiring = false;
    [SerializeField] float lastFireTime = -Mathf.Infinity;
    [SerializeField] float fireCooldown = 1f;

    Transform player;  // Referencia al jugador

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
        StopFiring();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            elaserL.Play();
            elaserR.Play();
        }
        AimAtPlayer();
    }

    void AimAtPlayer()
    {
        if (player == null)
        {
            return;
        }

        float targetDistance = Vector3.Distance(transform.position, player.position);

        if (targetDistance <= range)
        {
            weapon.LookAt(player);
            Debug.Log("Te tengo que mirar");

            // Controla el disparo respetando el cooldown
            if (Time.time > lastFireTime + fireCooldown)
            {
                StartFiring();
                lastFireTime = Time.time;
            }
        }
        else
        {
            StopFiring();
        }
       
    }
    void Firing()
    {// Reproduce el sonido de disparo
        if (elaserSfx != null && !elaserSfx.isPlaying)
        {
            elaserSfx.loop = false; // Solo queremos un disparo individual
            elaserSfx.Play();
        }

        // Activa las partículas
        if (elaserL != null && elaserR != null)
        {
            elaserL.Play();
            elaserR.Play();
            elaserL2.Play();
            elaserR2.Play();
        }
    }
        void StartFiring()
    {
        if (isFiring) return; // Evita que se inicie el disparo varias veces innecesariamente

        isFiring = true;

        // Activa las partículas y el sonido
        var emissionR = elaserR.emission;
        var emissionL = elaserL.emission;
        emissionR.enabled = true;
        emissionL.enabled = true;
        Firing();

        StartCoroutine(StopFiringAfterDelay());
    }
    IEnumerator StopFiringAfterDelay()
    {
        yield return new WaitForSeconds(0.5f); // Ajusta según la duración del disparo
        StopFiring();
    }
    void StopFiring()
    {
        if (!isFiring) return; // Si ya está desactivado, no hace nada

        isFiring = false;

        // Detiene las partículas
        var emissionR = elaserR.emission;
        var emissionL = elaserL.emission;
        emissionR.enabled = false;
        emissionL.enabled = false;

        // Detiene el sonido inmediatamente
        elaserSfx.Stop();
    }
    
    void AddRigidBody()
    {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void OnParticleCollision(GameObject other)
    {
        ProcessHit();
        if (hitPoints <= 0)
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