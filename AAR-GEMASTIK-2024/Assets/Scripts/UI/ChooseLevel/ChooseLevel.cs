using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseLevel : BasePreparingPlayerUI, IDataPersistance
{
    [SerializeField] private Transform mainPanel;
    [SerializeField] private RectTransform Level1, Level2, Level3;
    [SerializeField] private TextMeshProUGUI levelUIText;
    private void Awake()
    {
        
    }
    
    public override IEnumerator OnEnterState()
    {
        gameObject.SetActive(true);
        mainPanel.gameObject.SetActive(true);
        GetComponent<RectTransform>().DOAnchorPosX(0, 0.7f);
        levelUIText.GetComponent<RectTransform>().DOAnchorPosY(5, 0.7f).SetEase(Ease.InOutQuint);
        AudioManager.Instance.PlaySFX(AudioContainerUI.instance.OnDisplay);
        levelUIText.text = $"Level : {GameManager.Instance.level}";
        GameManager.Instance.OnChangeLevelChoice += Instance_OnChangeLevelChoice;
        yield return null;
    }

    

    public override IEnumerator OnExitState()
    {
        GetComponent<RectTransform>().DOAnchorPosX(1000, 0.7f);
        AudioManager.Instance.PlaySFX(AudioContainerUI.instance.OnHide);
        GameManager.Instance.OnChangeLevelChoice -= Instance_OnChangeLevelChoice;
        levelUIText.GetComponent<RectTransform>().DOAnchorPosY(-50, 0.7f).SetEase(Ease.InOutQuint);
        yield return new WaitForSeconds(0.8f);
        mainPanel.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    public void OnChooseLevel(int level)
    {
        levelUIText.text = $"Level : {level}";
    }

    public void LoadScene(GameData gameData)
    {
        Level1.GetComponent<Button>().interactable = gameData.levels[0].hasBeenUnlocked;
        Level1.GetComponent<Button>().onClick.AddListener(()=>GameManager.Instance.SetLevel(1));
        Level2.GetComponent<Button>().interactable = gameData.levels[1].hasBeenUnlocked;
        Level2.GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.SetLevel(2));
        Level3.GetComponent<Button>().interactable = gameData.levels[2].hasBeenUnlocked;
        Level3.GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.SetLevel(3));

    }
    private void Instance_OnChangeLevelChoice(int index)
    {
        
    }
    public void SaveScene(ref GameData gameData)
    {
        
    }
}
