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

    public async Task<bool> CreateAccountAsync(string emailText, string pwText)
    {
        bool isSuccess = false;
        try
        {
            // Firebase ���� ��û ���
            var authResult = await auth.CreateUserWithEmailAndPasswordAsync(emailText, pwText);

            FirebaseUser newUser = authResult.User;
            isSuccess = true;
            Debug.Log("ȸ������ ����");

            await DB_Firebase.Instance.GetGameDataAsync();
            await DB_Firebase.Instance.GetBaseAccountDataAsync();
            Debug.Log("������ �Ϸ�");

            await DB_Firebase.Instance.UploadWritableJsonFilesAsync();
            Debug.Log("�� ������ ���� �Ϸ�");

            Managers.Instance.Data.Init();
            Debug.Log("Auth ������ �ʱ�ȭ");

            

        }
        catch (Exception ex)
        {
            // ���� ó��
            Debug.LogError($"ȸ������ ����: {ex.Message}");
        }

        return isSuccess;
    }


    public async Task<bool> LoginAsync(string emailText, string pwText)
    {
        bool isSuccess = false;
        try
        {
            // Firebase ���� ��û ���
            var authResult = await auth.SignInWithEmailAndPasswordAsync(emailText, pwText);

            FirebaseUser newUser = authResult.User;
            isSuccess = true;
            Debug.Log("�α��� �Ϸ�");

            await DB_Firebase.Instance.GetGameDataAsync();
            await DB_Firebase.Instance.GetAccountDataAsync(UserId);
            Debug.Log("������ �Ϸ�");
            Managers.Instance.Data.Init();
            Debug.Log("Auth ������ �ʱ�ȭ");
        }
        catch (Exception ex)
        {
            // ���� ó��
            Debug.LogError($"�α��� ����: {ex.Message}");
        }

        
        return isSuccess;
    }

    public void LogOut()
    {
        DB_Firebase.DeleteJsonData();
        auth.SignOut();
    }
}
