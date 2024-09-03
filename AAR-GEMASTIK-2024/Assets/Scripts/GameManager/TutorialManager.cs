using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        
    }
    public void StartTutorial(NPCConversation conversation)
    {
        ConversationManager.Instance.StartConversation(conversation);
    }    
}
