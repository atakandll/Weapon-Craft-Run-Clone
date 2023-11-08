using Runtime.Commands.Level;
using Runtime.Signals;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region Self Variables

    #region  Serialized Variables

    [Header("Holder")] [SerializeField] internal GameObject levelHolder;
    [Space] [SerializeField] private byte totalLeventCount;

    #endregion
    
    #region Private Variables

    private LevelLoaderCommand _levelLoader;
    private LevelDestroyerCommand _levelDestroyer;
    private byte _currentLevel;

    #endregion
    
    
    #endregion
    
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _levelLoader = new LevelLoaderCommand(this);
        _levelDestroyer = new LevelDestroyerCommand(this);
    }
    private void OnEnable()
    {
        SubscribeEvents();

        _currentLevel = GetLevelID();
        CoreGameSignals.Instance.onLevelInitialize?.Invoke(_currentLevel);
    }

    private void SubscribeEvents()
    {
        CoreGameSignals.Instance.onLevelInitialize += _levelLoader.Execute;
        CoreGameSignals.Instance.onClearActiveLevel += _levelDestroyer.Execute;
        CoreGameSignals.Instance.onGetLevelID += GetLevelID;
        CoreGameSignals.Instance.onNextLevel += OnNextLevel;
        CoreGameSignals.Instance.onRestartLevel += OnRestartLevel;
    }
    private void UnsubscribeEvents()
    {
        CoreGameSignals.Instance.onLevelInitialize -= _levelLoader.Execute;
        CoreGameSignals.Instance.onClearActiveLevel -= _levelDestroyer.Execute;
        CoreGameSignals.Instance.onGetLevelID -= GetLevelID;
        CoreGameSignals.Instance.onNextLevel -= OnNextLevel;
        CoreGameSignals.Instance.onRestartLevel -= OnRestartLevel;
    }
    private void OnDisable()
    {
        UnsubscribeEvents();
    }


    public byte GetLevelID()
    {
        if (!ES3.FileExists()) return 0;
        return (byte)(ES3.KeyExists("Level") ? ES3.Load<int>("Level") % totalLeventCount : 0);
    }

    public void OnNextLevel()
    {
        _currentLevel++;
        SaveSignals.Instance.onSaveGameData?.Invoke();
        CoreGameSignals.Instance.onClearActiveLevel?.Invoke();
        CoreGameSignals.Instance.onLevelInitialize?.Invoke(GetLevelID());
    }

   
    public void OnRestartLevel()
    {
        SaveSignals.Instance.onSaveGameData?.Invoke();
        CoreGameSignals.Instance.onClearActiveLevel?.Invoke();
        CoreGameSignals.Instance.onLevelInitialize?.Invoke(GetLevelID());
    }

    
}
