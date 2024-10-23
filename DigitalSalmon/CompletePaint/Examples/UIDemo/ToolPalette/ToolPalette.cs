using System.Linq;
using UnityEngine;

namespace DigitalSalmon.CompletePaint {
	public class ToolPalette : BaseBehaviour {
		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------

		[SerializeField]
		protected ToolButton undoButton;

		[SerializeField]
		protected ToolButton redoButton;

		[SerializeField]
		protected PaintBrush paintBrush;

		[SerializeField]
		protected PaintCanvas paintCanvas;

		//-----------------------------------------------------------------------------------------
		// Private Fields:
		//-----------------------------------------------------------------------------------------

		protected ToolButton[] toolButtons;
		protected SwatchButton[] swatchButtons;
		private BrushSizeSlider brushSizeSlider;

		//-----------------------------------------------------------------------------------------
		// Protected Properties:
		//-----------------------------------------------------------------------------------------

		protected BrushToolButton[] BrushToolButtons { get { return toolButtons.Where(t => t is BrushToolButton).Cast<BrushToolButton>().ToArray(); } }

		//-----------------------------------------------------------------------------------------
		// Unity Lifecycle:
		//-----------------------------------------------------------------------------------------

		protected void Awake() {
			toolButtons = GetComponentsInChildren<ToolButton>(true);
			swatchButtons = GetComponentsInChildren<SwatchButton>(true);
			brushSizeSlider = GetComponentInChildren<BrushSizeSlider>();
		}

		protected void OnEnable() {
			foreach (ToolButton toolButton in toolButtons) toolButton.Clicked += ToolButton_Clicked;
			foreach (BrushToolButton brushToolButton in BrushToolButtons) brushToolButton.OnConfigurationSelected += BrushToolButton_ConfigurationSelected;
			undoButton.Clicked += UndoButton_Clicked;
			redoButton.Clicked += RedoButton_Clicked;
			foreach (SwatchButton swatchButton in swatchButtons) swatchButton.Selected += SwatchButton_Selected;
			brushSizeSlider.Changed += BrushSizeSlider_Changed;
		}

		protected void Start() {
			ToolButton_Clicked(toolButtons.First());
			SwatchButton_Selected(swatchButtons.First());
			BrushToolButton_ConfigurationSelected(BrushToolButtons.First());
		}

		protected void OnDisable() {
			foreach (ToolButton toolButton in toolButtons) toolButton.Clicked -= ToolButton_Clicked;
			foreach (BrushToolButton brushToolButton in BrushToolButtons) brushToolButton.OnConfigurationSelected -= BrushToolButton_ConfigurationSelected;
			undoButton.Clicked -= UndoButton_Clicked;
			redoButton.Clicked -= RedoButton_Clicked;
			brushSizeSlider.Changed -= BrushSizeSlider_Changed;
		}

		//-----------------------------------------------------------------------------------------
		// Event Handlers:
		//-----------------------------------------------------------------------------------------

		private void BrushSizeSlider_Changed(float size) {
			paintBrush.AssignSizeMultiplier(size);
		}

		private void SwatchButton_Selected(SwatchButton swatchButton) {
			paintBrush.AssignPaint(swatchButton.Paint);
			foreach (SwatchButton swatch in swatchButtons) {
				swatch.SetFocused(swatch == swatchButton);
			}
		}

		private void ToolButton_Clicked(ToolButton toolButton) { }

		private void BrushToolButton_ConfigurationSelected(BrushToolButton brushToolButton) {
			foreach (BrushToolButton button in BrushToolButtons) {
				button.SetFocused(button == brushToolButton);
			}
			paintBrush.AssignCofigurationProvider(brushToolButton.Configuration);
		}

		private void UndoButton_Clicked(ToolButton toolButton) { paintCanvas.Undo(); }

		private void RedoButton_Clicked(ToolButton toolButton) { paintCanvas.Redo(); }
	}
}