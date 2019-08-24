using System;
using UnityEngine;

[Serializable]
public class AxisConfiguration
{
    private const float Neutral = 0.0f;
    private const float Positive = 1.0f;
    private const float Negative = -1.0f;

    public string name;
    public string description;
    public KeyCode positive;
    public KeyCode negative;
    public KeyCode altPositive;
    public KeyCode altNegative;
    public float deadZone;
    public float gravity = 3.0f;
    public float sensitivity = 1.0f;
    public bool snap;
    public bool invert;
    public InputType type = InputType.DigitalAxis;
    public int axis;
    public int joystick;

    private float _value;
    private int _lastAxis;
    private int _lastJoystick;
    private InputType _lastType;
    private float _deltaTime;

    public bool AnyInput
    {
        get
        {
            if (type == InputType.Button)
                return (Input.GetKey(positive) || Input.GetKey(altPositive));
            if (type == InputType.DigitalAxis)
                return Mathf.Abs(_value) >= 1.0f;

            return false;
        }
    }

    public AxisConfiguration(string name)
    {
        this.name = name;
        description = string.Empty;
        positive = KeyCode.None;
        altPositive = KeyCode.None;
        negative = KeyCode.None;
        altNegative = KeyCode.None;
        type = InputType.Button;
        gravity = 1.0f;
        sensitivity = 1.0f;
    }

    public void Update()
    {
        _deltaTime = Time.deltaTime;

        if (_lastType != type || _lastAxis != axis || _lastJoystick != joystick)
        {
            if (_lastType != type && type == InputType.DigitalAxis)
                _value = Neutral;

            _lastType = type;
            _lastAxis = axis;
            _lastJoystick = joystick;
        }

        bool positiveAndNegativeDown = (Input.GetKey(positive) || Input.GetKey(altPositive)) &&
                                        (Input.GetKey(negative) || Input.GetKey(altNegative));
        if (type == InputType.DigitalAxis && !positiveAndNegativeDown)
        {
            UpdateDigitalAxisValue();
        }
    }

    private void UpdateDigitalAxisValue()
    {
        if (Input.GetKey(positive) || Input.GetKey(altPositive))
        {
            if (_value < Neutral && snap)
            {
                _value = Neutral;
            }

            _value += sensitivity * _deltaTime;
            if (_value > Positive)
            {
                _value = Positive;
            }
        }
        else if (Input.GetKey(negative) || Input.GetKey(altNegative))
        {
            if (_value > Neutral && snap)
            {
                _value = Neutral;
            }

            _value -= sensitivity * _deltaTime;
            if (_value < Negative)
            {
                _value = Negative;
            }
        }
        else
        {
            if (_value < Neutral)
            {
                _value += gravity * _deltaTime;
                if (_value > Neutral)
                {
                    _value = Neutral;
                }
            }
            else if (_value > Neutral)
            {
                _value -= gravity * _deltaTime;
                if (_value < Neutral)
                {
                    _value = Neutral;
                }
            }
        }
    }

    public float GetAxis()
    {
        float axis = Neutral;

        if (type == InputType.DigitalAxis)
        {
            axis = _value;
        }
        return invert ? -axis : axis;
    }

    public float GetAxisRaw()
    {
        float axis = Neutral;

        if (type == InputType.DigitalAxis)
        {
            if (Input.GetKey(positive) || Input.GetKey(altPositive))
                axis = Positive;
            else if (Input.GetKey(negative) || Input.GetKey(altNegative))
                axis = Negative;
        }

        return invert ? -axis : axis;
    }

    public bool GetButton()
    {
        return Input.GetKey(positive) || Input.GetKey(altPositive) || Input.GetKey(negative) || Input.GetKey(altNegative);
    }

    public bool GetButtonDown()
    {
        return Input.GetKeyDown(positive) || Input.GetKeyDown(altPositive) || Input.GetKeyDown(negative) || Input.GetKeyDown(altNegative);
    }

    public bool GetButtonUp()
    {
        return Input.GetKeyUp(positive) || Input.GetKeyUp(altPositive) || Input.GetKeyUp(negative) || Input.GetKeyUp(altNegative);
    }

    public void Copy(AxisConfiguration source)
    {
        name = source.name;
        description = source.description;
        positive = source.positive;
        altPositive = source.altPositive;
        negative = source.negative;
        altNegative = source.altNegative;
        deadZone = source.deadZone;
        gravity = source.gravity;
        sensitivity = source.sensitivity;
        snap = source.snap;
        invert = source.invert;
        type = source.type;
        axis = source.axis;
        joystick = source.joystick;
    }

    public static KeyCode StringToKey(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return KeyCode.None;
        }
        try
        {
            return (KeyCode)Enum.Parse(typeof(KeyCode), value, true);
        }
        catch
        {
            return KeyCode.None;
        }
    }
}
