using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Realms;
public class UserDataController : MonoBehaviour
{
    private Realm _realm;

    void Start()
    {
        _realm = Realm.GetInstance();
    }

    public UserData ReadUserDataByID(int id)
    {
        return _realm.Find<UserData>(id);
    }

    public void UpdateUserData(int id, UserData inputUserData)
    {
        UserData userData = _realm.Find<UserData>(id);
        _realm.Write(() =>
        {
            userData.FirstName = inputUserData.FirstName;
            userData.LastName = inputUserData.LastName;
            userData.Age = inputUserData.Age;
        });
    }

    public void WriteUserData(UserData inputUserData)
    {
        _realm.Add(inputUserData);
    }

    public void DeleteUserData(int id)
    {
        UserData userData = _realm.Find<UserData>(id);
        _realm.Remove(userData);
    }
}
