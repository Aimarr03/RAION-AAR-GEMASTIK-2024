using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveUI : MonoBehaviour
{
    [SerializeField] private RectTransform ObjectiveContainer;
    [SerializeField] private TextMeshProUGUI objectiveText;

    private List<UI_ObjectiveDataHolder> List_CurrentObjectives;
    void Start()
    {
        ObjectiveManager.OnObjectiveProgress += ObjectiveManager_OnObjectiveProgress;
        ObjectiveManager.OnPhaseCompleted += ObjectiveManager_OnPhaseCompleted;
    }

    private void ObjectiveManager_OnPhaseCompleted(List<ObjectiveData> DataList)
    {
        List_CurrentObjectives = new List<UI_ObjectiveDataHolder>();
        foreach(RectTransform data in ObjectiveContainer)
        {
            if(data.gameObject != objectiveText.gameObject)
            {
                Destroy(data.gameObject);
            }
        }
        for(int index = 0;index < DataList.Count; index++)
        {
            TextMeshProUGUI currentText = Instantiate(objectiveText, ObjectiveContainer);
            currentText.gameObject.SetActive(true);
            ObjectiveData data = DataList[index];
            List_CurrentObjectives.Add(new UI_ObjectiveDataHolder(DataList[index], currentText));
            currentText.text = $"{data.description} {data.currentProgress}/{data.maxProgress}";
        }
    }

    private void ObjectiveManager_OnObjectiveProgress(ObjectiveData obj)
    {
        foreach(UI_ObjectiveDataHolder UI_ObjectiveData in List_CurrentObjectives)
        {
            if(UI_ObjectiveData.data == obj)
            {
                ObjectiveData data = UI_ObjectiveData.data;
                UI_ObjectiveData.UI_Text.text = $"{data.description} {data.currentProgress}/{data.maxProgress}";
            }
        }
    }

    private void OnDisable()
    {
        ObjectiveManager.OnObjectiveProgress -= ObjectiveManager_OnObjectiveProgress;
        ObjectiveManager.OnPhaseCompleted -= ObjectiveManager_OnPhaseCompleted;
    }
}
[Serializable]
public class UI_ObjectiveDataHolder
{
    public ObjectiveData data;
    public TextMeshProUGUI UI_Text;
    public UI_ObjectiveDataHolder(ObjectiveData data, TextMeshProUGUI text)
    {
        this.data = data;
        this.UI_Text = text;
    }
}
