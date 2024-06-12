using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseLevel : BasePreparingPlayerUI, IDataPersistance
{
    [SerializeField] private Transform mainPanel;
    [SerializeField] private RectTransform Level1, Level2, Level3;
    private void Awake()
    {
        
    }
    
    public override IEnumerator OnEnterState()
    {
        gameObject.SetActive(true);
        mainPanel.gameObject.SetActive(true);
        GetComponent<RectTransform>().DOAnchorPosX(0, 0.7f);
        yield return null;
    }

    public override IEnumerator OnExitState()
    {
        GetComponent<RectTransform>().DOAnchorPosX(1000, 0.7f);
        yield return new WaitForSeconds(0.8f);
        mainPanel.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    public void OnChooseLevel(int level)
    {

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

    public void SaveScene(ref GameData gameData)
    {
        
    }
}
