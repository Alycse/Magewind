using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerParasol : MonoBehaviour
{

    //Settings

    //References

    [Header("References")]

    [SerializeField]
    private MeshRenderer m_OpenedParasolMeshRenderer;

    [SerializeField]
    private MeshRenderer m_ClosedParasolMeshRenderer;

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
        UpdateParasol();
    }

    //Public Methods

    //Private Methods

    private void UpdateParasol()
    {
        if (Input.GetButtonDown("OpenParasol"))
        {
            OpenParasol();
        }
        else if (Input.GetButtonUp("OpenParasol"))
        {
            CloseParasol();
        }
    }

    private void OpenParasol()
    {
        IsParasolOpen = true;
        m_OpenedParasolMeshRenderer.enabled = true;
        m_ClosedParasolMeshRenderer.enabled = false;
    }

    private void CloseParasol()
    {
        IsParasolOpen = false;
        m_OpenedParasolMeshRenderer.enabled = false;
        m_ClosedParasolMeshRenderer.enabled = true;
    }
}