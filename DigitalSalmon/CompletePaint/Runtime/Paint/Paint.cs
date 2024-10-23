using UnityEngine;

namespace DigitalSalmon.CompletePaint {
	/// <summary>
	/// Paint is an object which closely matches the 'real-world' namesake.
	/// For now, 'Paint' just has a colour.
	/// </summary>
	public abstract class Paint : BaseBehaviour {
		//-----------------------------------------------------------------------------------------
		// Events:
		//-----------------------------------------------------------------------------------------

		public event EventHandler ColourChanged;

		//-----------------------------------------------------------------------------------------
		// Public Methods:
		//-----------------------------------------------------------------------------------------

		public abstract Color GetPaintColor(float distance);

		//-----------------------------------------------------------------------------------------
		// Protected Methods:
		//-----------------------------------------------------------------------------------------

		protected void InvokeColourChanged() { ColourChanged?.Invoke(); }
	}
}