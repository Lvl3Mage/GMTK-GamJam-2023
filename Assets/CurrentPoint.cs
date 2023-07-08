using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentPoint : MonoBehaviour
{


	[Header("Indicator settings")]
	[SerializeField] Transform indicator;
	[SerializeField] float indicatorSize, maxIndicatorSize;
	// [SerializeField] float indicatorAspect;

	[Header("Current mechanics")]
	[SerializeField] float velocityBleedoverSpeed;
	[SerializeField] [Range(0,1)] float velocityDecaySpeed;
	[SerializeField] [Range(0,1)] float velocitySpread;

	[Header("Current Noise settings")]
	[SerializeField] float noiseTimeScale;
	[SerializeField] float noiseScale;
	[SerializeField] float noiseStrength;
	[SerializeField] float noiseAcceleration;
	[SerializeField] Vector2 noiseXOffset, noiseYOffset;
	[SerializeField] Vector2 noiseAverage;
	public Vector2 velocity {get; private set;}
	CurrentController controller;
	Vector2 noiseXPos, noiseYPos;
	public bool frozen {get; private set;}
	Vector2 surfaceDir;
	void Awake()
	{
		controller = CurrentController.instance;
		noiseXPos = ((Vector2)transform.position + noiseXOffset)*noiseScale;
		noiseYPos = ((Vector2)transform.position + noiseYOffset)*noiseScale;
	}
	void OnTriggerEnter2D(Collider2D col){

		if(col.gameObject.layer == 7){
			frozen = true;
			if(!col.OverlapPoint(transform.position)){
				surfaceDir = (col.ClosestPoint(transform.position) - (Vector2)transform.position).normalized;
			}

			// 2d colliders don't have a fucking closest point on bounds method (why are you giving me your toughest battles unity)
			// thus the code below must not be read by any human beings who want to mantain their sanity

			// it does some shit like this

			// \|/
			// -x-
			// /|\


			Vector2[] canditates = new Vector2[]{
				(Vector2)transform.position + Vector2.up,
				(Vector2)transform.position + Vector2.up + Vector2.left,
				(Vector2)transform.position + Vector2.left,
				(Vector2)transform.position + Vector2.left + Vector2.down,
				(Vector2)transform.position + Vector2.down,
				(Vector2)transform.position + Vector2.down + Vector2.right,
				(Vector2)transform.position + Vector2.right,
				(Vector2)transform.position + Vector2.right + Vector2.up,
			};

			Vector2 closest = transform.position;
			Vector2 closestCandidate = transform.position;
			float minDistSqr = Mathf.Infinity;
			for(int i = 0; i < canditates.Length; i++){
				Vector2 newPoint = col.ClosestPoint(canditates[i]);
				float dist = ((Vector2)transform.position - newPoint).sqrMagnitude;
				if(minDistSqr > dist){
					closest = newPoint;
					closestCandidate = canditates[i];
					minDistSqr = dist;
				}
			}
			surfaceDir = (closest -closestCandidate).normalized;
		}
	}
	void Update()
	{
		VelocityBleed();
		VelocityDecay();
		VelocityNoise();

		indicator.rotation = Quaternion.Euler(0,0,Mathf.Atan2(velocity.y,velocity.x)*Mathf.Rad2Deg - 90f);
		indicator.localScale = new Vector3(Mathf.Min(velocity.magnitude*indicatorSize, maxIndicatorSize), Mathf.Min(velocity.magnitude*indicatorSize, maxIndicatorSize), indicator.localScale.z);
	}
	void VelocityBleed(){
		if(velocity == Vector2.zero){return;}
		Vector2 normVel = velocity.normalized;
		CurrentPoint nextPoint = GetPointInDir(normVel);

		CurrentPoint nextRight = GetPointInDir(normVel + new Vector2(normVel.y, -normVel.x));

		CurrentPoint nextLeft = GetPointInDir(normVel + new Vector2(-normVel.y, normVel.x));

		if(frozen){
			nextPoint.AddVelocity(velocity*Mathf.Lerp(1f, 0.33f, velocitySpread));
			nextRight.AddVelocity(velocity*Mathf.Lerp(0, 0.33f, velocitySpread));
			nextLeft.AddVelocity(velocity*Mathf.Lerp(0, 0.33f, velocitySpread));
			velocity = Vector2.zero;
			return;
		}
		float bleedover = velocity.magnitude * velocityBleedoverSpeed * Time.deltaTime;
		nextPoint.AddVelocity(velocity*bleedover*Mathf.Lerp(1f, 0.33f, velocitySpread));
		nextRight.AddVelocity(velocity*bleedover*Mathf.Lerp(0, 0.33f, velocitySpread));
		nextLeft.AddVelocity(velocity*bleedover*Mathf.Lerp(0, 0.33f, velocitySpread));
		if(!frozen){
			AddVelocity(-velocity*bleedover);
		}
	}

	void VelocityDecay(){
		velocity = Vector2.Lerp(velocity, Vector2.zero,1- Mathf.Pow(1-velocityDecaySpeed, Time.deltaTime));
	}
	void VelocityNoise(){
		if(frozen){return;}
		float x = (Perlin3D(noiseXPos.x, noiseXPos.y, Time.time*noiseTimeScale)-0.5f)*2;
		float y = (Perlin3D(noiseYPos.x, noiseYPos.y, Time.time*noiseTimeScale)-0.5f)*2;
		Vector2 noiseTarget = new Vector2(x, y) + noiseAverage;
		Vector2 noiseAccel = (noiseTarget*noiseStrength - velocity)*noiseAcceleration*Time.deltaTime; 
		velocity += noiseAccel;

		CurrentPoint nextPoint = GetPointInDir(-noiseAccel);
		nextPoint.AddVelocity(noiseAccel);
	}
	public void AddVelocity(Vector2 addedValue){
		if(frozen){
			velocity += Vector2.Reflect(addedValue, surfaceDir);
			return;
		}

		velocity += addedValue;
		velocity = Vector2.ClampMagnitude(velocity, 100f);
	}
	CurrentPoint GetPointInDir(Vector2 dir){
		return controller.GetClosestPoint((Vector2)transform.position + dir.normalized*controller.spread*2);
	}
	float Perlin3D(float x, float y, float z){
		float XY = Mathf.PerlinNoise(x, y);
		float YZ = Mathf.PerlinNoise(y, z);
		float ZX = Mathf.PerlinNoise(z, x);
		
		float YX = Mathf.PerlinNoise(y, z);
		float ZY = Mathf.PerlinNoise(z, y);
		float XZ = Mathf.PerlinNoise(x, z);
		
		float val = (XY + YZ + ZX + YX + ZY + XZ)/6f;
		return val;
	}
}

// [System.Serializable]
// public class PrecalculatedPerlin{
// 	[Tooltip("Defines the number of steps in the pregenerated 256 unit perlin range (the more there are the more perlin values will be generated within the same range)")]
// 	[SerializeField] int PerlinSteps;
// 	float[] GeneratedPerlin;
// 	public void Initialize(float x, float y){
// 		GeneratedPerlin = GeneratePerlin(x, y, PerlinSteps);
// 	}
// 	float[] GeneratePerlin(float x, float y, int PerlinSteps){ // generates an array of perlin values in a range of 0-256 with a defined length
// 		float[] GeneratedPerlin = new float[PerlinSteps];
// 		float step = 256f/((float)(PerlinSteps)); // calculating the step size defined as the fraction of 256 devided into the steps defined by the user
// 		for(int i = 0; i<PerlinSteps; i++){ // iterating to for the array
// 			float sampleCoord = i*step; // calculating the coordinates of this step and adding the seed to it
// 			float sampleVal = Perlin3D(x, y, sampleCoord); // sampling perlin
// 			GeneratedPerlin[i] = sampleVal;
// 		}
// 		return GeneratedPerlin;
// 	}
// 	public float SamplePerlin(float t){
// 		float unroundedID = (t)%(GeneratedPerlin.Length-1); // Calculates the coordinates using multiplication of current time and the timescale clamped in between the array length
// 		int prevID = Mathf.FloorToInt(unroundedID); 
// 		int nextID = Mathf.CeilToInt(unroundedID);
// 		float idPercentage = unroundedID-prevID; // defines where the unrounded id was on the prevID-nextID spectrum 
// 		float prevVal = GeneratedPerlin[prevID];
// 		float nextVal = GeneratedPerlin[nextID];
// 		float perlinVal = Mathf.Lerp(prevVal, nextVal, idPercentage); //lerps between the previous value and the next value based on how close we were to the next/ previous value
// 		return perlinVal;
// 	}
// 	float Perlin3D(float x, float y, float z){
// 		float XY = Mathf.PerlinNoise(x, y);
// 		float YZ = Mathf.PerlinNoise(y, z);
// 		float ZX = Mathf.PerlinNoise(z, x);
		
// 		float YX = Mathf.PerlinNoise(y, z);
// 		float ZY = Mathf.PerlinNoise(z, y);
// 		float XZ = Mathf.PerlinNoise(x, z);
		
// 		float val = (XY + YZ + ZX + YX + ZY + XZ)/6f;
// 		return val;
// 	}
// }