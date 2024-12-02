using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class WinCon : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Image fadeImage; // La imagen negra para el efecto de fade
    [SerializeField] float fadeDuration = 1f; // Duración del fade
    [SerializeField] int targetSceneIndex = 3; // Índice de la escena a cargar
    [SerializeField] float delayBeforeTransition = 115f; // Tiempo antes de iniciar la transición
    [SerializeField] AudioSource musicAudioSource; // Asigna el AudioSource de la música en el inspector
    float fadeOutDuration = 1.5f; // Duración del desvanecimiento en segundos
    [SerializeField] GameObject finalBossGO;
    [SerializeField] PlayableDirector timelineDirector;
    FinalBoss finalBoss;
    float skippedTime = 0f;

    private void Start()
    {
        // Inicia la transición después del tiempo indicado
        StartCoroutine(TransitionAfterDelay());
        finalBoss = FindObjectOfType<FinalBoss>();
        timelineDirector.stopped += OnTimelineStopped;
        skippedTime = finalBoss.skippedTime;
    }

    void OnTimelineStopped(PlayableDirector director)
    {
        if (director == timelineDirector)
        {
            StartCoroutine(FadeOutMusic());
        }
    }
    private IEnumerator TransitionAfterDelay()
    {
        float adjustedLevelEndTime = delayBeforeTransition - skippedTime;
        bool musicFadedOut = false;
        // Mientras no se haya alcanzado el tiempo ajustado, espera
        while (timelineDirector.time < adjustedLevelEndTime)
        {
            if (!musicFadedOut && timelineDirector.time >= adjustedLevelEndTime - 1f)
            {
                StartCoroutine(FadeOutMusic());
                musicFadedOut = true; // Marca como iniciado el fade
            }
            yield return null; // Esperar un frame
        }

        // Comienza el fade hacia negro
        yield return StartCoroutine(FadeToBlack());

        if (finalBossGO == null)
        {
            SceneManager.LoadScene(targetSceneIndex);
        }
        else if (finalBossGO != null)
        {
            SceneManager.LoadScene(targetSceneIndex - 1);
        }
    }

    IEnumerator FadeOutMusic()
    {
        float startVolume = musicAudioSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            musicAudioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeOutDuration);
            yield return null; // Espera al siguiente frame
        }

        musicAudioSource.volume = 0f;
        musicAudioSource.Stop(); // Detener la música completamente
    }

    private IEnumerator FadeToBlack()
    {
        // Asegúrate de que la imagen esté completamente transparente al inicio
        fadeImage.color = new Color(0, 0, 0, 0);

        float elapsedTime = 0f;

        // Incrementa el alpha de la imagen gradualmente
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null; // Espera un frame
        }

        // Asegúrate de que la imagen sea completamente negra al final
        fadeImage.color = new Color(0, 0, 0, 1);
    }
}