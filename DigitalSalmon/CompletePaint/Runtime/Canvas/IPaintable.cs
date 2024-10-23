namespace DigitalSalmon.CompletePaint {
	public interface IPaintable {
		//-----------------------------------------------------------------------------------------
		// Properties:
		//-----------------------------------------------------------------------------------------

		PaintCanvas.UvChannels UvChannel { get; }

		//-----------------------------------------------------------------------------------------
		// Methods:
		//-----------------------------------------------------------------------------------------

		void ApplyPaint(PaintBrush.PaintBlob paintBlob, BrushConfigurationInfo brushConfiguration, Paint paint);
		void StoreUndoStep();
	}
}