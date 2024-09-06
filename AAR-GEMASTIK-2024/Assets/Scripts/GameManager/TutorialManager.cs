using DialogueEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour, IDataPersistance
{
    [SerializeField] private NPCConversation FinishTutorialConversation;
    public static TutorialManager instance;
    private int maxNPCDialogueTutorial;
    private int currentNPCDialogueFinished;
    private bool finishedTutorial;
    public bool isFinishedTutorial => finishedTutorial;
    public static Action FinishTutorialAction;
    public bool UI_Tutorial;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        maxNPCDialogueTutorial = FindObjectsOfType<TutorialEventAction>().Length;
        currentNPCDialogueFinished = 0;
    }
    private void Start()
    {
        ConversationManager.OnConversationEnded += OnConversationEnded;
    }
    private void OnDisable()
    {
        ConversationManager.OnConversationEnded -= OnConversationEnded;
    }
    private void OnConversationEnded()
    {
        if (finishedTutorial)
        {
            Debug.Log("Tutorial Finished");
            FinishTutorialAction.Invoke();
            return;
        }
        currentNPCDialogueFinished++;
        Debug.Log("Remaining Tutorial " + (maxNPCDialogueTutorial - currentNPCDialogueFinished));
        if(!UI_Tutorial)
        {
            if (currentNPCDialogueFinished >= maxNPCDialogueTutorial)
            {
                OnDelayConversationTutorial();
            }
        }
    }
    public void StartTutorial(NPCConversation conversation)
    {
        ConversationManager.Instance.StartConversation(conversation);
    }   
    private async void OnDelayConversationTutorial()
    {
        await Task.Delay(1200);
        ConversationManager.Instance.StartConversation(FinishTutorialConversation);
        finishedTutorial = true;
    }

    public void LoadScene(GameData gameData)
    {
        if (UI_Tutorial)
        {
            if(gameData.tutorialShop) Destroy(gameObject);
        }
        else
        {

        }
    }

    public void SaveScene(ref GameData gameData)
    {
        if (UI_Tutorial)
        {
            if(currentNPCDialogueFinished >= 3)
            {
                gameData.tutorialShop = true;
            }
        }
        else
        {

        }
    }
}
