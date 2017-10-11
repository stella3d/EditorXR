namespace UnityEditor.Experimental.EditorVR.Proxies
{
    public enum VRControl
    {
        // Standardized.
        Analog0,

        LeftStickX = Analog0,
        LeftStickY,

        Trigger1,
        Trigger2,

        Analog4,
        Analog5,
        Analog6,
        Analog7,
        Analog8,
        Analog9,

        LocalPositionX,
        LocalPositionY,
        LocalPositionZ,

        LocalRotationX,
        LocalRotationY,
        LocalRotationZ,
        LocalRotationW,

        Action1,
        Action2,
        Action3,
        Action4,
        Action5,

        LeftStickButton,

        // Compound controls.

        LocalPosition,
        LocalRotation,
        LeftStick,

        // Not standardized, but provided for convenience.

        Back,
        Start,
        //Select,
        //Pause,
        //Menu,
        //Share,
        //View,
        //Options

        Action1Touch,
        Action2Touch,
        Trigger1Touch,
        Trigger1NearTouch,
        ThumbTouch,
        ThumbNearTouch,
        LeftStickTouch,
    }
}
