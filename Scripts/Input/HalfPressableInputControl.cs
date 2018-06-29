using UnityEngine;
using UnityEngine.InputNew;

public class HalfPressableInputControl
{
    const float k_DefaultFullPressThreshold = 0.95f;
    const float k_DefaultHalfPressThreshold = 0.02f;
    const float k_DefaultHalfPressMinimumDuration = 0.3f;

    float m_HalfPressThreshold;
    float m_FullPressThreshold;
    float m_HalfPressMinimumDuration;
    float m_TimeInHalfRange;

    InputControl m_InputControl;

    public bool isPressed { get; private set; }

    public bool isHalfPressed { get; private set; }

    public bool isFullPressed { get; private set; }

    public bool wasJustPressed {get; private set; }
    
    public bool wasJustHalfPressed { get; private set; }

    public bool wasJustFullPressed { get; private set; }

    public bool wasJustReleased { get; private set; }

    public bool halfWasJustReleased { get; private set; }

    public bool fullWasJustReleased { get; private set; }

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

    public float halfToFullValue
    {
        get { return Mathf.Clamp01((m_InputControl.rawValue - m_HalfPressThreshold) / (m_FullPressThreshold - m_HalfPressThreshold)); }
    }

    public HalfPressableInputControl(InputControl inputControl, 
        float halfPressThreshold = k_DefaultHalfPressThreshold, float fullPressThreshold = k_DefaultFullPressThreshold, 
        float halfPressMinimumDuration = k_DefaultHalfPressMinimumDuration)
    {
        m_InputControl = inputControl;
        if (inputControl == null)
            Debug.LogError("Half press input control is being created with a null input control.");

        m_HalfPressThreshold = halfPressThreshold;
        m_FullPressThreshold = fullPressThreshold;
        m_HalfPressMinimumDuration = halfPressMinimumDuration;
    }

    public void ProcessInput()
    {
        var rawValue = m_InputControl.rawValue;
        var currentIsPressed = rawValue >= m_HalfPressThreshold;
        wasJustPressed = !isPressed && currentIsPressed;
        wasJustReleased = isPressed && !currentIsPressed;
        isPressed = currentIsPressed;        
        
        var inHalfPressRange = rawValue >= m_HalfPressThreshold && rawValue < m_FullPressThreshold;
        m_TimeInHalfRange = inHalfPressRange ? (m_TimeInHalfRange + Time.deltaTime) : 0f;
        var currentIsHalfPressed = m_TimeInHalfRange >= m_HalfPressMinimumDuration;
        wasJustHalfPressed = !isHalfPressed && currentIsHalfPressed;
        halfWasJustReleased = isHalfPressed && !currentIsHalfPressed;
        isHalfPressed = currentIsHalfPressed;

        var currentIsFullPressed = rawValue >= m_FullPressThreshold;
        wasJustFullPressed = !isFullPressed && currentIsFullPressed;
        fullWasJustReleased = isFullPressed && !currentIsFullPressed;
        isFullPressed = currentIsFullPressed;
    }
}
