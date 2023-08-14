using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Wind : MonoBehaviour
{
    //Settings

    [Header("Settings")]

    [SerializeField]
    public float WindStrength;

    [SerializeField]
    private float m_BoxColliderAdjustmentSpeed;

    [SerializeField]
    private float m_WindLifeSpan;

    [SerializeField]
    private ParticleSystem m_ParticleSystem;

    //References

    private BoxCollider m_BoxCollider;

    //Events

    //Public Fields

    public Vector3 Direction
    {
        get
        {
            return transform.forward;
        }
    }

    //Private Fields

    private float m_BoxColliderTargetZSize;
    private float m_CurBoxColliderZSize;
    private float m_CurWindLifeSpan;

    //Initialization Methods

    private void GetReferences()
    {
        m_BoxCollider = GetComponent<BoxCollider>();
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
        m_BoxColliderTargetZSize = m_BoxCollider.size.z;
        m_CurBoxColliderZSize = 0.0f;
        m_CurWindLifeSpan = m_WindLifeSpan;
    }

    private void Update()
    {
        UpdateBoxColliderSizeAdjustment();
        UpdateLifeSpan();
    }

    //Public Methods

    //Private Methods

    private void UpdateBoxColliderSizeAdjustment()
    {
        m_CurBoxColliderZSize = Mathf.Clamp(m_CurBoxColliderZSize + (m_BoxColliderAdjustmentSpeed * Time.deltaTime), 0.0f, m_BoxColliderTargetZSize);

        Vector3 newBoxColliderSize = m_BoxCollider.size;
        newBoxColliderSize.z = m_CurBoxColliderZSize;
        m_BoxCollider.size = newBoxColliderSize;

        Vector3 newBoxColliderCenter = m_BoxCollider.center;
        newBoxColliderCenter.z = -m_BoxColliderTargetZSize / 2.0f + m_CurBoxColliderZSize / 2.0f;
        m_BoxCollider.center = newBoxColliderCenter;
    }

    private void UpdateLifeSpan()
    {
        if(m_CurWindLifeSpan > 0.0f)
        {
            m_CurWindLifeSpan -= Time.deltaTime;
        }
        else
        {
            DisableWind();
        }
    }

    private void DisableWind()
    {
        m_BoxCollider.enabled = false;
        m_ParticleSystem.Stop();
    }

}