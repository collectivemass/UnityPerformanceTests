//*********************************
//* MAIN CONTROLLER CLASS FOR TEST
//*********************************
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//*********************************
//* NAMESPACE
//*********************************
namespace UnityTests.ListsVsArrays{

	//*********************************
	//* CLASS
	//*********************************
	public class Main : MonoBehaviour {

		//*********************************
		//* CONSTANTS
		//*********************************
		private const int ITERATION = 1000000;
		private static readonly int[] COUNT = new int[]{100, 1000, 10000, 1000000};

		//*********************************
		//* PROPERTIES
		//*********************************
		private int[] arrayInt;
		private List<int> listInt;

		private StreamWriter metricStream;
		private float metricTimeStart;

		//*********************************
		//* VARIABLES
		//*********************************

		//*********************************
		//* UNITY MESSAGES
		//*********************************
		private void Awake(){
			Array_CreateAndAssign();
		}
		private void Update () {
		}

		//*********************************
		//* MAIN METHODS
		//*********************************
		private void Create(){

			//*** Variables
			int i;

			//*** Init Metrics
			Metric_Init();

			//*** Loop through sizes
			for(i=0; i<COUNT.Length; i++){

				//*** Start Timing Array
				Metric_Start();
				arrayInt = new int[COUNT[i]];
				Metric_Stop();

				//*** Create Array
				Metric_Start();
				listInt = new List<int>(COUNT[i]);
				Metric_Stop();
			}

			//*** Write Metrics to disk
			Metric_Flush();
		}
		private void Array_CreateAndAssign(){

			//*** Variables
			int i, ii;

			//*** Init Metrics
			Metric_Init();

			//*** Loop through sizes
			for(i=0; i<COUNT.Length; i++){

				//*** Start Timing Array
				Metric_Start();
				arrayInt = new int[COUNT[i]];
				for(ii=0; ii<COUNT[i]; ii++){
					arrayInt[ii] = ii;
				}
				Metric_Stop("[Create & Assign] Array " + COUNT[i] + ", Time ");

				//*** Create Array
				Metric_Start();
				listInt = new List<int>(COUNT[i]);
				for(ii=0; ii<COUNT[i]; ii++){
					arrayInt[ii] = ii;
				}
				Metric_Stop("[Create & Assign] List<> " + COUNT[i] + ", Time ");
			}

			//*** Write Metrics to disk
			Metric_Flush();
		}

		//*********************************
		//* TIME & LOGGING METHODS
		//*********************************
		private void Metric_Init(){
			metricStream = File.CreateText(Application.dataPath + "/output.txt");
		}
		private void Metric_Start(){

			//*** Save Start time
			metricTimeStart = Time.realtimeSinceStartup;
		}
		private void Metric_Stop(string pMessage = ""){

			//*** Calculate Delta
			float delta = (Time.realtimeSinceStartup - metricTimeStart) * 1000;

			//*** Add to file stream
			metricStream.WriteLine(pMessage + delta.ToString("0.000000000"));
		}
		private void Metric_Flush(){
			metricStream.Close();
		}
	}
}