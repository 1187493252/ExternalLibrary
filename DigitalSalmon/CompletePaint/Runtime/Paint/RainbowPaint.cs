using DigitalSalmon;
using DigitalSalmon.CompletePaint;
using UnityEngine;

public class RainbowPaint : Paint {
	//-----------------------------------------------------------------------------------------
	// Inspector Variables:
	//-----------------------------------------------------------------------------------------

	[SerializeField]
	protected float frequency;

	[SerializeField]
	protected float saturation = 0.95f;

	[SerializeField]
	protected float value = 1;

	//-----------------------------------------------------------------------------------------
	// Unity Lifecycle
	//-----------------------------------------------------------------------------------------

	protected void Update() { InvokeColourChanged(); }

	//-----------------------------------------------------------------------------------------
	// Public Methods:
	//-----------------------------------------------------------------------------------------

	public override Color GetPaintColor(float distance) { return Color.HSVToRGB(MathUtilities.Frac(Time.time * frequency), saturation, value); }
}