using UnityEngine;
using UnityEngine.InputSystem;

public enum InputType
{
    Up = 1,
    Down = 2,
    Left = 4,
    Right = 8,
    UpLeft = Up | Left,
    URight = Up | Right,
    DownLeft = Down | Left,
    DownRight = Down | Right,
    UpDownRightLeft = Up | Down | Right | Left,
    Cancel = 16,
    Decide = 32,
}
public interface IInput
{
    bool GetInput(InputType type);
    bool GetDownInput(InputType type);
}

public class InputManager : SingletonComponent<InputManager>, IInput
{
    PlayerInput playerInput;

    int inputBitFlag = 0;
    int preInputbitFlag = 0;
    int inputDownBitFlag = 0;   // 入力の瞬間を管理するビット群
    int inputUpBitFlag = 0;     // 入力が離れる瞬間を管理するビット群

    protected override void Awake()
    {
        base.Awake();
        playerInput = new PlayerInput();

        playerInput.Player.Move.performed += OnMove;
        playerInput.Player.Move.canceled += OffMove;
        playerInput.Player.Cancel.performed += OnCancel;
        playerInput.Player.Cancel.canceled += OffCancel;
        playerInput.Player.Move.Enable();
        playerInput.Player.Cancel.Enable();

        playerInput.Player.Decide.performed += OnDecide;
        playerInput.Player.Decide.canceled += OffDecide;
        playerInput.Player.Decide.Enable();

    }

    private void Update()
    {
        // 各種桁 0→1になる瞬間だけ1になる
        // 0,0→0 0,1→1 1,0→0 1,1→0
        inputDownBitFlag = ~preInputbitFlag & inputBitFlag;


        // 各種桁 1→0になる瞬間だけ1になる
        // 0,0→0 0,1→0 1,0→1 1,1→0
        inputUpBitFlag = preInputbitFlag & ~inputBitFlag;


        preInputbitFlag = inputBitFlag;
    }

    /// <summary>
    /// 入力状態を取得
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool GetInput(InputType type)
    {
        return (inputBitFlag & (int)type) != 0;
    }

    /// <summary>
    /// 入力の瞬間を取得
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool GetDownInput(InputType type)
    {
        return (inputDownBitFlag & (int)type) != 0;
    }

    /// <summary>
    /// 入力を離した瞬間を取得
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool GetUpInput(InputType type)
    {
        return (inputUpBitFlag & (int)type) != 0;
    }

    void OnMove(InputAction.CallbackContext context)
    {
        var vector = context.ReadValue<Vector2>();

        if (vector.x > 0) OnInputBit(InputType.Right);
        else OffInputBit(InputType.Right);
        if (vector.x < 0) OnInputBit(InputType.Left);
        else OffInputBit(InputType.Left);

        if (vector.y > 0) OnInputBit(InputType.Up);
        else OffInputBit(InputType.Up);
        if (vector.y < 0) OnInputBit(InputType.Down);
        else OffInputBit(InputType.Down);

    }

    void OffMove(InputAction.CallbackContext context)
    {
        OffInputBit(InputType.UpDownRightLeft);
    }

    void OnCancel(InputAction.CallbackContext context)
    {
        OnInputBit(InputType.Cancel);
    }

    void OffCancel(InputAction.CallbackContext context)
    {
        OffInputBit(InputType.Cancel);
    }

    void OnDecide(InputAction.CallbackContext context)
    {
        OnInputBit(InputType.Decide);
    }

    void OffDecide(InputAction.CallbackContext context)
    {
        OffInputBit(InputType.Decide);
    }

    /// <summary>
    /// ビットを１にする
    /// </summary>
    /// <param name="type"></param>
    void OnInputBit(InputType type)
    {
        inputBitFlag |= (int)type;
    }

    /// <summary>
    /// ビットを０にする
    /// </summary>
    /// <param name="type"></param>
    void OffInputBit(InputType type)
    {
        inputBitFlag &= ~(int)type;
    }


}
