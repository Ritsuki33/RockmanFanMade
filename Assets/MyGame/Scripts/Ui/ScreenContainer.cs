using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public interface IScreen<T> where T : Enum
{
    IScreenPresenter<T> ScreenPresenter { get; }

    /// <summary>
    /// ビュー、コントローラー、ビューモデルの準備
    /// </summary>
    /// <returns></returns>
    public IEnumerator Configure();

    public void Open();   // 表示
    public void Hide();         // 非表示

    public IEnumerator OpenCoroutine(); // 表示
    public IEnumerator HideCoroutine();   // 非表示

    public void SetActive(bool isActive);

    public void Initialize(IViewModel<T> viewModel);
    public void Deinitialize();

    public ScreenContainer<T> Container { set; }

    public void SetSiblingIndex();
}

public interface IScreenPresenter<T> where T : Enum
{
    public IEnumerator Configure(IScreen<T> screen);

    public void Deinitialize();

    public void InputUpdate(InputInfo info);
}

public interface IViewModel<T> where T : Enum
{
    public IEnumerator Configure();
}
public class BaseScreen<S, SP, VM, T> : MonoBehaviour, IScreen<T>
    where S : IScreen<T>, new()
    where SP : IScreenPresenter<T>, new()
    where VM : IViewModel<T>, new()
    where T : Enum
{
    ScreenContainer<T> container;
    IScreenPresenter<T> screenPresenter = null;

    private IInput InputController => InputManager.Instance;

    IScreenPresenter<T> IScreen<T>.ScreenPresenter => screenPresenter;

    public SP ScreenPresenter => (SP)screenPresenter;
    
    public IEnumerator Configure()
    {
        if (screenPresenter == null) screenPresenter = new SP();

        yield return screenPresenter.Configure(this);
    }

    private void Update()
    {
        InputInfo inputInfo = default;
        inputInfo.SetInput(InputController);

        // コントローラーへ入力情報を委譲
        if (inputInfo.IsInput) screenPresenter?.InputUpdate(inputInfo);
        OnUpdate();
    }

    public void TransitScreen(T request, bool immediate) => container.TransitScreen(request, immediate);

    void IScreen<T>.Open() { }
    void IScreen<T>.Hide() { }
    IEnumerator IScreen<T>.OpenCoroutine() { yield return OpenCoroutine(); }
    IEnumerator IScreen<T>.HideCoroutine() { yield return HideCoroutine(); }
    void IScreen<T>.SetActive(bool isActive) => gameObject.SetActive(isActive);
    void IScreen<T>.Initialize(IViewModel<T> viewModel) => Initialize((VM)viewModel);
    ScreenContainer<T> IScreen<T>.Container { set => container = value; }
    void IScreen<T>.SetSiblingIndex() => transform.SetSiblingIndex(transform.parent.childCount - 1);
    void IScreen<T>.Deinitialize()
    {
        screenPresenter.Deinitialize();
        Deinitialize();
    }

    protected virtual void Open() { }
    protected virtual void Hide() { }
    protected virtual void Initialize(VM viewModel) { }
    protected virtual void Deinitialize() { }
    protected virtual void OnUpdate() { }
    protected virtual IEnumerator OpenCoroutine() { yield return null; }
    protected virtual IEnumerator HideCoroutine() { yield return null; }


}

public class BaseScreenPresenter<S, SP, VM, T>: IScreenPresenter<T>
    where S : IScreen<T>, new()
    where SP : IScreenPresenter<T>, new()
    where VM : IViewModel<T>, new()
    where T : Enum
{
    IEnumerator IScreenPresenter<T>.Configure(IScreen<T> screen)
    {
        // モデルの構成
        VM viewModel = new VM();
        yield return viewModel.Configure();

        // コントローラーの初期化
        Initialize((S)screen, viewModel);

        // シーンの初期化
       screen.Initialize(viewModel);
    }
    void IScreenPresenter<T>.InputUpdate(InputInfo info) => InputUpdate(info);
    void IScreenPresenter<T>.Deinitialize() => Deinitialize();

    protected virtual void Initialize(S screen, VM viewModel) { }
    protected virtual void InputUpdate(InputInfo info) { }
    protected virtual void Deinitialize() { }

}

public class BaseViewModel<T> : IViewModel<T>
    where T : Enum
{
    IEnumerator IViewModel<T>.Configure() => Configure();

    protected virtual IEnumerator Configure() { yield return null; }
}

public class ScreenContainer<T> where T: Enum
{
    Dictionary<T, IScreen<T>> list = new Dictionary<T, IScreen<T>>();

    IScreen<T> curScreen = default;

    Coroutine coroutine = null;

    public IEnumerator Initialize(T request,bool immediately)
    {
        if (!list.ContainsKey(request))
        {
            Debug.Log($"{request} キーは存在しないため、遷移はしません。");
            yield break;
        }

        var newScreen = list[request];
        if (coroutine == null && curScreen != newScreen)
        {
            yield return TransitCroutine(newScreen, immediately);
        }
    }

    /// <summary>
    /// シーンの追加
    /// </summary>
    /// <param name="id"></param>
    /// <param name="screen"></param>
    public void Add(T id, IScreen<T> screen)
    {
        if (!list.ContainsKey(id))
        {
            list.Add(id, screen);
            if (screen != null)
            {
                screen.Container = this;
                screen.SetActive(false);
            }
            else
            {
                Debug.Log($"スクリーンが設定されていません。");
            }

        }

        else
        {
            Debug.Log($"{id} キーは既に登録されています。");
        }
    }

    /// <summary>
    /// シーンの削除
    /// </summary>
    /// <param name="id"></param>
    public void Remove(T id)
    {
        if (list.ContainsKey(id))
        {
            list.Remove(id);
        }
        else
        {
            Debug.Log($"{id} キーは存在しません。");
        }
    }

    public void Clear()
    {
        list.Clear();
        curScreen = null;
    }

    /// <summary>
    /// 遷移要求
    /// </summary>
    /// <param name="id"></param>
    /// <param name="immediately"></param>
    public void TransitScreen(T request, bool immediately)
    {
        if (!list.ContainsKey(request))
        {
            Debug.Log($"{request} キーは存在しないため、遷移はしません。");
            return;
        }

        var newScreen = list[request];

        if (coroutine == null && curScreen != newScreen)
        {
            coroutine = ProjectManager.Instance.StartCoroutine(TransitCroutine(newScreen, immediately));
        }
    }

    IEnumerator TransitCroutine(IScreen<T> newScreen, bool immediately)
    {
        if (immediately)
        {
            curScreen?.Hide();
            curScreen?.Deinitialize();

            yield return newScreen.Configure();

            newScreen.SetSiblingIndex();
            newScreen.SetActive(true);

            yield return null;

            curScreen?.SetActive(false);

            curScreen = newScreen;

            curScreen.Open();
        }
        else
        {
            yield return curScreen?.HideCoroutine();
            curScreen?.Deinitialize();

            yield return newScreen.Configure();

            newScreen.SetSiblingIndex();
            newScreen.SetActive(true);

            yield return null;

            curScreen?.SetActive(false);

            curScreen = newScreen;

            yield return curScreen?.OpenCoroutine();
        }

        coroutine = null;
    }

    public void Close(bool immediately)
    {
        if (coroutine == null)
        {
            coroutine = ProjectManager.Instance.StartCoroutine(CloseCo());
        }

        IEnumerator CloseCo()
        {
            if (immediately)
            {
                curScreen?.Hide();
            }
            else
            {
                yield return curScreen?.HideCoroutine();
            }

            curScreen?.Deinitialize();

            coroutine = null;

            curScreen = null;
        }
    }
}
