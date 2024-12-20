using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Logger
{
    // 게임 폴더를 기준으로 Logs 디렉토리 설정
    private static readonly string BaseLogsDirectory = System.IO.Path.Combine(Application.dataPath, "../GameLogs");

    public static void Log(string message, string eventType = "Info",  string customFileName = "GeneratedMap")
    {
        if(!GameManager.GetInstance.UseDebugLogs || MapManager.GetInstance.CurrentMap == null)
        {
            return;
        }
        
        // 상위 Logs 폴더 생성
        if (!System.IO.Directory.Exists(BaseLogsDirectory))
        {
            System.IO.Directory.CreateDirectory(BaseLogsDirectory);
        }

        // 맵 생성 시간 기반 폴더 생성
        string mapPath = MapManager.GetInstance.CurrentMap.createdTime;
        string mapFolderPath = System.IO.Path.Combine(BaseLogsDirectory, mapPath);
        if (!System.IO.Directory.Exists(mapFolderPath))
        {
            System.IO.Directory.CreateDirectory(mapFolderPath);
        }

        // 로그 파일 경로 생성
        string filePath = System.IO.Path.Combine(mapFolderPath, $"{customFileName}.txt");
        string logMessage = $"[{System.DateTime.Now}] [{eventType}]\n{message}";
        
        SaveToFile(filePath, logMessage);
    }

    private static void SaveToFile(string filePath, string logMessage)
    {
        try
        {
            System.IO.File.AppendAllText(filePath, logMessage + "\n");
            Debug.Log($"File saved at: {filePath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to save log file: {ex.Message}");
        }
    }
    
    public static void BattleLog(object message, string eventType)
    {
        if(MapManager.GetInstance.CurrentMap == null)
        {
            return;
        }
        var currentPoint = MapManager.GetInstance.CurrentMap.path[MapManager.GetInstance.CurrentMap.path.Count - 1];
        int currentFloor = currentPoint.y;
        Log(message.ToString(), eventType, "Floor" + currentFloor);
    }
    
}
