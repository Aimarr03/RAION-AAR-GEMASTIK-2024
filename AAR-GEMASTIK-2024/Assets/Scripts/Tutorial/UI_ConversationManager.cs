using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ConversationManager : MonoBehaviour
{
    [SerializeField] private NPCConversation conversation;
    [SerializeField] private bool StatusDetailedCardConversation = false;
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
        TutorialManager.instance.StartTutorial(conversation);
    }
}
