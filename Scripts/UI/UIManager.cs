using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject characterInfoUI;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private InputManager inputManager;

    private bool isCharacterInfoUIVisible = false;
    private bool isSkillTreeUIVisible = false;


    private void Start()
    {
        gameplayUI.SetActive(true);
        characterInfoUI.SetActive(false);
        skillTreeUI.SetActive(false);

        inputManager.ToggleCharacterInfo += ToggleCharacterInfoUI;
    }

    public void ToggleCharacterInfoUI()
    {
        if (isSkillTreeUIVisible) return;
        
        isCharacterInfoUIVisible = !isCharacterInfoUIVisible;
        characterInfoUI.SetActive(isCharacterInfoUIVisible);
    }

    public void OpenSkillTreeUI()
    {
        if (isCharacterInfoUIVisible)
        {
            isCharacterInfoUIVisible = !isCharacterInfoUIVisible;
            characterInfoUI.SetActive(isCharacterInfoUIVisible);

            isSkillTreeUIVisible = !isSkillTreeUIVisible;
            skillTreeUI.SetActive(isSkillTreeUIVisible);
        }
        else
        {

            isSkillTreeUIVisible = !isSkillTreeUIVisible;
            skillTreeUI.SetActive(isSkillTreeUIVisible);
            
        }
    }

    private void OnDestroy()
    {
        if (inputManager != null)
        {
            inputManager.ToggleCharacterInfo -= ToggleCharacterInfoUI;
        }
    }
}