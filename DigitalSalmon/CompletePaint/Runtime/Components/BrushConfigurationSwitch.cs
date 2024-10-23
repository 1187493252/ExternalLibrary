using UnityEngine;
using UnityEngine.XR;

namespace DigitalSalmon.CompletePaint {
	public class BrushConfigurationSwitch : BaseBehaviour {
		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------

		[SerializeField]
		protected BrushConfiguration[] configurations;

		//-----------------------------------------------------------------------------------------
		// Private Fields:
		//-----------------------------------------------------------------------------------------

		private PaintBrush paintBrush;

		private int activeConfigIndex;

		//-----------------------------------------------------------------------------------------
		// Unity Lifecycle:
		//-----------------------------------------------------------------------------------------

		protected void Awake() {
			paintBrush = GetComponent<PaintBrush>();
			if (paintBrush == null) {
				Debug.LogWarning($"Cannot use {typeof(BrushConfigurationSwitch)} without a {typeof(PaintBrush)}component. Please add a {typeof(PaintBrush)} component.");
			}
		}

		protected void Start() {
			paintBrush.AssignCofigurationProvider(IncrementActiveConfig());
		}

		protected void Update() {

			InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonPressed);

			if (primaryButtonPressed) {
				paintBrush.AssignCofigurationProvider(IncrementActiveConfig());
			}
		}

		//-----------------------------------------------------------------------------------------
		// Private Methods:
		//-----------------------------------------------------------------------------------------

		private BrushConfiguration IncrementActiveConfig() { return configurations[IncrementActiveConfigIndex()]; }

		private int IncrementActiveConfigIndex() {
			int currentIndex = activeConfigIndex;
			activeConfigIndex++;
			if (activeConfigIndex > configurations.Length - 1) activeConfigIndex = 0;
			return currentIndex;
		}
	}
}