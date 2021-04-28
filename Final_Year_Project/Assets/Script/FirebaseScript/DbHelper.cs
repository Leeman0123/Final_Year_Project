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
            if (!teacherData.Exists)
                return null;
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
            if (!studentData.Exists)
                return null;
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
                if (!studentSnapshot.Exists)
                    break;
                emailList.Add(snapshot.Child("email").Value.ToString());
            }
            foreach(DataSnapshot snapshot in teacherSnapshot.Children)
            {
                if (!teacherSnapshot.Exists)
                    break;
                emailList.Add(snapshot.Child("email").Value.ToString());
            }
            return emailList;
        }
    }

    public static async Task<bool> AddNewEngVocabOneResult(string uid, int correctCount, int questionsTotal, int timeCount, int available)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        McQuestionQuiz eqvao = new McQuestionQuiz();
        eqvao.uid = uid;
        eqvao.correctCount = correctCount;
        eqvao.questionsTotal = questionsTotal;
        eqvao.timeCount = timeCount;
        eqvao.attemptLeft = available;
        string json = JsonUtility.ToJson(eqvao);
        var task = reference.Child("EnglishQuiz").Child("PrimaryOne").Child("EnglishQuizVocabAnimalsOne").Child(uid).SetRawJsonValueAsync(json);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<McQuestionQuiz> GetEngVocabAnimOneResultById(string uid)
    {
        var task = FirebaseDatabase.DefaultInstance
                    .GetReference("EnglishQuiz")
                    .Child("PrimaryOne")
                    .Child("EnglishQuizVocabAnimalsOne")
                    .Child(uid)
                    .GetValueAsync();
        await task;
        if (task.Exception != null)
        {
            return null;
        }
        DataSnapshot data = task.Result;
        if (!data.Exists) {
            return null;
        }
        McQuestionQuiz eqvao = new McQuestionQuiz();
        eqvao.uid = data.Child("uid").Value.ToString();
        eqvao.correctCount = Convert.ToInt32(data.Child("correctCount").Value);
        eqvao.questionsTotal = Convert.ToInt32(data.Child("questionsTotal").Value);
        eqvao.timeCount = Convert.ToInt32(data.Child("timeCount").Value);
        eqvao.attemptLeft = Convert.ToInt32(data.Child("attemptLeft").Value);
        return eqvao;
    }

    public static async Task<bool> UpdateStudentCoins(string userId, int coins) {
        var task = FirebaseDatabase.DefaultInstance
                    .GetReference("students")
                    .Child(userId)
                    .Child("coins").SetValueAsync(coins);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> UpdateAnimalsOneQuizTimes(string userId, int timesCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("EnglishQuiz")
            .Child("PrimaryOne")
            .Child("EnglishQuizVocabAnimalsOne")
            .Child(userId)
            .Child("timeCount").SetValueAsync(timesCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }
    public static async Task<bool> UpdateAnimalsOneQuizCorrectCount(string userId, int correctCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("EnglishQuiz")
            .Child("PrimaryOne")
            .Child("EnglishQuizVocabAnimalsOne")
            .Child(userId)
            .Child("correctCount").SetValueAsync(correctCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }
    public static async Task<bool> UpdateAnimalsOneQuizAttempt(string userId, int attemptCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("EnglishQuiz")
            .Child("PrimaryOne")
            .Child("EnglishQuizVocabAnimalsOne")
            .Child(userId)
            .Child("attemptLeft").SetValueAsync(attemptCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> AddNewEngVocaVehiclebOneResult(string uid, int correctCount, int questionsTotal, int timeCount, int available)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        McQuestionQuiz eqvao = new McQuestionQuiz();
        eqvao.uid = uid;
        eqvao.correctCount = correctCount;
        eqvao.questionsTotal = questionsTotal;
        eqvao.timeCount = timeCount;
        eqvao.attemptLeft = available;
        string json = JsonUtility.ToJson(eqvao);
        var task = reference.Child("EnglishQuiz").Child("PrimaryOne").Child("EnglishQuizVocabVehicleOne").Child(uid).SetRawJsonValueAsync(json);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<McQuestionQuiz> GetEngVocabVehicleOneResultById(string uid)
    {
        var task = FirebaseDatabase.DefaultInstance
                    .GetReference("EnglishQuiz")
                    .Child("PrimaryOne")
                    .Child("EnglishQuizVocabVehicleOne")
                    .Child(uid)
                    .GetValueAsync();
        await task;
        if (task.Exception != null)
        {
            return null;
        }
        DataSnapshot data = task.Result;
        if (!data.Exists)
        {
            return null;
        }
        McQuestionQuiz eqvao = new McQuestionQuiz();
        eqvao.uid = data.Child("uid").Value.ToString();
        eqvao.correctCount = Convert.ToInt32(data.Child("correctCount").Value);
        eqvao.questionsTotal = Convert.ToInt32(data.Child("questionsTotal").Value);
        eqvao.timeCount = Convert.ToInt32(data.Child("timeCount").Value);
        eqvao.attemptLeft = Convert.ToInt32(data.Child("attemptLeft").Value);
        return eqvao;
    }

    public static async Task<bool> UpdateVehicleOneQuizCorrectCount(string userId, int correctCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("EnglishQuiz")
            .Child("PrimaryOne")
            .Child("EnglishQuizVocabVehicleOne")
            .Child(userId)
            .Child("correctCount").SetValueAsync(correctCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }
    public static async Task<bool> UpdateVehicleOneQuizAttempt(string userId, int attemptCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("EnglishQuiz")
            .Child("PrimaryOne")
            .Child("EnglishQuizVocabVehicleOne")
            .Child(userId)
            .Child("attemptLeft").SetValueAsync(attemptCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> UpdateVehiceOneQuizTimes(string userId, int timesCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("EnglishQuiz")
            .Child("PrimaryOne")
            .Child("EnglishQuizVocabVehicleOne")
            .Child(userId)
            .Child("timeCount").SetValueAsync(timesCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> AddNewEngVocaVehiclebTwoResult(string uid, int correctCount, int questionsTotal, int timeCount, int available)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        McQuestionQuiz eqvao = new McQuestionQuiz();
        eqvao.uid = uid;
        eqvao.correctCount = correctCount;
        eqvao.questionsTotal = questionsTotal;
        eqvao.timeCount = timeCount;
        eqvao.attemptLeft = available;
        string json = JsonUtility.ToJson(eqvao);
        var task = reference.Child("EnglishQuiz").Child("PrimaryTwo").Child("EnglishQuizVocabVehicleTwo").Child(uid).SetRawJsonValueAsync(json);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<McQuestionQuiz> GetEngVocabVehicleTwoResultById(string uid)
    {
        var task = FirebaseDatabase.DefaultInstance
                    .GetReference("EnglishQuiz")
                    .Child("PrimaryTwo")
                    .Child("EnglishQuizVocabVehicleTwo")
                    .Child(uid)
                    .GetValueAsync();
        await task;
        if (task.Exception != null)
        {
            return null;
        }
        DataSnapshot data = task.Result;
        if (!data.Exists)
        {
            return null;
        }
        McQuestionQuiz eqvao = new McQuestionQuiz();
        eqvao.uid = data.Child("uid").Value.ToString();
        eqvao.correctCount = Convert.ToInt32(data.Child("correctCount").Value);
        eqvao.questionsTotal = Convert.ToInt32(data.Child("questionsTotal").Value);
        eqvao.timeCount = Convert.ToInt32(data.Child("timeCount").Value);
        eqvao.attemptLeft = Convert.ToInt32(data.Child("attemptLeft").Value);
        return eqvao;
    }

    public static async Task<bool> UpdateVehicleTwoQuizCorrectCount(string userId, int correctCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("EnglishQuiz")
            .Child("PrimaryTwo")
            .Child("EnglishQuizVocabVehicleTwo")
            .Child(userId)
            .Child("correctCount").SetValueAsync(correctCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }
    public static async Task<bool> UpdateVehicleTwoQuizAttempt(string userId, int attemptCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("EnglishQuiz")
            .Child("PrimaryTwo")
            .Child("EnglishQuizVocabVehicleTwo")
            .Child(userId)
            .Child("attemptLeft").SetValueAsync(attemptCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> UpdateVehiceTwoQuizTimes(string userId, int timesCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("EnglishQuiz")
            .Child("PrimaryTwo")
            .Child("EnglishQuizVocabVehicleTwo")
            .Child(userId)
            .Child("timeCount").SetValueAsync(timesCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }
}
