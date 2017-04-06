using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public GameObject baseDeck; 
	public Card[] cardsArray = new Card[52];
	public Deck[] bottomDeckArray = new Deck[7];
	public Deck[] aceDeckArray = new Deck[4];
	public Deck   wasteDeck;
	public Deck   packDeck; 

	public EventManager evtMgr;    //
	// Use this for initialization
	void Start () { 
        //初始化牌组
        this.InitCardNodes();

        //cards
        int baseIndex = baseDeck.transform.GetSiblingIndex(); 
        int k = 0;
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < i + 1; j++)
            {
                this.bottomDeckArray[i].PushCard(this.cardsArray[k]);  
                k++;
            }
            this.bottomDeckArray[i].UpdateCardsPosition(true);
        }
	}

	void Awake(){
		evtMgr = new EventManager ();
	}

    void InitCardNodes()
    {
        for (int i = 0; i < 52; i++)
        {
            GameObject objPrefab = (GameObject)Resources.Load("Prefabs/card");
            objPrefab = Instantiate(objPrefab);
			objPrefab.transform.SetParent(wasteDeck.transform.parent); 
            this.cardsArray[i] = objPrefab.GetComponent<Card>();
			this.cardsArray [i].InitWithNumber (i);
			this.cardsArray [i].gameMgr = this;

        }
    }
	
	// Update is called once per frame
	void Update () {

	}

}
