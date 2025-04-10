using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public PlayerControls playerControls;
    public AIControls[] aiControls;
    public LapManager lapTracker;
    public TricolorLights tricolorLights;
    public AudioSource audioSource;
    public AudioClip lowBeep;
    public AudioClip highBeep;

    void Awake()
    {
        StartGame();
    }
    public void StartGame()
    {
        FreezePlayers(true);
        StartCoroutine("Countdown");
    }
    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(1);

        Debug.Log("3");
        tricolorLights.SetProgress(1);
        audioSource.PlayOneShot(lowBeep);  // Son grave
        yield return new WaitForSeconds(1);

        Debug.Log("2");
        tricolorLights.SetProgress(2);
        audioSource.PlayOneShot(lowBeep);  // Son grave
        yield return new WaitForSeconds(1);

        Debug.Log("1");
        tricolorLights.SetProgress(3);
        audioSource.PlayOneShot(lowBeep);  // Son grave
        yield return new WaitForSeconds(1);

        Debug.Log("GO");
        tricolorLights.SetProgress(4);
        audioSource.PlayOneShot(highBeep); // Son aigu
        StartRacing();

        yield return new WaitForSeconds(2f);
        tricolorLights.SetAllLightsOff();
    }

    public void StartRacing()
    {
        FreezePlayers(false);
    }
    void FreezePlayers(bool freeze)
    {
        // Désactiver le contrôle du joueur
        playerControls.enabled = !freeze;

        // Désactiver le contrôle des IA
        foreach (AIControls ai in aiControls)
        {
            ai.enabled = !freeze;
        }
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(3);
    }
}