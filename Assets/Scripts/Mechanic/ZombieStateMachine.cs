using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieStateMachine 
{
    private ZombieState_ currentState;
    private Zombie zombie;


    public ZombieStateMachine(Zombie zombie)
    {
        this.zombie = zombie; //Lưu zombie mà state machine đang quản lý
    }
    public void ChangeState(ZombieState_ state)
    {
        currentState?.ExitState(zombie); //Thực thi logic thoát khỏi trạng thái cũ
        currentState = state;
        currentState.EnterState(zombie); //Thực thi logic khởi tạo trạng thái mới
    }

    public void UpdateState()
    {
        Debug.Log("Zombie current state: " + currentState);
        currentState?.Handle(zombie, zombie.GetHealth()); // Xử lí logic của trạng thái hiện tại
    }

    public ZombieState_ GetCurrentState()
    {
        return currentState;
    }
}
