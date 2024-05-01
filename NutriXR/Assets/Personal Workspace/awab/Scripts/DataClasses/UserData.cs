using System.Collections;
using System.Collections.Generic;
using Realms;
using UnityEngine;

public class UserData : RealmObject
{
    [PrimaryKey] [Preserve]
    public int ID { get; set; }
    [Preserve] public string FirstName { get; set; }
    [Preserve] public string LastName { get; set; }
    [Preserve] public int Age { get; set; }

    public UserData(){}
    public UserData(string firstName, string lastName, int age)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
    }
}
