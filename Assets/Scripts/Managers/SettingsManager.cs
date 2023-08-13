using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    //Singleton

    public static SettingsManager Instance { get; private set; }

    //Settings

    [Header ("Settings")]

	[SerializeField]
	private float m_DefaultMouseSensitivity;

	//References

	[Header ("References")]

    [SerializeField]
    private GameObject m_SettingsUIGameObject;

    [SerializeField]
	private Slider m_MouseSensitivitySlider;

	//Events

	//Public Fields

	public float MouseSensitivity { get; private set; }

    //Private Fields

    private bool IsSettingsUIShown;

	//Initialization Methods

	private void GetReferences ()
	{
		InitializeSingleton();
	}

    private void SubscribeToEvents ()
	{
        m_MouseSensitivitySlider.onValueChanged.AddListener(OnMouseSensitivitySliderChanged);
    }

    private void InitializeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    //Play Methods

    private void Awake ()
	{
		GetReferences();
		SubscribeToEvents();
	}

	private void Start()
	{
		LoadSettings();
        HideSettingsUI();
	}

    private void Update()
	{
        UpdateSettings();
	}

    //Public Methods

    //Private Methods

    private void UpdateSettings()
    {
        if (Input.GetButtonDown("ToggleSettings"))
        {
            if (IsSettingsUIShown)
            {
                HideSettingsUI();
            }
            else
            {
                ShowSettingsUI();
            }
        }
    }

    private void ShowSettingsUI()
    {
        IsSettingsUIShown = true;
        m_SettingsUIGameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    private void HideSettingsUI()
    {
        IsSettingsUIShown = false;
        m_SettingsUIGameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnMouseSensitivitySliderChanged(float newValue)
    {
		SetMouseSensitivity(newValue);
    }

    private void LoadSettings()
    {
        LoadMouseSensitivity();
    }

    private void LoadMouseSensitivity()
    {
        if (PlayerPrefs.HasKey("MouseSensitivity"))
        {
			SetMouseSensitivity(PlayerPrefs.GetFloat("MouseSensitivity"));
        }
        else
        {
            SetMouseSensitivity(m_DefaultMouseSensitivity);
        }
    }

    private void SaveMouseSensitivity()
    {
        PlayerPrefs.SetFloat("MouseSensitivity", MouseSensitivity);
    }

	private void SetMouseSensitivity(float mouseSensitivity)
    {
        MouseSensitivity = mouseSensitivity;
        m_MouseSensitivitySlider.value = mouseSensitivity;
        SaveMouseSensitivity();
    }

}