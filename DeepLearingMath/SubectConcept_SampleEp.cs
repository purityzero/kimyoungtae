using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 기본적으로 수학 앱에서는 린 트윈으로 갑니다.
/// </summary>
namespace SubjectConcept                // nameSpace는 무조건 SubjectConcept로 묶어주세요.
{
    public class SubectConcept_SampleEp : LearningUI
    {
        /// <summary>
        /// 처음 실행할때 호출 하는 함수 입니다. 꼭 여기에다가 셋팅 값 함수를 호출 해주세요!
        /// </summary>
        public void Execute()
        {
            SetUIMedalCount(0);             // 개념은 MedalCount 가 없습니다.

            SetTopUI("일차", "단원명", null, "지시문"); // null 값은 나중에 사운드 // 지시문은 바뀔 수 있습니다.
            SetTopUI(null, null, null, "지시문"); // 바뀌는 지시문은 이렇게 바꾸시면 됩니다.

            // 하단 UI를 생성합니다. -추후 수정 
            // 첫 번째 값은 1 default 입니다. 1이 개념학습이기 때문에
            //////////////////////////////////SetBottomBase(1, gameObject.GetComponentInParent<Subject_Sample_WeekManager>().gameObject);
            //////////////////////////////////SetBottomUIBtn(false, 0, true);         // 첫번째는 그리는 기능이 없으므로 false, 두번째 index 아무거나, 세번째는 처음게임인지 여부 --- 삭제
         
        }

        /// <summary>
        /// BackBtn 기능 override 함수.
        /// </summary>
        public override void TopUIPushBackBtn()
        {
            gameObject.GetComponentInParent<Subject_Sample_WeekManager>().LearningBackPush();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 함수 명은 자유롭게 해도 됩니다. 게임 종료시 부모 값에서  NextGame함수를 불러주세요.
        /// 코루틴 말고 그냥 함수도 있으니까 주의!!!            - 변경
        /// </summary>
        public void ExGameEnd()
        {
            // 이러면 위에서 BottomBase 셋팅값에 의하여 위의 next를 부릅니다.
            StartCoroutine(gameObject.GetComponentInParent<Subject_Sample_WeekManager>().NextGame(2));
        }
    }
}