using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PodiumManager : MonoBehaviour
{
    // Référence au Canvas du podium
    public GameObject podiumCanvas;

    // Textes pour afficher les noms des pilotes/voitures
    public Text firstPlaceText;
    public Text secondPlaceText;
    public Text thirdPlaceText;

    // Délais entre les placements
    public float delayBetweenPlacements = 1.0f;

    // Référence au gestionnaire de course
    public LapManager lapManager;

    public ParticleSystem confettiSystem;

    private void Start()
    {
        // Cacher le podium au démarrage
        if (podiumCanvas != null)
            podiumCanvas.SetActive(false);
    }

    // Appelé par le LapManager quand la course est terminée
    public void ShowPodiumUI()
    {
        StartCoroutine(ShowPodiumSequence());
    }

    private IEnumerator ShowPodiumSequence()
    {
        // Attendre un peu avant d'afficher le podium
        yield return new WaitForSeconds(1.5f);

        PlayConfettiEffect();

        // Activer le canvas du podium
        podiumCanvas.SetActive(true);

        // Obtenir les résultats de la course
        List<CarIdentity> raceResults = lapManager.GetRaceResults();

        // Vérifier qu'il y a au moins un participant
        if (raceResults.Count == 0)
            yield break;

        // Placer les voitures une par une

        // 1ère place
        if (raceResults.Count >= 1)
        {
            yield return StartCoroutine(PlaceCarOnPodium(raceResults[0], firstPlaceText, 1));
            PlayPlacementEffect(1);
        }

        // 2ème place
        if (raceResults.Count >= 2)
        {
            yield return new WaitForSeconds(delayBetweenPlacements);
            yield return StartCoroutine(PlaceCarOnPodium(raceResults[1], secondPlaceText, 2));
            PlayPlacementEffect(2);
        }

        // 3ème place
        if (raceResults.Count >= 3)
        {
            yield return new WaitForSeconds(delayBetweenPlacements);
            yield return StartCoroutine(PlaceCarOnPodium(raceResults[2], thirdPlaceText, 3));
            PlayPlacementEffect(3);
        }

        // Afficher un bouton "Continuer" ou "Rejouer"
        ShowContinueButton();
    }

    private IEnumerator PlaceCarOnPodium(CarIdentity car, Text nameText, int place)
    {
        // Mettre à jour le texte uniquement
        if (nameText != null)
            nameText.text = car.carName;

        yield return new WaitForSeconds(0.5f);

    }

    private void PlayConfettiEffect()
    {
        // Assurez-vous que le système de confettis existe
        if (confettiSystem != null)
        {
            confettiSystem.Play();
        }
        else
        {
            Debug.LogWarning("Confetti system not assigned!");
        }
    }

    private void PlayPlacementEffect(int place)
    {
        // Jouer son
        AudioClip sound = null;
        switch (place)
        {
            case 1:
                sound = Resources.Load<AudioClip>("Sounds/win");  // Charge un son de 1ère place
                break;
            case 2:
                sound = Resources.Load<AudioClip>("Sounds/win");  // Charge un son de 2ème place
                break;
            case 3:
                sound = Resources.Load<AudioClip>("Sounds/win");  // Charge un son de 3ème place
                break;
        }

        // Jouer le son si disponible
        if (sound != null)
        {
            AudioSource.PlayClipAtPoint(sound, Camera.main.transform.position);
        }
    }

    private void ShowContinueButton()
    {
        // Activer le bouton pour continuer ou rejouer
        Transform continueButton = podiumCanvas.transform.Find("ContinueButton");
        if (continueButton != null)
            continueButton.gameObject.SetActive(true);
    }
}

// Script auxiliaire pour lier les positions UI aux positions 3D
public class PodiumPosition3D : MonoBehaviour
{
    public Transform worldPosition;
}