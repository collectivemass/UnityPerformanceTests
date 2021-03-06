﻿//*********************************
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
namespace UnityTests.ListsFindVsItteration{

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

		//*********************************
		//* VARIABLES
		//*********************************
		private List<Data> listOfData = new List<Data>();
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

			//*** Loop through sizes
			for(i=0; i<LENGTH; i++){
				listOfData.Add(new Data {
					name = System.Guid.NewGuid().ToString(),
					value = Random.Range(int.MinValue, int.MaxValue)
				});
			}
		}
		private void Run(){

			//*** Init Metrics
			Metric_Init();

			string toFindEnd = listOfData[listOfData.Count - 1].name;
			string toFindMid = listOfData[Mathf.FloorToInt(listOfData.Count * 0.5f)].name;


			Get(toFindEnd);
			Get(toFindEnd);
			Get(toFindEnd);
			Get(toFindMid);
			Get(toFindMid);
			Get(toFindMid);


			//*** Write Metrics to disk
			Metric_Flush();
		}

		private void Get(string pObjectToFind) {
			Data foundObject;
			
			
			Metric_Start();
			for (int i = 0; i < listOfData.Count; i++) {
				if (listOfData[i].name == pObjectToFind) {
					foundObject = listOfData[i];
					break;
				}
			}
			Metric_Stop("Itteration ");

			Metric_Start();
			foundObject = listOfData.Find(objectToFind => objectToFind.name == pObjectToFind);
			Metric_Stop("List Find ");



			Metric_Start();
			foundObject = listOfData.Find(objectToFind => objectToFind.name == pObjectToFind);
			Metric_Stop("List Find ");
			
			Metric_Start();
			for (int i = 0; i < listOfData.Count; i++) {
				if (listOfData[i].name == pObjectToFind) {
					foundObject = listOfData[i];
					break;
				}
			}
			Metric_Stop("Itteration ");
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