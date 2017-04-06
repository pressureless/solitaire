using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //
    private bool isMouseDown = false;
    private Vector3 lastMousePosition = Vector3.zero;
    private Vector3 oldPosition;
    private Vector3 screenPoint;
    private Vector3 offset;

    private int zIndex;

    RectTransform rt;
    Vector3 newPosition;
    //
	public int type = 0;        //0:spade; 1:heart; 2:club; 3:diamond;
    public int cardNumber = 0;  //0,1,2,3,...,51
	public int number = 0;      //1:A;2:2;...;13:K
	public int status = 0;      //0:back; 1:front;
	public int color = 0;       //0:black; 1:red;
    public bool draggable = false;  //

    private RectTransform parentRectTransform; 
    private Deck _deck;
	public GameManager gameMgr;
    public Deck deck
    {
        get
        {
            return _deck;
        }
        set
        {
            _deck = value;
        }
    }
    public GameManager spiderGameMgr;

	// Use this for initialization
	void Start () {
        zIndex = transform.GetSiblingIndex();


        //SetBackgroundImg("club1");
	}

    public void SetBackgroundImg(string str)
    {
        Image image = this.GetComponent<Image>(); 
        Sprite tempType = Resources.Load("Sprites/cards/" + str, typeof(Sprite)) as Sprite;
        image.overrideSprite = tempType;
    }

    void Awake()
    {
        rt = this.GetComponent<RectTransform>() ; 
    }
	
	// Update is called once per frame
	void Update () {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            isMouseDown = true;
            oldPosition = this.transform.position;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
            lastMousePosition = Vector3.zero;
            this.transform.position = oldPosition;
        }

        if (isMouseDown)
        {
            if (lastMousePosition != Vector3.zero)
            {
                Vector3 offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - lastMousePosition;
                this.transform.position += offset;
            }
            lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
         */
	}

     void OnMouseDown()
     { 
         isMouseDown = true;
         oldPosition = this.transform.position; 
     }
 
     void OnMouseDrag()
     {
         transform.SetAsLastSibling();
         if (lastMousePosition != Vector3.zero)
         {
             Vector3 offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - lastMousePosition;
             this.transform.position += offset;
         }
         lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
     }

     void OnMouseUp()
     {
         isMouseDown = false;
         this.transform.position = oldPosition;
         lastMousePosition = Vector3.zero;
     }


    //
    public void OnBeginDrag (PointerEventData eventData)
    { 
		if (!draggable) {
			return;
		}
        isMouseDown = true;
        oldPosition = this.transform.position; 
        zIndex = transform.GetSiblingIndex();
        this._deck.SetCardsToTop(this);  //指定所有牌

        //
        //originalPanelLocalPosition = panelRectTransform.localPosition;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, data.position, data.pressEventCamera, out originalLocalPointerPosition);  
    
    }

    void IDragHandler.OnDrag (PointerEventData eventData)
	{
		if (!draggable) {
			return;
		}
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, Input.mousePosition, eventData.enterEventCamera, out newPosition);
        if (lastMousePosition != Vector3.zero)
        {
            //RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, Input.mousePosition, eventData.enterEventCamera, out newPosition);


            Vector3 offset = newPosition - lastMousePosition;
            this.transform.position += offset;
            //lastMousePosition = newPosition;
            this._deck.SetPositionFromCard(this, this.transform.position.x, this.transform.position.y);  //同时更新多张牌的坐标
        }
        lastMousePosition = newPosition;
    }


    public void OnEndDrag(PointerEventData eventData)
	{
		if (!draggable) {
			return;
		}
        transform.SetSiblingIndex(zIndex); 
        isMouseDown = false;
        this.transform.position = oldPosition;
        lastMousePosition = Vector3.zero;
        //
        this._deck.UpdateCardsPosition(false);
    }



    //
    public string GetTexture()
    {
        string texture = "back";
        if (this.status != 0)
        {
            switch (this.type)
            {
                case    0:
                    texture = "spade";
                    break;
                case    1:
                    texture = "heart";
                    break;
                case    2:
                    texture = "club";
                    break;
                case    3:
                    texture = "diamond";
                    break;
                default:
                    break;
            }
            texture += this.number;
        }
		Debug.Log ("texture is " + texture);
        return texture;
    }


    public void RestoreBackView()
    {
        this.status = 0;

    }

    public void SetPosition(Vector3 position)
    {
        this.transform.position = position;
    }

	public void InitWithNumber(int cardNum){
		this.cardNumber = cardNum;
		this.type = Mathf.FloorToInt (cardNum / 13);
		if (this.type == 1 || this.type == 3) {
			this.color = 1;
		} else {
			this.color = 0;
		}
		this.number = (cardNum % 13) + 1;
		this.status = 0;
		//
		SetBackgroundImg(GetTexture());
	}

	public void UpdateCardImg(bool rotate){
		SetBackgroundImg (GetTexture());
	}

}
