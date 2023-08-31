/**********************************************************
* 전투 State 관리
***********************************************************/
using UnityEngine;

public class StateMachineController : MonoBehaviour
{
    public static StateMachineController instance;

    // 스킬 아이콘 cancas 등

    private State currentState; // 현재 상태 저장
    private bool busy; // 상태 변경 중인지 확인
    
    //public Transform tileSelector; // 선택한 타일 정보?
    //public TileLogic selectedTile; // 현재 선택된 타일 

    //public List<Unit> units; // 유닛 리스트

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning($"{GetType()} - Destory");
            Destroy(gameObject);
        }
    }


    void Start()
    {
        ChangeTo<LoadState>();
    }

    /**********************************************************
    * asdsad
    ***********************************************************/
    public void ChangeTo<T>() where T : State
    {
        State state = GetState<T>();

        if (currentState != state)
        {
            ChangeState(state);
        }
    }

    /**********************************************************
    * asdsad
    ***********************************************************/
    public T GetState<T>() where T : State
    {
        T target = GetComponent<T>();
        if (target == null)
        {
            target = gameObject.AddComponent<T>();
        }
        return target;
    }

    /**********************************************************
    * 현재 전투 State 변경
    ***********************************************************/
    private void ChangeState(State value)
    {
        //if (busy)
        //{
        //    return;
        //}
        //busy = true;

        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = value;

        if (currentState != null)
        {
            currentState.Enter();
        }

        // busy = false;
    }
}
