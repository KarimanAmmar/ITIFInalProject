using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// /new things added :
/// removing the coint UI count because its no longer needed
/// create a function to display a progress bar for the level up
/// </summary>
public class UI_Manager : MonoBehaviour
{
    [SerializeField] private Image HpBar;
    [SerializeField] private Image LevelBar;
    [Header(" Events ")]
    [SerializeField] private Float_event HP_UI_event;
    [SerializeField] private Float_event LevelUP_UI_Event;
    [SerializeField] private Upgrade_list_event List_Event;
    [SerializeField] private GameEvent UI_Activate_Event;
    [SerializeField] private GameEvent UI_Deactivate_Event;
    [SerializeField] private GameEvent UI_Death_Event;
    [Header(" UI panels ")]
    [SerializeField] private GameObject MovementPanel;
    [SerializeField] private GameObject UpgradePanel;
    [SerializeField] private GameObject SettingPanel;
    [SerializeField] private GameObject DeathPanel;
    [SerializeField] private Upgrade_Option[] options;
    [SerializeField] private AudioClip ClickAudio;
    /// <summary>
    /// i referance the upgrade manager here to add to select upgrade function for each button
    /// </summary>
    [SerializeField] private UpgradeManager UpgradeManager;
    private void OnEnable()
    {
        
        HP_UI_event.RegisterListener(Update_Hp);
        LevelUP_UI_Event.RegisterListener(Update_Level);
        List_Event.RegisterListener(DisplayOptions);
        UI_Activate_Event.GameAction += ActivateUpgradesPanel;
        UI_Deactivate_Event.GameAction += DeactivateUpgradesPanel;
        UI_Death_Event.GameAction += DisplayDeath;

    }
    private void OnDisable()
    {
        
        HP_UI_event.UnregisterListener(Update_Hp);
        LevelUP_UI_Event.UnregisterListener(Update_Level);
        List_Event.UnregisterListener(DisplayOptions);
        UI_Activate_Event.GameAction -=ActivateUpgradesPanel;
        UI_Deactivate_Event.GameAction -= DeactivateUpgradesPanel;
        UI_Death_Event.GameAction -= DisplayDeath;
    }
    private void Start()
    {
        LevelBar.fillAmount = 0;
        MovementPanel.SetActive(true);
        UpgradePanel.SetActive(false);
        SettingPanel.SetActive(false);
        DeathPanel.SetActive(false);
    }
    
    private void Update_Hp(float amount)
    {
        HpBar.fillAmount=amount;
    }
    private void Update_Level(float amount)
    {
       LevelBar.fillAmount=amount;
    }
    private void PauseGame()
    {
        Time.timeScale=0.0f;
    }
    private void ResumeGame()
    {
        Time.timeScale=1.0f;
    }
    public void OpenSettings()
    {
        PauseGame();
        OpenSettingsPanel();
    }
    public void CloseSettings()
    {
        CloseSettingsPanel();
        ResumeGame();
    }
    private void OpenSettingsPanel()
    {
        MovementPanel.SetActive(false);
        SettingPanel.SetActive(true);
    }
    private void CloseSettingsPanel()
    {
        SettingPanel.SetActive(false);
        MovementPanel.SetActive(true);
    }

    private void DisplayDeath()
    {
        PauseGame();
        DeathPanel.SetActive(true);
    }
    private void ActivateUpgradesPanel()
    {
        MovementPanel.SetActive(false);
        PauseGame();
        UpgradePanel.SetActive(true);

    }
    private void DeactivateUpgradesPanel()
    {
        MovementPanel.SetActive(true);
        ResumeGame();
        UpgradePanel.SetActive(false);
    }
    private void DisplayOptions(Upgrade[] upgrades)
    {
        for (int i = 0; i < upgrades.Length; i++)
        {
            options[i].setType(upgrades[i].GetUpgradeType());
            options[i].setValue(upgrades[i].GetString());
            options[i].GetButton().gameObject.SetActive(true);

            if (options[i].GetButton().onClick != null)
            {
                options[i].GetButton().onClick.RemoveAllListeners();
            }
            

            Upgrade currentUpgrade = upgrades[i];

            options[i].GetButton().onClick.AddListener(() => UpgradeManager.select_upgrade(currentUpgrade));
            options[i].GetButton().onClick.AddListener(() => AudioManager.Instance.PlySfx(ClickAudio));
        }

    }
}
