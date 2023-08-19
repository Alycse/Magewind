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
    private PlayerMovement m_PlayerMovement;

    [SerializeField]
    private Transform m_PlayerBodyTransform;

    [SerializeField]
    private PlayerParasol m_PlayerParasol;

    [SerializeField]
    private Animator m_PlayerBodyAnimator;

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
        if (SettingsManager.Instance.IsSettingsUIShown)
        {
            return;
        }

        UpdateFan();
    }

    //Public Methods

    //Private Methods

    private void UpdateFan()
    {
        if(m_PlayerMovement.IsGrounded)
        {
            if (Input.GetButtonDown("ProduceWind"))
            {
                if (m_PlayerLook.IsAiming)
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
    }

    private void ProduceWind()
    {
        m_PlayerLook.RotateBodyToMovement();
        m_PlayerBodyAnimator.SetTrigger("ProduceHorizontalWind");
        m_ProduceWindSound.Play();
    }

    private void ProduceAimedWind()
    {
        ProduceWind();

        Quaternion windRotation = m_PlayerLook.transform.rotation;
        Vector3 offsetFromPlayer = m_PlayerLook.transform.forward * m_AimedWindDistanceToPlayer;
        Vector3 windPosition = transform.position + offsetFromPlayer;

        Instantiate(m_WindObject, windPosition, windRotation);
    }

    private void ProduceUpwardWind()
    {
        ProduceWind();

        Quaternion windRotation = Quaternion.Euler(-90, 0, 0);

        Vector3 playerLookForward = m_PlayerBodyTransform.transform.forward;
        playerLookForward.y = 0.0f;
        Vector3 offsetFromPlayer = playerLookForward * m_UpwardWindDistanceToPlayer;
        Vector3 windPosition = transform.position + offsetFromPlayer;
        windPosition.y += m_UpwardWindOffsetY;

        Instantiate(m_WindObject, windPosition, windRotation);
    }

    private void ProduceHorizontalWind()
    {
        ProduceWind();

        Vector3 playerLookForward = m_PlayerBodyTransform.transform.forward;
        playerLookForward.y = 0.0f;
        playerLookForward.Normalize();

        Vector3 offsetFromPlayer = playerLookForward * m_HorizontalWindDistanceToPlayer;
        Vector3 windPosition = transform.position + offsetFromPlayer;
        windPosition.y += m_HorizontalWindOffsetY;

        Quaternion windRotation = Quaternion.LookRotation(playerLookForward);

        Instantiate(m_WindObject, windPosition, windRotation);
    }

    private void HideFan()
    {
        m_FanMeshRenderer.enabled = false;
    }

}