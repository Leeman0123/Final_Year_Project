using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Firebase;
using Firebase.Database;
using System.Threading.Tasks;
using System.Linq;

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

    public static async Task<bool> AddNewEngVocaAnimalsTwoTwoResult(string uid, int correctCount, int questionsTotal, int timeCount, int available)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        McQuestionQuiz eqvao = new McQuestionQuiz();
        eqvao.uid = uid;
        eqvao.correctCount = correctCount;
        eqvao.questionsTotal = questionsTotal;
        eqvao.timeCount = timeCount;
        eqvao.attemptLeft = available;
        string json = JsonUtility.ToJson(eqvao);
        var task = reference.Child("EnglishQuiz").Child("PrimaryTwo").Child("EnglishQuizVocabAnimalsTwo").Child(uid).SetRawJsonValueAsync(json);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<McQuestionQuiz> GetEngVocabAnimalsTwoResultById(string uid)
    {
        var task = FirebaseDatabase.DefaultInstance
                    .GetReference("EnglishQuiz")
                    .Child("PrimaryTwo")
                    .Child("EnglishQuizVocabAnimalsTwo")
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

    public static async Task<bool> UpdateAnimalsTwoQuizCorrectCount(string userId, int correctCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("EnglishQuiz")
            .Child("PrimaryTwo")
            .Child("EnglishQuizVocabAnimalsTwo")
            .Child(userId)
            .Child("correctCount").SetValueAsync(correctCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }
    public static async Task<bool> UpdateAnimalsTwoQuizAttempt(string userId, int attemptCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("EnglishQuiz")
            .Child("PrimaryTwo")
            .Child("EnglishQuizVocabAnimalsTwo")
            .Child(userId)
            .Child("attemptLeft").SetValueAsync(attemptCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> UpdateAnimalsTwoQuizTimes(string userId, int timesCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("EnglishQuiz")
            .Child("PrimaryTwo")
            .Child("EnglishQuizVocabAnimalsTwo")
            .Child(userId)
            .Child("timeCount").SetValueAsync(timesCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> AddNewEngP3CompleteSentencesResult(string uid, int correctCount, int questionsTotal, int timeCount, int available)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        McQuestionQuiz eqvao = new McQuestionQuiz();
        eqvao.uid = uid;
        eqvao.correctCount = correctCount;
        eqvao.questionsTotal = questionsTotal;
        eqvao.timeCount = timeCount;
        eqvao.attemptLeft = available;
        string json = JsonUtility.ToJson(eqvao);
        var task = reference.Child("EnglishQuiz").Child("PrimaryThree").Child("EnglishQuizCompleteSentences").Child(uid).SetRawJsonValueAsync(json);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<McQuestionQuiz> GetEngP3CompleteSentencesResultById(string uid)
    {
        var task = FirebaseDatabase.DefaultInstance
                    .GetReference("EnglishQuiz")
                    .Child("PrimaryThree")
                    .Child("EnglishQuizCompleteSentences")
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

    public static async Task<bool> UpdateEngP3CompleteSentencesCorrectCount(string userId, int correctCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("EnglishQuiz")
            .Child("PrimaryThree")
            .Child("EnglishQuizCompleteSentences")
            .Child(userId)
            .Child("correctCount").SetValueAsync(correctCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }
    public static async Task<bool> UpdateEngP3CompleteSentencesAttempt(string userId, int attemptCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("EnglishQuiz")
            .Child("PrimaryThree")
            .Child("EnglishQuizCompleteSentences")
            .Child(userId)
            .Child("attemptLeft").SetValueAsync(attemptCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> UpdateEngP3CompleteSentencesTimes(string userId, int timesCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("EnglishQuiz")
            .Child("PrimaryThree")
            .Child("EnglishQuizCompleteSentences")
            .Child(userId)
            .Child("timeCount").SetValueAsync(timesCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> AddNewEngP3PrepositionResult(string uid, int correctCount, int questionsTotal, int timeCount, int available)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        McQuestionQuiz eqvao = new McQuestionQuiz();
        eqvao.uid = uid;
        eqvao.correctCount = correctCount;
        eqvao.questionsTotal = questionsTotal;
        eqvao.timeCount = timeCount;
        eqvao.attemptLeft = available;
        string json = JsonUtility.ToJson(eqvao);
        var task = reference.Child("EnglishQuiz").Child("PrimaryThree").Child("EnglishQuizPreposition").Child(uid).SetRawJsonValueAsync(json);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<McQuestionQuiz> GetEngP3PrepositionResultById(string uid)
    {
        var task = FirebaseDatabase.DefaultInstance
                    .GetReference("EnglishQuiz")
                    .Child("PrimaryThree")
                    .Child("EnglishQuizPreposition")
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

    public static async Task<bool> UpdateEngP3PrepositionCorrectCount(string userId, int correctCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("EnglishQuiz")
            .Child("PrimaryThree")
            .Child("EnglishQuizPreposition")
            .Child(userId)
            .Child("correctCount").SetValueAsync(correctCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }
    public static async Task<bool> UpdateEngP3PrepositionAttempt(string userId, int attemptCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("EnglishQuiz")
            .Child("PrimaryThree")
            .Child("EnglishQuizPreposition")
            .Child(userId)
            .Child("attemptLeft").SetValueAsync(attemptCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> UpdateEngP3PrepositionTimes(string userId, int timesCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("EnglishQuiz")
            .Child("PrimaryThree")
            .Child("EnglishQuizPreposition")
            .Child(userId)
            .Child("timeCount").SetValueAsync(timesCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> AddNewMathsP1Additionesult(string uid, int correctCount, int questionsTotal, int timeCount, int available)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        McQuestionQuiz eqvao = new McQuestionQuiz();
        eqvao.uid = uid;
        eqvao.correctCount = correctCount;
        eqvao.questionsTotal = questionsTotal;
        eqvao.timeCount = timeCount;
        eqvao.attemptLeft = available;
        string json = JsonUtility.ToJson(eqvao);
        var task = reference.Child("MathsQuiz").Child("PrimaryOne").Child("MathsQuizAddition").Child(uid).SetRawJsonValueAsync(json);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<McQuestionQuiz> GetMathsP1AdditionResultById(string uid)
    {
        var task = FirebaseDatabase.DefaultInstance
                    .GetReference("MathsQuiz")
                    .Child("PrimaryOne")
                    .Child("MathsQuizAddition")
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

    public static async Task<bool> UpdateMathsP1AdditionCorrectCount(string userId, int correctCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("MathsQuiz")
            .Child("PrimaryOne")
            .Child("MathsQuizAddition")
            .Child(userId)
            .Child("correctCount").SetValueAsync(correctCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }
    public static async Task<bool> UpdateMathsP1AdditionAttempt(string userId, int attemptCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("MathsQuiz")
            .Child("PrimaryOne")
            .Child("MathsQuizAddition")
            .Child(userId)
            .Child("attemptLeft").SetValueAsync(attemptCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> UpdateMathsP1AdditionTimes(string userId, int timesCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("MathsQuiz")
            .Child("PrimaryOne")
            .Child("MathsQuizAddition")
            .Child(userId)
            .Child("timeCount").SetValueAsync(timesCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> AddNewMathsP1SubtractResult(string uid, int correctCount, int questionsTotal, int timeCount, int available)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        McQuestionQuiz eqvao = new McQuestionQuiz();
        eqvao.uid = uid;
        eqvao.correctCount = correctCount;
        eqvao.questionsTotal = questionsTotal;
        eqvao.timeCount = timeCount;
        eqvao.attemptLeft = available;
        string json = JsonUtility.ToJson(eqvao);
        var task = reference.Child("MathsQuiz").Child("PrimaryOne").Child("MathsQuizSubtract").Child(uid).SetRawJsonValueAsync(json);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<McQuestionQuiz> GetMathsP1SubtractResultById(string uid)
    {
        var task = FirebaseDatabase.DefaultInstance
                    .GetReference("MathsQuiz")
                    .Child("PrimaryOne")
                    .Child("MathsQuizSubtract")
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

    public static async Task<bool> UpdateMathsP1SubtractCorrectCount(string userId, int correctCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("MathsQuiz")
            .Child("PrimaryOne")
            .Child("MathsQuizSubtract")
            .Child(userId)
            .Child("correctCount").SetValueAsync(correctCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }
    public static async Task<bool> UpdateMathsP1SubtractAttempt(string userId, int attemptCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("MathsQuiz")
            .Child("PrimaryOne")
            .Child("MathsQuizSubtract")
            .Child(userId)
            .Child("attemptLeft").SetValueAsync(attemptCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> UpdateMathsP1SubtractTimes(string userId, int timesCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("MathsQuiz")
            .Child("PrimaryOne")
            .Child("MathsQuizSubtract")
            .Child(userId)
            .Child("timeCount").SetValueAsync(timesCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> AddNewMathsP2MuDivResult(string uid, int correctCount, int questionsTotal, int timeCount, int available)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        McQuestionQuiz eqvao = new McQuestionQuiz();
        eqvao.uid = uid;
        eqvao.correctCount = correctCount;
        eqvao.questionsTotal = questionsTotal;
        eqvao.timeCount = timeCount;
        eqvao.attemptLeft = available;
        string json = JsonUtility.ToJson(eqvao);
        var task = reference.Child("MathsQuiz").Child("PrimaryTwo").Child("MathsQuizMuDiv").Child(uid).SetRawJsonValueAsync(json);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<McQuestionQuiz> GetMathsP2MuDivResultById(string uid)
    {
        var task = FirebaseDatabase.DefaultInstance
                    .GetReference("MathsQuiz")
                    .Child("PrimaryTwo")
                    .Child("MathsQuizMuDiv")
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

    public static async Task<bool> UpdateMathsP2MuDivCorrectCount(string userId, int correctCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("MathsQuiz")
            .Child("PrimaryTwo")
            .Child("MathsQuizMuDiv")
            .Child(userId)
            .Child("correctCount").SetValueAsync(correctCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }
    public static async Task<bool> UpdateMathsP2MuDivAttempt(string userId, int attemptCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("MathsQuiz")
            .Child("PrimaryTwo")
            .Child("MathsQuizMuDiv")
            .Child(userId)
            .Child("attemptLeft").SetValueAsync(attemptCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> UpdateMathsP2MuDivTimes(string userId, int timesCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("MathsQuiz")
            .Child("PrimaryTwo")
            .Child("MathsQuizMuDiv")
            .Child(userId)
            .Child("timeCount").SetValueAsync(timesCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> AddNewMathsP2SubAddResult(string uid, int correctCount, int questionsTotal, int timeCount, int available)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        McQuestionQuiz eqvao = new McQuestionQuiz();
        eqvao.uid = uid;
        eqvao.correctCount = correctCount;
        eqvao.questionsTotal = questionsTotal;
        eqvao.timeCount = timeCount;
        eqvao.attemptLeft = available;
        string json = JsonUtility.ToJson(eqvao);
        var task = reference.Child("MathsQuiz").Child("PrimaryTwo").Child("MathsQuizSubAdd").Child(uid).SetRawJsonValueAsync(json);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<McQuestionQuiz> GetMathsP2SubAddResultById(string uid)
    {
        var task = FirebaseDatabase.DefaultInstance
                    .GetReference("MathsQuiz")
                    .Child("PrimaryTwo")
                    .Child("MathsQuizSubAdd")
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

    public static async Task<bool> UpdateMathsP2SubAddCorrectCount(string userId, int correctCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("MathsQuiz")
            .Child("PrimaryTwo")
            .Child("MathsQuizSubAdd")
            .Child(userId)
            .Child("correctCount").SetValueAsync(correctCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }
    public static async Task<bool> UpdateMathsP2SubAddAttempt(string userId, int attemptCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("MathsQuiz")
            .Child("PrimaryTwo")
            .Child("MathsQuizSubAdd")
            .Child(userId)
            .Child("attemptLeft").SetValueAsync(attemptCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> UpdateMathsP2SubAddTimes(string userId, int timesCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("MathsQuiz")
            .Child("PrimaryTwo")
            .Child("MathsQuizSubAdd")
            .Child(userId)
            .Child("timeCount").SetValueAsync(timesCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> AddNewMathsP3DecimalResult(string uid, int correctCount, int questionsTotal, int timeCount, int available)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        McQuestionQuiz eqvao = new McQuestionQuiz();
        eqvao.uid = uid;
        eqvao.correctCount = correctCount;
        eqvao.questionsTotal = questionsTotal;
        eqvao.timeCount = timeCount;
        eqvao.attemptLeft = available;
        string json = JsonUtility.ToJson(eqvao);
        var task = reference.Child("MathsQuiz").Child("PrimaryThree").Child("MathsQuizDecimal").Child(uid).SetRawJsonValueAsync(json);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<McQuestionQuiz> GetMathsP3DecimalResultById(string uid)
    {
        var task = FirebaseDatabase.DefaultInstance
                    .GetReference("MathsQuiz")
                    .Child("PrimaryThree")
                    .Child("MathsQuizDecimal")
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

    public static async Task<bool> UpdateMathsP3DecimalCorrectCount(string userId, int correctCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("MathsQuiz")
            .Child("PrimaryThree")
            .Child("MathsQuizDecimal")
            .Child(userId)
            .Child("correctCount").SetValueAsync(correctCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }
    public static async Task<bool> UpdateMathsP3DecimalAttempt(string userId, int attemptCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("MathsQuiz")
            .Child("PrimaryThree")
            .Child("MathsQuizDecimal")
            .Child(userId)
            .Child("attemptLeft").SetValueAsync(attemptCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> UpdateMathsP3DecimalTimes(string userId, int timesCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("MathsQuiz")
            .Child("PrimaryThree")
            .Child("MathsQuizDecimal")
            .Child(userId)
            .Child("timeCount").SetValueAsync(timesCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> AddNewMathsP3ArithResult(string uid, int correctCount, int questionsTotal, int timeCount, int available)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        McQuestionQuiz eqvao = new McQuestionQuiz();
        eqvao.uid = uid;
        eqvao.correctCount = correctCount;
        eqvao.questionsTotal = questionsTotal;
        eqvao.timeCount = timeCount;
        eqvao.attemptLeft = available;
        string json = JsonUtility.ToJson(eqvao);
        var task = reference.Child("MathsQuiz").Child("PrimaryThree").Child("MathsQuizArith").Child(uid).SetRawJsonValueAsync(json);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<McQuestionQuiz> GetMathsP3ArithResultById(string uid)
    {
        var task = FirebaseDatabase.DefaultInstance
                    .GetReference("MathsQuiz")
                    .Child("PrimaryThree")
                    .Child("MathsQuizArith")
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

    public static async Task<bool> UpdateMathsP3ArithCorrectCount(string userId, int correctCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("MathsQuiz")
            .Child("PrimaryThree")
            .Child("MathsQuizArith")
            .Child(userId)
            .Child("correctCount").SetValueAsync(correctCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }
    public static async Task<bool> UpdateMathsP3ArithAttempt(string userId, int attemptCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("MathsQuiz")
            .Child("PrimaryThree")
            .Child("MathsQuizArith")
            .Child(userId)
            .Child("attemptLeft").SetValueAsync(attemptCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> UpdateMathsP3ArithTimes(string userId, int timesCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("MathsQuiz")
            .Child("PrimaryThree")
            .Child("MathsQuizArith")
            .Child(userId)
            .Child("timeCount").SetValueAsync(timesCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> AddNewChineseP1FillInResult(string uid, int correctCount, int questionsTotal, int timeCount, int available)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        McQuestionQuiz eqvao = new McQuestionQuiz();
        eqvao.uid = uid;
        eqvao.correctCount = correctCount;
        eqvao.questionsTotal = questionsTotal;
        eqvao.timeCount = timeCount;
        eqvao.attemptLeft = available;
        string json = JsonUtility.ToJson(eqvao);
        var task = reference.Child("ChineseQuiz").Child("PrimaryOne").Child("ChineseFillIn").Child(uid).SetRawJsonValueAsync(json);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<McQuestionQuiz> GetChineseP1FillInResultById(string uid)
    {
        var task = FirebaseDatabase.DefaultInstance
                    .GetReference("ChineseQuiz")
                    .Child("PrimaryOne")
                    .Child("ChineseFillIn")
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

    public static async Task<bool> UpdateChineseP1FillInorrectCount(string userId, int correctCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("ChineseQuiz")
            .Child("PrimaryOne")
            .Child("ChineseFillIn")
            .Child(userId)
            .Child("correctCount").SetValueAsync(correctCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }
    public static async Task<bool> UpdateChineseP1FillInAttempt(string userId, int attemptCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("ChineseQuiz")
            .Child("PrimaryOne")
            .Child("ChineseFillIn")
            .Child(userId)
            .Child("attemptLeft").SetValueAsync(attemptCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> UpdateChineseP1FillInTimes(string userId, int timesCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("ChineseQuiz")
            .Child("PrimaryOne")
            .Child("ChineseFillIn")
            .Child(userId)
            .Child("timeCount").SetValueAsync(timesCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> AddNewChineseP1UnitResult(string uid, int correctCount, int questionsTotal, int timeCount, int available)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        McQuestionQuiz eqvao = new McQuestionQuiz();
        eqvao.uid = uid;
        eqvao.correctCount = correctCount;
        eqvao.questionsTotal = questionsTotal;
        eqvao.timeCount = timeCount;
        eqvao.attemptLeft = available;
        string json = JsonUtility.ToJson(eqvao);
        var task = reference.Child("ChineseQuiz").Child("PrimaryOne").Child("ChineseUnit").Child(uid).SetRawJsonValueAsync(json);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<McQuestionQuiz> GetChineseP1UnitResultById(string uid)
    {
        var task = FirebaseDatabase.DefaultInstance
                    .GetReference("ChineseQuiz")
                    .Child("PrimaryOne")
                    .Child("ChineseUnit")
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

    public static async Task<bool> UpdateChineseP1UnitCorrectCount(string userId, int correctCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("ChineseQuiz")
            .Child("PrimaryOne")
            .Child("ChineseUnit")
            .Child(userId)
            .Child("correctCount").SetValueAsync(correctCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }
    public static async Task<bool> UpdateChineseP1UnitAttempt(string userId, int attemptCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("ChineseQuiz")
            .Child("PrimaryOne")
            .Child("ChineseUnit")
            .Child(userId)
            .Child("attemptLeft").SetValueAsync(attemptCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> UpdateChineseP1Unitimes(string userId, int timesCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("ChineseQuiz")
            .Child("PrimaryOne")
            .Child("ChineseUnit")
            .Child(userId)
            .Child("timeCount").SetValueAsync(timesCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> AddNewChineseP2FillInAdvResult(string uid, int correctCount, int questionsTotal, int timeCount, int available)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        McQuestionQuiz eqvao = new McQuestionQuiz();
        eqvao.uid = uid;
        eqvao.correctCount = correctCount;
        eqvao.questionsTotal = questionsTotal;
        eqvao.timeCount = timeCount;
        eqvao.attemptLeft = available;
        string json = JsonUtility.ToJson(eqvao);
        var task = reference.Child("ChineseQuiz").Child("PrimaryTwo").Child("ChineseFillInAdv").Child(uid).SetRawJsonValueAsync(json);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<McQuestionQuiz> GetChineseP2FillInAdvResultById(string uid)
    {
        var task = FirebaseDatabase.DefaultInstance
                    .GetReference("ChineseQuiz")
                    .Child("PrimaryTwo")
                    .Child("ChineseFillInAdv")
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

    public static async Task<bool> UpdateChineseP2FillInAdvCorrectCount(string userId, int correctCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("ChineseQuiz")
            .Child("PrimaryTwo")
            .Child("ChineseFillInAdv")
            .Child(userId)
            .Child("correctCount").SetValueAsync(correctCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }
    public static async Task<bool> UpdateChineseP2FillInAdvAttempt(string userId, int attemptCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("ChineseQuiz")
            .Child("PrimaryTwo")
            .Child("ChineseFillInAdv")
            .Child(userId)
            .Child("attemptLeft").SetValueAsync(attemptCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> UpdateChineseP2FillInAdvTimes(string userId, int timesCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("ChineseQuiz")
            .Child("PrimaryTwo")
            .Child("ChineseFillInAdv")
            .Child(userId)
            .Child("timeCount").SetValueAsync(timesCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> AddNewChineseP2HeadResult(string uid, int correctCount, int questionsTotal, int timeCount, int available)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        McQuestionQuiz eqvao = new McQuestionQuiz();
        eqvao.uid = uid;
        eqvao.correctCount = correctCount;
        eqvao.questionsTotal = questionsTotal;
        eqvao.timeCount = timeCount;
        eqvao.attemptLeft = available;
        string json = JsonUtility.ToJson(eqvao);
        var task = reference.Child("ChineseQuiz").Child("PrimaryTwo").Child("ChineseHead").Child(uid).SetRawJsonValueAsync(json);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<McQuestionQuiz> GetChineseP2HeadResultById(string uid)
    {
        var task = FirebaseDatabase.DefaultInstance
                    .GetReference("ChineseQuiz")
                    .Child("PrimaryTwo")
                    .Child("ChineseHead")
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

    public static async Task<bool> UpdateChineseP2HeadCorrectCount(string userId, int correctCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("ChineseQuiz")
            .Child("PrimaryTwo")
            .Child("ChineseHead")
            .Child(userId)
            .Child("correctCount").SetValueAsync(correctCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }
    public static async Task<bool> UpdateChineseP2HeadAttempt(string userId, int attemptCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("ChineseQuiz")
            .Child("PrimaryTwo")
            .Child("ChineseHead")
            .Child(userId)
            .Child("attemptLeft").SetValueAsync(attemptCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> UpdateChineseP2HeadTimes(string userId, int timesCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("ChineseQuiz")
            .Child("PrimaryTwo")
            .Child("ChineseHead")
            .Child(userId)
            .Child("timeCount").SetValueAsync(timesCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> AddNewChineseP3IdiomResult(string uid, int correctCount, int questionsTotal, int timeCount, int available)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        McQuestionQuiz eqvao = new McQuestionQuiz();
        eqvao.uid = uid;
        eqvao.correctCount = correctCount;
        eqvao.questionsTotal = questionsTotal;
        eqvao.timeCount = timeCount;
        eqvao.attemptLeft = available;
        string json = JsonUtility.ToJson(eqvao);
        var task = reference.Child("ChineseQuiz").Child("PrimaryThree").Child("ChineseIdiom").Child(uid).SetRawJsonValueAsync(json);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<McQuestionQuiz> GetChineseP3IdiomResultById(string uid)
    {
        var task = FirebaseDatabase.DefaultInstance
                    .GetReference("ChineseQuiz")
                    .Child("PrimaryThree")
                    .Child("ChineseIdiom")
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

    public static async Task<bool> UpdateChineseP3IdiomCorrectCount(string userId, int correctCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("ChineseQuiz")
            .Child("PrimaryThree")
            .Child("ChineseIdiom")
            .Child(userId)
            .Child("correctCount").SetValueAsync(correctCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }
    public static async Task<bool> UpdateChineseP3IdiomAttempt(string userId, int attemptCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("ChineseQuiz")
            .Child("PrimaryThree")
            .Child("ChineseIdiom")
            .Child(userId)
            .Child("attemptLeft").SetValueAsync(attemptCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> UpdateChineseP3IdiomTimes(string userId, int timesCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("ChineseQuiz")
            .Child("PrimaryThree")
            .Child("ChineseIdiom")
            .Child(userId)
            .Child("timeCount").SetValueAsync(timesCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> AddNewChineseP3Idiom2Result(string uid, int correctCount, int questionsTotal, int timeCount, int available)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        McQuestionQuiz eqvao = new McQuestionQuiz();
        eqvao.uid = uid;
        eqvao.correctCount = correctCount;
        eqvao.questionsTotal = questionsTotal;
        eqvao.timeCount = timeCount;
        eqvao.attemptLeft = available;
        string json = JsonUtility.ToJson(eqvao);
        var task = reference.Child("ChineseQuiz").Child("PrimaryThree").Child("ChineseIdiom2").Child(uid).SetRawJsonValueAsync(json);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<McQuestionQuiz> GetChineseP3Idiom2ResultById(string uid)
    {
        var task = FirebaseDatabase.DefaultInstance
                    .GetReference("ChineseQuiz")
                    .Child("PrimaryThree")
                    .Child("ChineseIdiom2")
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

    public static async Task<bool> UpdateChineseP3Idiom2CorrectCount(string userId, int correctCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("ChineseQuiz")
            .Child("PrimaryThree")
            .Child("ChineseIdiom2")
            .Child(userId)
            .Child("correctCount").SetValueAsync(correctCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }
    public static async Task<bool> UpdateChineseP3Idiom2Attempt(string userId, int attemptCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("ChineseQuiz")
            .Child("PrimaryThree")
            .Child("ChineseIdiom2")
            .Child(userId)
            .Child("attemptLeft").SetValueAsync(attemptCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> UpdateChineseP3Idiom2Times(string userId, int timesCount)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("ChineseQuiz")
            .Child("PrimaryThree")
            .Child("ChineseIdiom2")
            .Child(userId)
            .Child("timeCount").SetValueAsync(timesCount);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<List<McQuestionQuiz>> GetQuizRank(string testName, string quizSubject, string primaryLevel)
    {
        List<McQuestionQuiz> list = new List<McQuestionQuiz>();
        var task = FirebaseDatabase.DefaultInstance
                    .GetReference(quizSubject)
                    .Child(primaryLevel)
                    .Child(testName)
                    .GetValueAsync();
        await task;
        DataSnapshot data = task.Result;
        if (!data.Exists)
        {
            return null;
        }
        foreach (var snapshot in data.Children)
        {
            string json = snapshot.GetRawJsonValue();
            McQuestionQuiz quiz = JsonUtility.FromJson<McQuestionQuiz>(json);
            list.Add(quiz);
        }
        return list.OrderByDescending(o => o.correctCount)
            .OrderBy(o => o.timeCount).ToList();
    }

    public static async Task<bool> AddNewExtraQuiz(string type, string quizName, string enable, string uid) 
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        ExtraQuizEntry eq = new ExtraQuizEntry();
        eq.uid = uid;
        eq.enable = enable;
        string json = JsonUtility.ToJson(eq);
        var task = reference.Child(type).Child(quizName).SetRawJsonValueAsync(json);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }

    public static async Task<ExtraQuizEntry> GetExtraQuizByName(string type, string quizName)
    {
        var task = FirebaseDatabase.DefaultInstance
                    .GetReference(type)
                    .Child(quizName)
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
        ExtraQuizEntry eqvao = new ExtraQuizEntry();
        eqvao.enable = data.Child("enable").Value.ToString();
        eqvao.uid = data.Child("uid").Value.ToString();
        return eqvao;
    }
}
