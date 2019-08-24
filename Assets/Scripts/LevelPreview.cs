using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelPreview : MonoBehaviour,IPointerEnterHandler
{
    [SerializeField]
    private Image _previewImage;
    [SerializeField]
    private Animator _previewAnimator;
    [SerializeField]
    private Sprite _previewSprite;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_previewImage.sprite != _previewSprite)
        {
            _previewImage.sprite = _previewSprite;
            _previewAnimator.Play("Fade In Out");
        }
    }
}
