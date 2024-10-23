using DigitalSalmon;
using DigitalSalmon.CompletePaint;
using DigitalSalmon.Extensions;
using UnityEngine;

public class BrushColorView : BaseBehaviour {
	//-----------------------------------------------------------------------------------------
	// Inspector Variables:
	//-----------------------------------------------------------------------------------------

	[SerializeField]
	protected Material targetMaterial;

	[SerializeField]
	protected PaintBrush paintBrush;

	//-----------------------------------------------------------------------------------------
	// Unity Lifecycle:
	//-----------------------------------------------------------------------------------------

	protected void Update() {
		if (paintBrush == null || paintBrush.Paint == null || targetMaterial == null) return;
		targetMaterial.SetColor("_EmissionColor", paintBrush.Paint.GetPaintColor(0).WithAlpha(1));
	}
}