using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColliderManager : MonoBehaviour    
{
    [SerializeField] float loadDelay = 1f;
    [SerializeField] ParticleSystem explosion;
       
    private void OnTriggerEnter(Collider other)

    {
        StartCrashSequence();
    }

    void StartCrashSequence()
    {
        explosion.Play();
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false; 
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", loadDelay);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
    