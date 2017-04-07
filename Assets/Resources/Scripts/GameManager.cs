using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public CardLogic cardLogic;
	public GameObject modalLayer;
	public GameObject winLayer;
	public GameObject settingLayer;
	public GameObject ruleLayer;
	public Text timeLabel;
	public Text scoreLabel;
	public Text stepsLabel;


	private int timeCount;    //计时器秒数
	private int stepCount;    //步数
	private int scoreCount;   //分数
	// Use this for initialization
	void Start () { 
		
	}

	void Awake(){	
		timeCount = 0;
		stepCount = 0;
		scoreLabel.text = "1000";
		SetTimeLabel (65);
	}
	//根据秒数显示时间
	void SetTimeLabel(int seconds){
		int sec = seconds % 60;
		int min = (seconds % 3600) / 60;
		timeLabel.text = string.Format ("{0,2}:{1,2}", min.ToString().PadLeft(2, '0'), sec.ToString().PadLeft(2, '0'));
	}

	// Update is called once per frame
	void Update () {

	}

	public void HasWinGame(){
		winLayer.SetActive (true);
	}

	public void OnClickModalRandom(){
	}

	public void OnClickModalReplay(){
	}

	public void OnClickModalClose(){
		modalLayer.SetActive (false); 
	}

	public void OnClickWinNewGame(){
	}

	public void OnClickSettingBtn(){
		settingLayer.SetActive (true);
	}

	public void OnClickPlayBtn(){
		modalLayer.SetActive (true);
	}

	public void OnClickRecommendBtn(){
	}

	public void OnClickUndoBtn(){
		
	}

	public void OnClickSettingBackBtn(){ 
		settingLayer.SetActive (false); 
	}

	public void OnClickSettingLayerRuleBtn(){ 
		ruleLayer.SetActive (true);
	}

	public void OnClickRuleBackBtn(){
		ruleLayer.SetActive (false);
	}

	public void CardMove(){
		stepCount++;
		stepsLabel.text = stepCount.ToString ();
	}
	//清空状态
	public void RestoreInitialState(){ 
		
	}


}
