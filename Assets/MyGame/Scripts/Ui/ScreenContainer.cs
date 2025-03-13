using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScreen<T> where T : Enum
{
    IScreenPresenter<T> ScreenPresenter { get; }

    /// <summary>
    /// ビュー、コントローラー、ビューモデルの準備
    /// </summary>
    /// <returns></returns>
    public IEnumerator Configure();

    public IEnumerator PlayOpen();
    public IEnumerator PlayHide();

    public void Open();   // 表示
    public void Hide();         // 非表示

    public void OnUpdate();
    public IEnumerator OpenCoroutine(); // 表示
    public IEnumerator HideCoroutine();   // 非表示

    public void SetActive(bool isActive);

    public IEnumerator Destroy();

    public void SetSiblingIndex();

}

public interface IScreenPresenter<T> where T : Enum
{
    public IEnumerator Configure(IScreen<T> screen);

    public void BeforeOpen();
    public void Open();
    public void AfterOpen();

    public void BeforeHide();
    public void Hide();
    public void AfterHide();

    public IEnumerator Destroy();

    public void InputUpdate(InputInfo info);
    public ScreenContainer<T> Container { set; }
}

public interface IViewModel<T> where T : Enum
{
    public IEnumerator Configure();

    public IEnumerator Destroy();
}

public class BaseScreen<S, SP, T> : MonoBehaviour, IScreen<T>
    where S : IScreen<T>, new()
    where SP : IScreenPresenter<T>, new()
    where T : Enum
{
    [SerializeField] BaseTweemAnimator tweemAnimator = default;

    public IEnumerator PlayOpen()
    {
        if (tweemAnimator == null) yield break;

        bool completed = false;
        tweemAnimator.PlayOpen(() => completed = true);
        while (!completed)
        {
            yield return null;
        }
    }

    public IEnumerator PlayHide()
    {
        if (tweemAnimator == null) yield break;

        bool completed = false;
        tweemAnimator.PlayClose(() => completed = true);
        while (!completed)
        {
            yield return null;
        }
    }

    IScreenPresenter<T> screenPresenter = null;

    private IInput InputController => InputManager.Instance;

    IScreenPresenter<T> IScreen<T>.ScreenPresenter => screenPresenter;

    public SP ScreenPresenter => (SP)screenPresenter;

    public IEnumerator Configure()
    {
        if (screenPresenter == null) screenPresenter = new SP();

        yield return screenPresenter.Configure(this);
    }

    void IScreen<T>.OnUpdate()
    {
        InputInfo inputInfo = default;
        inputInfo.SetInput(InputController);

        // コントローラーへ入力情報を委譲
        if (inputInfo.IsInput) screenPresenter?.InputUpdate(inputInfo);
        OnUpdate();
    }

    void IScreen<T>.Open() => Open();
    void IScreen<T>.Hide() => Hide();
    IEnumerator IScreen<T>.OpenCoroutine() { yield return OpenCoroutine(); }
    IEnumerator IScreen<T>.HideCoroutine() { yield return HideCoroutine(); }
    void IScreen<T>.SetActive(bool isActive) => gameObject.SetActive(isActive);
    void IScreen<T>.SetSiblingIndex() => transform.SetSiblingIndex(transform.parent.childCount - 1);
    public IEnumerator Destroy()
    {
        yield return screenPresenter.Destroy();
    }

    protected virtual void Open() { }
    protected virtual void Hide() { }
    protected virtual void OnUpdate() { }
    protected virtual IEnumerator OpenCoroutine() { yield return null; }
    protected virtual IEnumerator HideCoroutine() { yield return null; }


}

public class BaseScreenPresenter<S, SP, VM, T> : IScreenPresenter<T>
    where S : IScreen<T>, new()
    where SP : IScreenPresenter<T>, new()
    where VM : IViewModel<T>, new()
    where T : Enum
{
    ScreenContainer<T> container;
    protected S m_screen;
    protected VM m_viewModel;

    IEnumerator IScreenPresenter<T>.Configure(IScreen<T> screen)
    {
        // モデルの構成
        m_viewModel = new VM();
        yield return m_viewModel.Configure();

        m_screen = (S)screen;
        // プレゼンターの初期化
        Initialize();
    }

    ScreenContainer<T> IScreenPresenter<T>.Container { set => container = value; }

    void IScreenPresenter<T>.InputUpdate(InputInfo info) => InputUpdate(info);
    IEnumerator IScreenPresenter<T>.Destroy()
    {
        yield return m_viewModel.Destroy();

        // プレゼンターの終了処理
        Destroy();
    }

    void IScreenPresenter<T>.BeforeOpen() => BeforeOpen();
    void IScreenPresenter<T>.Open() => Open();
    void IScreenPresenter<T>.AfterOpen() => AfterOpen();
    void IScreenPresenter<T>.BeforeHide() => BeforeHide();
    void IScreenPresenter<T>.Hide() => Hide();
    void IScreenPresenter<T>.AfterHide() => AfterHide();

    protected void TransitToScreen(T nextScreen, bool immediate = false)
    {
        container.TransitScreen(nextScreen, immediate);
    }

    protected virtual void Initialize() { }
    protected virtual void BeforeOpen() { }
    protected virtual void Open() { }
    protected virtual void AfterOpen() { }
    protected virtual void BeforeHide() { }
    protected virtual void Hide() { }
    protected virtual void AfterHide() { }
    protected virtual void InputUpdate(InputInfo info) { }
    protected virtual void Destroy() { }
}

public class BaseViewModel<T> : IViewModel<T>
    where T : Enum
{
    IEnumerator IViewModel<T>.Configure() => Configure();
    IEnumerator IViewModel<T>.Destroy() => Destroy();

    protected virtual IEnumerator Configure() { yield return null; }
    protected virtual IEnumerator Destroy() { yield return null; }
}

public class ScreenContainer<T> where T : Enum
{
    Dictionary<T, IScreen<T>> list = new Dictionary<T, IScreen<T>>();

    IScreen<T> curScreen = default;

    Coroutine coroutine = null;

    public void OnUpdate()
    {
        if (curScreen != null) curScreen.OnUpdate();
    }

    public IEnumerator Initialize(T request, bool immediately)
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
            curScreen?.ScreenPresenter?.BeforeHide();
            yield return curScreen?.PlayHide();
            curScreen?.Hide();
            curScreen?.ScreenPresenter?.Hide();
            curScreen?.ScreenPresenter?.AfterHide();

            yield return curScreen?.Destroy();

            yield return newScreen.Configure();

            newScreen.SetSiblingIndex();
            newScreen.SetActive(true);
            curScreen?.SetActive(false);

            curScreen = newScreen;

            if (curScreen?.ScreenPresenter != null) curScreen.ScreenPresenter.Container = this;

            curScreen?.ScreenPresenter?.Open();
            curScreen?.ScreenPresenter?.BeforeOpen();
            curScreen.Open();
            yield return curScreen.PlayOpen();
            curScreen?.ScreenPresenter?.AfterOpen();
        }
        else
        {
            curScreen?.ScreenPresenter?.BeforeHide();
            yield return curScreen?.PlayHide();
            yield return curScreen?.HideCoroutine();
            curScreen?.ScreenPresenter?.Hide();
            curScreen?.ScreenPresenter?.AfterHide();

            yield return curScreen?.Destroy();

            yield return newScreen.Configure();

            newScreen.SetSiblingIndex();
            newScreen.SetActive(true);

            yield return null;

            curScreen?.SetActive(false);

            curScreen = newScreen;

            curScreen?.ScreenPresenter?.BeforeOpen();
            curScreen?.ScreenPresenter?.Open();
            yield return curScreen?.OpenCoroutine();
            yield return curScreen?.PlayOpen();
            curScreen?.ScreenPresenter?.AfterOpen();
        }

        coroutine = null;
    }

    public void Close(bool immediately, Action closeCallback)
    {
        if (coroutine == null)
        {
            coroutine = ProjectManager.Instance.StartCoroutine(CloseCo());
        }

        IEnumerator CloseCo()
        {
            curScreen?.ScreenPresenter?.BeforeHide();
            yield return curScreen?.PlayHide();
            curScreen?.ScreenPresenter?.Hide();

            if (immediately)
            {
                curScreen?.Hide();
            }
            else
            {
                yield return curScreen?.HideCoroutine();
            }

            curScreen?.ScreenPresenter?.AfterHide();

            yield return curScreen?.Destroy();

            coroutine = null;

            curScreen = null;

            closeCallback?.Invoke();
        }
    }
}
