using UnityEngine;
using System;
using System.Collections;

public class InputManager : MonoBehaviour
{
    #region Properties
    private bool _JumpInputDetected
    {
        get
        {
            return Input.GetKeyDown(KeyCode.Space);
        }
    }

    private bool _ForwardInputDetected
    {
        get
        {
            return Input.GetKey(KeyCode.W);
        }
    }

    private bool _BackInputDetected
    {
        get
        {
            return Input.GetKey(KeyCode.S);
        }
    }

    private bool _LeftInputDetected
    {
        get
        {
            return Input.GetKey(KeyCode.A);
        }
    }

    private bool _RightInputDetected
    {
        get
        {
            return Input.GetKey(KeyCode.D);
        }
    }

    private bool _NoMovementDetected
    {
        get
        {
            return new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")) != Vector3.zero ? false : true;
        }
    }

    private bool _SprintInputDetected
    {
        get
        {
            return Input.GetKey(KeyCode.LeftShift);
        }
    }

    private bool _CrouchInputDetected
    {
        get
        {
            return Input.GetKey(KeyCode.CapsLock);
        }
    }
    #endregion

    void Update()
    {
        if (_JumpInputDetected)
        {
            EventHandler.Instance.Broadcast(new JumpEvent());
        }

        if (_ForwardInputDetected)
        {
            EventHandler.Instance.Broadcast(new ForwardMovementEvent());
        }
        else if (_BackInputDetected)
        {
            EventHandler.Instance.Broadcast(new BackMovementEvent());
        }
        if (_LeftInputDetected)
        {
            EventHandler.Instance.Broadcast(new LeftMovementEvent());
        }
        else if (_RightInputDetected)
        {
            EventHandler.Instance.Broadcast(new RightMovementEvent());
        }

        if (_SprintInputDetected && _ForwardInputDetected)
        {
            EventHandler.Instance.Broadcast(new SprintMovementEvent());
        }
        else
        {
            EventHandler.Instance.Broadcast(new SprintEndEvent());
        }

        if (_CrouchInputDetected)
        {
            EventHandler.Instance.Broadcast(new CrouchMovementEvent());
        }
        else
        {
            EventHandler.Instance.Broadcast(new CrouchEndEvent());
        }

        if (_NoMovementDetected)
        {
            EventHandler.Instance.Broadcast(new NoInputEvent());
        }
    }
}
