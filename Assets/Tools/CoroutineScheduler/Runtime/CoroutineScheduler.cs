using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 규칙 1. 우선 순위는 숫자가 클수록 먼저 실행 됩니다.
/// 규칙 2. 우선 순위가 같은 스케줄이 추가 되면, 같은 작업 중 가장 뒤에 추가 됩니다.
/// 규칙 3. 추가 되는 스케줄의 우선 순위가 실행 중인 스케줄의 우선순위 보다 높더라도, 실행 중인 스케줄이 끝난 뒤 실행 됩니다.
/// 규칙 4. 실행중인 스케줄이 없을때 우선순위가 없는 작업이 추가 되면 추가된 작업의 우선순위는 0 입니다.
/// </summary>
public class CoroutineScheduler
{
    private class Schedule
    {
        private Func<IEnumerator> work;
        
        public string Tag { get; }

        public int Priority { get; }
        

        
        public Schedule(string tag, int priority, Func<IEnumerator> work)
        {
            Tag = tag;
            Priority = priority;
            this.work = work;
        }

        public IEnumerator Invoke()
        {
            if (work == null)
            {
                yield break;
            }
            
            yield return work.Invoke();
        }
    }

    
    
    private MonoBehaviour target; 
    
    private Coroutine runningCoroutine = null;

    private List<Schedule> schedules = new();
    
    
    
    public CoroutineScheduler(MonoBehaviour target)
    {
        this.target = target;
    }
    
    /// <summary>
    /// 스케줄을 등록 및 실행 합니다.
    /// </summary>
    /// <param name="work">실행 되는 로직 입니다.</param>
    /// <param name="tag">삭제시 식별을 위한 값 입니다.</param>
    /// <param name="priority">스케줄이 등록 되는 우선 순위 입니다. 값을 입력 하지 않으면 가장 마지막에 추가 됩니다.</param>
    /// <returns></returns>
    public Coroutine StartSchedule(Func<IEnumerator> work, string tag = "noTags", int? priority = null)
    {
        int priorityInt = PriorityConvertToInt(priority);

        Schedule newSchedule = new Schedule(tag, priorityInt, work);
        
        AddSchedule(newSchedule);
        
        TrySchedulesStart();

        return runningCoroutine;



        // 우선순위 값이 없는 경우 스케줄의 가장 마지막 우선순위를 부여 합니다.
        int PriorityConvertToInt(int? value)
        {
            if (value != null)
            {
                return value.Value;
            }
                
            if (schedules.Count > 0)
            {
                return schedules[^1].Priority;
            }
                
            return 0;
        }
        
        // 스케줄의 우선순위에 맞춰 리스트에 삽입 합니다.
        void AddSchedule(Schedule schedule)
        {
            // index 0 은 실행중인 스케줄이므로 실행중인 스케줄을 제외 하고 우선 순위를 비교해서 삽입 합니다.
            for (int i = 1; i < schedules.Count; i++)
            {
                if (schedule.Priority > schedules[i].Priority)
                {
                    schedules.Insert(i, schedule);
                    return;
                } 
            }
            
            schedules.Add(schedule);
        }
        
        // 코루틴이 실행중이 아닌 경우 실행 합니다.
        void TrySchedulesStart()
        {
            if (runningCoroutine == null)
            {
                runningCoroutine = target.StartCoroutine(SchedulesRun());
            }
        
            IEnumerator SchedulesRun()
            {
                while (schedules.Count > 0)
                {
                    yield return schedules[0].Invoke();
                    schedules.RemoveAt(0);            
                }

                runningCoroutine = null;
            }        
        }
    }

    /// <summary>
    /// 실행중이 아닌 스케줄을 제거 할 수 있습니다.
    /// </summary>
    public void RemoveSchedule(string tag)
    {
        // index 0 은 실행중인 스케줄이라 제외
        int index = 1;
        
        while (index < schedules.Count)
        {
            if (schedules[index].Tag == tag)
            {
                schedules.RemoveAt(index);
            }
            else
            {
                index++;
            }
        }
    }
}
