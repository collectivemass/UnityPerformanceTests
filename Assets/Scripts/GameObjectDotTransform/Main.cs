//*********************************
//* MAIN CONTROLLER CLASS FOR TEST
//*********************************
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using TMPro;

//*********************************
//* NAMESPACE
//*********************************
using UnityEngine.Scripting;
using UnityEngine.Profiling.Memory.Experimental;
namespace UnityTests.GameObjectDotTransform{

	//*********************************
	//* CLASS
	//*********************************
	public class Main : MonoBehaviour {

		//*********************************
		//* CONSTANTS
		//*********************************
		private enum OutputType : int {
			File = 0,
			Screen = 1
		}

		//*********************************
		//* CONSTANTS
		//*********************************
		private const int COUNT = 1000000;
		private const int POSITIONS_COUNT = 1000;

		//*********************************
		//* PROPERTIES
		//*********************************
		[SerializeField]
		private TextMeshProUGUI textOutput;
		[SerializeField]
		private OutputType outputType;
		[SerializeField]
		private GameObject gameObject;

		//*********************************
		//* VARIABLES
		//*********************************
		private StreamWriter metricStream;
		private float metricTimeStart;
		private float metricsDeltaTime;
		private string metricsMessage;

		private Transform cachedTransform;
		private Vector3[] positions = new Vector3[POSITIONS_COUNT];

		private int runA = 0;
		private int runB = 0;
		private int delayedMetricsStop = 0;

		//*********************************
		//* UNITY MESSAGES
		//*********************************
		private void Awake(){
			Create();
			
			//*** Init Metrics
			Metric_Init();
		}

		private void Update() {

			// We are triggering the test after a couple of frames here because the Unity events system
			// causes garbage.
			if (runA > 0) {
				runA--;
				if (runA == 0) {
					Run_A();
				}
			}

			if (runB > 0) {
				runB--;
				if (runB == 0) {
					Run_B();
				}
			}

			if (delayedMetricsStop > 0) {
				delayedMetricsStop--;
				if (delayedMetricsStop == 0) {
					Metric_Stop_Impl();
				}
			}

		}
		private void OnDestroy() {
			Metric_Flush();
		}

		//*********************************
		//* MAIN METHODS
		//*********************************
		private void Create(){

			//*** Cache Transform
			cachedTransform = gameObject.transform;

			//*** Create a collection of random Positions
			for (int i = 0; i < POSITIONS_COUNT; i++) {
				positions[i] = Random.insideUnitSphere * Random.Range(0, 100);
			}
		}

		public void Clear() {
			textOutput.text = string.Empty;
		}

		public void Log() {
			Debug.Log(textOutput.text);
		}

		private void Run_A(){

			//*** Tests
			Set_A();
		}

		private void Run_B(){

			//*** Tests
			Set_B();
		}

		private void Set_A() {
			Metric_Start();
			for (int i = 0; i < COUNT; i++) {
				cachedTransform.position = positions[i % POSITIONS_COUNT];
			}
			Metric_Stop("Cached Transfom....: ");
		}

		private void Set_B() {
			Metric_Start();
			for (int i = 0; i < COUNT; i++) {
				gameObject.transform.position = positions[i % POSITIONS_COUNT];
			}
			Metric_Stop("GameObject.Transfom: ");
		}

		public void Trigger_A() {
			runA = 30;
		}

		public void Trigger_B() {
			runB = 30;
		}

		//*********************************
		//* TIME & LOGGING METHODS
		//*********************************
		private void Metric_Init(){
			if (outputType == OutputType.File) {
				string filePath = Application.dataPath + "/output.txt";
				metricStream = File.CreateText(filePath);
				Debug.Log(filePath);
			}
		}
		private void Metric_Start(){

			//*** Save Start time
			metricTimeStart = Time.realtimeSinceStartup;
		}
		private void Metric_Stop(string pMessage = ""){

			//*** Calculate Delta
			metricsDeltaTime = (Time.realtimeSinceStartup - metricTimeStart) * 1000;
			metricsMessage = pMessage;

			delayedMetricsStop = 30;
		}
		private void Metric_Stop_Impl(){

			//*** Add to file stream
			string output = metricsMessage + metricsDeltaTime.ToString("0.000000000");
			if (outputType == OutputType.File) {
				metricStream.WriteLine(output);
			} else {
				textOutput.text += "\n" + output;
			}
		}
		private void Metric_Flush(){
			if (outputType == OutputType.File) {
				metricStream.Close();
			}
		}
	}
}