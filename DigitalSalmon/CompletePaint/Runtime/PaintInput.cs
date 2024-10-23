using UnityEngine;
using UnityEngine.XR;

namespace DigitalSalmon.CompletePaint {
	public class PaintInput : ProtectedSingleton<PaintInput> {
		//-----------------------------------------------------------------------------------------
		// Type Definitions:
		//-----------------------------------------------------------------------------------------

		public enum Laterals {
			Left,
			Right
		}

		//-----------------------------------------------------------------------------------------
		// Public Properties:
		//-----------------------------------------------------------------------------------------

		public static bool PrimaryTriggerDown { get; private set; }
		public static float PrimaryTrigger => Either(XrPrimaryTrigger, MousePrimaryTrigger);
		public static bool PrimaryTriggerUp { get; private set; }
	
		public static bool PrimaryButtonDown => XrPrimaryMenuDown || MouseSecondaryDown;
		public static bool PrimaryButton => XrPrimaryMenu | MouseSecondary;
		public static bool PrimaryButtonUp => XrPrimaryMenuUp | MouseSecondaryUp;

		public static Vector3 BrushPosition => GetBrushPosition();
		public static Quaternion BrushRotation => GetBrushRotation();

		//-----------------------------------------------------------------------------------------
		// Constants:
		//-----------------------------------------------------------------------------------------

		private static Laterals PrimaryLateral => Laterals.Right;

		//-----------------------------------------------------------------------------------------
		// Mouse Properties:
		//-----------------------------------------------------------------------------------------

		private static float MousePrimaryTrigger =>		Input.GetMouseButton(0) ? 1 : 0;

		private static bool MousePrimaryDown =>			Input.GetMouseButtonDown(0);
		private static bool MousePrimary =>				Input.GetMouseButton(0);
		private static bool MousePrimaryUp =>			Input.GetMouseButtonUp(0);

		private static bool MouseSecondaryDown =>		Input.GetMouseButtonDown(1);
		private static bool MouseSecondary =>			Input.GetMouseButton(1);
		private static bool MouseSecondaryUp =>			Input.GetMouseButtonUp(1);

		//-----------------------------------------------------------------------------------------
		// XR Properties:
		//-----------------------------------------------------------------------------------------

		// Primary
		private static bool XrPrimaryMenuDown =>		PrimaryLateral == Laterals.Left ? XrLeftHandMenuDown : XrRightHandMenuDown;
		private static bool XrPrimaryMenu =>			PrimaryLateral == Laterals.Left ? XrLeftHandMenu : XrRightHandMenu;
		private static bool XrPrimaryMenuUp =>			PrimaryLateral == Laterals.Left ? XrLeftHandMenuUp : XrRightHandMenuUp;

		private static bool XrPrimaryTrackpadDown =>	PrimaryLateral == Laterals.Left ? XrLeftHandTrackpadDown : XrRightHandTrackpadDown;
		private static bool XrPrimaryTrackpad =>		PrimaryLateral == Laterals.Left ? XrLeftHandTrackpad : XrRightHandTrackpad;
		private static bool XrPrimaryTrackpadUp =>		PrimaryLateral == Laterals.Left ? XrLeftHandTrackpadUp : XrRightHandTrackpadUp;

		private static float XrPrimaryTrigger =>		PrimaryLateral == Laterals.Left ? XrLeftHandTrigger : XrRightHandTrigger;
		private static float XrPrimaryGrip =>			PrimaryLateral == Laterals.Left ? XrLeftHandGrip : XrRightHandGrip;

		// Menu Button
		private static bool XrLeftHandMenuDown =>		Input.GetKeyDown	("joystick button 2");
		private static bool XrLeftHandMenu =>           Input.GetKey		("joystick button 2");
		private static bool XrLeftHandMenuUp =>         Input.GetKeyUp		("joystick button 2");
		private static bool XrRightHandMenuDown =>      Input.GetKeyDown	("joystick button 0");
		private static bool XrRightHandMenu =>          Input.GetKey		("joystick button 0");
		private static bool XrRightHandMenuUp =>        Input.GetKeyUp		("joystick button 0");

		// Trackpad Button
		private static bool XrLeftHandTrackpadDown =>   Input.GetKeyDown	("joystick button 8");
		private static bool XrLeftHandTrackpad =>       Input.GetKey		("joystick button 8");
		private static bool XrLeftHandTrackpadUp =>     Input.GetKeyUp		("joystick button 8");
		private static bool XrRightHandTrackpadDown =>  Input.GetKeyDown	("joystick button 9");
		private static bool XrRightHandTrackpad =>      Input.GetKey		("joystick button 9");
		private static bool XrRightHandTrackpadUp =>    Input.GetKeyUp		("joystick button 9");

		// Trigger & Grip
		private static float XrLeftHandTrigger =>       Input.GetAxis("joystick axis 9");
		private static float XrRightHandTrigger =>      Input.GetAxis("joystick axis 10");
		private static float XrLeftHandGrip =>          Input.GetAxis("joystick axis 11");
		private static float XrRightHandGrip =>         Input.GetAxis("joystick axis 12");

		//-----------------------------------------------------------------------------------------
		// Private Fields:
		//-----------------------------------------------------------------------------------------

		private float previousPrimaryTrigger;

		//-----------------------------------------------------------------------------------------
		// Unity Lifecycle:
		//-----------------------------------------------------------------------------------------
		
		protected void Update() {
			PrimaryTriggerDown = false;
			PrimaryTriggerUp = false;

			if (previousPrimaryTrigger <= 0 && PrimaryTrigger > 0) PrimaryTriggerDown = true;
			if (previousPrimaryTrigger > 0 && PrimaryTrigger == 0) PrimaryTriggerUp = true;

			previousPrimaryTrigger = PrimaryTrigger;
		}
	
		//-----------------------------------------------------------------------------------------
		// Private Methods:
		//-----------------------------------------------------------------------------------------

		private static Vector3 GetBrushPosition() {
			switch (PrimaryLateral) {
				case Laterals.Left: return InputTracking.GetLocalPosition(XRNode.LeftHand);
				case Laterals.Right: return InputTracking.GetLocalPosition(XRNode.RightHand);
			}
			return Vector3.zero;
		}

		private static Quaternion GetBrushRotation() {
			switch (PrimaryLateral) {
				case Laterals.Left: return InputTracking.GetLocalRotation(XRNode.LeftHand);
				case Laterals.Right: return InputTracking.GetLocalRotation(XRNode.RightHand);
			}
			return Quaternion.identity;
		}

		private static float Either(float a, float b) { return UnityEngine.Mathf.Max(a, b); }
	}
}