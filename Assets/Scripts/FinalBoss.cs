using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class FinalBoss : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Image bossSprite; // Imagen donde se mostrará el sprite del jefe
    [SerializeField] UnityEngine.UI.Image bossDialogueSprite; // Imagen donde se mostrarán los diálogos
    [Header("Dialogues")]
    [SerializeField] Sprite bossDialogueIntro;
    [SerializeField] Sprite bossDialogueResponse;
    [SerializeField] Sprite bossDialogueDefeat;
    [SerializeField] Sprite bossDialogueVictory;

    [SerializeField] Sprite playerDialogueIntro;
    [SerializeField] Sprite playerDialogueResponse;
    [SerializeField] Sprite playerDialogueDefeat;
    [SerializeField] Sprite playerDialogueVictory;

    [Header("Boss Sprites")]
    [SerializeField] Sprite bossNormal;
    [SerializeField] Sprite bossDefeated;
    [SerializeField] Sprite bossVictorious;
    [SerializeField] float delayBeforeSprite1 = 2f;

    [SerializeField] GameObject finalBossGO;
    [SerializeField] float fadeDuration = 1f;

    [SerializeField] public PlayableDirector timelineDirector;
    [SerializeField] float timeSkip = 95f;
    public float skippedTime;

    void Start()
    {
        StartCoroutine(HandleBossDialogues());
    }

    void Update()
    {
        // Detectar si el jefe ha sido derrotado
        if (finalBossGO == null)
        {
            AdvanceTimelineDynamically();
        }
    }

    void AdvanceTimelineDynamically()
    {
        float currentTimelineTime = (float)timelineDirector.time; // Tiempo actual en la Timeline
        float timeToSkip = timeSkip - currentTimelineTime; // Tiempo restante hasta el final del enfrentamiento

        if (timeToSkip > 0f)
        {
            // Avanzar al tiempo final de la pelea
            timelineDirector.time += timeToSkip;
            timelineDirector.Play();
            skippedTime = timeToSkip;
            Debug.Log($"Timeline adelantada dinámicamente {timeToSkip} segundos. Nuevo tiempo: {timelineDirector.time}.");
        }
    }

    IEnumerator HandleBossDialogues()
    {
        // Espera inicial antes de que aparezca el diálogo del jefe
        yield return new WaitForSeconds(50f);

        // Aparece el diálogo del jefe con un fade-in
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration) // Aparece el cuadro de diálogo
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            bossSprite.color = new Color(255f, 255f, 255f, alpha); // Normalizamos valores de color
            bossDialogueSprite.color = new Color(255f, 255f, 255f, 190f);
            yield return null; // Espera un frame
        }

        // Muestra los diálogos iniciales
        yield return new WaitForSeconds(delayBeforeSprite1);
        bossDialogueSprite.sprite = playerDialogueIntro;

        yield return new WaitForSeconds(delayBeforeSprite1);
        bossDialogueSprite.sprite = bossDialogueResponse;

        yield return new WaitForSeconds(delayBeforeSprite1);
        bossDialogueSprite.sprite = playerDialogueResponse;
        yield return new WaitForSeconds(delayBeforeSprite1);
        // Desvanece el cuadro de diálogo (fade-out)
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - elapsedTime / fadeDuration);
            bossDialogueSprite.color = new Color(255f, 255f, 255f, alpha);
            yield return null; // Espera un frame
        }

        // Espera hasta 40 segundos o hasta que el jefe sea derrotado
        float timer = 0f;
        while (timer < 40f)
        {
            // Verifica si el jefe fue derrotado
            if (finalBossGO == null)
            {
                // El jefe fue derrotado antes de los 40 segundos
                bossDialogueSprite.color = new Color(255f, 255f, 255f, 190f);
                bossSprite.sprite = bossDefeated;
                bossDialogueSprite.sprite = bossDialogueDefeat;
                yield return new WaitForSeconds(delayBeforeSprite1);
                bossDialogueSprite.sprite = playerDialogueVictory;
                yield break; // Sale de la corutina, ya que se resolvió el caso
            }

            timer += Time.deltaTime;
            yield return null; // Espera un frame
        }

        // Si se alcanzan los 40 segundos y el jefe no ha sido derrotado
        bossDialogueSprite.color = new Color(255f, 255f, 255f, 190f);
        bossSprite.sprite = bossVictorious;
        bossDialogueSprite.sprite = playerDialogueDefeat;
        yield return new WaitForSeconds(delayBeforeSprite1);
        bossDialogueSprite.sprite = bossDialogueVictory;       
    }
}