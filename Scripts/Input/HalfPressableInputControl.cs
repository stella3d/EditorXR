using UnityEngine;
using UnityEngine.InputNew;

public class HalfPressableInputControl
{
    const float k_DefaultFullPressThreshold = 0.9f;
    const float k_DefaultHalfPressThreshold = 0.05f;
    float m_HalfPressThreshold;
    float m_FullPressThreshold;
    InputControl m_InputControl;

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

        if (wasJustFullPressed || wasJustHalfPressed || fullWasJustReleased || halfWasJustReleased)
        {
            Debug.Log("Half: " + isHalfPressed + " Full: " + isFullPressed);
            if (!isHalfPressed && !isFullPressed)
            {
                Debug.Log("Currently not pressed: " + m_InputControl.rawValue);
            }
        }


        // ResetJustChangedStates();

//        if (m_InputControl.rawValue < m_HalfPressThreshold) // Less than half pressed
//        {
//            if (isHalfPressed)
//            {
//                halfWasJustReleased = true;
//                isHalfPressed = false;
//            }
//        }
//        else // More than half pressed
//        {
//            if (!isHalfPressed)
//            {
//                wasJustHalfPressed = true;
//                isHalfPressed = true;
//            }
//        }
//
//        if (m_InputControl.rawValue < m_FullPressThreshold) // Less than full pressed
//        {
//            if (isFullPressed)
//            {
//                fullWasJustReleased = true;
//                isFullPressed = false;
//            }
//        }
//        else // full pressed
//        {
//            if (!isFullPressed)
//            {
//                wasJustFullPressed = true;
//                isFullPressed = true;
//            }
//        }
    }
//
//    void ResetJustChangedStates()
//    {
//        wasJustFullPressed = false;
//        wasJustHalfPressed = false;
//        halfWasJustReleased = false;
//        fullWasJustReleased = false;
//    }
}
