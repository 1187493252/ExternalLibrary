using UnityEngine;

public class NoiseRotationBehaviour : MonoBehaviour {

	[SerializeField]
	protected float frequency;

	protected void Update() { transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(RandomVector3(Time.time * frequency)), 2 * Time.deltaTime); }

	private Vector3 RandomVector3(float seed) { return new Vector3(RandomFloat(seed * 0.5f), RandomFloat((seed*0.64f) + 665.22f), RandomFloat((seed * 1.4f) + 212.42f)); }

	private float RandomFloat(float seed) { return Mathf.Sin(seed); }
}