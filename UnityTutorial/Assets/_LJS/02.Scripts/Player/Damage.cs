using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    private const string bulletTag = "BULLET";
    // private const string enemyTag = "ENEMY";
    private float initHp = 100.0f;
    public float currentHp;
    // BloodScreen 텍스쳐를 저장하기 위한 변수
    public Image bloodScreen;
    // HPBar 이미지를 저장하기 위한 변수
    public Image hpBar;
    // HP 게이지의 초기 색상 (녹색)
    private readonly Color initColor = new Vector4(0, 1.0f, 0.0f, 1.0f);
    private Color currentColor;

    // 델리게이트 및 이벤트 선언
    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;

    void OnEnable()
    {
        GameManager.OnItemChange += UpdateSetup;
    }

    void UpdateSetup()
    {
        initHp = GameManager.instance.gameData.hp;
        currentHp += GameManager.instance.gameData.hp - currentHp;
    }

    // Start is called before the first frame update
    void Start()
    {
        // 불러온 데이터 값을 HP에 적용
        initHp = GameManager.instance.gameData.hp;
        currentHp = initHp;

        // HP 게이지의 초기 색상 설정
        hpBar.color = initColor;
        currentColor = initColor;
    }

    // 충돌한 Collider의 IsTrigger 옵션이 체크됐을 때 발생
    void OnTriggerEnter(Collider coll)
    {
        // 충돌한 Collider의 태그가 BULLET이면 Player의 currentHp를 차감
        if (coll.CompareTag(bulletTag))
        {
            Destroy(coll.gameObject);

            // 혈흔 효과를 표현할 코루틴 함수 호출
            StartCoroutine(ShowBloodScreen());

            currentHp -= 5.0f;
            Debug.Log("Player HP = " + currentHp.ToString());

            // HP 게이지의 색상 및 크기 변경 함수 호출
            DisplayHpBar();
            
            // Player의 HP가 0 이하면 사망 처리
            if (currentHp <= 0.0f)
            {
                PlayerDie();
            }
        }
    }

    // 혈흔 효과 함수
    IEnumerator ShowBloodScreen()
    {
        // BloodScreen 텍스쳐의 알파값을 불규칙하게 변경
        bloodScreen.color = new Color(1, 0, 0, Random.Range(0.2f, 0.3f));

        yield return new WaitForSeconds(0.1f);

        // BloodScreen 텍스쳐의 색상을 모두 0으로 변경
        bloodScreen.color = Color.clear;
    }

    // HP 게이지 출력 함수
    void DisplayHpBar()
    {
        // HP 수치가 50%일 때까지는 녹색에서 노란색으로 변경
        if ((currentHp / initHp) > 0.5f)
        {
            currentColor.r = (1 - (currentHp / initHp)) * 2.0f;
        }
        // HP 수치가 0%일 때까지는 노란색에서 빨간색으로 변경
        else
        {
            currentColor.g = (currentHp / initHp) * 2.0f;
        }

        // HPBar 색상 변경
        hpBar.color = currentColor;
        // HPBar 크기 변경
        hpBar.fillAmount = (currentHp / initHp);
    }

    // Player의 사망 처리 루틴
    void PlayerDie()
    {
        OnPlayerDie();
        GameManager.instance.isGameOver = true;
        // Debug.Log("PlayerDie !");
        // // "ENEMY" 태그로 지정된 모든 적 캐릭터를 추출해 배열에 저장
        // GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        // 
        // // 배열의 처음부터 순회하면서 적 캐릭터의 OnPlayerDie 함수를 호출
        // for (int i = 0; i < enemies.Length; i++)
        // {
        //     enemies[i].SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        // }
    }
}
