using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class GameOverGameplayUI : MonoBehaviour
{
    [SerializeField] private RectTransform backgroundGameOverContent;
    [SerializeField] private RectTransform GameOverContent;
    [SerializeField] private TextMeshProUGUI descriptionOnWhyLose;
    
    private void Awake()
    {
        backgroundGameOverContent.gameObject.SetActive(false);
        GameOverContent.DOAnchorPosY(-1080, 0);
    }
    private void Start()
    {
        ExpedictionManager.Instance.OnLose += Instance_OnLose;
    }
    private void OnDisable()
    {
        ExpedictionManager.Instance.OnLose -= Instance_OnLose;
    }

    private async void Instance_OnLose(string type)
    {
        await Task.Delay(800);
        string description = type;
        
        descriptionOnWhyLose.text = $"Alasan Mengapa Kalah \n{description}";
        backgroundGameOverContent.gameObject.SetActive(true);
        GameOverContent.DOAnchorPosY(0, 0.8f).SetEase(Ease.OutBack);
    }
    public void OnLoadScene(string sceneName)
    {
        GameManager.Instance.LoadScene(sceneName);
    }
}
