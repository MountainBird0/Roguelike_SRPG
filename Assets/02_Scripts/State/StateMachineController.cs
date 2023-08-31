/**********************************************************
* ���� State ����
***********************************************************/
using UnityEngine;

public class StateMachineController : MonoBehaviour
{
    public static StateMachineController instance;

    // ��ų ������ cancas ��

    private State currentState; // ���� ���� ����
    private bool busy; // ���� ���� ������ Ȯ��
    
    //public Transform tileSelector; // ������ Ÿ�� ����?
    //public TileLogic selectedTile; // ���� ���õ� Ÿ�� 

    //public List<Unit> units; // ���� ����Ʈ

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
    * ���� ���� State ����
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
