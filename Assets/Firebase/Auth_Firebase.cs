using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Auth_Firebase
{
    private static Auth_Firebase instance = null;
    public static Auth_Firebase Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new Auth_Firebase();
            }

            return instance;
        }
    }

    public Action<bool> LoginState;

    public FirebaseAuth auth;
    public FirebaseUser user;

    public string UserId => user.UserId;

    public void Init()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += OnChanged;
    }

    private void OnChanged(object sender, EventArgs e)
    {
        if (auth.CurrentUser != user)
        {
            bool signed = (auth.CurrentUser != user && auth.CurrentUser != null);
            if (!signed && user != null)
            {
                //로그아웃된 상태
                LoginState?.Invoke(false);
            }

            user = auth.CurrentUser;
            if (signed)
            {
                //로그인 상태
                LoginState?.Invoke(true);
            }

        }
    }


    

    public async Task<bool> CreateAccountAsync(string emailText, string pwText)
    {
        bool isSuccess = false;
        try
        {
            // Firebase 인증 요청 대기
            var authResult = await auth.CreateUserWithEmailAndPasswordAsync(emailText, pwText);

            FirebaseUser newUser = authResult.User;
            isSuccess = true;
            Debug.Log("회원가입 성공");
            //todo -> 회원가입에 성공했다면 firebase 데이터베이스에 해당 계정의 정보 추가
        }
        catch (Exception ex)
        {
            // 오류 처리
            Debug.LogError($"회원가입 실패: {ex.Message}");
        }

        return isSuccess;
    }


    public async Task<bool> LoginAsync(string emailText, string pwText)
    {
        bool isSuccess = false;
        try
        {
            // Firebase 인증 요청 대기
            var authResult = await auth.SignInWithEmailAndPasswordAsync(emailText, pwText);

            FirebaseUser newUser = authResult.User;
            isSuccess = true;
            Debug.Log("로그인 완료");
        }
        catch (Exception ex)
        {
            // 오류 처리
            Debug.LogError($"로그인 실패: {ex.Message}");
        }

        return isSuccess;
    }

    public void LogOut()
    {
        auth.SignOut();
    }
}
