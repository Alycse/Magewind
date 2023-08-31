using System.Collections;
using UnityEngine;

public class PlayerFan : MonoBehaviour
{

    //Settings

    [Header("Settings")]

    [SerializeField]
    private float m_WindCooldownInSeconds;

    [SerializeField]
    private float m_WindDelayInSeconds;

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
    private AudioSource m_ProduceWindSound, m_AimSwosh, m_HorizontalSwosh, m_UpwardSwosh;

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

    private bool m_IsProducingWind = false;

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
        if (!m_IsProducingWind && m_PlayerMovement.IsGrounded)
        {
            if (Input.GetButtonDown("ProduceWind"))
            {
                m_IsProducingWind = true;
                m_PlayerBodyAnimator.SetTrigger("ProduceHorizontalWind");
                StartCoroutine(ProduceWindWithDelay(m_PlayerLook.IsAiming ? "Aimed" : "Horizontal", m_WindDelayInSeconds));
            }
            else if (Input.GetButtonDown("ProduceUpwardWind"))
            {
                m_IsProducingWind = true;
                m_PlayerBodyAnimator.SetTrigger("ProduceUpwardWind");
                StartCoroutine(ProduceWindWithDelay("Upward", m_WindDelayInSeconds));
            }
        }
    }

    private IEnumerator ProduceWindWithDelay(string type, float delay)
    {
        yield return new WaitForSeconds(delay);

        switch (type)
        {
            case "Aimed":
                ProduceAimedWind();
                break;
            case "Upward":
                ProduceUpwardWind();
                break;
            case "Horizontal":
                ProduceHorizontalWind();
                break;
        }

        yield return new WaitForSeconds(m_WindCooldownInSeconds);

        m_IsProducingWind = false;
    }

    private void ProduceWind()
    {
        m_PlayerLook.RotateBodyToMovement();
        //m_ProduceWindSound.Play();
    }

    private void PlaySwosh(int wind)
    {

        if (wind == 1)
        {
            m_AimSwosh.Play();
        }
        else if(wind == 2)
        {
            m_UpwardSwosh.Play();
        }
        else if (wind == 3)
        {
            m_HorizontalSwosh.Play();
        }
        
    }

    private void ProduceAimedWind()
    {
        ProduceWind();
        PlaySwosh(1);

        Quaternion windRotation = m_PlayerLook.transform.rotation;
        Vector3 offsetFromPlayer = m_PlayerLook.transform.forward * m_AimedWindDistanceToPlayer;
        Vector3 windPosition = transform.position + offsetFromPlayer;

        Instantiate(m_WindObject, windPosition, windRotation);
    }

    private void ProduceUpwardWind()
    {
        ProduceWind();
        PlaySwosh(2);

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
        PlaySwosh(3);

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
        //m_FanMeshRenderer.enabled = false;
    }

}