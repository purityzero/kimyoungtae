using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SubjectConcept
{
    public class SubjectConcept_WeekManager : LearningUI
    {
        [HideInInspector]
        public int DayNum;

       /// <summary>
       /// 중간매니저 상에서 게임을 호출할때 Init을 부른다. Day 값을 확인하여 게임을 List에 담는다. 
       /// </summary>
       /// <param name="tDay">날짜</param>
        public virtual void Init(int tDay)
        {
           
        }
    }
}