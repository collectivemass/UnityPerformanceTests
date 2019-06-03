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
namespace UnityTests.NullCheck{

	public class Data {
		public string name;
		public int value;
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
		private const int LENGTH = 1000;

		//*********************************
		//* PROPERTIES
		//*********************************
		[SerializeField]
		private TextMeshProUGUI textOutput;
		[SerializeField]
		private OutputType outputType;
		[SerializeField]
		private Object objectToCheck;

		//*********************************
		//* VARIABLES
		//*********************************
		private StreamWriter metricStream;
		private float metricTimeStart;

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

			Get_B();
			Get_A();
			Get_B();
			Get_C();
			Get_B();
			Get_A();
			Get_C();
			Get_A();
			Get_B();

			//*** Write Metrics to disk
			Metric_Flush();
		}

		private void Get_A() {
			
			Metric_Start();
			for (int i = 0; i < LENGTH; i++) {
				if (objectToCheck == null) {
				} else {
				}
			}
			Metric_Stop("Null ");
		}
		private void Get_B() {

			Metric_Start();
			for (int i = 0; i < LENGTH; i++) {
				if (ReferenceEquals(objectToCheck, null)) {
				} else {
				}
			}
			Metric_Stop("Ref Comp ");
		}
		private void Get_C() {

			Metric_Start();
			for (int i = 0; i < LENGTH; i++) {
				if (objectToCheck is null) {
				} else {
				}
			}
			Metric_Stop("is Null ");
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