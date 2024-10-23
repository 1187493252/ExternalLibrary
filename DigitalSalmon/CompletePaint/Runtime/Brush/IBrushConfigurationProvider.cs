namespace DigitalSalmon.CompletePaint {
	public interface IBrushConfigurationProvider {
		BrushConfigurationInfo GetConfiguration(PaintBrush.PaintBlob paintBlob);
	}
}