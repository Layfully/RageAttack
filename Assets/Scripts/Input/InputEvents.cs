using System;
using System.Collections.Generic;
using UnityEngine;

public delegate void InputDelegate();

public class InputEvents : MonoBehaviour
{
    private static List<AxisEvent> axisEventList;
    private static List<ButtonEvent> buttonEventList;
    private bool shouldCallMethod = false;
    public static void RegisterAxisEvent(string name, InputDelegate method)
    {
        if (axisEventList == null)
        {
            axisEventList = new List<AxisEvent>();
        }

        AxisEvent existingEvent = GetAxisEventFromList(name);

        if (existingEvent == null)
        {
            existingEvent = CreateAxisEvent(name);
            axisEventList.Add(existingEvent);
        }

        existingEvent.method += method;
    }

    private static AxisEvent GetAxisEventFromList(string name)
    {
        if (axisEventList != null)
        {
            foreach (AxisEvent e in axisEventList)
            {
                if (e.Name == name)
                {
                    return e;
                }
            }
        }

        return null;
    }

    private static AxisEvent CreateAxisEvent(string name)
    {
        AxisEvent newEvent = new AxisEvent { Name = name };


        return newEvent;
    }

    public static void RegisterButtonEvent(InputPhase phase, string name, InputDelegate method)
    {
        if (buttonEventList == null)
        {
            buttonEventList = new List<ButtonEvent>();
        }

        ButtonEvent existingEvent = GetButtonEventFromList(phase, name);

        if (existingEvent == null)
        {
            existingEvent = CreateButtonEvent(phase, name);
            buttonEventList.Add(existingEvent);
        }

        existingEvent.method += method;
    }

    public static void ClearEvents()
    {
        axisEventList.Clear();
        buttonEventList.Clear();
    }

    private static ButtonEvent GetButtonEventFromList(InputPhase phase, string name)
    {
        if (buttonEventList != null)
        {
            foreach (ButtonEvent e in buttonEventList)
            {
                if (e.Phase == phase && e.Name == name)
                {
                    return e;
                }
            }
        }

        return null;
    }

    private static ButtonEvent CreateButtonEvent(InputPhase phase, string name)
    {
        ButtonEvent newEvent = new ButtonEvent
        {
            Phase = phase,
            Name = name
        };


        return newEvent;
    }

    private void Update()
    {
        CheckKeyboardInput();
    }

    private void CheckKeyboardInput()
    {
        if (axisEventList != null)
        {
            for (int i = 0; i < axisEventList.Count; i++)
            {
                axisEventList[i].ExecuteMethod();
            }
        }

        if (buttonEventList != null)
        {
            for (int i = 0; i < buttonEventList.Count; i++)
            {
                shouldCallMethod = false;

                switch (buttonEventList[i].Phase)
                {
                    case InputPhase.OnPressed:
                        shouldCallMethod = InputManager.GetButtonDown(buttonEventList[i].Name);
                        break;
                    case InputPhase.OnHold:
                        shouldCallMethod = InputManager.GetButton(buttonEventList[i].Name);
                        break;
                    case InputPhase.OnReleased:
                        shouldCallMethod = InputManager.GetButtonUp(buttonEventList[i].Name);
                        break;
                }
                if (shouldCallMethod)
                {
                    buttonEventList[i].ExecuteMethod();
                }
            }

            foreach (ButtonEvent e in buttonEventList)
            {





            }
        }
    }
}

public class AxisEvent
{
    public string Name { get; set; }
    public event InputDelegate method;

    public void ExecuteMethod()
    {
        if (method != null) method();
    }
}

public class ButtonEvent
{
    public String Name { get; set; }
    public InputPhase Phase { get; set; }
    public event InputDelegate method;

    public void ExecuteMethod()
    {
        if (method != null) method();
    }
}