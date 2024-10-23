using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DigitalSalmon.CompletePaint {
	public class ToolButton : BaseBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
		//-----------------------------------------------------------------------------------------
		// Constants:
		//-----------------------------------------------------------------------------------------

		protected const string VISIBILITY_PROPERTY = "Visible";
		protected const string HOVERED_PROPERTY = "Hovered";

		protected readonly int VISIBLE_STATE_HASH = Animator.StringToHash("Visible");
		protected readonly int HIDDEN_STATE_HASH = Animator.StringToHash("Hidden");

		//-----------------------------------------------------------------------------------------
		// Events:
		//-----------------------------------------------------------------------------------------

		public event EventHandler<ToolButton> Clicked;

		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------

		[Header("Assignment")]
		[SerializeField]
		protected Image background;

		[SerializeField]
		protected Image highlight;

		[Header("Colours")]
		[SerializeField]
		protected Color backgroundIdle;

		[SerializeField]
		protected Color backgroundHovered;

		[SerializeField]
		protected Color backgroundFocused;

		[SerializeField]
		protected Color highlightTint;

		//-----------------------------------------------------------------------------------------
		// Private Fields:
		//-----------------------------------------------------------------------------------------

		private bool focused;

		private bool hovered;

		//-----------------------------------------------------------------------------------------
		// Unity Lifecycle:
		//-----------------------------------------------------------------------------------------

		protected virtual void Awake() { SetFocused(false); }

		//-----------------------------------------------------------------------------------------
		// Interface Methods:
		//-----------------------------------------------------------------------------------------

		void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
			Clicked.InvokeSafe(this);
			OnClicked();
		}

		void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) { SetHovered(true); }

		void IPointerExitHandler.OnPointerExit(PointerEventData eventData) { SetHovered(false); }
		//-----------------------------------------------------------------------------------------
		// Public methods:
		//-----------------------------------------------------------------------------------------

		public void SetFocused(bool newFocused) {
			focused = newFocused;
			UpdateBackgroundColor();
			UpdateHighlight();
		}

		public void SetHovered(bool newHovered) {
			hovered = newHovered;
			UpdateBackgroundColor();
		}
		//-----------------------------------------------------------------------------------------
		// Private Methods:
		//-----------------------------------------------------------------------------------------

		protected virtual void OnClicked() { }

		private void UpdateBackgroundColor() {
			if (background != null) background.color = focused ? backgroundFocused : hovered ? backgroundHovered : backgroundIdle;
		}

		private void UpdateHighlight() {
			if (highlight != null) highlight.enabled = focused;
		}
	}
}