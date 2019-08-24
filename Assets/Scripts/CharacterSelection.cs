using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private static GameObject _player1Selection;
    private static GameObject _player2Selection;

    public Animator ModelAnimator;
    public Button NextButton;
    public Image TargetGraphic;
    public MainMenu MenuScript;
    public List<GameObject> PlayerPrefabsList;
    public UnityEvent ClickEvent;
    public GameObject SelectionGameObject;
    public GameObject SelectionParent;

    [SerializeField]private SelectionSettings _mySettings;
    private bool _isLeftButton;
    private bool _isHovered;
    private bool isSet;

    void Awake()
    {
        TargetGraphic.CrossFadeColor(_mySettings.NormalColor, 0.1f, true, true);
    }

    void OnEnable()
    {
        GameManager.Instance._playerPrefabsList = new List<GameObject>();
        Destroy(_player1Selection);
        Destroy(_player2Selection);
        NextButton.gameObject.SetActive(false);

    }

    void Update()
    {
        if (SelectionParent.transform.childCount > 0 || TargetGraphic.color == _mySettings.NormalColor || _isHovered)
        {
            return;
        }

        if (_player1Selection != null && _player2Selection != null)
        {
            ModelAnimator.SetBool("Dead", true);
            NextButton.gameObject.SetActive(true);
        }


        TargetGraphic.CrossFadeColor(_mySettings.NormalColor, 0.1f, false, true);
    }

    public void SetCharacterModel(int model)
    {
        if (GameManager.Instance._playerPrefabsList.Count == 0)
        {
            GameObject[] emptyGameObjects = new GameObject[2];
            GameManager.Instance._playerPrefabsList.AddRange(emptyGameObjects);
        }


        if (_isLeftButton && GameManager.Instance._playerPrefabsList[0] != PlayerPrefabsList[model])
        {
            if (_player1Selection != null)
            {
                DestroyImmediate(_player1Selection);
            }

            _player1Selection = Instantiate(SelectionGameObject, SelectionParent.transform);
            TextMeshProUGUI selectionText = _player1Selection.GetComponent<TextMeshProUGUI>();
            GameManager.Instance._playerPrefabsList[0] = PlayerPrefabsList[model];
            selectionText.text = "Player 1";
            ModelAnimator.SetTrigger("Success");

        }
        else if(!_isLeftButton && GameManager.Instance._playerPrefabsList[1] != PlayerPrefabsList[model])
        {

            if (_player2Selection != null)
            {
                Destroy(_player2Selection);
            }

            _player2Selection = Instantiate(SelectionGameObject, SelectionParent.transform);
            TextMeshProUGUI selectionText = _player2Selection.GetComponent<TextMeshProUGUI>();
            GameManager.Instance._playerPrefabsList[1] = PlayerPrefabsList[model];
            selectionText.text = "Player 2";
            ModelAnimator.SetTrigger("Success");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHovered = true;
        ModelAnimator.SetBool("Dead", false);
        TargetGraphic.CrossFadeColor(_mySettings.HighlightColor, 0.1f, true, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHovered = false;
        if (!(SelectionParent.transform.childCount > 0))
        {
            TargetGraphic.CrossFadeColor(_mySettings.NormalColor, 0.1f, true, true);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        TargetGraphic.CrossFadeColor(_mySettings.PressColor, 0.1f, false, true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isLeftButton = eventData.button == PointerEventData.InputButton.Left;


        ClickEvent.Invoke();

        TargetGraphic.CrossFadeColor(_isHovered ? _mySettings.HighlightColor : _mySettings.NormalColor, 0.1f, false, true);
    }
}
