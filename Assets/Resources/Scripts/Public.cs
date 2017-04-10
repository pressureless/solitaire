using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Public : MonoBehaviour {

	public const int DECK_TYPE_BOTTOM = 1;
	public const int DECK_TYPE_ACE = 2;
	public const int DECK_TYPE_PACK = 3;
	public const int DECK_TYPE_WASTE = 4;

	public const int CARD_NUMS = 52;

    public const int SCORE_NUMBER = 600000;

    public const int SCORE_MOVE_TO_ACE = 10;  //移到回收单元，加分
    public const int SOCRE_OVER_THIRTY_SECONDS = -30;  //每过30秒，减分

    public const int AUDIO_TYPE_CANCEL = 0;
    public const int AUDIO_TYPE_ROTATE = 1;
    public const int AUDIO_TYPE_CYCLE = 2;
    public const int AUDIO_TYPE_WIN = 3;
}
