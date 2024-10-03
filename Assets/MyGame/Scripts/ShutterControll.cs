using System;
using System.Collections;
using UnityEngine;

public class ShutterControll : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] CameraControllArea nextCameraControllArea;
    [SerializeField] ActionChainExecuter eventController;
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
        if (!GameManager.Instance.MainCameraControll.Equal(nextCameraControllArea.VirtualCamera)) eventController.StartEvent();
    }

    public void ShutterOpen(Action finishCallback)
    {
        StartCoroutine(ShutterOpenCo(finishCallback));
        IEnumerator ShutterOpenCo(Action finishCallback)
        {
            _animator.enabled = true;
            _animator.Play(animationOpenHash);

            while (_animator.IsPlayingCurrentAnimation(animationOpenHash)) yield return null;

            boxCollider.enabled = false;

            finishCallback?.Invoke();
        }
    }

    public void ShutterClose(Action finishCallback)
    {
        StartCoroutine(ShutterCloseCo(finishCallback));

        IEnumerator ShutterCloseCo(Action finishCallback)
        {
            _animator.Play(animationCloseHash);

            while (_animator.IsPlayingCurrentAnimation(animationCloseHash)) yield return null;
            boxCollider.enabled = true;

            _animator.enabled = false;
            finishCallback?.Invoke();
        }
    }

   
}
