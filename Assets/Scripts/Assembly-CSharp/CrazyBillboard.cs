using UnityEngine;

public class CrazyBillboard : MonoBehaviour
{
	public Vector3 axisMult;
	private void LateUpdate()
	{
		if (Time.timeScale == 0) return;
		float x = Random.Range(-360f, 360f);
        float y = Random.Range(-360f, 360f);
        float z = Random.Range(-360f, 360f);
		transform.rotation = Quaternion.Euler(x * axisMult.x, y * axisMult.y, z * axisMult.z);
	}
}
