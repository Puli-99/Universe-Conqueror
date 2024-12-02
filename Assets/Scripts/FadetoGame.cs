using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadetoGame : MonoBehaviour
{
    [SerializeField] CanvasGroup menuCanvasGroup; // Canvas Group para el menú
    [SerializeField] float fadeDuration = 1f;     // Duración del efecto de fade
    [SerializeField] string sceneToLoad = "SampleScene 2"; // Nombre de la escena a cargar

    void Start()
    {
        // Inicializa el fade al comienzo (opcional)
        if (menuCanvasGroup != null)
        {
            menuCanvasGroup.alpha = 1f; // Asegúrate de que el menú esté visible al inicio
        }
    }

    public void LoadSceneWithFade()
    {
        StartCoroutine(LoadSceneAsyncWithFade());
    }

    private IEnumerator LoadSceneAsyncWithFade()
    {
        // 1. Fade-out del menú
        yield return StartCoroutine(FadeMenu(0f)); // Fade out para el menú

        // 2. Carga la nueva escena en segundo plano
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        asyncLoad.allowSceneActivation = false;

        // 3. Espera a que la escena termine de cargarse
        while (!asyncLoad.isDone)
        {
            // Revisa si la escena está completamente cargada
            if (asyncLoad.progress >= 0.9f)
            {
                // Activa la nueva escena
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }

        // No es necesario hacer un fade-in aquí, ya que el menú ya está oculto.
    }

    private IEnumerator FadeMenu(float targetAlpha)
    {
        // Cambia el alpha del Canvas del menú de manera progresiva
        float startAlpha = menuCanvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            menuCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            yield return null; // Espera un frame
        }

        // Asegúrate de que el alpha llegue al valor exacto
        menuCanvasGroup.alpha = targetAlpha;
    }
}