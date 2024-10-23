using System.Linq;
using DigitalSalmon;
using DigitalSalmon.CompletePaint;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwatchButton : BaseBehaviour, IPointerClickHandler {
	//-----------------------------------------------------------------------------------------
	// Public Events:
	//-----------------------------------------------------------------------------------------

	public event EventHandler<SwatchButton> Selected;

	protected Image image;
	protected Paint paint;
	private Image outline;

	private bool focused;

	public Paint Paint => paint;

	protected void Awake() {
		image = GetComponent<Image>();
		paint = GetComponent<Paint>();
		outline = GetComponentsInChildren<Image>(true).FirstOrDefault(i => i != image);
		SetFocused(false);
	}

	protected void OnEnable() { paint.ColourChanged += Paint_ColorChanged; }

	protected void OnValidate() {
		if (!Application.isPlaying) {
			image = GetComponent<Image>();
			paint = GetComponent<Paint>();
		}
		UpdateImageColor();
	}

	protected void Start() { UpdateImageColor(); }

	protected void OnDisable() { paint.ColourChanged -= Paint_ColorChanged; }

	private void Paint_ColorChanged() { UpdateImageColor(); }

	public void OnPointerClick(PointerEventData eventData) { Selected.InvokeSafe(this); }

	public void SetFocused(bool newFocused) {
		focused = newFocused;
		UpdateOutline();
	}

	private void UpdateOutline() {
		if (outline != null) outline.enabled = focused;
	}

	private void UpdateImageColor() {
		if (image != null) image.color = paint.GetPaintColor(1);
	}
}