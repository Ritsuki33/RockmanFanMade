using System;


public interface ISubsribeOnlyReactiveProperty<T>
{
    void Subscribe(Action<T> onChangePropertyCallback);
    void Dispose(Action<T> onChangePropertyCallback = null);

    void Refresh();
}

public class ReactiveProperty<T> : ISubsribeOnlyReactiveProperty<T>
{
    T value;

    public ReactiveProperty(T value)
    {
        this.value = value;
    }

    public T Value
    {
        get { return this.value; }
        set
        {
            this.value = value;
            _onChangePropertyCallback?.Invoke(value);
        }
    }

    public void Subscribe(Action<T> onChangePropertyCallback)
    {
        _onChangePropertyCallback += onChangePropertyCallback;
    }

    public void Dispose(Action<T> onChangePropertyCallback = null)
    {
        if (onChangePropertyCallback == null) _onChangePropertyCallback = null;
        else _onChangePropertyCallback -= onChangePropertyCallback;
    }

    public void Refresh()
    {
        _onChangePropertyCallback?.Invoke(value);
    }

    event Action<T> _onChangePropertyCallback = default;
}
