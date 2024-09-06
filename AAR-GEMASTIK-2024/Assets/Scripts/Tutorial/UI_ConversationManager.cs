using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ConversationManager : MonoBehaviour, IDataPersistance
{
    [SerializeField] private NPCConversation DetailedShopconversation;
    [SerializeField] private NPCConversation DetailedLevelConversation;
    [SerializeField] private NPCConversation DetailedSetUpBeforeExpediction;
    [SerializeField] private bool StatusDetailedCardConversation = false;
    [SerializeField] private bool StatusDetailedLevelConversation = false;
    [SerializeField] private bool StatusDetailedSetUpExpediction = false;
    public static UI_ConversationManager Instance;
    private void Awake()
    {
        if(Instance == null)Instance = this;
        else
        {
            Destroy(gameObject);
        }

    }
    public void PlayDetailedCard()
    {
        if (StatusDetailedCardConversation) return;
        StatusDetailedCardConversation = true;
        TutorialManager.instance.StartTutorial(DetailedShopconversation);
    }
    public void PlayLevelChoiceConversation()
    {
        if (StatusDetailedLevelConversation) return;
        StatusDetailedLevelConversation = true;
        TutorialManager.instance.StartTutorial(DetailedLevelConversation);
    }
    public void PlaySetUpConversation()
    {
        if (StatusDetailedSetUpExpediction) return;
        StatusDetailedSetUpExpediction = true;
        TutorialManager.instance.StartTutorial(DetailedSetUpBeforeExpediction);
    }
    public void LoadScene(GameData gameData)
    {
        
    }

    public void SaveScene(ref GameData gameData)
    {
        
    }
}
