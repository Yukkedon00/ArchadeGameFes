
using Cysharp.Threading.Tasks;
using Photon.Pun;
using R3;
using UnityEngine;


public class AirHockeyController : MonoBehaviour
{
    [SerializeField] private AirHockeyPointController pointController;
    [SerializeField] private AirHockeyAreaManager areaManager;

    [SerializeField] private Transform resetPackPosition;
    
    private static int defaultSetPoint = 10;
    
    private AirHockeyModel playerPointModel;
    private AirHockeyModel enemyPointModel;

    private Pack packObj;

    private bool isEnd = false;

    // プレイヤーネームもここらへんでやる
    public void Initialize(AirHockeyPhotonStatus status)
    {
        // NOTE:詳細は今後実装
        playerPointModel = new AirHockeyModel(
            0,
            0,
            ePointType.Player,
            "PlayerName");
        enemyPointModel = new AirHockeyModel(
            0,
            0,
            ePointType.Enemy,
            "EnemyName");

        playerPointModel.ScorePoint.Subscribe(point =>
        {
            if (IsEnd(point))
            {
                isEnd = true;
            }
        }).AddTo(this);
        
        // MEMO:ゴール処理ここって間違ってね？とは思いつつ、
        // プロトタイプだから許そう
        GoalArea.GoalTypeObserver.SubscribeAwait(async (type, ct) =>
        {
            AddScorePoint(type, 1);
            
            await UniTask.WaitForSeconds(2f, cancellationToken: ct);
            
            Debug.Log($"Player:{playerPointModel.ScorePoint} Enemy:{enemyPointModel.ScorePoint}");

            if (PhotonNetwork.IsMasterClient)
            {
                packObj.ResetPosition(resetPackPosition.position);
            }
            
        }).AddTo(this);
        
        pointController.Initialize(playerPointModel, enemyPointModel);
        
        areaManager.Initialize(status.AreaPos);
    }

    public void GameStart()
    {
        packObj.ResetPosition(resetPackPosition.position);
    }

    public void SetPack(Pack pack)
    {
        packObj = pack;
    }

    private bool IsGoal()
    {
        
        return false;
    }

    private bool IsEnd(int point)
    {
        return false;
    }

    private void AddScorePoint(ePointType type, int addScore)
    {
        switch (type)
        {
            case ePointType.Enemy:
                enemyPointModel.AddScorePoint(addScore);
                pointController.UpdateEnemyPoint(enemyPointModel.ScorePoint.CurrentValue);
                break;
            case ePointType.Player:
                playerPointModel.AddScorePoint(addScore);
                pointController.UpdatePlayerPoint(playerPointModel.ScorePoint.CurrentValue);
                break;
        }
    }
}
