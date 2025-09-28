using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    // 상수
    private const int MAX_PAN = 20;

    // private 필드(컴포넌트)
    private CardSettingManager cardSettingManager;
    private SaveLoadManager saveLoadManager;
    private DeckManager deckManager;
    private HandManager handManager;

    // private 필드
    private int currentRound; // 현재 판 수
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
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // 컴포넌트 할당
        TryGetComponent(out cardSettingManager);
        TryGetComponent(out saveLoadManager);
        TryGetComponent(out deckManager);
        TryGetComponent(out handManager);
    }
    private async void Start()
    {
        // 새 게임 혹은 이어하기를 눌렀을 때(현재는 새 게임으로 항상 가정)
        await InitGame(InitGameEnum.NewGame);
    }

    // 메인
    private async Task InitGame(InitGameEnum initGame) // 새 게임 혹은 이어하기(타이틀의 버튼에 연결)
    {
        currentRound = 0;

        if (initGame == InitGameEnum.NewGame) // 새 게임인 경우
        {
            if (!PlayerPrefs.HasKey("TutorialCompleted")) // 첫 실행인 경우 튜토리얼
            {
                PlayerPrefs.SetInt("TutorialCompleted", 0);
                PlayerPrefs.Save();
                PlayerPrefs.SetInt("TutorialCompleted", 1);

                // TODO
            }
            else if (PlayerPrefs.GetInt("TutorialCompleted") == 0) // 튜토리얼을 완료하지 못한 경우 튜토리얼
            {
                PlayerPrefs.SetInt("TutorialCompleted", 1);

                // TODO
            }
            else // 튜토리얼을 완료한 경우 바로 본 게임
            {
                await StartRound();
            }
        }
        else if (initGame == InitGameEnum.LoadGame) // 이어하기인 경우
        {
            // TODO
        }
    }
    private async Task StartRound() // 라운드 시작
    {
        cardSettingManager.SetMiddlePile(); // 해당 판에서 사용할 패 오브젝트들을 생성
        await cardSettingManager.DealCards(); // 손 패 나눠주기
    }
    private void StartSetUp() // 판 종료 후 화투를 추가하거나 규칙을 추가하는 정비 시작
    {

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