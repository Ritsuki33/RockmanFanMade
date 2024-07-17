using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

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
    Jump = 16,
}
public interface IInput
{
    bool GetInput(InputType type);
}

public class DummyInput: IInput
{
    public bool GetInput(InputType type)
    {
        return false;
    }
}

public class InputManager : SingletonComponent<InputManager>, IInput
{
    PlayerInput playerInput;

    int inputBitFlag = 0;

    public int InputBitFlag { get; set; }

    protected override void Awake()
    {
        base.Awake();
        playerInput = new PlayerInput();

        playerInput.Player.Move.performed += OnMove;
        playerInput.Player.Move.canceled += OffMove;
        playerInput.Player.Jump.performed += OnJump;
        playerInput.Player.Jump.canceled += OffJump;
        playerInput.Player.Move.Enable();
        playerInput.Player.Jump.Enable();
    }

    public bool GetInput(InputType type)
    {
        return (inputBitFlag & (int)type) != 0;
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

    void OnJump(InputAction.CallbackContext context)
    {
        OnInputBit(InputType.Jump);
    }

    void OffJump(InputAction.CallbackContext context)
    {
        OffInputBit(InputType.Jump);
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
