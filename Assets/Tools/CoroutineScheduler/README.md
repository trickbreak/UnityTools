# CoroutineScheduler

## 기본 설명
- 여러 행위(이후 스케줄이라 명명)가 동시다발로 발생하는 경우, 이를 순차 처리하기 위한 스케줄러입니다.
- 등록된 순서 혹은 스케줄의 우선순위에 따라서 순차적으로 스케줄이 실행할 수 있습니다.

## 기본 규칙
1. 우선순위의 숫자가 클수록 먼저 실행됩니다.
2. 실행 중인 스케줄이 없을 때 스케줄이 등록되면 즉시 실행됩니다.
3. 실행 중인 스케줄이 없을 때 등록된 스케줄의 우선순위가 명시되지 않았다면 우선순위 값은 0입니다.
4. 실행 중인 스케줄이 있을 때 등록된 스케줄의 우선순위가 명시되지 않았다면 우선순위 값은 가장 마지막에 있는 스케줄의 우선순위 값과 동일합니다.
5. 우선순위가 같은 스케줄이 추가되면, 우선순위가 같은 작업 중 가장 뒤에 추가됩니다.
6. 추가되는 스케줄의 우선순위가 실행 중인 스케줄의 우선순위 보다 높더라도, 실행 중인 스케줄이 끝난 뒤 실행됩니다.

## 사용 예시
- RankingManager와 EventManager가 동시에 각자의 스케줄을 실행하기 위해서 매초 검사 중입니다.
- RankingManager와 EventManager에서 동시에 조건을 만족하게 되면 스케줄이 겹치게 됩니다.
- 스케줄러를 도입하여 등록된 순서 또는 우선순위에 맞춰 순차 처리됩니다.
```csharp
using System.Collections;
using Trickbreak.CoroutineScheduler;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CoroutineScheduler Scheduler { get; private set; }

    private void Awake()
    {
        Scheduler = new CoroutineScheduler(this);
    }
}

public class EventManager
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
        // 다른 곳에서 이미 EventTimelinePlay 스케줄을 등록 한 경우 제거하고 다시 등록 합니다.
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



public class RankingManager
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
        // 다른 곳에서 이미 RankingScoreCounting 스케줄을 등록 한 경우 제거하고 다시 등록 합니다.
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
```
