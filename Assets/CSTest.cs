using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSTest : MonoBehaviour
{
	[SerializeField] ComputeShader cs;
	[SerializeField] MeshRenderer velocityDisplay;
	[SerializeField] Material velocityMaterial;
	[SerializeField] Camera camera;

	RenderTexture velocityMap;
	RenderTexture pressureMap;
	RenderTexture accelerationMap;

	int kernel_CalculateAcceleration, kernel_AddAcceleration, kernel_ApplyAcceleration;
	void Start()
	{
		kernel_CalculateAcceleration = cs.FindKernel("FluidSimStep");
		kernel_AddAcceleration = cs.FindKernel("FluidApplyVelocity");
		kernel_ApplyAcceleration = cs.FindKernel("ApplyAcceleration");

		velocityMap = CreateFluidMap();
		accelerationMap = CreateFluidMap();
		Material velocityMatCloned = Instantiate(velocityMaterial); 
		AssignTexture(velocityMatCloned,velocityMap);
		AssignMat(velocityDisplay, velocityMatCloned);



		cs.SetTexture(kernel_CalculateAcceleration, "velocityMap", velocityMap);
		cs.SetTexture(kernel_ApplyAcceleration, "velocityMap", velocityMap);

		cs.SetTexture(kernel_CalculateAcceleration, "accelerationMap", accelerationMap);
		cs.SetTexture(kernel_AddAcceleration, "accelerationMap", accelerationMap);
		cs.SetTexture(kernel_ApplyAcceleration, "accelerationMap", accelerationMap);

		// cs.SetInts("dimentions", new int[]{velocityMap.width, velocityMap.height});


		pastMousePos = GetMousePos();
	}
	Vector2 GetMousePos(){
		Vector3 worldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
		return worldPosition;
	}
	Vector2 pastMousePos;
	void FixedUpdate()
	{
		cs.SetFloat("inv_halfBleedoverSpeed", 1/(150f));
		cs.SetFloat("timeScale", Time.fixedDeltaTime*50*1f);


		Vector2 mousePos = GetMousePos();
		Vector2 mouseDelta = mousePos - pastMousePos;
		if(Input.GetMouseButton(0)){
			InvokeCursorAcceleration(mousePos,mouseDelta);
		}

		InvokeFluidStep();
		pastMousePos = GetMousePos();
		
	}
	void AssignMat(MeshRenderer mr, Material mat){
		Material[] mats = mr.materials;
		mats[0] = mat;
		mr.materials = mats;
	}
	void AssignTexture(Material mat, RenderTexture texture){
		mat.SetTexture("_MainTex", texture);
	}
	RenderTexture CreateFluidMap(){
		RenderTexture r = new RenderTexture(256, 128, 16, RenderTextureFormat.RGFloat);
		r.enableRandomWrite = true;
		r.filterMode = FilterMode.Point;
		return r;
	}
	void InvokeFluidStep(){

		// calcluate acceleration from velocity map
		var (fuidX,fluidY,fluidZ) = GetKernelGroupSize(kernel_CalculateAcceleration);

		for(int i = 0; i <= 2; i++){
			for(int j = 0; j <= 2; j++){
				cs.SetInts("blockOffset", new int[]{i, j});
				cs.Dispatch(kernel_CalculateAcceleration, (int)Mathf.Ceil((velocityMap.width ) / (float)fuidX / 3f), (int)Mathf.Ceil((velocityMap.height) / (float)fluidY/3f), 1);
			}
		}

		//Apply acceleration to velocity map
		var (blitX, blitY, blitZ) = GetKernelGroupSize(kernel_ApplyAcceleration);
		cs.Dispatch(kernel_ApplyAcceleration, (int)Mathf.Ceil(velocityMap.width/(float)blitX), (int)Mathf.Ceil(velocityMap.height/(float)blitY), 1);
	}
	void InvokeCursorAcceleration(Vector2 pos, Vector2 delta){

		var (x, y, z) = GetKernelGroupSize(kernel_AddAcceleration);
		cs.SetInts("appliedPoint", new int[]{(int)Mathf.Round((pos.x/20+0.5f)*velocityMap.width) - (int)x/2, (int)Mathf.Round((pos.y/10+0.5f)*velocityMap.height) - (int)y/2});
		cs.SetFloats("appliedVelocity", new float[]{delta.x*20,delta.y*20});
		cs.Dispatch(kernel_AddAcceleration, 1, 1, 1);

	}
	(int,int,int) GetKernelGroupSize(int kernelID){
		uint x, y, z;
		cs.GetKernelThreadGroupSizes(kernelID, out x, out y, out z);
		return ((int)x,(int)y,(int)z);
	}
}
