using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadetoGame : MonoBehaviour
{
    [SerializeField] CanvasGroup menuCanvasGroup; // Canvas Group para el men�
    [SerializeField] float fadeDuration = 1f;     // Duraci�n del efecto de fade
    [SerializeField] string sceneToLoad = "SampleScene 2"; // Nombre de la escena a cargar

    void Start()
    {
        // Inicializa el fade al comienzo (opcional)
        if (menuCanvasGroup != null)
        {
            menuCanvasGroup.alpha = 1f; // Aseg�rate de que el men� est� visible al inicio
        }
    }

    public void LoadSceneWithFade()
    {
        StartCoroutine(LoadSceneAsyncWithFade());
    }

    private IEnumerator LoadSceneAsyncWithFade()
    {
        // 1. Fade-out del men�
        yield return StartCoroutine(FadeMenu(0f)); // Fade out para el men�

        // 2. Carga la nueva escena en segundo plano
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        asyncLoad.allowSceneActivation = false;

        // 3. Espera a que la escena termine de cargarse
        while (!asyncLoad.isDone)
        {
            // Revisa si la escena est� completamente cargada
            if (asyncLoad.progress >= 0.9f)
            {
                // Activa la nueva escena
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }

        // No es necesario hacer un fade-in aqu�, ya que el men� ya est� oculto.
    }

    private IEnumerator FadeMenu(float targetAlpha)
    {
        // Cambia el alpha del Canvas del men� de manera progresiva
        float startAlpha = menuCanvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            menuCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            yield return null; // Espera un frame
        }

        // Aseg�rate de que el alpha llegue al valor exacto
        menuCanvasGroup.alpha = targetAlpha;
    }
}