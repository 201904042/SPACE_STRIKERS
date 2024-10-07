using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LoginInterface : UIInterface
{

    public Transform InputFields;
    public TMP_InputField emailField;
    public TMP_InputField pwField;

    public Transform Btns;
    public Button cancelBtn;
    public Button logInBtn;
    public Button createAccount; //회원가입 버튼

    protected override void Awake()
    {
        base.Awake();
        SetBtnHandler();
    }

    private void OnEnable()
    {
        ResetAll();
    }

    private void SetBtnHandler()
    {
        cancelBtn.onClick.RemoveAllListeners();
        logInBtn.onClick.RemoveAllListeners();
        createAccount.onClick.RemoveAllListeners();
        cancelBtn.onClick.AddListener(CloseInterface);
        logInBtn.onClick.AddListener(LoginHandler);
        createAccount.onClick.AddListener(CreateAuthHandler);
    }

    protected override void OnConfirm(bool isConfirmed)
    {
        base.OnConfirm(isConfirmed);
    }

    public override void SetComponent()
    {
        base.SetComponent();
        InputFields = transform.GetChild(1);
        emailField = InputFields.GetChild(0).GetComponentInChildren<TMP_InputField>();
        pwField = InputFields.GetChild(1).GetComponentInChildren<TMP_InputField>();
        Btns = transform.GetChild(2);
        cancelBtn = Btns.GetChild(0).GetComponent<Button>();
        logInBtn = Btns.GetChild(1).GetComponent<Button>();
        createAccount = Btns.GetChild(2).GetComponent<Button>();
    }

    public void ResetAll()
    {
        emailField.text = "";
        pwField.text = "";
    }

    //회원가입 버튼
    public void CreateAuthHandler()
    {
        CreateDoubleCheck();
        //Application.OpenURL("https://naver.com"); //홈페이지의 회원가입 페이지로 이동
    }

    private void CreateDoubleCheck()
    {
        StartUI.tfInterface.SetTFContent("해당 아이디와 비밀번호로 아이디를 생성하시겠습니까?");
        StartCoroutine(CreateTFCheck());
    }

    private IEnumerator CreateTFCheck()
    {
        TFInterface tFInterface = StartUI.tfInterface;

        if ((bool)tFInterface.result)
        {
            if (string.IsNullOrEmpty(emailField.text) || string.IsNullOrEmpty(pwField.text))
            {
                StartUI.alertInterface.SetAlert("아이디 혹은 비밀번호를 확인하세요");
                yield break;
            }

            Task<bool> createTask = Auth_Firebase.Instance.CreateAccountAsync(emailField.text, pwField.text);
            yield return new WaitUntil(() => createTask.IsCompleted);

            if (createTask.Result)
            {
                //회원가잆 성공
                //todo -> 회원정보와 더불어 코드를 통해 회원의 게임정보 데이터베이스 생성
                ResetAll();
            }
            else
            {
                StartUI.alertInterface.SetAlert("회원가입 실패");
                ResetAll();
            }
        }
        else
        {
            StartUI.alertInterface.SetAlert("회원가입 취소됨");
        }
    }


    //로그인 버튼
    public void LoginHandler()
    {
        LoginDoubleCheck();
    }

    private void LoginDoubleCheck()
    {
        StartUI.tfInterface.SetTFContent("해당 아이디와 비밀번호로 로그인하시겠습니까?");
        StartCoroutine(LoginTFCheck());
    }

    private IEnumerator LoginTFCheck()
    {
        TFInterface tFInterface = StartUI.tfInterface;

        yield return StartCoroutine(tFInterface.GetValue());

        if ((bool)tFInterface.result)
        {
            if (string.IsNullOrEmpty(emailField.text) || string.IsNullOrEmpty(pwField.text))
            {
                StartUI.alertInterface.SetAlert("아이디 혹은 비밀번호를 확인하세요");
                yield break;
            }

            Task<bool> loginTask = Auth_Firebase.Instance.LoginAsync(emailField.text, pwField.text);
            yield return new WaitUntil(() => loginTask.IsCompleted);

            if (loginTask.Result)
            {
                CloseInterface();
            }
            else
            {
                StartUI.alertInterface.SetAlert("로그인 실패");
                ResetAll();
            }
        }
        else
        {
            StartUI.alertInterface.SetAlert("로그인이 취소됨");
        }
    }


}
