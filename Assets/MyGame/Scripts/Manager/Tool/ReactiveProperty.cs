using System;

public interface IReadOnlyReactiveProperty<T>
{
    void Subscribe(Action<T> onChangePropertyCallback);
}

public class ReactiveProperty<T> : IReadOnlyReactiveProperty<T>
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
        _onChangePropertyCallback = onChangePropertyCallback;
    }

    event Action<T> _onChangePropertyCallback = default;
}
