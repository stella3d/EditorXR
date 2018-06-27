using UnityEngine;
using UnityEngine.InputNew;

public class HalfPressableInputControl
{
    const float k_DefaultFullPressThreshold = 0.9f;
    const float k_DefaultHalfPressThreshold = 0.05f;
    float m_HalfPressThreshold;
    float m_FullPressThreshold;
    InputControl m_InputControl;

    public float halfPressThreshold
    {
        get { return m_HalfPressThreshold; }
        set { m_HalfPressThreshold = value; }
    }

    public float fullPressThreshold
    {
        get { return m_FullPressThreshold; }
        set { m_FullPressThreshold = value; }
    }

    public InputControl inputControl
    {
        get { return m_InputControl; }
        set { m_InputControl = value; }
    }

    public bool isHalfPressed { get; private set; }

    public bool isFullPressed { get; private set; }

    public bool wasJustHalfPressed { get; private set; }

    public bool halfWasJustReleased { get; private set; }

    public bool wasJustFullPressed { get; private set; }

    public bool fullWasJustReleased { get; private set; }

    public HalfPressableInputControl(InputControl inputControl, float halfPressThreshold = k_DefaultHalfPressThreshold, float fullPressThreshold = k_DefaultFullPressThreshold)
    {
        m_InputControl = inputControl;
        if (inputControl == null)
            Debug.LogError("Half press input control is being created with a null input control.");

        m_HalfPressThreshold = halfPressThreshold;
        m_FullPressThreshold = fullPressThreshold;
    }

    public void ProcessInput()
    {
        var currentIsHalfPressed = m_InputControl.rawValue >= m_HalfPressThreshold;
        wasJustHalfPressed = !isHalfPressed && currentIsHalfPressed;
        halfWasJustReleased = isHalfPressed && !currentIsHalfPressed;
        isHalfPressed = currentIsHalfPressed;

        var currentIsFullPressed = m_InputControl.rawValue >= m_FullPressThreshold;
        wasJustFullPressed = !isFullPressed && currentIsFullPressed;
        fullWasJustReleased = isFullPressed && !currentIsFullPressed;
        isFullPressed = currentIsFullPressed;
    }
}
