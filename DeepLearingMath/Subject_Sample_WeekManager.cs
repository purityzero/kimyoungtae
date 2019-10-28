using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SubjectConcept
{
    /// <summary>
    /// 주차 관리 코드 Subject_WeekManager를 상속 받는다
    /// </summary>
    public class Subject_Sample_WeekManager : SubjectConcept_WeekManager
    {
        public                              List<GameObject> EpList = new List<GameObject>();                   // 해당 주차에 있는 게임 List
        [HideInInspector]
        public                              List<GameObject> DayGameList = new List<GameObject>();              // 일차가 들어오면 EpList에 있는 게임을 받아온다.
        [HideInInspector]
        private int                         GameNum;                                                            // 게임이 넘어가면 카운트를 세서 DayGameList에 있는 수와 동일해지면 개념학습을 종료한다.

        /// <summary>
        /// SubjectConcept_WeekManager 에서 가상함수를 만들어서 Override, 최상위 매니저에서 Init을 부른다. 
        /// GameNum을 초기화 시키고 해당 Day를 받아오고, GameList를 셋팅하는 함수를 부른다.
        /// </summary>
        /// <param name="tDay"></param>
        public override void Init(int tDay)
        {
            DayNum = tDay;
            GameNum = 0;
            GameListSet();
        }

        /// <summary>
        /// Day를 조건문으로 GameList를 담는다. 그리고 게임을 실행 시킨다. 
        /// </summary>
        private void GameListSet()
        {
            DayGameList.Clear();
            if (DayNum == 0)
            {
                // DayGameList.Add(EpList[0]);
                GameStart();        // -- 추가
            }
            else if (DayNum == 1)
            {
                // DayGameList.Add(EpList[1]);
                // DayGameList.Add(EpList[2]);
                GameStart();        // -- 추가
            }
            else
            {
                // 게임이 없을때 부릅니다. Day가 없을때는 항상 else문으로 해주세요.     - 추가 해주세요!
                DayGameNull();
            }
            //GameStart(); -- 삭제
        }

        /// <summary>
        /// 일차에 게임이 없을 때 불리는 함수 입니다. GameListSet()에서 부릅니다.   - 추가 해주세요!
        /// </summary>
        private void DayGameNull()
        {
            object[] obj = new object[4];
            gameObject.SetActive(false);
            gameObject.GetComponentInParent<SubjectConcept_LearningBase>().LearningEnd(obj);
        }

        /// <summary>
        /// 게임 리스트를 담은 후 게임을 실행시키는 함수 
        /// </summary>
        private void GameStart()
        {
            // SendMassage를 부르게 한다. 추후 수정 예정
            DayGameList[0].SetActive(true);
            DayGameList[GameNum].SendMessage("Execute");
        }

        /// <summary>
        /// 한 게임을 Clear하면 NextGame 하위 계층에서 함수를 부른다. 추후에 하위 계층에서 OnEnalbe을 SendMassage 함수명을 Execute?로 바꾼다. 
        /// </summary>
        public void NextGame()
        {
            object[] obj = new object[4];
            GameNum = GameNum + 1;
            if (DayNum == 0)
            {
                if (GameNum == DayGameList.Count)
                {
                    DayGameList[GameNum - 1].SetActive(false);
                    DayGameList.Clear();
                    gameObject.SetActive(false);
                    gameObject.GetComponentInParent<SubjectConcept_LearningBase>().LearningEnd(obj);
                }
                else
                {
                    DayGameList[GameNum - 1].SetActive(false);
                    DayGameList[GameNum].SetActive(true);
                    DayGameList[GameNum].SendMessage("Execute");
                }
            }
            else
            {
                if (GameNum == DayGameList.Count)
                {
                    DayGameList[GameNum - 1].SetActive(false);
                    DayGameList.Clear();
                    gameObject.SetActive(false);
                    gameObject.GetComponentInParent<SubjectConcept_LearningBase>().LearningEnd(obj);

                }
                else
                {
                    DayGameList[GameNum - 1].SetActive(false);
                    DayGameList[GameNum].SetActive(true);
                    DayGameList[GameNum].SendMessage("Execute");
                }
            }
        }

        // 일반적으로 게임이 끝났을떄 부르는 함수
        public IEnumerator NextGame(float sec)
        {
            yield return new WaitForSeconds(sec);
            object[] obj = new object[4];
            GameNum = GameNum + 1;
            if (DayNum == 0)
            {
                if (GameNum == DayGameList.Count)
                {
                    DayGameList[GameNum - 1].SetActive(false);
                    DayGameList.Clear();
                    gameObject.SetActive(false);
                    gameObject.GetComponentInParent<SubjectConcept_LearningBase>().LearningEnd(obj);
                }
                else
                {
                    DayGameList[GameNum - 1].SetActive(false);
                    DayGameList[GameNum].SetActive(true);
                    DayGameList[GameNum].SendMessage("Execute");
                }
            }
            else
            {
                if (GameNum == DayGameList.Count)
                {
                    DayGameList[GameNum - 1].SetActive(false);
                    DayGameList.Clear();
                    gameObject.SetActive(false);
                    gameObject.GetComponentInParent<SubjectConcept_LearningBase>().LearningEnd(obj);

                }
                else
                {
                    DayGameList[GameNum - 1].SetActive(false);
                    DayGameList[GameNum].SetActive(true);
                    DayGameList[GameNum].SendMessage("Execute");
                }
            }
        }
        /// <summary>
        /// Main에서 부르는 함수 - 삭제
        /// </summary>
        //public void PreiviewGame()
        //{
        //    DayGameList[GameNum].SetActive(false);
        //    DayGameList[GameNum - 1].SetActive(true);
        //    DayGameList[GameNum - 1].SendMessage("Execute");
        //}

        /// <summary>
        ///  게임 중도 포기시 부르는 함수.
        /// </summary>
        public void LearningBackPush()
        {
            for (int i = 0; i < DayGameList.Count; i++)
            {
                DayGameList[i].SetActive(false);
            }
            DayGameList.Clear();
            gameObject.SetActive(false);
            gameObject.GetComponentInParent<SubjectConcept_LearningBase>().LearningBackBtn();
        }
    }
}
