using DigitalSalmon.CompletePaint;
using UnityEngine;

public class ColorPaint : Paint {
	//-----------------------------------------------------------------------------------------
	// Inspector Variables:
	//-----------------------------------------------------------------------------------------

	[SerializeField]
	protected Color paintColor;

	//-----------------------------------------------------------------------------------------
	// Public Methods:
	//-----------------------------------------------------------------------------------------

	public override Color GetPaintColor(float distance) { return paintColor; }
}