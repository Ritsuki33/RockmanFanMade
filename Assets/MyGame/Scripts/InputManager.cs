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
    Cancel = 16,
    Decide = 32,
}
public interface IInput
{
    bool GetInput(InputType type);
    bool GetDownInput(InputType type);
}

//public class DummyInput: IInput
//{
//    public bool GetInput(InputType type)
//    {
//        return false;
//    }
//}

public class InputManager : SingletonComponent<InputManager>, IInput
{
    PlayerInput playerInput;

    int inputBitFlag = 0;
    int inputDownBitFlag = 0;

    public int InputBitFlag { get; set; }

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

    public bool GetInput(InputType type)
    {
        return (inputBitFlag & (int)type) != 0;
    }


    public bool GetDownInput(InputType type)
    {
        bool input = GetInput(type);

        if (!input)
        {
            inputDownBitFlag &= ~(int)type;
            return false;
        }

        if ((inputDownBitFlag & (int)type) != 0)
        {
            return false;
        }
        else
        {
            inputDownBitFlag |= (int)type;
            return true;
        }
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
