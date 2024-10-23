using UnityEngine;

namespace DigitalSalmon.CompletePaint {
	[RequireComponent(typeof(BrushConfiguration))]
	public class BrushToolButton : ToolButton {
		//-----------------------------------------------------------------------------------------
		// Events:
		//-----------------------------------------------------------------------------------------

		public event EventHandler<BrushToolButton> OnConfigurationSelected;

		//-----------------------------------------------------------------------------------------
		// Private Fields:
		//-----------------------------------------------------------------------------------------

		private BrushConfiguration brushConfiguration;

		public BrushConfiguration Configuration { get { return brushConfiguration; } }

		//-----------------------------------------------------------------------------------------
		// Unity Lifecycle:
		//-----------------------------------------------------------------------------------------

		protected override void Awake() {
			base.Awake();
			brushConfiguration = GetComponent<BrushConfiguration>();
		}

		//-----------------------------------------------------------------------------------------
		// Event Handlers:
		//-----------------------------------------------------------------------------------------

		protected override void OnClicked() {
			base.OnClicked();
			OnConfigurationSelected.InvokeSafe(this);
		}
	}
}