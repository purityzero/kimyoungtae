using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SubjectConcept
{

    public class SubjectConcept_LearningBase : MonoBehaviour, LearningBase
    {
        private Dictionary<string, GameObject> WeekPanel = new Dictionary<string, GameObject>();
        private GameObject WeekGameObj;
        [HideInInspector]
        public object[] Object;
        [HideInInspector]
        public GameObject Learning;
        public float offsety;
        public bool IsTestMode;
        public const string prefabPath = "Prefabs/Learning/Subject/SubjectConcept/";

        public int TestDay;
        public int TestWeeks;
        public int TestAge;

        /// <summary>
        /// LearningManager에서 호출
        /// </summary>
        /// <param name="obj">
        /// [0] -> GameObject
        /// [1] -> GameType(0 ->Day / 1->Idx) 
        /// [2] -> OffsetY                    
        /// [3] -> Day                        
        /// [4] -> Idx                        
        /// </param>
        /// 
        public void LearningStart(object[] obj)
        {
            Learning = (GameObject)obj[0];
            offsety = (float)obj[2];
            int day = (int)obj[3];
            int Idx = (int)obj[4];

            int tweek = (day - 1) / 5;
            int tday = (day - 1) % 5;

            ResourcesLoad(tweek, tday);
        }

        /// <summary>
        /// ResourcesLoad 함수는 주차와 날짜값을 받아와서 프리팹을 생성한다.
        /// 추후 AssetBundle Load로 수정 예정
        /// </summary>
        /// <param name="weeks">추차</param>
        /// <param name = "day" ></ param >
        public void ResourcesLoad(int weeks, int day)
        {
            Vector3 ScaleSet = Vector3.one;
            //CommonLoad(); 준호껀데, 계속 가져가면 이거 4~6학년에 넣어줘야함.
            int age;
            if (IsTestMode == true)
            {
                age = TestAge;
            }
            else
            {
                age = GlobalStateData.Age;
            }

            string str = age + "Age_" + (weeks + 1) + "Weeks";

            if (WeekPanel.ContainsKey(str) == false)
            {
                WeekGameObj = Instantiate(Resources.Load("Prefabs/Learning/Subject/SubjectConcept/" + age + "Age/" + (weeks + 1).ToString() + "Weeks", typeof(GameObject))) as GameObject;
                WeekGameObj.name = str;
                WeekPanel[str] = WeekGameObj;
                WeekPanel[str].SetActive(true);
                Debug.Log(" WeekPanel[str] : " + WeekPanel[str]);
                WeekPanel[str].transform.parent = gameObject.transform;
                WeekPanel[str].transform.localScale = ScaleSet;
                WeekPanel[str].GetComponent<SubjectConcept_WeekManager>().Init(day);
            }
            else
            {
                WeekPanel[str].SetActive(true);
                WeekPanel[str].GetComponent<SubjectConcept_WeekManager>().DayNum = day;
                WeekPanel[str].GetComponent<SubjectConcept_WeekManager>().Init(day);
            }
        }

        /// <summary>
        /// 마지막 게임에서 호출, 하위 매니저에서 NextGame이 끝날때 사용한다.
        /// </summary>
        /// <param name="obj"> </param>
        public void LearningEnd(object[] obj)
        {
            Debug.Log("Call Learning End");
            gameObject.SetActive(false);
            if (IsTestMode == true)
            {
                gameObject.SendMessage("LearningGameEnd", obj, SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                obj[0] = 0;
                obj[1] = 0;
                obj[2] = 1;
                Learning.SendMessage("LearningGameEnd", obj, SendMessageOptions.DontRequireReceiver);
            }
        }

        /// <summary>
        /// Test Mode에서 호출 추후 사라질 예정
        /// </summary>
        public void LearningBackTest()
        {
            Debug.Log("Succeeded Calling Learning Back Test");
        }

        /// <summary>
        /// 게임을 중도 포기 시 호출한다. BackBtn 버튼에 넣어주자!
        /// </summary>
        public void LearningBackBtn()
        {
            gameObject.SetActive(false);
            if (IsTestMode == true)
            {
                gameObject.SendMessage("LearningBackTest", SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                Learning.SendMessage("LearningBack", SendMessageOptions.DontRequireReceiver);
            }
        }

        public void WeeksGameSetFalse()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 테스트 모드용
        /// </summary>
        public void Start()
        {
            if (IsTestMode == true)
            {
                ResourcesLoad(TestWeeks - 1, TestDay);
            }
        }
    }
}