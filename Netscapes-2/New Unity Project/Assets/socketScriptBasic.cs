using UnityEngine;
using System.Collections;
using System;
using WebSocketSharp; // this needed to run websocket extensions
using System.Collections.Generic;

  public class socketScriptBasic : MonoBehaviour{
	
	WebSocket ws = new WebSocket ("ws://127.0.0.1:1880/unitySocket"); //setup for node-red ip and websocket name
	
	public GameObject tweetObject; 
	// inspector reference to a prefab, later used for dynamically creating instances
	// for testing create a 3D object, and some 3D text. Make text child of object so that it follows it.


	public List<string> stringList = new List<string>(); // creation of a list to hold each tweet

	public int lastListLength; // number for calculating how many objects are created
	public int currentListLength; // number of listed tweets to compare with objects created

	string content; // to hold the actual tweet
	public int sentiment = 0; // to hold the tweets corresponding sentiment score -5 > +5

	//Everything that has been removed was just an example of setup up different colours of 
	//text and materials epending on the sentiment score


void Start ()
     {
     	//setup for recieving data from the websocket
     	//the tweet comes first, followed directly by its seperate sentiment score (the score is nver more than 2 digits)

		ws.OnMessage += (sender, e) => { //bring in data from websocket and place in 'e'
			if(e.Data.Length > 2){ // if data is greater than 2 (its a tweet)
				stringList.Add (e.Data); // add tweet text to the string list
				//print ("tweet: " + e.Data);
			}else{
				// otherwise it is smaller than 2 digits and so the corresponding sentiment score
				sentiment = int.Parse(e.Data); // convert this to a whole number
			}
			currentListLength=stringList.Count; //current list lentgh is how tweets in the string list
		};	

		ws.Connect (); // with setup done, connent to websocket

	}

void Update(){
		
		if (currentListLength > lastListLength) { // a way to create objects 1 by 1, by checking if actual tweets recevied number is bigger than our own created number
			
			//create a random vector3 for position
			Vector3 spawnPos = new Vector3(UnityEngine.Random.Range (-200f,200f),UnityEngine.Random.Range(-50f,50f), UnityEngine.Random.Range (-200f, 200f));

			//content is gathered and updated using the SortedString method below
			content = SortedString(stringList[stringList.Count-1]);

			//get the text object attached to the object you've created and referenced above
			TextMesh GetText = tweetObject.transform.Find ("msgObj").gameObject.GetComponent<TextMesh> ();

			// write the tweet out into the text field
			GetText.text = content;
			
			//aactually now create an instantance of this object (your prefab, position set above, at prefab rotation)
			GameObject Tweet = Instantiate (tweetObject, spawnPos, Quaternion.identity) as GameObject;
			
			// now we have created 1 object add 1 to our object list
			lastListLength+=1;
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

	