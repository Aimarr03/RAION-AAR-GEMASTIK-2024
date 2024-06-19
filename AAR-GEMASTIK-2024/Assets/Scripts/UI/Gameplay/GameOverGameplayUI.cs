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
    string healthProblem = "Kapal Selam tidak kuat lagi bertahan dari serangan luar :<";
    string energyProblem = "Kapal Selam kehabisan energi untuk bergerak";
    string oxygenProblem = "Pengendara tidak bisa bernafas karena kehabisan oksigen di dalam kapal selam";
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

    private async void Instance_OnLose(SustainabilityType type)
    {
        await Task.Delay(800);
        string description = "";
        switch (type)
        {
            case SustainabilityType.Health:
                description = healthProblem;
                break;
            case SustainabilityType.Energy:
                description = energyProblem;
                break;
            case SustainabilityType.Oxygen:
                description = oxygenProblem;
                break;
        }
        descriptionOnWhyLose.text = $"Alasan Mengapa Kalah \n{description}";
        backgroundGameOverContent.gameObject.SetActive(true);
        GameOverContent.DOAnchorPosY(0, 0.8f).SetEase(Ease.OutBack);
    }
    public void OnLoadScene(string sceneName)
    {
        GameManager.Instance.LoadScene(sceneName);
    }
}
