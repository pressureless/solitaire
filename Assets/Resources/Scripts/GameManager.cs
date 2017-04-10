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
    public Text timeWinLabel;
    public Text scoreWinLabel;
    public Text stepsWinLabel;
    public Text toastLabel;

	public Image soundSwitch;

	private int timeCount;    //计时器秒数
	private int stepCount;    //步数
	private int scoreCount;   //分数
    private Coroutine timeCoroutine; 

	private bool soundEnable; //开关控制

    public AudioClip cycleClip;
    public AudioClip winClip;
    public AudioClip cancelClip;
    public AudioClip rotateClip;
	// Use this for initialization
	void Start () {
        InitMenuView(); 
	}

    void Awake()
    {
        soundEnable = true;
	}

    void InitMenuView()
    {
        timeCount = 0;
        SetTimeLabel(0);
        stepCount = 0;
        stepsLabel.text = stepCount.ToString();
        scoreCount = 0; 
        scoreLabel.text = scoreCount.ToString();
        StopGameTimer();
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
        StopGameTimer();
        timeWinLabel.text = "YOUR TIME: " + timeLabel.text;
        scoreWinLabel.text = "YOUR SCORE: " + (scoreCount + Public.SCORE_NUMBER/timeCount).ToString();
        stepsWinLabel.text = "YOUR MOVES: " + stepCount.ToString();
        PlayGameAudio(Public.AUDIO_TYPE_WIN);
	} 

	public void OnClickModalRandom(){
		modalLayer.SetActive (false);
        cardLogic.Shuffle(false);
	}

	public void OnClickModalReplay(){
		modalLayer.SetActive (false);
        cardLogic.Shuffle(true);
	}

	public void OnClickModalClose(){
		modalLayer.SetActive (false); 
	}

	public void OnClickWinNewGame(){
        winLayer.SetActive(false);
        cardLogic.Shuffle(false);
	}

	public void OnClickSettingBtn(){
		settingLayer.SetActive (true);
	}

	public void OnClickPlayBtn(){
		modalLayer.SetActive (true);
	}

	public void OnClickRecommendBtn(){
        ShowTips();
	}

	public void OnClickUndoBtn(){
        ShowTips();
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

	public void OnClickSoundSwitch(){ 
		soundEnable = !soundEnable;
		UpdateSoundSwitchImg ();
	}

	public void UpdateSoundSwitchImg()
	{ 
		string imgName = "sound_switch_on";
		if (!soundEnable) {
			imgName = "sound_switch_off";
		} 
		Sprite tempType = Resources.Load("Sprites/settings/" + imgName, typeof(Sprite)) as Sprite; 
		soundSwitch.overrideSprite = tempType;
	}

	public void CardMove(){
		stepCount++;
		stepsLabel.text = stepCount.ToString ();
        if (stepCount == 1) //有效操作之后开始计时
        {
            timeCoroutine = StartCoroutine(GameTimer());
        }
	}
	//清空状态
	public void RestoreInitialState(){
        InitMenuView();
	}

    IEnumerator GameTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            timeCount++;
            if (timeCount % 30 == 0)
            {
                AddScoreValue(Public.SOCRE_OVER_THIRTY_SECONDS);
            }
            SetTimeLabel(timeCount);
        }
    }

    void StopGameTimer() {
        if (timeCoroutine != null)
        {
            StopCoroutine(timeCoroutine);
        }
    }

    public void AddScoreValue(int value)
    {
        scoreCount += value;
        if (scoreCount < 0)
        {
            scoreCount = 0;
        }
        scoreLabel.text = scoreCount.ToString();
    }

    public void PlayGameAudio(int type) {
        if (!soundEnable)
        {
            return;
        }
        AudioSource audioSource = this.GetComponent<AudioSource>(); 
        switch (type)
        {
            case Public.AUDIO_TYPE_CANCEL:
                audioSource.clip = cancelClip;
                break;
            case Public.AUDIO_TYPE_ROTATE:
                audioSource.clip = rotateClip;
                break;
            case Public.AUDIO_TYPE_CYCLE:
                audioSource.clip = cycleClip;
                break;
            case Public.AUDIO_TYPE_WIN:
                audioSource.clip = winClip;
                break;
            default:
                break;
        }
        audioSource.Play();
    }

    public void ShowTips()
    {
        toastLabel.gameObject.SetActive(true);
        Invoke("HideTips", 1f);
    }

    public void HideTips()
    {
        toastLabel.gameObject.SetActive(false);
        Debug.Log("Hide tips");
    }
}
