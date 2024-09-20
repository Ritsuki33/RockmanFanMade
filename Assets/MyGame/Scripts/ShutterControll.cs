using System.Collections;
using UnityEngine;

public class ShutterControll : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] CameraControllArea nextCameraControllArea;

    static int animationOpenHash = Animator.StringToHash("Open");
    static int animationCloseHash = Animator.StringToHash("Close");

    BoxCollider2D boxCollider;
    private void Awake()
    {
        _animator.enabled = false;
        boxCollider = GetComponent<BoxCollider2D>();
    }
    public void Enter()
    {
        GameManager.Instance.ChangeCamera(nextCameraControllArea, ShutterOpen(), ShutterClose());
    }

    IEnumerator ShutterOpen()
    {
        _animator.enabled = true;
        _animator.Play(animationOpenHash);

        while (_animator.IsPlayingCurrentAnimation(animationOpenHash)) yield return null;

        boxCollider.enabled = false;
    }

    IEnumerator ShutterClose()
    {
        _animator.Play(animationCloseHash);

        while (_animator.IsPlayingCurrentAnimation(animationCloseHash)) yield return null;
        boxCollider.enabled = true;
        _animator.enabled = false;
    }
}
