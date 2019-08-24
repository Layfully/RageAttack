using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private List<Player> PlayerList
    {
        get
        {
            return _playerList;
        }
    }


    [SerializeField] public List<Player> _playerList;
    [SerializeField] public List<GameObject> _playerPrefabsList;
    [SerializeField] public List<MovementData> _movementDataList;
    [SerializeField] public List<InputSettings> _inputSettingsList;
    [SerializeField] public List<PlayerStats> _playerStatsList;

    [SerializeField] public GameObject _healthBarPrefab;
    [SerializeField] public GameObject _chiBarPrefab;
    [SerializeField] public GameObject _pauseCanvas;
    [SerializeField] private GameObject _gameOverCanvas;
    [SerializeField] private GameObject _player1StatsValues;
    [SerializeField] private GameObject _player2StatsValues;

    [SerializeField] private TextMeshProUGUI _player1Label;
    [SerializeField] private TextMeshProUGUI _player2Label;

    [SerializeField] private bool _paused = false;

    private List<GameObject> _spawnPoints;
    private Camera2D _camera;
    private EventSystem _eventSystem;
    private StandaloneInputModule _inputModule;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += LevelLoaded;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }


    private void LevelLoaded(Scene newScene, LoadSceneMode mode)
    {
        InitializeLevel();
    }

    private void InitializeLevel()
    {
        if (SceneManager.GetActiveScene().name != "Main Menu")
        {
            _pauseCanvas = GameObject.FindGameObjectWithTag("Pause");
            _gameOverCanvas = GameObject.FindGameObjectWithTag("Game Over");
            _spawnPoints = GameObject.FindGameObjectsWithTag("Spawn Point").ToList();
            _player1Label = GameObject.FindWithTag("Player1Label").GetComponent<TextMeshProUGUI>();
            _player2Label = GameObject.FindWithTag("Player2Label").GetComponent<TextMeshProUGUI>();
            _camera = FindObjectOfType<Camera2D>();
            _eventSystem = FindObjectOfType<EventSystem>();
            _inputModule = FindObjectOfType<StandaloneInputModule>();

            _gameOverCanvas.SetActive(false);
            PlayerList.Clear();
            TogglePause();

            for (int i = 0; i < 2; i++)
            {
                PlayerList.Add(Instantiate(_playerPrefabsList[i], _spawnPoints[i].transform.position, Quaternion.identity).GetComponent<Player>());
                PlayerList[i].MovementData = _movementDataList[i];
                PlayerList[i].InputSettings = _inputSettingsList[i];
                PlayerList[i].PlayerStats = _playerStatsList[i];
                PlayerList[i].PlayerStatsHud.ChiBar = Instantiate(_chiBarPrefab, GameObject.Find("P" + (i + 1)).transform, false).GetComponent<Slider>();
                PlayerList[i].PlayerStatsHud.HealthBar = Instantiate(_healthBarPrefab, GameObject.Find("P" + (i + 1)).transform, false).GetComponent<Slider>();
            }

            PlayerList[1].PlayerTargetting.EnemyPlayerStats = PlayerList[0].PlayerVitals;
            PlayerList[0].PlayerTargetting.EnemyPlayerStats = PlayerList[1].PlayerVitals;
            PlayerList[0].PlayerTargetting.EnemyTransform = PlayerList[1].transform;
            PlayerList[1].PlayerTargetting.EnemyTransform = PlayerList[0].transform;

            _camera.Targets.Add(PlayerList[0].transform);
            _camera.Targets.Add(PlayerList[1].transform);

            PlayerList[0].PlayerStats.Reset();
            PlayerList[1].PlayerStats.Reset();
        }
    }

    public void TogglePause()
    {
        _paused = !_paused;
        if (_paused)
        {
            Time.timeScale = 0;
            InputManager.Enabled = false;
            _inputModule.enabled = true;
            _eventSystem.enabled = true;
            _pauseCanvas.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            _eventSystem.enabled = false;
            _inputModule.enabled = false;
            InputManager.Enabled = true;
            _pauseCanvas.SetActive(false);
        }
    }

    public void EndGame()
    {
        Time.timeScale = 0;
        InputManager.Enabled = false;
        _eventSystem.enabled = true;
        _inputModule.enabled = true;
        _gameOverCanvas.SetActive(true);
        _paused = !_paused;

        _player1StatsValues = GameObject.FindWithTag("Player1Values");
        _player2StatsValues = GameObject.FindWithTag("Player2Values");


        string[] player1Stats = PlayerList[0].PlayerStats.GenerateStatsStrings();
        string[] player2Stats = PlayerList[1].PlayerStats.GenerateStatsStrings();

        for (int i = 0; i < _player1StatsValues.transform.childCount; i++)
        {
            _player1StatsValues.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = player1Stats[i];
            _player2StatsValues.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = player2Stats[i];
        }

        _player1Label.text = int.Parse(player1Stats[0]) >= int.Parse("100") ? "<color=\"red\"> Player1 </color>" : "<color=\"green\"> Player1 </color>";
        _player2Label.text = int.Parse(player2Stats[0]) >= int.Parse("100") ? "<color=\"red\"> Player2 </color>" : "<color=\"green\"> Player2 </color>";
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1;
        InputEvents.ClearEvents();
    }
}
