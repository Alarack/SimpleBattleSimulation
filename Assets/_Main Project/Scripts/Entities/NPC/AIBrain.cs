using LL.Events;
using LL.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIBrain : MonoBehaviour {



    [Header("State Data")]
    public List<StateData> stateData = new List<StateData>();

    public AISensor Sensor { get; private set; }
    public NPCMovement Movement { get; private set; }
    public WeaponManager WeaponManager { get; private set; }
    public NPC Owner { get; private set; }

    public string CurrentStateName { get { return fsm.CurrentState.stateName; } }

    public string debugCurrentState;
    public string debugPreviousState;

    private FSM fsm;


    private void Awake() {
        Owner = GetComponent<NPC>();
        Movement = GetComponent<NPCMovement>();
        WeaponManager = GetComponent<WeaponManager>();
        Sensor = GetComponentInChildren<AISensor>();
    }


    private void Start() {

        if (Sensor != null)
            Sensor.Initialize(Owner, this);

        fsm = new FSM(Owner, stateData);

        debugCurrentState = fsm.CurrentState != null ? fsm.CurrentState.stateName : "No Current State";
        debugPreviousState = fsm.PreviousState != null ? fsm.PreviousState.stateName : "No Previous State";
    }


    private void Update() {
        if (Owner.active == false)
            return;

        fsm.ManagedUpdate();
    }

    private void FixedUpdate() {
        if (Owner.active == false)
            return;

        fsm.ManagedFixedUpdate();
    }

    public void Attack() {
        WeaponManager.FireAllWeapons();
    }

    public void OnUnitDetected() {
        ReceiveStateChange("Attack");
    }

    public void OnNoTargetsLeft() {
        ReceiveStateChange("Idle");
    }

    public bool GetLatestSensorTarget() {
        if (Sensor.LatestTarget == null) {
            Movement.SetTarget(null);
            return false;
        }

        Movement.SetTarget(Sensor.LatestTarget.transform);
        return true;
    }


    #region AI TRIGGER BUSINESS

    public void ReceiveStateChange(string stateName) {

        fsm.ChangeState(stateName);

        debugCurrentState = fsm.CurrentState != null ? fsm.CurrentState.stateName : "No Current State";
        debugPreviousState = fsm.PreviousState != null ? fsm.PreviousState.stateName : "No Previous State";
    }

   

    #endregion


}
