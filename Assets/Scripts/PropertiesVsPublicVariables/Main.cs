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
namespace UnityTests.PropertiesVsPublicVariables{

	public class ObjectA {
		public int value;
	}

	public class ObjectB {
		public int value {
			get; set;
		}
	}


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

		//*********************************
		//* PROPERTIES
		//*********************************
		[SerializeField]
		private TextMeshProUGUI textOutput;
		[SerializeField]
		private OutputType outputType;

		//*********************************
		//* VARIABLES
		//*********************************
		private StreamWriter metricStream;
		private float metricTimeStart;

		private ObjectA objectA = new ObjectA();
		private ObjectB objectB = new ObjectB();

		//*********************************
		//* UNITY MESSAGES
		//*********************************
		private void Awake(){
			Create();
			Run();
		}

		//*********************************
		//* MAIN METHODS
		//*********************************
		private void Create(){

			//*** Variables
			int i;
		}
		private void Run(){

			//*** Init Metrics
			Metric_Init();

			//*** Tests
			Set_A();
			Set_B();
			Get_A();
			Get_B();
			Set_A();
			Set_B();
			Get_A();
			Get_B();
			Set_A();
			Set_B();
			Get_A();
			Get_B();
			
			//*** Write Metrics to disk
			Metric_Flush();
		}

		private void Set_A() {
			Metric_Start();
			for (int i = 0; i < COUNT; i++) {
				objectA.value = Random.Range(int.MinValue, int.MaxValue);
			}
			Metric_Stop("Variable Set ");
		}

		private void Set_B() {
			Metric_Start();
			for (int i = 0; i < COUNT; i++) {
				objectB.value = Random.Range(int.MinValue, int.MaxValue);
			}
			Metric_Stop("Property Set ");
		}

		private void Get_A() {
			Metric_Start();
			int value = 0;
			for (int i = 0; i < COUNT; i++) {
				value = objectA.value;
			}
			Metric_Stop("Variable Get ");
		}

		private void Get_B() {
			Metric_Start();
			int value = 0;
			for (int i = 0; i < COUNT; i++) {
				value = objectB.value;
			}
			Metric_Stop("Property Get ");
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
			float delta = (Time.realtimeSinceStartup - metricTimeStart) * 1000;

			//*** Add to file stream
			string output = pMessage + delta.ToString("0.000000000");
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

			Debug.Log("Completed");
		}
	}
}