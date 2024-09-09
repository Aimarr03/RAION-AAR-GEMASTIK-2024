using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueLevel : MonoBehaviour, IDataPersistance
{
    public string id;
    public bool OnAwake;
    public bool PhaseComplete;
    public int Phase;
    [ContextMenu("Generate ID")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
    public NPCConversation conversation;
    public bool isDone;
    private void Awake()
    {
        conversation = GetComponent<NPCConversation>();
        
    }
    private void Start()
    {
        if(PhaseComplete && !isDone) ObjectiveManager.OnPhaseCompleted += ObjectiveManager_OnPhaseCompleted;
        if (OnAwake && !isDone)
        {
            ConversationManager.Instance.StartConversation(conversation);
            isDone = true;
        }
    }
    private void OnDisable()
    {
        if (PhaseComplete && !isDone) ObjectiveManager.OnPhaseCompleted -= ObjectiveManager_OnPhaseCompleted;
    }
    private void ObjectiveManager_OnPhaseCompleted(List<ObjectiveData> obj)
    {
        if (isDone) return;
        if(ObjectiveManager.Instance.currentPhase == Phase)
        {
            ConversationManager.Instance.StartConversation(conversation);
            ObjectiveManager.OnPhaseCompleted -= ObjectiveManager_OnPhaseCompleted;
            isDone = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out PlayerCoreSystem coreSystem) && !isDone)
        {
            ConversationManager.Instance.StartConversation(conversation);
            isDone = true;
        }
    }
    public void LoadScene(GameData gameData)
    {
        if (TutorialManager.instance != null) return;
        SubLevelData levelData = gameData.GetSubLevelData(GameManager.Instance.currentLevelChoice);
        levelData.conversationList.TryGetValue(id, out bool value);
        if (value)
        {
            gameObject.SetActive(false);
            isDone = true;
        }
    }

    public void SaveScene(ref GameData gameData)
    {
        SubLevelData levelData = gameData.GetSubLevelData(GameManager.Instance.currentLevelChoice);
        if (levelData.conversationList.ContainsKey(id))
        {
            levelData.conversationList.Remove(id);
        }
        levelData.conversationList.Add(id, isDone);
    }

}
