using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColliderManager : MonoBehaviour
{
    [SerializeField] float loadDelay = 1f;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] AudioClip explosionSfx;
    AudioSource audioSource;
    [SerializeField] int health = 100;
    bool isDestroyed = false;
    HealthBoard healthBoard;
    PilotExpression pilotExpression;
    private void Start()
    {
        healthBoard = FindObjectOfType<HealthBoard>();
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)

    {
        health = 0;
        StartCrashSequence();
    }

    void StartCrashSequence()
    {if (audioSource != null && !audioSource.isPlaying)
    {
        audioSource.PlayOneShot(explosionSfx);
    }
        explosion.Play();
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false; 
        GetComponent<Movement>().enabled = false;
        Invoke("LoseScreen", loadDelay);
    }
    void OnParticleCollision(GameObject other)
    {
        if (isDestroyed) {  return; }
        if (other.CompareTag("EnemyLaser"))
        {
            health--;
            HealthManager();
            pilotExpression.ShowDamage();
            if (health <= 0)
            {
                StartCrashSequence();
                isDestroyed = true;
            }
        }
    }
    void LoseScreen()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    void HealthManager()
    {
        healthBoard.DecreaseHealth(1);
    }
}    