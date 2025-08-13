using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    [Header("컴포넌트")]
    PlayerCard pCard;

    FieldCardManager fieldCardManager;
    HandCardManager handCardManager;
    AcquiredCardManager acquiredCardManager;

    [Header("프리팹")]
    [SerializeField] GameObject prefab_CardObj;

    [Header("주요 프로퍼티")]
    const int MAX_ROUND = 24;
    int currentRound;
    public GameFlowStateEnum GameFlowState;

    [Header("싱글턴")]
    static GameFlowManager instance;
    public static GameFlowManager Instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        pCard = GetComponent<PlayerCard>();

        fieldCardManager = GetComponent<FieldCardManager>();
        handCardManager = GetComponent<HandCardManager>();
        acquiredCardManager = GetComponent<AcquiredCardManager>();
    }

    #region 주요 메서드
    void InitGame(InitGameEnum initGame) // 게임 실행 후 새 게임, 이어하기 모두 여기서 관리
    {
        currentRound = 0;

        if (initGame == InitGameEnum.NewGame) // 새 게임인 경우
        {
            if (!PlayerPrefs.HasKey("TutorialCompleted")) // 첫 실행인 경우 튜토리얼
            {
                PlayerPrefs.SetInt("TutorialCompleted", 0);
                PlayerPrefs.Save();
                // TODO
            }
            else if (PlayerPrefs.GetInt("TutorialCompleted") == 0) // 튜토리얼을 완료하지 못한 경우 튜토리얼
            {
                // TODO
            }
            else // 튜토리얼을 완료한 경우 바로 본 게임
            {
                StartRound();
            }
        }
        else if (initGame == InitGameEnum.LoadGame) // 이어하기인 경우
        {
            // TODO
        }
    }
    void StartRound() // 본 게임 시작
    {
        // 본 게임 화면으로 넘어온다.

        fieldCardManager.GenerateMiddlePile(); // 중간 더미 생성
        handCardManager.SetPlayerHandCards(); // 내가 먼저 세팅했던 손패를 받는다.
        // 상대에게 패 10장을 준다.
        // 바닥에 8장을 깐다.
        // 나 먼저 카드를 낸다.
    }
    void StartSetting() // 본 게임 전 구매 및 덱 정비
    {
        // 내 카드 10장을 뽑고 그 중 1장에 꽃을 그려넣는다.
        // 사용 카드인 버섯 카드를 구매한다.
        // 달성한 꽃 키우기를 클릭하여 보상을 얻는다.
        // 모든 세팅을 마친 플레이어는 시작 버튼을 눌러 본 게임을 시작한다.

    }
    #endregion
    #region 보조 메서드
    public bool IsInState(List<GameFlowStateEnum> states)
    {
        return states.Contains(GameFlowState);
    }
    public bool IsInState(GameFlowStateEnum states)
    {
        return states == GameFlowState;
    }
    #endregion
}
public enum GameFlowStateEnum
{
    None, Setting, Round,
}
public enum InitGameEnum
{
    None, NewGame, LoadGame
}