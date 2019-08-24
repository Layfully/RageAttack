using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField]
    private Animator _cloudAnimator;
    [SerializeField]
    private AnimationClip _cloudAnimation;

    void Start()
    {
        _cloudAnimator.Play(_cloudAnimation.name, 0, Random.Range(0.0f, 1.0f));
    }
}
