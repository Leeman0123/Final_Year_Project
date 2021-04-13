using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Firebase;
using Firebase.Database;
using System.Threading.Tasks;

public class DbHelper : MonoBehaviour
{
    public static async Task<bool> AddNewStudent(string userId, string email, string name)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        Students student = new Students();
        student.SetUid(userId);
        student.SetEmail(email);
        student.SetName(name);
        string json = JsonUtility.ToJson(student);
        Debug.Log(json);
        Task task = reference.Child("students").Child(userId).SetRawJsonValueAsync(json);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> AddNewTeacher(string userId, string email, string name)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        Teachers teacher = new Teachers();
        teacher.SetUid(userId);
        teacher.SetEmail(email);
        teacher.SetName(name);
        string json = JsonUtility.ToJson(teacher);
        Debug.Log(json);
        Task task = reference.Child("teachers").Child(userId).SetRawJsonValueAsync(json);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<Teachers> GetTeacherById(string userId)
    {
        Teachers teacher;
        var task = FirebaseDatabase.DefaultInstance
                        .GetReference("teachers")
                        .Child(userId)
                        .GetValueAsync();
        await task;
        if (task.Exception != null)
        {
            return null;
        }
        else
        {
            teacher = new Teachers();
            DataSnapshot teacherData = task.Result;
            teacher.SetEmail(teacherData.Child("email").Value.ToString());
            teacher.SetName(teacherData.Child("name").Value.ToString());
            teacher.SetUid(teacherData.Child("uid").Value.ToString());
            return teacher;
        }
    }

    public static async Task<Students> GetStudentById(string userId)
    {
        Students student;
        var task = FirebaseDatabase.DefaultInstance
                        .GetReference("students")
                        .Child(userId)
                        .GetValueAsync();
        await task;
        if (task.Exception != null)
        {
            return null;
        }
        else
        {
            student = new Students();
            DataSnapshot studentData = task.Result;
            student.SetCoins(Convert.ToInt32(studentData.Child("coins").Value));
            student.SetEmail(studentData.Child("email").Value.ToString());
            student.SetName(studentData.Child("name").Value.ToString());
            student.SetUid(studentData.Child("uid").Value.ToString());
            return student;
        }
    }

    public static async Task<HashSet<string>> GetAllUserEmail()
    {
        var studentTask = FirebaseDatabase.DefaultInstance
                            .GetReference("students")
                            .GetValueAsync();
        await studentTask;
        var teacherTask = FirebaseDatabase.DefaultInstance
                            .GetReference("teachers")
                            .GetValueAsync();
        await teacherTask;

        if (studentTask.Exception != null || teacherTask.Exception != null)
        {
            return null;
        }
        else
        {
            HashSet<string> emailList = new HashSet<string>();
            DataSnapshot studentSnapshot = studentTask.Result;
            DataSnapshot teacherSnapshot = teacherTask.Result;
            foreach(DataSnapshot snapshot in studentSnapshot.Children)
            {
                emailList.Add(snapshot.Child("email").Value.ToString());
            }
            foreach(DataSnapshot snapshot in teacherSnapshot.Children)
            {
                emailList.Add(snapshot.Child("email").Value.ToString());
            }
            return emailList;
        }
    }

    public async Task<bool> AddNewEngVocabOneResult(string uid, int correctCount, int questionsTotal, int timeCount)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        EnglishQuizVocabAnimalsOne eqvao = new EnglishQuizVocabAnimalsOne();
        eqvao.uid = uid;
        eqvao.correctCount = correctCount;
        eqvao.questionsTotal = questionsTotal;
        eqvao.timeCount = timeCount;
        eqvao.attemptLeft = 1;
        string json = JsonUtility.ToJson(eqvao);
        var task = reference.Child("EnglishQuiz").Child("PrimaryOne").Child(uid).SetRawJsonValueAsync(json);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public async Task<EnglishQuizVocabAnimalsOne> GetEngVocabAnimOneResultById(string uid)
    {
        var task = FirebaseDatabase.DefaultInstance
                    .GetReference("EnglishQuiz")
                    .Child("PrimaryOne")
                    .Child(uid)
                    .GetValueAsync();
        await task;
        if (task.Exception != null)
        {
            return null;
        }
        DataSnapshot data = task.Result;
        EnglishQuizVocabAnimalsOne eqvao = new EnglishQuizVocabAnimalsOne();
        eqvao.uid = data.Child("uid").Value.ToString();
        eqvao.correctCount = Convert.ToInt32(data.Child("correctCount").Value);
        eqvao.questionsTotal = Convert.ToInt32(data.Child("questionsTotal").Value);
        eqvao.timeCount = Convert.ToInt32(data.Child("timeCount").Value);
        eqvao.attemptLeft = Convert.ToInt32(data.Child("attemptLeft").Value);
        return eqvao;
    }
}
