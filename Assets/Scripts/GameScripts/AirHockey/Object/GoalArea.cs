using System;
using R3;
using UnityEngine;

public class GoalArea : MonoBehaviour
{
    public static Observable<ePointType> GoalTypeObserver => goalTypeSubject;
    private static Subject<ePointType> goalTypeSubject = new Subject<ePointType>();
    
    private ePointType goalType;

    public void Initialize(ePointType type)
    {
        goalType = type;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ball")
        {
            // ここパックの処理をここでやってんのちょっとおかしくね？
            var pack = other.gameObject.GetComponent<Pack>();
            pack.InactivePack();
            goalTypeSubject.OnNext(goalType);
        }
    }
}
