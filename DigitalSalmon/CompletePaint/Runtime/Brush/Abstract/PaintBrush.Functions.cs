using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DigitalSalmon.CompletePaint {
	public partial class PaintBrush {
		//-----------------------------------------------------------------------------------------
		// Protected Methods:
		//-----------------------------------------------------------------------------------------

		protected static void RaycastUi(Vector2 position, List<RaycastResult> results) {
			if (EventSystem.current == null) return;
			results.Clear();
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			pointerEventData.position = position;
			EventSystem.current.RaycastAll(pointerEventData, results);
		}

		protected static PaintBlob[] GetPaintBlobs(RaycastHit[] raycastHits) {
			IEnumerable<RaycastHit> validHits = raycastHits.Where(IsValidHit);
			return validHits.Select(ToPaintBlob).Where(p => p.Valid).ToArray();
		}

		protected static PaintBlob[] GetPaintBlobs(List<RaycastResult> results) {
			IEnumerable<RaycastResult> validHits = results.Where(IsValidResult);
			return validHits.Select(ToPaintBlob).Where(p => p.Valid).ToArray();
		}

		protected static bool IsValidHit(RaycastHit hit) => hit.transform != null && hit.transform.GetComponent<IPaintable>() != null;
		protected static bool IsValidResult(RaycastResult result) => result.isValid && result.gameObject.GetComponent<IPaintable>() != null;

		protected static PaintBlob ToPaintBlob(RaycastHit hit) => new PaintBlob(hit.transform.GetComponent<IPaintable>(), hit.textureCoord, hit.textureCoord2, hit.distance);

		protected static PaintBlob ToPaintBlob(RaycastResult result) =>
			new PaintBlob(result.gameObject.GetComponent<IPaintable>(), UvPositionFromWorldPosition(result), Vector2.zero, result.distance);

		protected static Vector2 UvPositionFromWorldPosition(RaycastResult raycastResult) {
			if (raycastResult.gameObject == null) return Vector2.zero;
			RectTransform hitRectTransform = raycastResult.gameObject.GetComponent<RectTransform>();
			if (hitRectTransform == null) return Vector2.zero;
			Vector2 localPosition;

			RectTransformUtility.ScreenPointToLocalPointInRectangle(hitRectTransform, raycastResult.screenPosition, Camera.main, out localPosition);

			Vector2 uvPosition = UvPositionFromLocalPositionInRect(localPosition, hitRectTransform);
			return uvPosition;
		}

		protected static Vector2 UvPositionFromLocalPositionInRect(Vector2 localPosition, RectTransform rect) =>
			new Vector2(localPosition.x / rect.rect.width, localPosition.y / rect.rect.height) + Vector2.one / 2;
	}
}