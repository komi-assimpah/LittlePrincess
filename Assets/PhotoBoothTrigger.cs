using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PhotoBoothTrigger : MonoBehaviour
{
    public GameObject photoTakenText;
    public GameObject postcardFrame;
    public GameObject uiMessage;
    public GameObject flashLight;
    public Camera studioCamera;
    public Camera playerCamera;
    public Transform photoPoseSpot;
    public GameObject player;
    public Animator playerAnimator;
    public GameObject finalPanel;
    public Image flashOverlay;
    public RawImage photoDisplay; // UI image qui affiche la capture
    public int photoWidth = 1920;
    public int photoHeight = 1080;

    private bool isPlayerNearby = false;
    private bool photoStarted = false;
    private bool isInPhotobooth = false;

    public int poseNumber = 1;

    void Update()
    {
        // Appui sur ESPACE : entrée dans le photobooth ou photo
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isPlayerNearby && !photoStarted)
            {
                photoStarted = true;
                isInPhotobooth = true;
                StartPhotoSession();
                Debug.Log("🎥 Entrée dans le photobooth");
            }
            else if (isInPhotobooth)
            {
                StartCoroutine(FlashThenShow());
                isInPhotobooth = false;
                Debug.Log("📸 Photo prise !");
            }
        }
        
        // Appui sur ECHAP : sortie du photobooth
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Echap");
            if (isInPhotobooth)
            {
                isInPhotobooth = false;
                photoStarted = false;
                playerCamera.enabled = true;
                studioCamera.enabled = false;
                postcardFrame.SetActive(false);
                playerAnimator.SetInteger("poseIndex", 0);
                // rotation 180% du player
                player.transform.rotation = Quaternion.Euler(0, 180, 0);
                Debug.Log("🚪 Sortie du photobooth");
            }
        }

        // Changement de pose
        if (Input.GetKeyDown(KeyCode.Alpha1) && isInPhotobooth)
        {
            poseNumber = 1;
            playerAnimator.SetInteger("poseIndex", poseNumber);
            Debug.Log("🎯 Pose 1 sélectionnée");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && isInPhotobooth)
        {
            poseNumber = 2;
            playerAnimator.SetInteger("poseIndex", poseNumber);
            Debug.Log("🎯 Pose 2 sélectionnée");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            uiMessage.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            uiMessage.SetActive(false);
        }
    }

    void StartPhotoSession()
    {
        MovePlayerToPoseSpot();
        playerAnimator.SetInteger("poseIndex", poseNumber);

        uiMessage.SetActive(false);

        flashLight.SetActive(true);
        Invoke("TurnOffFlash", 0.3f);

        playerCamera.enabled = false;
        studioCamera.enabled = true;
        Debug.Log(studioCamera.enabled);
        Debug.Log(playerCamera.enabled);

        postcardFrame.SetActive(true);
    }

    void TurnOffFlash()
    {
        flashLight.SetActive(false);
    }

    void MovePlayerToPoseSpot()
    {
        player.transform.position = photoPoseSpot.position;
        player.transform.rotation = photoPoseSpot.rotation;
    }

    public void TriggerFlashAndShowPanel()
    {
        StartCoroutine(FlashThenShow());
    }

    IEnumerator FlashThenShow()
    {
        // Active le flash visuel
        flashOverlay.gameObject.SetActive(true);
        flashOverlay.color = new Color(1f, 1f, 1f, 1f);

        yield return new WaitForSeconds(0.2f);

        // Capture la photo de la caméra
        yield return CapturePhoto();

        // Fondu du flash
        float duration = 0.5f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            flashOverlay.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }

        flashOverlay.gameObject.SetActive(false);

        // Affiche le panneau final
        finalPanel.SetActive(true);
    }

    IEnumerator CapturePhoto()
    {
        yield return new WaitForEndOfFrame();

        RenderTexture rt = new RenderTexture(photoWidth, photoHeight, 24);
        studioCamera.targetTexture = rt;

        Texture2D photo = new Texture2D(photoWidth, photoHeight, TextureFormat.RGB24, false);
        studioCamera.Render();
        RenderTexture.active = rt;
        photo.ReadPixels(new Rect(0, 0, photoWidth, photoHeight), 0, 0);
        photo.Apply();

        studioCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        photoDisplay.texture = photo;
    }
}
