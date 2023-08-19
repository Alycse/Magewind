using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerParasol))]
public class PlayerMovement : MonoBehaviour
{
    //Settings

    [Header("Settings")]

    [SerializeField]
    private float m_MoveSpeed;

    [SerializeField]
    private float m_FallingMoveSpeed;

    [SerializeField]
    private float m_ParasolMoveSpeed;

    [SerializeField]
    private float m_JumpForce;

    [SerializeField]
    private float m_ParasolGravityScale;

    [SerializeField]
    private float m_GroundCheckLength;

    [SerializeField]
    float m_SlideAnimationVelocityOffset;

    //References

    [Header("References")]

    [SerializeField]
    private PlayerLook m_PlayerLook;

    [SerializeField]
    private Animator m_PlayerBodyAnimator;

    private Rigidbody m_PlayerRigidbody;
    private PlayerParasol m_PlayerParasol;
    private PlayerBlowable m_PlayerBlowable;

    //Events

    //Public Fields

    public Vector3 MovementInput { private set; get; }

    public bool IsGrounded { private set; get; }

    //Private Fields

    private float m_OriginalGravity;

    //Initialization Methods

    private void GetReferences()
    {
        m_PlayerRigidbody = GetComponent<Rigidbody>();
        m_PlayerParasol = GetComponent<PlayerParasol>();
        m_PlayerBlowable = GetComponent<PlayerBlowable>();
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
        m_OriginalGravity = Physics.gravity.y;
    }

    private void Update()
    {
        if (SettingsManager.Instance.IsSettingsUIShown)
        {
            return;
        }

        UpdateMovement();
        UpdateJump();
    }

    void FixedUpdate()
    {
        UpdateParasolMovement();
        UpdateParasolGravity();
    }

    //Public Methods

    //Private Methods

    private void UpdateMovement()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");

        MovementInput = cameraForward * inputVertical + cameraRight * inputHorizontal;

        IsGrounded = Physics.Raycast(transform.position, Vector3.down, m_GroundCheckLength);

        if (MovementInput != Vector3.zero)
        {
            m_PlayerLook.RotateBodyToMovement();

            if (!m_PlayerParasol.IsParasolOpen || IsGrounded)
            {
                m_PlayerRigidbody.MovePosition(m_PlayerRigidbody.position + MovementInput * m_MoveSpeed * Time.deltaTime);
            }

            if (!m_PlayerParasol.IsParasolOpen && !m_PlayerBlowable.IsInWind())
            {
                m_PlayerRigidbody.velocity = new Vector3(0.0f, m_PlayerRigidbody.velocity.y, 0.0f);
            }
        }

        if (MovementInput.x != 0.0f || MovementInput.z != 0.0f
            || Mathf.Abs(m_PlayerRigidbody.velocity.x) >= m_SlideAnimationVelocityOffset || Mathf.Abs(m_PlayerRigidbody.velocity.z) >= m_SlideAnimationVelocityOffset)
        {
            m_PlayerBodyAnimator.SetBool("isRunning", true);
            m_PlayerLook.RotateBodyToMovement();
        }
        else if (Mathf.Abs(m_PlayerRigidbody.velocity.x) < m_SlideAnimationVelocityOffset && Mathf.Abs(m_PlayerRigidbody.velocity.z) < m_SlideAnimationVelocityOffset)
        {
            m_PlayerBodyAnimator.SetBool("isRunning", false);
        }

        if(Mathf.Abs(m_PlayerRigidbody.velocity.y) >= 0.01f || !IsGrounded)
        {
            m_PlayerBodyAnimator.SetBool("inAir", true);
        }
        else
        {
            m_PlayerBodyAnimator.SetBool("inAir", false);
        }
    }

    private void UpdateJump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded)
        {
            Jump();
        }
    }

    private void UpdateParasolMovement()
    {
        if (MovementInput != Vector3.zero)
        {
            if (m_PlayerParasol.IsParasolOpen && !IsGrounded)
            {
                m_PlayerRigidbody.AddForce(MovementInput * m_ParasolMoveSpeed, ForceMode.Acceleration);
            }
        }
    }

    private void UpdateParasolGravity()
    {
        if (!m_PlayerBlowable.IsInWind())
        {
            float gravityForce = m_PlayerParasol.IsParasolOpen ? m_OriginalGravity * m_ParasolGravityScale : m_OriginalGravity;
            m_PlayerRigidbody.AddForce(new Vector3(0, gravityForce, 0), ForceMode.Acceleration);
        }
        else if (!m_PlayerParasol.IsParasolOpen)
        {
            m_PlayerRigidbody.AddForce(new Vector3(0, m_OriginalGravity, 0), ForceMode.Acceleration);
        }
    }

    private void Jump()
    {
        m_PlayerRigidbody.velocity = new Vector3(m_PlayerRigidbody.velocity.x, 0.0f, m_PlayerRigidbody.velocity.z);
        m_PlayerRigidbody.AddForce(Vector3.up * m_JumpForce, ForceMode.Impulse);
    }
}