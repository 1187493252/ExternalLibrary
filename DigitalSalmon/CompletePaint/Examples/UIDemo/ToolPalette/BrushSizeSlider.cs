using DigitalSalmon;
using UnityEngine;
using UnityEngine.UI;

public class BrushSizeSlider : BaseBehaviour {
	//-----------------------------------------------------------------------------------------
	// Events:
	//-----------------------------------------------------------------------------------------

	public event EventHandler<float> Changed;

	//-----------------------------------------------------------------------------------------
	// Protected Fields:
	//-----------------------------------------------------------------------------------------

	protected Slider slider;

	//-----------------------------------------------------------------------------------------
	// Inspector Variables:
	//-----------------------------------------------------------------------------------------

	[SerializeField]
	protected float minSize;

	[SerializeField]
	protected float maxSize;

	//-----------------------------------------------------------------------------------------
	// Unity Lifecycle:
	//-----------------------------------------------------------------------------------------

	protected void Awake() {
		slider = GetComponentInChildren<Slider>();
		slider.minValue = minSize;
		slider.maxValue = maxSize;
		slider.value = Mathf.Lerp(minSize, maxSize, 0.5f);
	}

	protected void Start() {
		SetBrushSize(slider.value);
	}

	protected void OnEnable() { slider.onValueChanged.AddListener(Slider_ValueChanged); }

	protected void OnDisable() { slider.onValueChanged.RemoveListener(Slider_ValueChanged); }

	//-----------------------------------------------------------------------------------------
	// Event Handlers:
	//-----------------------------------------------------------------------------------------

	private void Slider_ValueChanged(float value) { SetBrushSize(value); }

	//-----------------------------------------------------------------------------------------
	// Private Methods:
	//-----------------------------------------------------------------------------------------

	private void SetBrushSize(float size) { Changed.InvokeSafe(size); }
}