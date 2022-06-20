using UnityEngine;
using UnityEngine.Serialization;

public class LaserController : MonoBehaviour
{

	public float length = 500f;
	public float width = 0.1f;
	[FormerlySerializedAs("OvarAll_Size")] public float overallSize = 1.0f;

	[FormerlySerializedAs("hit_effect")] public GameObject hitEffect;

	[FormerlySerializedAs("laser_add")] [SerializeField]
	private GameObject laserAdd;

	[FormerlySerializedAs("laser_alpha")] [SerializeField]
	private GameObject laserAlpha;

	[FormerlySerializedAs("trf_scaleController")] [SerializeField]
	private GameObject trfScaleController;


	void Start()
	{
		// Effect Scale
		if (trfScaleController)
		{
			trfScaleController.transform.localScale = new Vector3(overallSize, overallSize, overallSize);
		}

		// laser width
		if (laserAdd)
		{
			var pa1_width = laserAdd.GetComponent<ParticleSystem>().main;
			pa1_width.startSize = width;
		}

		if (laserAlpha)
		{
			var pa2_width = laserAlpha.GetComponent<ParticleSystem>().main;
			pa2_width.startSize = width;
		}

		// laser length
		if (laserAdd)
		{
			var pa1_length = laserAdd.GetComponent<ParticleSystemRenderer>();
			pa1_length.lengthScale = length / width / 10;
		}

		if (laserAlpha)
		{
			var pa2_length = laserAlpha.GetComponent<ParticleSystemRenderer>();
			pa2_length.lengthScale = length / width / 10;
		}
	}


	void Update()
	{
		// Effect Scale
		if (trfScaleController)
		{
			trfScaleController.transform.localScale = new Vector3(overallSize, overallSize, overallSize);
		}

		// laser length
		if (laserAdd)
		{
			var pa1_length = laserAdd.GetComponent<ParticleSystemRenderer>();
			pa1_length.lengthScale = length / width / 10;
		}

		if (laserAlpha)
		{
			var pa2_length = laserAlpha.GetComponent<ParticleSystemRenderer>();
			pa2_length.lengthScale = length / width / 10;
		}

		// Hit Controller:
		if (Physics.Raycast(transform.position, transform.forward, out var hit))
		{
			Debug.Log(hit.distance);
			if (hit.collider && hit.distance <= length / 10 * overallSize)
			{

				if (laserAdd)
				{
					var pa1_length = laserAdd.GetComponent<ParticleSystemRenderer>();
					pa1_length.lengthScale = hit.distance * 10 / width / 10 / overallSize;
				}

				if (laserAlpha)
				{
					var pa2_length = laserAlpha.GetComponent<ParticleSystemRenderer>();
					pa2_length.lengthScale = hit.distance * 10 / width / 10 / overallSize;
				}

				//Hit Effect Instance
				GameObject insHitEff = (GameObject) Instantiate(hitEffect, hit.point, Quaternion.identity);
				insHitEff.transform.localScale = new Vector3(overallSize, overallSize, overallSize);
			}
		}
		else
		{
			// laser length
			if (laserAdd)
			{
				var pa1_length = laserAdd.GetComponent<ParticleSystemRenderer>();
				pa1_length.lengthScale = length / width / 10;
			}

			if (laserAlpha)
			{
				var pa2_length = laserAlpha.GetComponent<ParticleSystemRenderer>();
				pa2_length.lengthScale = length / width / 10;
			}
		}

		// laser width
		if (laserAdd)
		{
			var pa1_width = laserAdd.GetComponent<ParticleSystem>().main;
			pa1_width.startSize = width;
		}

		if (laserAlpha)
		{
			var pa2_width = laserAlpha.GetComponent<ParticleSystem>().main;
			pa2_width.startSize = width;
		}
	}
}
