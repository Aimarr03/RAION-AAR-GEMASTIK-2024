using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public WeaponSO chosenWeaponSO;
    public AbilitySO chosenAbilitySO;
    //public ConsumableItemSO chosenConsumableItemSO;
    public HealthItemSO chosenHealthItemSO;
    public OxygenItemSO chosenOxygenItemSO;
    public EnergyItemSO chosenEnergyItemSO;
    public int level;
    public event Action<int> OnChangeLevelChoice;
    private bool isLoading = false;
    [SerializeField] private RectTransform canvasLoadingScreen;
    [SerializeField] private Image screenLoader;
    [SerializeField] private Image iconLoader;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetChosenItemSO(ConsumableItemSO itemSO)
    {
        switch (itemSO.type)
        {
            case SustainabilityType.Health:
                chosenHealthItemSO = itemSO as HealthItemSO;
                break;
            case SustainabilityType.Oxygen:
                chosenOxygenItemSO = itemSO as OxygenItemSO;
                break;
            case SustainabilityType.Energy: 
                chosenEnergyItemSO = itemSO as EnergyItemSO;
                break;
        }
    }
    public bool CheckHasAssigned()
    {
        return chosenAbilitySO != null && chosenWeaponSO != null;
    }
    public void SetLevel(int index)
    {
        level = index;
        OnChangeLevelChoice?.Invoke(index);
    }
    public void LoadLevel()
    {
        OnLoadScene($"Level{level}");
    }
    public void LoadScene(string sceneName)
    {
        OnLoadScene(sceneName);
    }
    public void LoadScene(int index)
    {
        OnLoadScene(index);
    }
    private async void OnLoadScene(int index)
    {
        AudioManager.Instance.StopMusic(0.8f);
        await LoadingScreen();
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(index);
        while (!loadingOperation.isDone)
        {
            await Task.Yield();
        }
        await DeloadingScreen();
    }
    private async void OnLoadScene(string sceneName)
    {
        await LoadingScreen();
        await Task.Delay(1000);
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!loadingOperation.isDone)
        {
            await Task.Yield();
        }
        await Task.Delay(1000);
        await DeloadingScreen();
    }
    public async Task LoadingScreen()
    {
        float maxDuration = 0.5f;
        float currentDuration = 0f;
        Color oldColor = screenLoader.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, 1);
        while(currentDuration < maxDuration)
        {
            currentDuration += Time.deltaTime;
            float t = currentDuration / maxDuration;
            screenLoader.fillAmount = t;
            screenLoader.color = Color.Lerp(oldColor, newColor, t);
            await Task.Yield();
        }
        screenLoader.fillAmount = 1;
        screenLoader.color = newColor;
        currentDuration = 0f;
        oldColor =iconLoader.color;
        newColor = new Color(oldColor.r, oldColor.g, oldColor.b, 1);
        while(currentDuration < maxDuration / 2)
        {
            currentDuration += Time.deltaTime;
            float t = currentDuration/(maxDuration/2);
            iconLoader.color = Color.Lerp(oldColor, newColor,t);
            await Task.Yield();
        }
        iconLoader.color = newColor;
        iconLoader.rectTransform.DORotate(new Vector3(0, 360, 0), 3.6f, RotateMode.WorldAxisAdd)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
    }
    public async Task DeloadingScreen()
    {
        Color screenLoaderColor =screenLoader.color;
        Color iconColor = iconLoader.color;
        screenLoader.DOColor(new Color(screenLoaderColor.r,screenLoaderColor.g,screenLoaderColor.b, 0), 0.5f);
        iconLoader.DOKill(true);
        iconLoader.DOColor(new Color(iconColor.r, iconColor.g, iconColor.b, 0), 0.5f);
        await Task.Yield() ;
    }
}
