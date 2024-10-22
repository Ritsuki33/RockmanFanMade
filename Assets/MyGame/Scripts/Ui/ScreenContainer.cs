using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScreen<T> where T : Enum
{
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

public interface IScreenController<T> where T : Enum
{
    public void Initialize() { }

    public IEnumerator Configure(IScreen<T> screen);

    public void End() { }

    public void InputUpdate(InputInfo info);
}

public interface IViewModel<T> where T : Enum
{
    public IEnumerator Configure();
}


public class BaseScreen<S, SC, VM, T> : MonoBehaviour, IScreen<T>
    where S : BaseScreen<S, SC, VM, T>, new()
    where SC : BaseScreenController<S, SC, VM, T>, new()
    where VM : BaseViewModel<T>, new()
    where T : Enum
{
    ScreenContainer<T> container;
    IScreenController<T> screenController = null;

    private IInput InputController => InputManager.Instance;

    IEnumerator IScreen<T>.Configure()
    {
        screenController = new SC();

        yield return screenController.Configure(this);
    }

    private void Update()
    {
        InputInfo inputInfo = default;
        inputInfo.SetInput(InputController);

        // コントローラーへ入力情報を委譲
        if (inputInfo.IsInput) screenController?.InputUpdate(inputInfo);
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
    void IScreen<T>.Deinitialize() => Deinitialize();

    protected virtual void Open() { }
    protected virtual void Hide() { }
    protected virtual void Initialize(VM viewModel) { }
    protected virtual void Deinitialize() { }
    protected virtual void OnUpdate() { }
    protected virtual IEnumerator OpenCoroutine() { yield return null; }
    protected virtual IEnumerator HideCoroutine() { yield return null; }


}

public class BaseScreenController<S, SC, VM, T>: IScreenController<T>
    where S : BaseScreen<S, SC, VM, T>, new()
    where SC : BaseScreenController<S, SC, VM, T>, new()
    where VM : BaseViewModel<T>, new()
    where T : Enum
{

    IEnumerator IScreenController<T>.Configure(IScreen<T> screen)
    {
        // モデルの構成
        IViewModel<T> viewModel = new VM();
        yield return viewModel.Configure();

        // コントローラーの初期化
        Initialize((S)screen, (VM)viewModel);

        // シーンの初期化
       screen.Initialize(viewModel);
    }
    void IScreenController<T>.InputUpdate(InputInfo info) => InputUpdate(info);

    protected virtual void Initialize(S screen, VM viewModel) { }
    protected virtual void InputUpdate(InputInfo info) { }
}

public class BaseViewModel<T> : IViewModel<T>
    where T : Enum
{
    IEnumerator IViewModel<T>.Configure()
    {
        yield return Configure();
    }

    protected virtual IEnumerator Configure() { yield return null; }
}


public class ScreenContainer<T> where T: Enum
{
    Dictionary<T, IScreen<T>> list = new Dictionary<T, IScreen<T>>();

    IScreen<T> curScreen = default;

    Coroutine coroutine = null;
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
            screen.Container = this;
            screen.SetActive(false);
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

    public void Clear()=> list.Clear();

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


        IEnumerator TransitCroutine(IScreen<T> newScene, bool immediately)
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

                var newScreen = list[request];

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
    }
}
