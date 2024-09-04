using DialogueEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialEventAction : MonoBehaviour
{
    [SerializeField] private bool PlayOnExitTrigger;
    [SerializeField] private bool PlayOnAwake;
    [SerializeField] private bool PlayOnEnterTrigger;
    [SerializeField] private bool PlayOnCertainAction;
    [SerializeField] private bool PlayOnClick;

    private bool isDone;
    public static event EventHandler DoneTutorialEvent;

    [SerializeField] private NPCConversation conversation;
    [SerializeField] private Button theButton;
    private void Awake()
    {
        if (!isDone && PlayOnClick)
        {
            theButton?.onClick.AddListener(OnClickEvent);
        }
        if (conversation == null) conversation = transform.GetChild(0).GetComponent<NPCConversation>();
        if(PlayOnCertainAction) TrashBase.OnCollectedEvent += TrashBase_OnCollectedEvent;
    }
    private void OnClickEvent()
    {
        if(PlayOnClick && !isDone)
        {
            TutorialManager.instance.StartTutorial(conversation);
        }
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

