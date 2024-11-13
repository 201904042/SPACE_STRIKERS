using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Auth_Firebase
{
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
            
            await Managers.Instance.FB_Db.GetGameDataAsync();
            await Managers.Instance.FB_Db.GetBaseAccountDataAsync();
            Debug.Log("������ �Ϸ�");

            await Managers.Instance.FB_Db.UploadAllWritableJsonFilesAsync();
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

            await Managers.Instance.FB_Db.GetAccountDataAsync(UserId);
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
        Managers.Instance.Data.ClearAllData();
        DB_Firebase.DeleteAccountData();
        auth.SignOut();
    }
}
