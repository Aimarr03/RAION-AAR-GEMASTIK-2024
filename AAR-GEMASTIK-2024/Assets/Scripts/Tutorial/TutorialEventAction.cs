using DialogueEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEventAction : MonoBehaviour
{
    [SerializeField] private bool PlayOnExitTrigger;
    [SerializeField] private bool PlayOnAwake;
    [SerializeField] private bool PlayOnEnterTrigger;
    [SerializeField] private bool PlayOnCertainAction;

    private bool isDone;
    public static event EventHandler DoneTutorialEvent;

    [SerializeField] private NPCConversation conversation;
    private void Awake()
    {
        if(PlayOnCertainAction) TrashBase.OnCollectedEvent += TrashBase_OnCollectedEvent;
    }

    private void TrashBase_OnCollectedEvent()
    {
        if (PlayOnCertainAction && !isDone)
        {
            TutorialManager.instance.StartTutorial(conversation);
            isDone = true;
            TrashBase.OnCollectedEvent -= TrashBase_OnCollectedEvent;
        }
    }

    private void Start()
    {
        if (!isDone && PlayOnAwake)
        {
            Debug.Log("Start Tutorial " + gameObject);
            TutorialManager.instance.StartTutorial(conversation);
            isDone = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (PlayOnExitTrigger && !isDone)
        {
            TutorialManager.instance.StartTutorial(conversation);
            isDone = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PlayOnEnterTrigger && !isDone)
        {
            TutorialManager.instance.StartTutorial(conversation);
            isDone = true;
        }
    }
}

