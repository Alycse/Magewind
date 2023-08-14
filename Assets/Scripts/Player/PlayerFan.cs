using System;
using UnityEngine;

public class PlayerFan : MonoBehaviour
{

    //Settings

    [Header("Settings")]

    [SerializeField]
    private float m_AimedWindDistanceToPlayer;

    [SerializeField]
    private float m_UpwardWindDistanceToPlayer;

    [SerializeField]
    private float m_HorizontalWindDistanceToPlayer;

    [SerializeField]
    private float m_UpwardWindOffsetY;

    [SerializeField]
    private float m_HorizontalWindOffsetY;

    [SerializeField]
    private AudioSource m_ProduceWindSound;

    //References

    [Header("References")]

    [SerializeField]
    private PlayerLook m_PlayerLook;

    [SerializeField]
    private PlayerParasol m_PlayerParasol;

    [SerializeField]
    private Animator m_PlayerAnimator;

    [SerializeField]
    private MeshRenderer m_FanMeshRenderer;

    [SerializeField]
    private UnityEngine.Object m_WindObject;

    //Events

    //Public Fields

    //Private Fields

    //Initialization Methods

    private void GetReferences()
    {

    }

    private void SubscribeToEvents()
    {

    }

    //Play Methods

    private void Awake()
    {
        GetReferences();
        SubscribeToEvents();
    }

    private void Start()
    {
        HideFan();
    }

    private void Update()
    {
        UpdateFan();
    }

    //Public Methods

    //Private Methods

    private void UpdateFan()
    {
        if (Input.GetButtonDown("ProduceWind"))
        {
            if(m_PlayerLook.IsAiming)
            {
                ProduceAimedWind();
            }
            else
            {
                ProduceHorizontalWind();
            }
        }
        else if (Input.GetButtonDown("ProduceUpwardWind"))
        {
            ProduceUpwardWind();
        }
    }

    private void ProduceAimedWind()
    {
        m_PlayerLook.RotateBodyToForward();

        Quaternion windRotation = m_PlayerLook.transform.rotation;
        Vector3 offsetFromPlayer = m_PlayerLook.transform.forward * m_AimedWindDistanceToPlayer;
        Vector3 windPosition = transform.position + offsetFromPlayer;

        Instantiate(m_WindObject, windPosition, windRotation);

        m_PlayerAnimator.SetTrigger("ProduceHorizontalWind");

        m_ProduceWindSound.Play();
    }

    private void ProduceUpwardWind()
    {
        m_PlayerLook.RotateBodyToForward();

        // Set the wind rotation to face upwards.
        Quaternion windRotation = Quaternion.Euler(-90, 0, 0); // This rotation makes sure the wind faces upwards. Adjust as needed.

        // Set the position right in front of where the player is looking, based on the specified distance.
        Vector3 playerLookForward = m_PlayerLook.transform.forward;
        playerLookForward.y = 0.0f;
        Vector3 offsetFromPlayer = playerLookForward * m_UpwardWindDistanceToPlayer;
        Vector3 windPosition = transform.position + offsetFromPlayer;
        windPosition.y += m_UpwardWindOffsetY;

        // Instantiate the wind object.
        Instantiate(m_WindObject, windPosition, windRotation);

        // Play the appropriate animation for the player, if there's any specific for upward wind.
        m_PlayerAnimator.SetTrigger("ProduceUpwardWind");

        m_ProduceWindSound.Play();
    }

    private void ProduceHorizontalWind()
    {
        m_PlayerLook.RotateBodyToForward();

        // Get the horizontal forward direction where the player is looking.
        Vector3 playerLookForward = m_PlayerLook.transform.forward;
        playerLookForward.y = 0.0f; // This ensures the direction is strictly horizontal.
        playerLookForward.Normalize(); // Normalize to maintain consistent magnitude.

        // Get the wind position offset based on this direction.
        Vector3 offsetFromPlayer = playerLookForward * m_HorizontalWindDistanceToPlayer;
        Vector3 windPosition = transform.position + offsetFromPlayer;
        windPosition.y += m_HorizontalWindOffsetY;

        // As for rotation, we can leverage Unity's LookRotation method to create a rotation that looks in a certain direction.
        Quaternion windRotation = Quaternion.LookRotation(playerLookForward);

        // Instantiate the wind object.
        Instantiate(m_WindObject, windPosition, windRotation);

        // Play the appropriate animation for the player.
        m_PlayerAnimator.SetTrigger("ProduceHorizontalWind");

        m_ProduceWindSound.Play();
    }

    private void HideFan()
    {
        m_FanMeshRenderer.enabled = false;
    }

}