using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CarEffects : MonoBehaviour
{
    public CarMovement carMovement;
    public Rigidbody rg;

    [Header("Tire trails")]
    public TrailRenderer trailLeft;
    public TrailRenderer trailRight;
    public float minTurnForceToShowTrails = 0.4f;

    [Header("Turn wheels")]
    public Transform leftWheel;
    public Transform rightWheel;

    [Header("Sounds and sfx")]
    public AudioClip engineSfx;
    public float engineSfxVelocityPitchFactor = 0.25f;
    public float engineSfxBasePitch = 1f;
    public AudioClip[] collisionSfxs;
    public AudioClip boostSfx;


    // private vars
    private AudioSource audioSource;
    private Color breakColor;
    private float divide = 5f;
    private float minus = 1f;

    void Awake()
    {
        // init audio source for engine sfx
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = engineSfx;
        audioSource.volume = .5f;
        audioSource.loop = true;
        audioSource.spatialBlend = 1;
        audioSource.minDistance = 25;
        audioSource.spread = 360;
        audioSource.Play();
    }

    void Update()
    {
        UpdateSkidEffect();
        UpdateTurnWheelsEffect();
        UpdateEngineSfx();
}

    public void PlayBoostSound()
    {
        if (boostSfx != null)
        {
            audioSource.PlayOneShot(boostSfx);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        PlayCollisionSfx(collision);
    }

    private void PlayCollisionSfx(Collision collision)
    {
        if (!rg || !audioSource || collision==null)
        {
            // Can't play collision sfx
            return;
        }
        audioSource.pitch = Random.Range(0.85f, 1f);
        // the more the collisison is big, the more the sound velocity
        float volumeScale = (collision.relativeVelocity.magnitude / divide) - minus;
        audioSource.PlayOneShot(collisionSfxs[Random.Range(0, collisionSfxs.Length - 1)], volumeScale);
    }

    private void UpdateSkidEffect()
    {
        if (!carMovement || !trailLeft || !trailRight)
        {
            // Can't play skid effect
            return;
        }

        bool isDrifting = carMovement.GetSpeed() > 5f && Mathf.Abs(carMovement.input.x) >= minTurnForceToShowTrails;
        trailLeft.emitting = isDrifting;
        trailRight.emitting = isDrifting;
    }

    private void UpdateTurnWheelsEffect()
    {
        if (!carMovement || !leftWheel || !rightWheel)
        {
            // Can't play turn wheels effect
            return;
        }

        leftWheel.localRotation = Quaternion.Euler(leftWheel.localRotation.eulerAngles.x, 90 + carMovement.input.x * 30f, leftWheel.localRotation.eulerAngles.z);
        rightWheel.localRotation = Quaternion.Euler(rightWheel.localRotation.eulerAngles.x, 90 + carMovement.input.x * 30f, rightWheel.localRotation.eulerAngles.z);
    }

    private void UpdateEngineSfx()
    {
        if (!rg || !audioSource)
        {
            // Can't play engine sfx
            return;
        }

        audioSource.pitch = engineSfxBasePitch + rg.velocity.magnitude * engineSfxVelocityPitchFactor;
    }
}
