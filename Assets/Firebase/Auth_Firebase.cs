using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
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


    public void Create(string emailText, string pwText)
    {
        auth.CreateUserWithEmailAndPasswordAsync(emailText, pwText).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("회원가입 취소");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("회원가입 실패");
            }

            AuthResult authResult = task.Result;
            FirebaseUser newUser = authResult.User;
            //회원가입에 성공했다면 그에따른 데이터베이스도 생성해야함
            Debug.Log("회원가입 완료");
        });
    }

    public void Login(string emailText, string pwText)
    {
        auth.SignInWithEmailAndPasswordAsync(emailText, pwText).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("로그인 취소");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("로그인 실패");
            }

            AuthResult authResult = task.Result;
            FirebaseUser newUser = authResult.User;
            Debug.Log("로그인 완료");
        });
    }

    public void LogOut()
    {
        auth.SignOut();
    }
}
