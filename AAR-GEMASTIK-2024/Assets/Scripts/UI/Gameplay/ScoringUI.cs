using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;
using System;

public class ScoringUI : MonoBehaviour, IDataPersistance
{
    [SerializeField] private Transform Container;
    [SerializeField] private TextMeshProUGUI trashCollectedTextUI;
    [SerializeField] private TextMeshProUGUI mutatedFishCollectedTextUI;
    [SerializeField] private TextMeshProUGUI coinObtainedTextUI;
    [SerializeField] private TextMeshProUGUI NumericProgressBar;
    [SerializeField] private Image ProgressBar;
    private List<TextMeshProUGUI> textUIList;
    private int totalEnemyDefeated = 0;
    private int moneyObtainedFromEnemyDefeated = 0;
    private float maxDuration = 2;

    
    private int trashTotalCollected = 0;
    private int trashCount = 0;
    private int fishTotalCollected = 0;
    private int fishCount = 0;
    
    private int totalCount = 0;
    private int totalCountCollected = 0;

    private float trashProgress = 0f;
    private float fishProgress = 0f;
    private float totalProgress = 0f;

    private void Awake()
    {
        textUIList = new List<TextMeshProUGUI>
        {
            trashCollectedTextUI, mutatedFishCollectedTextUI, coinObtainedTextUI,
        };
        foreach (TextMeshProUGUI text in textUIList)
        {
            text.text = "";
        }
        Container.GetComponent<RectTransform>().DOAnchorPosY(1000, 0f);
        Container.gameObject.SetActive(false);
        
    }
    private void Start()
    {
        OnCalculateTrashAndEnemy();
        ExpedictionManager.Instance.OnDoneExpediction += Instance_OnDoneExpediction;
        TrashBase.OnCollectedEvent += TrashBase_OnCollectedEvent;
        EnemyBase.OnCaught += EnemyBase_OnCaught;
    }
    private void OnDisable()
    {
        ExpedictionManager.Instance.OnDoneExpediction -= Instance_OnDoneExpediction;
        TrashBase.OnCollectedEvent -= TrashBase_OnCollectedEvent;
        EnemyBase.OnCaught -= EnemyBase_OnCaught;
    }
    private void EnemyBase_OnCaught(int bounty)
    {
        fishTotalCollected++;
        totalCountCollected++;
        moneyObtainedFromEnemyDefeated += bounty;
    }

    private void TrashBase_OnCollectedEvent()
    {
        trashTotalCollected++;
        totalCountCollected++;
    }

    private void OnDestroy()
    {
        
    }

    private void Instance_OnDoneExpediction(bool obj, PlayerCoreSystem coreSystem)
    {
        Debug.Log("Invoke when Done Expediction");
        Container.gameObject.SetActive(true);
        StartCoroutine(OnStartDisplayScore(coreSystem));
        ExpedictionManager.Instance.OnDoneExpediction -= Instance_OnDoneExpediction;
    }
    private IEnumerator OnStartDisplayScore(PlayerCoreSystem coreSystem)
    {
        float trashValue = coreSystem.GetSustainabilitySystem(SustainabilityType.Capacity).GetCurrentData(SustainabilityType.Capacity).currentValue;
        Container.GetComponent<RectTransform>().DOAnchorPosY(0, 0.4f).SetEase(Ease.OutBack);
        
        yield return new WaitForSeconds(0.57f);
        yield return OnIncrementValueUI(trashCollectedTextUI, trashValue);
        yield return OnIncrementValueUI(mutatedFishCollectedTextUI, fishTotalCollected);

        int totalMoneyObtained = EconomyManager.Instance.GetMoneyMultiplierBasedOnTrash(trashValue);
        totalMoneyObtained += moneyObtainedFromEnemyDefeated;
        
        yield return OnIncrementValueUI(coinObtainedTextUI, totalMoneyObtained);
        EconomyManager.Instance.OnGainMoney(totalMoneyObtained);

        float totalProgress = totalCountCollected / totalCount;
        float trashProgress = trashTotalCollected / trashCount;
        float enemyProgress = fishTotalCollected / fishCount;

        totalProgress = (float)Math.Round(totalProgress, 2);
        trashProgress = (float)Math.Round(trashProgress, 2);
        enemyProgress = (float)Math.Round(enemyProgress, 2);

        this.totalProgress = totalProgress;
        this.trashProgress = trashProgress;
        this.fishProgress = enemyProgress;

        yield return OnIncrementValueUI(NumericProgressBar, ProgressBar, totalProgress);
    }
    private IEnumerator OnIncrementValueUI(TextMeshProUGUI text, Image targetUI, float targetValue)
    {
        int startValue = 0;
        float currentDuration = 0;
        while (currentDuration < maxDuration)
        {
            currentDuration += Time.deltaTime;
            float t = Mathf.Clamp01(currentDuration / maxDuration);
            float currentValue = Mathf.Lerp(startValue, targetValue, t);
            targetUI.fillAmount = currentValue;
            text.text = targetUI.fillAmount.ToString() + " %";
            yield return null;
        }
    }
    private IEnumerator OnIncrementValueUI(TextMeshProUGUI targetUI, float targetValue)
    {
        int startValue = 0;
        float currentDuration = 0;
        while(currentDuration < maxDuration)
        {
            currentDuration += Time.deltaTime;
            float t = Mathf.Clamp01(currentDuration / maxDuration);
            float currentValue = Mathf.Lerp(startValue, targetValue, t);
            targetUI.text = currentValue.ToString("0.0");
            yield return null;
        }
    }
    public void OnLoadShopping()
    {
        DataManager.instance.SaveGame();
        SceneManager.LoadScene("ShoppingMenu");
    }
    private void OnCalculateTrashAndEnemy()
    {
        IEnumerable<TrashBase> trashCalculatedFromScene = FindObjectsOfType<MonoBehaviour>().OfType<TrashBase>();
        trashTotalCollected = 0;
        foreach (TrashBase trash in trashCalculatedFromScene)
        {
            if(trash.HasBeenCollected()) trashTotalCollected++;
        }
        trashCount = trashCalculatedFromScene.Count();

        IEnumerable<EnemyBase> enemyCalculatedFromScene = FindObjectsOfType<MonoBehaviour>().OfType<EnemyBase>();
        fishTotalCollected = 0;
        foreach(EnemyBase enemy in enemyCalculatedFromScene)
        {
            if(enemy.GetIsFishKnockout()) fishTotalCollected++;
        }
        fishCount = enemyCalculatedFromScene.Count();

        totalCount = fishCount + trashCount;
        totalCountCollected = fishTotalCollected + trashTotalCollected;
    }

    public void LoadScene(GameData gameData)
    {
        
    }

    public void SaveScene(ref GameData gameData)
    {
        LevelData levelData = gameData.GetLevelData(GameManager.Instance.level);
        levelData.fishProgress = fishProgress;
        levelData.trashProgress = trashProgress;
        Debug.Log(levelData.fishProgress);
        Debug.Log(levelData.trashProgress);
    }

}
