using System;

public interface IDisposableReactiveProperty
{
    void Dispose();
}

public interface IReadOnlyReactiveProperty<T>
{
    IDisposableReactiveProperty Subscribe(Action<T> onChangePropertyCallback);
}

public class ReactiveProperty<T> : IReadOnlyReactiveProperty<T>, IDisposableReactiveProperty
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

    public IDisposableReactiveProperty Subscribe(Action<T> onChangePropertyCallback)
    {
        _onChangePropertyCallback = onChangePropertyCallback;

        return this;
    }

    public void Dispose()
    {
        _onChangePropertyCallback = null;
    }

    event Action<T> _onChangePropertyCallback = default;
}
