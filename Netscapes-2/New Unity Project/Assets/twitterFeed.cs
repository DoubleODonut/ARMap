using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System;

public class twitterFeed : MonoBehaviour {

	//WebSocket ws = new WebSocket("ws://127.0.0.1:1880/unitySocket4"); //Setup for the Node-Red IP and the websocket name
	WebSocket ws = new WebSocket("ws://10.188.46.16:1880/unitySocket4"); //Setup for the Node-Red IP off my laptop

	public GameObject tweetObject; 




	public List<string> stringList = new List<string>(); //Creates a list to hold the tweets 

	public int lastListLength; //Number for calculating how many objects are created
	public int currentListLength; //Number of listed tweets to compare with objects created

	string content; // to hold the actual tweet




	void Start ()
	{
		
		ws.OnMessage += (sender, e) => { //bring in data from websocket and place in 'e'
			if(e.Data.Length > 2){ // if data is greater than 2 (its a tweet)
				stringList.Add (e.Data); // add tweet text to the string list

			}else{
				print ("This is not a tweet");
			}
			currentListLength= 5; //Current list length is how many tweets is in the string list
		};	

		ws.Connect (); // with setup done, connent to websocket

	}

	public int LastTime = 0;

	void Update(){
		

		if (LastTime+5 < System.DateTime.Now.Second) { //If last time + 5 is less than current time, run function 
			if (currentListLength > lastListLength) { 

				//create a random vector3 for position - x, y, z 
				Vector3 spawnPos = new Vector3 (1500, 1000, 1000);
			

				//content is gathered and updated using the SortedString method below
				content = SortedString (stringList [stringList.Count - 1]);

				//get the text object attached to the object you've created and referenced above
				TextMesh GetText = GameObject.Find ("Text").gameObject.GetComponent<TextMesh> ();

				// write the tweet out into the text field
				GetText.text = content;

				//aactually now create an instantance of this object (your prefab, position set above, at prefab rotation)
				//GameObject Tweet = Instantiate (tweetObject, spawnPos, Quaternion.identity) as GameObject;

				// now we have created 1 object add 1 to our object list
				//lastListLength+=1;
			}
			LastTime = System.DateTime.Now.Second; //lastTime = Current Time
		}


	}


	private string SortedString(string str){

		int TweetNum = 0; //current tweet number
		string sorted = ""; //final output string
		string[] tweetText = new string[50]; // list containing tweets, setup maximum charcter allowance set to 50
		string[] strWords = str.Split (' '); // list containing words, split incoming text by spaces
		for (int i = 0; i < strWords.Length; i++) { //loop setup to cycle depending on how many words are in a tweet
			tweetText[TweetNum] = tweetText[TweetNum] + " " + strWords[i]; // set the tweet text to be made of the strings words with a space inbetween each

			if(tweetText[TweetNum].Length > 25){  // if tweet is more than 25 charcters
				tweetText[TweetNum] = tweetText[TweetNum] + " \n"; // add a new line
				TweetNum++; // add to current tweets sorted
				print ("TweetNum: " + TweetNum);
			}
		}
		for(int i = 0; i < tweetText.Length; i++){ 
			sorted = sorted + tweetText[i]; // sets sorted to be the current sorted tweet text
		}

		return sorted; //returns to sortedstring function to be read by content in the update
	}



}



