using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance;

    [Header("Time Control Settings")]
    [SerializeField] private float normalTimeScale = 1f;
    [SerializeField] private float fastTimeScale = 2f;
    [SerializeField] private float superFastTimeScale = 4f;


    private float currentTimeScale = 1f;
    private void Awake(){
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    private void Start(){
        SetNormalSpeed();
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            SetNormalSpeed();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2)){
            SetFastSpeed();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3)){
            SetSuperFastSpeed();
        }
    }

    public void SetTimeScale(float timeScale){

        currentTimeScale = timeScale;
        Time.timeScale = currentTimeScale;
    }

    public void SetNormalSpeed(){
        SetTimeScale(normalTimeScale);
        Debug.Log("Normal Speed");
    }

    public void SetFastSpeed(){
        SetTimeScale(fastTimeScale);
        Debug.Log("Fast Speed");
    }

    public void SetSuperFastSpeed(){
        SetTimeScale(superFastTimeScale);
        Debug.Log("Super Fast Speed");
    }
}
