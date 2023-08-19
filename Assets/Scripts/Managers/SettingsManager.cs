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
	private float m_DefaultAimSensitivity;

	//References

	[Header ("References")]

    [SerializeField]
    private GameObject m_SettingsUIGameObject;

    [SerializeField]
	private Slider m_AimSensitivitySlider;

    //Events

    //Public Fields

    public bool IsSettingsUIShown { get; private set; }

    public float AimSensitivity { get; private set; }

    //Private Fields

	//Initialization Methods

	private void GetReferences ()
	{
		InitializeSingleton();
	}

    private void SubscribeToEvents ()
	{
        m_AimSensitivitySlider.onValueChanged.AddListener(OnAimSensitivitySliderChanged);
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
        UpdateAimSensitivitySlider();
    }

    private void FixedUpdate()
    {
        UpdateAimSensitivitySlider();
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

    private void UpdateAimSensitivitySlider()
    {
        if (IsSettingsUIShown)
        {
            SetAimSensitivity(m_AimSensitivitySlider.value + (Input.GetAxis("Horizontal") + Input.GetAxis("Analog X")) * 0.5f);
        } 
    }

    private void ShowSettingsUI()
    {
        IsSettingsUIShown = true;
        m_SettingsUIGameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0.0f;
    }

    private void HideSettingsUI()
    {
        IsSettingsUIShown = false;
        m_SettingsUIGameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1.0f;
    }

    private void OnAimSensitivitySliderChanged(float newValue)
    {
		SetAimSensitivity(newValue);
    }

    private void LoadSettings()
    {
        LoadAimSensitivity();
    }

    private void LoadAimSensitivity()
    {
        if (PlayerPrefs.HasKey("AimSensitivity"))
        {
			SetAimSensitivity(PlayerPrefs.GetFloat("AimSensitivity"));
        }
        else
        {
            SetAimSensitivity(m_DefaultAimSensitivity);
        }
    }

    private void SaveAimSensitivity()
    {
        PlayerPrefs.SetFloat("AimSensitivity", AimSensitivity);
    }

	private void SetAimSensitivity(float aimSensitivity)
    {
        AimSensitivity = aimSensitivity;
        m_AimSensitivitySlider.value = aimSensitivity;
        SaveAimSensitivity();
    }

}