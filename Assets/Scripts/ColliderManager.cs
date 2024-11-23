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
    [SerializeField] int health = 10;
    bool isDestroyed = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)

    {
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
        Invoke("ReloadLevel", loadDelay);
    }
    void OnParticleCollision(GameObject other)
    {
        if (isDestroyed) {  return; }
        health--;
        if (health <= 0)
        {
            StartCrashSequence();
            isDestroyed = true;
        }
        Debug.Log($"The health is: {health}");
    }
    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
    