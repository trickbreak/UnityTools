using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trickbreak.CoroutineScheduler
{
    public class Scheduler
    {
        private MonoBehaviour target; 
        
        private Coroutine runningCoroutine = null;

        private List<ScheduleBase> schedules = new();



        public bool IsRunnig => runningCoroutine != null;
        
        

        /// <summary>
        /// 스케줄을 만들기전 우선순위를 계산 합니다.
        /// value 값이 null인 경우 스케줄의 가장 마지막 우선순위를 부여 합니다.
        /// </summary>
        private int PriorityCalculate(int? value)
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

        /// <summary>
        /// 스케줄을 등록 및 실행 합니다.
        /// </summary>
        private void StartSchedule(ScheduleBase schedule)
        {
            AddSchedule(schedule);

            TrySchedulesStart();
            


            // 스케줄의 우선순위에 맞춰 리스트에 삽입 합니다.
            void AddSchedule(ScheduleBase schedule)
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
                        if (schedules[0] is ScheduleCoroutine scheduleCoroutine)
                        {
                            yield return scheduleCoroutine.Invoke();
                        }
                        else if (schedules[0] is ScheduleAction scheduleAction)
                        {
                            scheduleAction.Invoke();
                        }

                        schedules.RemoveAt(0);
                    }

                    runningCoroutine = null;
                }
            }
        }



        public Scheduler(MonoBehaviour target)
        {
            this.target = target;
        }

        /// <summary>
        /// 코루틴 타입의 스케줄을 등록 및 실행 합니다.
        /// </summary>
        /// <param name="work">실행 되는 로직 입니다.</param>
        /// <param name="tag">삭제시 식별을 위한 값 입니다.</param>
        /// <param name="priority">스케줄이 등록 되는 우선 순위 입니다. 값을 입력 하지 않으면 가장 마지막에 추가 됩니다.</param>
        public void StartSchedule(Func<IEnumerator> work, string tag = "noTags", int? priority = null)
        {
            int calculatedPriority = PriorityCalculate(priority);

            ScheduleCoroutine newSchedule = new ScheduleCoroutine(tag, calculatedPriority, work);

            StartSchedule(newSchedule);
        }

        /// <summary>
        /// 액션 타입의 스케줄을 등록 및 실행 합니다.
        /// </summary>
        /// <param name="work">실행 되는 로직 입니다.</param>
        /// <param name="tag">삭제시 식별을 위한 값 입니다.</param>
        /// <param name="priority">스케줄이 등록 되는 우선 순위 입니다. 값을 입력 하지 않으면 가장 마지막에 추가 됩니다.</param>
        public void StartSchedule(Action work, string tag = "noTags", int? priority = null)
        {
            int calculatedPriority = PriorityCalculate(priority);

            ScheduleAction newSchedule = new ScheduleAction(tag, calculatedPriority, work);

            StartSchedule(newSchedule);
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

        /// <summary>
        /// 진행중인 스케줄까지만 진행 하고, 나머지 모든 스케줄을 제거 합니다.
        /// </summary>
        public void AllStop()
        {
            if (!IsRunnig)
            {
                schedules.Clear();
            }
            else
            {
                schedules.RemoveRange(1, schedules.Count - 1);
            }
        }
        
        /// <summary>
        /// 진행 중인 스케줄을 즉시 멈추고, 모든 스케줄을 제거 합니다.
        /// </summary>
        public void AllStopImmediate()
        {
            if (runningCoroutine != null)
            {
                target.StopCoroutine(runningCoroutine);
                runningCoroutine = null;
            }
            
            schedules.Clear();
        }
    }
}
