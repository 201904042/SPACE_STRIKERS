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

            await DB_Firebase.Instance.GetGameDataAsync();
            await DB_Firebase.Instance.GetBaseAccountDataAsync();
            Debug.Log("데이터 완료");

            await DB_Firebase.Instance.UploadWritableJsonFilesAsync();
            Debug.Log("뉴 데이터 전송 완료");

            Managers.Instance.Data.Init();
            Debug.Log("Auth 데이터 초기화");

            

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

            await DB_Firebase.Instance.GetGameDataAsync();
            await DB_Firebase.Instance.GetAccountDataAsync(UserId);
            Debug.Log("데이터 완료");
            Managers.Instance.Data.Init();
            Debug.Log("Auth 데이터 초기화");
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
        DB_Firebase.DeleteJsonData();
        auth.SignOut();
    }
}
