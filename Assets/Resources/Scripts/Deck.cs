using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {
    public GameManager spiderGameMgr;
    public int deckNum = 0;
    public int deckType = 0;
    public Card topCard;
    public ArrayList cards = new ArrayList();

	public float width;  	      //牌的宽度
	public float height;   		  //牌的高度
	private float verticalSpace;  //牌的垂直间距
	// Use this for initialization
	void Start () {
		
	}

	void Awake(){
		Vector3[] corners = new Vector3[4];
		this.GetComponent<RectTransform> ().GetWorldCorners (corners);
		height = corners [2].y - corners [0].y;
		width = corners [2].x - corners [0].x; 
		verticalSpace = height / 3.5f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PushCard(Card card)
    {
        card.deck = this;
        card.draggable = true;
        card.status = 1;
        this.cards.Add(card);
    }

    public void PushCardArray(Card[] cardArray)
    {
        for (int i = 0; i < cardArray.Length; i++ )
        {
            cardArray[i].deck = this;
            cardArray[i].draggable = true;
            cardArray[i].status = 1;
            this.cards.Add(cardArray[i]);
        }
    }

    public Card Pop()
    {
        Card retCard = null;
        int count = this.cards.Count;
        if (count > 0)
        {
            retCard = (Card)this.cards[count - 1];
            retCard.deck = null;
            this.cards.Remove(retCard);
        }
        return retCard;
    }

	public void UpdateCardsPosition(bool firstTime)
    {  
        for (int i = 0; i < this.cards.Count; i++)
		{
			Card card = (Card)this.cards[i]; 
			 
			card.gameObject.transform.position = this.gameObject.transform.position - i * new Vector3(0, verticalSpace, 0);  
			card.GetComponent<RectTransform> ().sizeDelta = new Vector2(width, height); 
            card.transform.SetAsLastSibling(); 

			if (i == this.cards.Count - 1) {//最顶层的牌才可以拖动
				card.draggable = true; 
				card.status = 1;
				card.UpdateCardImg (firstTime);
			} else {
				if (firstTime) { //第一次下面的牌都不可以移动
					card.draggable = false;
					card.status = 0;
				} 
				card.UpdateCardImg (false);
			}
        }
    }

    public void SetPositionFromCard(Card card, float x, float y)
    {
        int i;
        for (i = 0; i < this.cards.Count; i++)
        {
            if (this.cards[i] == card)
            {
                break;
            }
        }
        int m = 0;
        for (int j = i; j < this.cards.Count; j++)
        {
			((Card)this.cards[j]).SetPosition(new Vector3(x, y - m++ * verticalSpace, 0));
        }
    }

    //从card开始的所有的牌置顶
    public void SetCardsToTop(Card card)
    {
        bool found = false;
        for (int i = 0; i < this.cards.Count; i++)
        {
            if (this.cards[i] == card)
            {
                found = true;
            }
            if (found)
            {
                ((Card)this.cards[i]).transform.SetAsLastSibling();
            }
        }
    }

	public Card GetTopCard(){
		if (this.cards.Count > 0) {
			return (Card)this.cards [this.cards.Count - 1];
		} else {
			return null;
		}
	}

	//是否能接收改card
	public bool AcceptCard(Card card){
		Card topCard = GetTopCard ();
		if (topCard != null) {
			if (topCard.color != card.color && topCard.number == card.number + 1) {
				return true;
			} else {
				return false;
			}
		} else {  //没有底牌，只能放K
			if (card.number == 13) {
				return true;
			}
			return false;
		}
	}
}
