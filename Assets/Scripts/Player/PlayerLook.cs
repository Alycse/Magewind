using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class PlayerLook : MonoBehaviour
{
    //Settings

    [Header("Settings")]

    [SerializeField]
    private float m_CameraClampAngle;

    [SerializeField]
    private float m_CameraDistanceX;

    [SerializeField]
    private float m_CameraDistanceXAiming;

    [SerializeField]
    private float m_CameraDistanceY;

    [SerializeField]
    private float m_CameraDistanceOffset;

    [SerializeField]
    private float m_CameraAimOffsetX;

    [SerializeField]
    private float m_CameraAimPositionChangeSpeed;

    [SerializeField]
    private float m_PlayerBodyTransformRotationSpeed;

    //References

    [Header("References")]

    [SerializeField]
    private Transform m_PlayerBodyTransform;

    [SerializeField]
    private Rigidbody m_PlayerRigidbody;

    [SerializeField]
    private PlayerMovement m_PlayerMovement;

    //Public Fields

    public bool IsAiming { get; private set; }

    //Private Fields

    private float m_CurCameraDistanceX;
    private float m_CurLookVerticalRotation;
    private float m_CurLookHorizontalRotation;
    private float m_CurrentAimOffsetX;
    private Quaternion m_TargetPlayerBodyTransformRotation;

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
        Vector3 localRotation = transform.localRotation.eulerAngles;
        m_CurLookVerticalRotation = localRotation.x;
        m_CurLookHorizontalRotation = localRotation.y;

        m_CurCameraDistanceX = m_CameraDistanceX;

        m_TargetPlayerBodyTransformRotation = Quaternion.Euler(0.0f, m_CurLookHorizontalRotation, 0.0f);

        StopAiming();
    }

    private void Update()
    {
        if(SettingsManager.Instance.IsSettingsUIShown)
        {
            return;
        }

        UpdateLook();
        UpdateRotation();
        UpdateAim();
    }

    //Public Methods

    public void RotateBodyToMovement()
    {
        Vector3 combinedMovement = m_PlayerRigidbody.velocity + m_PlayerMovement.MovementInput;

        combinedMovement.y = 0.0f;

        if (combinedMovement.magnitude > 0.1f)
        {
            m_TargetPlayerBodyTransformRotation = Quaternion.LookRotation(combinedMovement.normalized);
        }
    }

    //Private Methods

    private void UpdateLook()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float analogX = Input.GetAxis("Analog X");
        float analogY = Input.GetAxis("Analog Y");

        m_CurLookHorizontalRotation += (mouseX + analogX) * SettingsManager.Instance.AimSensitivity * Time.deltaTime;
        m_CurLookVerticalRotation -= (mouseY + analogY) * SettingsManager.Instance.AimSensitivity * Time.deltaTime;

        m_CurLookVerticalRotation = Mathf.Clamp(m_CurLookVerticalRotation, -m_CameraClampAngle, m_CameraClampAngle);

        Quaternion localRotation = Quaternion.Euler(m_CurLookVerticalRotation, m_CurLookHorizontalRotation, 0.0f);
        transform.rotation = localRotation;

        float cameraDistanceXToUse;

        if (IsAiming)
        {
            cameraDistanceXToUse = m_CameraDistanceXAiming;
        }
        else
        {
            cameraDistanceXToUse = m_CameraDistanceX;
        }

        m_CurCameraDistanceX = Mathf.Lerp(m_CurCameraDistanceX, cameraDistanceXToUse, Time.deltaTime * m_CameraAimPositionChangeSpeed);

        float distanceToBody = Mathf.Lerp(m_CurCameraDistanceX, m_CameraDistanceY, m_CurLookVerticalRotation / m_CameraClampAngle);

        float targetAimOffsetX = IsAiming ? m_CameraAimOffsetX : 0f;
        m_CurrentAimOffsetX = Mathf.Lerp(m_CurrentAimOffsetX, targetAimOffsetX, Time.deltaTime * m_CameraAimPositionChangeSpeed);
        Vector3 aimingOffset = transform.right * m_CurrentAimOffsetX;

        transform.position = m_PlayerBodyTransform.position - transform.forward * distanceToBody + Vector3.up * m_CameraDistanceOffset + aimingOffset;
    }

    private void UpdateRotation()
    {
        m_PlayerBodyTransform.rotation = Quaternion.Lerp(m_PlayerBodyTransform.rotation, m_TargetPlayerBodyTransformRotation, m_PlayerBodyTransformRotationSpeed * Time.deltaTime);
    }

    private void UpdateAim()
    {
        if (Input.GetMouseButton(1) || Input.GetAxis("Aim") > 0.0f)
        {
            StartAiming();
        }
        else if (Input.GetAxis("Aim") == 0.0f)
        {
            StopAiming();
        }
    }

    private void StartAiming()
    {
        IsAiming = true;
    }

    private void StopAiming()
    {
        IsAiming = false;
    }
}
