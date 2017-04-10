using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Deck : MonoBehaviour,  IPointerClickHandler{ 
	public CardLogic cardLogic;
    public int deckNum = 0;
    public int deckType = 0;
    public Card topCard;
    public ArrayList cards = new ArrayList();

	public float width;  	      //牌的宽度
	public float height;   		  //牌的高度
	private float verticalSpace;  //牌的垂直间距
	private float wasteHorSpace;  //waste牌桌的牌间距
	// Use this for initialization
	void Start () {
		
	}

	void Awake(){
		Vector3[] corners = new Vector3[4];
		this.GetComponent<RectTransform> ().GetWorldCorners (corners);
		height = corners [2].y - corners [0].y;
		width = corners [2].x - corners [0].x; 
		verticalSpace = height / 3.5f;
		wasteHorSpace = width / 3.0f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SetBackgroundImg(string str)
    {
        Image image = this.GetComponent<Image>();
        Sprite tempType = Resources.Load("Sprites/decks/" + str, typeof(Sprite)) as Sprite;
        image.overrideSprite = tempType;
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
	//弹出从某一张牌开始到顶部的所有牌
	public Card[] PopFromCard(Card card){
		int i = 0;
		int count = cards.Count;
		while (i < count) {
			if (cards[i] == card) {
				break;
			}
			i++;
		}
		Card[] cardArray = new Card[count - i];
		int k = 0;
		for (int j = i; j < count; j++) {
			cardArray [count - i - 1 - (k++)] = Pop (); 
		}
		return cardArray;
	}

	public void UpdateCardsPosition(bool firstTime)
    {  
        for (int i = 0; i < this.cards.Count; i++)
		{
			Card card = (Card)this.cards[i]; 
			card.GetComponent<RectTransform> ().sizeDelta = new Vector2(width, height); 
			card.transform.SetAsLastSibling(); 
			if (deckType == Public.DECK_TYPE_PACK) {
				card.draggable = false;
				card.gameObject.transform.position = this.gameObject.transform.position;
				card.RestoreBackView ();
			} else {
				if (deckType == Public.DECK_TYPE_BOTTOM) {
					card.gameObject.transform.position = this.gameObject.transform.position - i * new Vector3 (0, verticalSpace, 0);  
				} else if (deckType == Public.DECK_TYPE_ACE) {
					card.gameObject.transform.position = this.gameObject.transform.position;
				} else {
					card.draggable = false;
					card.gameObject.transform.position = this.gameObject.transform.position;
					if (cards.Count == 2) {
						if (i == 1) {
							card.gameObject.transform.position = this.gameObject.transform.position + new Vector3 (wasteHorSpace, 0, 0);
							card.draggable = true;
						}
					}
					else if (cards.Count >= 3) {
						if (i == cards.Count - 1) {
							card.gameObject.transform.position = this.gameObject.transform.position + new Vector3 (2 * wasteHorSpace, 0, 0);
							card.draggable = true;
						}
						else if(i == cards.Count - 2){
							card.gameObject.transform.position = this.gameObject.transform.position + new Vector3 (wasteHorSpace, 0, 0);
						}
					}
				}

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
		UpdateCardsActiveStatus ();
    }

	//更改牌的active状态，为了避免牌的重叠导致的阴影
	public void UpdateCardsActiveStatus(){
		int compareNum = 4;
		if (deckType == Public.DECK_TYPE_ACE || deckType == Public.DECK_TYPE_WASTE || deckType == Public.DECK_TYPE_PACK) {
			if (cards.Count > 0) {
				int j = 0;
				if (deckType == Public.DECK_TYPE_PACK) {
					compareNum = 2;
				}
				for (int i = cards.Count - 1; i >= 0; i--) {
					Card card = (Card)cards [i];
					if (j < compareNum) {
						card.gameObject.SetActive (true);
						j++;
					} else {
						card.gameObject.SetActive (false);
					}
				}
			}
		} else {
			for (int i = cards.Count - 1; i >= 0; i--) {
				((Card)cards [i]).gameObject.SetActive (true);
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
		Debug.Log ("Position is " + card.transform.position);
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
	//获取最上方的牌
	public Card GetTopCard(){
		if (this.cards.Count > 0) {
			return (Card)this.cards [this.cards.Count - 1];
		} else {
			return null;
		}
	}
	//获取已翻转的最下面的牌
	public Card GetBottomFrontCard(){
		Card card = null;
		if (deckType == Public.DECK_TYPE_PACK) {
			return GetTopCard ();
		}
		for (int i = 0; i < cards.Count; i++) {
			if (((Card)cards[i]).status == 1) {
				card = (Card)cards[i];
				break;
			}
		}
		return card;
	}
	//是否能接收改card
	public bool AcceptCard(Card card){
		Card topCard = GetTopCard ();
		switch (deckType) {
		case	Public.DECK_TYPE_BOTTOM:
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
			break;
		case	Public.DECK_TYPE_ACE:
			Deck srcDeck = card.deck;
			if (srcDeck.GetTopCard() != card) { //确保是最上层的牌才考虑是否接收
				return false;
			}
			if (topCard != null) {
				if (topCard.type == card.type && topCard.number == card.number - 1) {
					return true;
				} else {
					return false;
				}
			} else {  //没有底牌，只能放A
				if (card.number == 1) {
					return true;
				}
				return false;
			}
			break;
		default:
			break;
		}
		return false;
	}
	//判断是否与该card重叠
	public bool OverlapWithCard(Card card){
		if (card.deck == this) {
			Debug.Log ("Self deck");
			return false;
		}
		bool bOverlaped = false;
		float x1 = this.transform.position.x;
		float x2 = x1 + width;
		float y1 = this.transform.position.y;
		Card topCard = GetTopCard ();
		if (topCard) {
			y1 = topCard.transform.position.y;
		} 
		float y2 = y1 + height;

		float x11 = card.transform.position.x;
		float x21 = x11 + width;
		float y11 = card.transform.position.y;
		float y21 = y11 + height;

		//Debug.Log ("x1:" + x1 + "; x2:" + x2 + "; y1:" + y1 + "; y2:" + y2 + ";");
		//Debug.Log ("x11:" + x11 + "; x21:" + x21 + "; y11:" + y11 + "; y21:" + y21 + ";");
		float INTERSECT_SPACE = 10;
		if ((x2 > (x11 + INTERSECT_SPACE ) && x1 < x11) || (x1 > x11 && x1 < (x21 - INTERSECT_SPACE)))
		{
			if ((y1 > y11 && y1 < y21) || (y1 < y11 && y2 > y11))
			{
				bOverlaped = true;
			}
		}
		return bOverlaped;
	}
	//处理点击事件
	public void OnPointerClick(PointerEventData eventData){
		cardLogic.OnClickPack ();
	}
	//获取牌的数目
	public int GetCardNums(){
		return cards.Count;
	}
	//清空状态
	public void RestoreInitialState(){
		for (int i = 0; i < cards.Count; i++) {
			Card card = (Card)cards[i];
			card.RestoreBackView ();
		}
		cards.Clear ();
	}
}
