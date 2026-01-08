// 실시간으로 플레이어의 스탯이 변경되는 것 들을 담는다
public class ObservableProperty<T> where T : struct
{
    private T _value;

    public T Value
    {
        get => _value;
        set
        {
            _value = value;
            OnValueChanged?.Invoke(_value);
        }
    }

    public event Action<T> OnValueChanged;

    public ObservableProperty(T value = default)
    {
        _value = value;
    }

    // 함수 추가 ( 구독자 추가 )
    public void AddListener(Action<T> action)
    {
        OnValueChanged += action;
    }
    public void RemoveListener(Action<T> action)
    {
        OnValueChanged -= action;
    }
    public void ClearListener()
    {
        OnValueChanged = null;
    }
}