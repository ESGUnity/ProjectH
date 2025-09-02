using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    // 상수
    private const int MAX_PAN = 20;

    // 프리팹
    [Header("프리팹")]
    [SerializeField] private GameObject prefab_CardObj;

    // private 필드(컴포넌트)
    private PlayerCard pCard;
    private OppoCard oCard;
    private CardSettingManager cardSettingManager;

    // private 필드
    private int currentPan; // 현재 판 수
    private GameStateEnum gameState;

    // public Getter
    public GameStateEnum GameState => gameState;

    // 싱글턴
    private static GameFlowManager instance;
    public static GameFlowManager Instance => instance;

    // 유니티 콜백
    private void Awake()
    {
        // 싱글턴
        if (instance != null)
        {
            Debug.LogError("GameFlowManager Send : 중복 싱글턴 생성 시도.");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // 컴포넌트 할당
        TryGetComponent(out pCard);
        TryGetComponent(out oCard);
        TryGetComponent(out cardSettingManager);
    }

    // 메인
    private void InitGame(InitGameEnum initGame) // 새 게임 혹은 이어하기(타이틀의 버튼에 연결)
    {
        currentPan = 0;

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
                StartPan();
            }
        }
        else if (initGame == InitGameEnum.LoadGame) // 이어하기인 경우
        {
            // TODO
        }
    }
    private void StartPan() // 판 시작
    {
        cardSettingManager.GenerateInitCards(); // 해당 판에서 사용할 패 오브젝트들을 생성
        cardSettingManager.DealHandCards(); // 손 패 나눠주기
    }
    private void StartSetUp() // 판 종료 후 화투를 추가하거나 규칙을 추가하는 정비 시작
    {
        // 내 카드 10장을 뽑고 그 중 1장에 꽃을 그려넣는다.
        // 사용 카드인 버섯 카드를 구매한다.
        // 달성한 꽃 키우기를 클릭하여 보상을 얻는다.
        // 모든 세팅을 마친 플레이어는 시작 버튼을 눌러 본 게임을 시작한다.

    }

    // 유틸
    public bool IsInState(List<GameStateEnum> states)
    {
        return states.Contains(gameState);
    }
    public bool IsInState(GameStateEnum states)
    {
        return states == gameState;
    }
}