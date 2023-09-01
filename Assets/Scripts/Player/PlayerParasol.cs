using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerParasol : MonoBehaviour
{

    //Settings

    //References

    [Header("References")]

    [SerializeField]
    private PlayerBlowable m_PlayerBlowable;

    [SerializeField]
    protected Rigidbody m_BlowableRigidbody;

    [SerializeField]
    private Animator m_PlayerBodyAnimator;

    [SerializeField]
    private PlayerMovement m_PlayerMovement;

    //Events

    //Public Fields

    public bool IsParasolOpen { get; private set; }

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
        CloseParasol();
    }

    private void Update()
    {
        if (SettingsManager.Instance.IsSettingsUIShown)
        {
            return;
        }

        UpdateParasol();
    }

    //Public Methods

    //Private Methods

    private void UpdateParasol()
    {
        if (Input.GetButtonDown("OpenParasol") || Input.GetAxis("OpenParasol") > 0.0f)
        {
            if (!IsParasolOpen)
            {
                OpenParasol();
            }
        }
        else if (Input.GetButtonUp("OpenParasol") || Input.GetAxis("OpenParasol") == 0.0f)
        {
            if (IsParasolOpen)
            {
                CloseParasol();
            }
        }
    }

    private void OpenParasol()
    {
        IsParasolOpen = true;
        m_PlayerBodyAnimator.SetBool("isParasolOpen", true);

        if (m_PlayerBlowable.IsInWind())
        {
            m_BlowableRigidbody.AddForce(Vector3.up * 1.0f, ForceMode.Impulse);
        }
        else if(!m_PlayerMovement.IsGrounded && m_PlayerMovement.FallTime >= 0.8f)
        {
            m_BlowableRigidbody.AddForce(Vector3.up * 20.0f, ForceMode.Impulse);
        }
    }

    private void CloseParasol()
    {
        IsParasolOpen = false;
        m_PlayerBodyAnimator.SetBool("isParasolOpen", false);
    }
}