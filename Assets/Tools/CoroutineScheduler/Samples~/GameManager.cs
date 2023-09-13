using System.Collections;
using Trickbreak.CoroutineScheduler;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Scheduler Scheduler { get; private set; }

    private void Awake()
    {
        Scheduler = new Scheduler(this);
    }
}

public class EventManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    /// <summary>
    /// 1초 마다 이벤트 연출을 보여줄 수 있는지 판단 하고, 할 수 있는 경우 이벤트 연출을 스케줄에 등록 합니다.
    /// </summary>
    private IEnumerator Start()
    {
        while (true)
        {
            if (CanEventTimeLinePlay())
            {
                AddSchedulerEventTimelinePlay();
                yield break;
            }
            else
            {
                yield return new WaitForSeconds(1.0f);
            }
        }
    }

    private bool CanEventTimeLinePlay()
    {
        // 이벤트 타임라인을 플레이 할 조건을 만족하면 true, 만족 하지 않으면 false
        return false;
    }
    
    private void AddSchedulerEventTimelinePlay()
    {
        gameManager.Scheduler.RemoveSchedule(tag: "EventTimelinePlay");
        gameManager.Scheduler.StartSchedule(EventTimelinePlay, tag: "EventTimelinePlay");

        
        
        IEnumerator EventTimelinePlay()
        {
            Debug.Log("이벤트 연출 시작");
            yield return new WaitForSeconds(2.0f);
            Debug.Log("이벤트 연출 종료");
        }
    }
}



public class RankingManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    /// <summary>
    /// 1초 마다 랭킹 집계를 할 수 있는지 판단 하고, 할 수 있는 경우 랭킹 집계를 스케줄에 등록 합니다.
    /// </summary>
    private IEnumerator Start()
    {
        while (true)
        {
            if (CanRankingScoreCounting())
            {
                AddSchedulerRankingScoreCounting();
                yield break;
            }
            else
            {
                yield return new WaitForSeconds(1.0f);
            }
        }
    }

    private bool CanRankingScoreCounting()
    {
        // 랭킹 집계를 할 수 있으면 true, 할 수 없으면 false
        return false;
    }
    
    private void AddSchedulerRankingScoreCounting()
    {
        gameManager.Scheduler.RemoveSchedule(tag: "RankingScoreCounting");
        gameManager.Scheduler.StartSchedule(work: RankingScoreCounting, tag: "RankingScoreCounting");
        
        
        
        IEnumerator RankingScoreCounting()
        {
            Debug.Log("랭킹 점수 집계 시작");
            yield return new WaitForSeconds(3.0f);
            Debug.Log("랭킹 점수 집계 종료");
        }
    }
}