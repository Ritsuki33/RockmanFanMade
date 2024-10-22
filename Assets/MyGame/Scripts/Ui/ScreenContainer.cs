using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScreen
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

    public void Initialize(IViewModel viewModel);
    public void Deinitialize();

    public ScreenContainer Container { set; }

    public void SetSiblingIndex();
}

public interface IScreenController
{
    public void Initialize() { }

    public IEnumerator Configure(IScreen screen);

    public void End() { }

    public void InputUpdate(InputInfo info);
}

public interface IViewModel
{
    public IEnumerator Configure();
}


public class BaseScreen<S, SC, VM> : MonoBehaviour, IScreen
    where S : BaseScreen<S, SC, VM>, new()
    where SC : BaseScreenController<S, SC, VM>, new()
    where VM : BaseViewModel, new()
{
    ScreenContainer container;
    IScreenController screenController = null;

    private IInput InputController => InputManager.Instance;

    IEnumerator IScreen.Configure()
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

    public void TransitScreen(int id, bool immediate) => container.TransitScreen(id, immediate);

    void IScreen.Open() { }
    void IScreen.Hide() { }
    IEnumerator IScreen.OpenCoroutine() { yield return OpenCoroutine(); }
    IEnumerator IScreen.HideCoroutine() { yield return HideCoroutine(); }
    void IScreen.SetActive(bool isActive) => gameObject.SetActive(isActive);
    void IScreen.Initialize(IViewModel viewModel) => Initialize((VM)viewModel);
    ScreenContainer IScreen.Container { set => container = value; }
    void IScreen.SetSiblingIndex() => transform.SetSiblingIndex(transform.parent.childCount - 1);
    void IScreen.Deinitialize() => Deinitialize();

    protected virtual void Open() { }
    protected virtual void Hide() { }
    protected virtual void Initialize(VM viewModel) { }
    protected virtual void Deinitialize() { }
    protected virtual void OnUpdate() { }
    protected virtual IEnumerator OpenCoroutine() { yield return null; }
    protected virtual IEnumerator HideCoroutine() { yield return null; }


}

public class BaseScreenController<S, SC, VM>: IScreenController
    where S : BaseScreen<S, SC, VM>, new()
    where SC : BaseScreenController<S, SC, VM>, new()
    where VM : BaseViewModel, new()
{

    IEnumerator IScreenController.Configure(IScreen screen)
    {
        // モデルの構成
        IViewModel viewModel = new VM();
        yield return viewModel.Configure();

        // コントローラーの初期化
        Initialize((S)screen, (VM)viewModel);

        // シーンの初期化
       screen.Initialize(viewModel);
    }
    void IScreenController.InputUpdate(InputInfo info) => InputUpdate(info);

    protected virtual void Initialize(S screen, VM viewModel) { }
    protected virtual void InputUpdate(InputInfo info) { }
}

public class BaseViewModel : IViewModel
{
    IEnumerator IViewModel.Configure()
    {
        yield return Configure();
    }

    protected virtual IEnumerator Configure() { yield return null; }
}


public class ScreenContainer
{
    Dictionary<int, IScreen> list = new Dictionary<int, IScreen>();

    IScreen curScreen = default;

    private int currentId = -1;

    Coroutine coroutine = null;
    /// <summary>
    /// シーンの追加
    /// </summary>
    /// <param name="id"></param>
    /// <param name="screen"></param>
    public void Add(int id, IScreen screen)
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
    public void Remove(int id)
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
    public void TransitScreen(int requestId, bool immediately)
    {
        if (!list.ContainsKey(requestId))
        {
            Debug.Log($"{requestId} キーは存在しないため、遷移はしません。");
            return;
        }

        if (coroutine == null && requestId >= 0 && currentId != requestId)
        {
            coroutine = ProjectManager.Instance.StartCoroutine(TransitCroutine(requestId, immediately));
        }


        IEnumerator TransitCroutine(int requestId, bool immediately)
        {
            if (immediately)
            {
                curScreen?.Hide();
                curScreen?.Deinitialize();

                var newScreen = list[requestId];
                currentId = requestId;

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

                var newScreen = list[requestId];
                currentId = requestId;

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
