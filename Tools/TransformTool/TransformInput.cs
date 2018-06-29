using UnityEngine;
using UnityEngine.InputNew;

// GENERATED FILE - DO NOT EDIT MANUALLY
namespace UnityEngine.InputNew
{
	public class TransformInput : ActionMapInput {
		public TransformInput (ActionMap actionMap) : base (actionMap) { }
		
		public ButtonInputControl @select { get { return (ButtonInputControl)this[0]; } }
		public ButtonInputControl @cancel { get { return (ButtonInputControl)this[1]; } }
		public AxisInputControl @modifierX { get { return (AxisInputControl)this[2]; } }
		public AxisInputControl @modifierY { get { return (AxisInputControl)this[3]; } }
		public Vector2InputControl @modifier { get { return (Vector2InputControl)this[4]; } }
	}
}
