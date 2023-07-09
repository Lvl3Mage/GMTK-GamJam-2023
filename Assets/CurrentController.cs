using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentController : MonoBehaviour
{
	[SerializeField] Vector2Int dimentions;


	[SerializeField] float Spread;
	public float spread {get{return Spread;} private set {Spread = value;}}
	[SerializeField] CurrentPoint pointPrefab;
	[SerializeField] Camera camera;
	[Header("Mouse controls")]
	[SerializeField] float maxDist;
	[SerializeField] float velocityScale;

	SliderController powerSlider;
	[SerializeField] float maxPower;
	[SerializeField] float powerRegainSpeed;
	float power;
	public static CurrentController instance {get; private set;}
	void Awake(){
		if(instance!=null){
			Debug.LogError("Another instance of CurrentController is defined");
			Destroy(this);
		}
		CurrentController.instance=this;
	}
	CurrentPoint[] points;
	CurrentPoint[,] pointMap;
	// Dictionary<(int,int), CurrentPoint> pointMap = new Dictionary<(int,int),CurrentPoint>();
	Vector2 prevMousePosition;
	void Start()
	{
		InitializePoints();
		prevMousePosition = GetMousePos();
		powerSlider = GameObject.FindGameObjectWithTag("Power").GetComponent<SliderController>();
		powerSlider.SetRange(0,maxPower);
	}
	void InitializePoints(){
		points = new CurrentPoint[dimentions.x * dimentions.y];
		pointMap = new CurrentPoint[dimentions.x, dimentions.y];
		for(int i = 0; i < dimentions.x; i++){
			for(int j = 0; j < dimentions.y; j++){
				Vector2 coord = IndexToWorldPos(new Vector2(i,j));
				CurrentPoint point = Instantiate(pointPrefab, new Vector3(coord.x,coord.y,transform.position.z), Quaternion.identity, transform);
				points[i*dimentions.y + j] = point;
				pointMap[i,j] = point;
			}
		}
	}

	void Update()
	{

		if(GameManager.instance.gamePaused) return;


		if(Input.GetMouseButton(0)){
			ApplyMouseDrag();
		}
		else{
			//recharge power
			power += powerRegainSpeed*Time.deltaTime;
			power = Mathf.Clamp(power,0,maxPower);
		}
		powerSlider.SetValue(power);
		prevMousePosition = GetMousePos();

	}
	void ApplyMouseDrag(){
		if(Time.deltaTime > 0.1f){return;}
		if(power <= 0){return;}
		Vector2 mousePosition = GetMousePos();
		Vector2 mouseVelocity = (mousePosition - prevMousePosition)*Time.deltaTime;


		CurrentPoint[] overlapPoints = GetPointsInCircle(mousePosition, maxDist);

		foreach(CurrentPoint point in overlapPoints){
			float dist = Vector2.Distance(mousePosition, point.transform.position);
			float strength = 1-dist/maxDist;
			point.AddVelocity(mouseVelocity*strength * velocityScale);
		}

		power -= mouseVelocity.magnitude;
		power = Mathf.Clamp(power,0,maxPower);
	}
	CurrentPoint[] GetPointsInCircle(Vector2 circlePos, float radius){
		Collider2D[] cols = Physics2D.OverlapCircleAll(circlePos, radius, 1 << 6 /* Current layer index */);
		CurrentPoint[] overlapPoints = new CurrentPoint[cols.Length];
		for (int i = 0; i < overlapPoints.Length; i++){
			overlapPoints[i] = cols[i].gameObject.GetComponent<CurrentPoint>();
		}
		return overlapPoints;
	}
	Vector2 GetMousePos(){
		Vector3 worldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
		return worldPosition;
	}
	Vector2 WorldToIndexPos(Vector2 worldPos){
		return (worldPos)/spread + new Vector2(dimentions.x-1, dimentions.y-1)*0.5f;
	}
	Vector2 IndexToWorldPos(Vector2 indexPos){
		return (indexPos - new Vector2(dimentions.x-1, dimentions.y-1)*0.5f)*spread;
	}

	public CurrentPoint GetClosestPoint(Vector2 worldPos){
		Vector2 localPos = WorldToIndexPos(worldPos);
		Vector2Int closestPos = new Vector2Int((int)Mathf.Round(localPos.x),(int)Mathf.Round(localPos.y));
		closestPos.x = Mathf.Clamp(closestPos.x, 0, dimentions.x-1);
		closestPos.y = Mathf.Clamp(closestPos.y, 0, dimentions.y-1);
		return pointMap[closestPos.x,closestPos.y];
	}
}
