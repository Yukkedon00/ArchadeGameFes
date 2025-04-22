using UnityEngine;

public enum eMoveAreaPos
{
    None,
    Left,
    Right
}

public class AirHockeyAreaManager : MonoBehaviour
{
    [SerializeField] private GameObject moveAreaLeft;
    [SerializeField] private GameObject moveAreaRight;

    [SerializeField] private GoalArea goalAreaLeft;
    [SerializeField] private GoalArea goalAreaRight;
    
    public void Initialize(eMoveAreaPos moveArea)
    {
        switch (moveArea)
        {
            case eMoveAreaPos.Left:
                moveAreaLeft.SetActive(true);
                moveAreaRight.SetActive(false);
                goalAreaLeft.Initialize(ePointType.Player);
                goalAreaRight.Initialize(ePointType.Enemy);
                break;
            case eMoveAreaPos.Right:
                moveAreaLeft.SetActive(false);
                moveAreaRight.SetActive(true);
                goalAreaLeft.Initialize(ePointType.Enemy);
                goalAreaRight.Initialize(ePointType.Player);
                break;
        }
    }
}
