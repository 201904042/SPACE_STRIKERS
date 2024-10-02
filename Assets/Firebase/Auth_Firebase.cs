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
                //�α׾ƿ��� ����
                LoginState?.Invoke(false);
            }

            user = auth.CurrentUser;
            if (signed)
            {
                //�α��� ����
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
                Debug.LogError("ȸ������ ���");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("ȸ������ ����");
            }

            AuthResult authResult = task.Result;
            FirebaseUser newUser = authResult.User;
            //ȸ�����Կ� �����ߴٸ� �׿����� �����ͺ��̽��� �����ؾ���
            Debug.Log("ȸ������ �Ϸ�");
        });
    }

    public void Login(string emailText, string pwText)
    {
        auth.SignInWithEmailAndPasswordAsync(emailText, pwText).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("�α��� ���");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("�α��� ����");
            }

            AuthResult authResult = task.Result;
            FirebaseUser newUser = authResult.User;
            Debug.Log("�α��� �Ϸ�");
        });
    }

    public void LogOut()
    {
        auth.SignOut();
    }
}
