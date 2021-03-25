using System;
using System.Collections;
using System.Collections.Generic;
using Discord;
using UnityEngine;

public class DiscordRPC : Singleton<DiscordRPC>
{
    public Discord.Discord discord;
    private ActivityManager activityManager;
    
    void Start()
    {
        discord = new Discord.Discord(750020541259448391, (ulong)Discord.CreateFlags.Default);

        activityManager = discord.GetActivityManager();
        var activity = new Discord.Activity()
        {
            Details = "On the main menu",
            State = "Playing Spaceship Defender"
        };
        
        activityManager.UpdateActivity(activity, result => { if (result == Result.Ok) Debug.Log("Discord status set");});
        
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        discord.RunCallbacks();
            

        if (Input.GetKey(KeyCode.B))
        {
            activityManager.ClearActivity(result => { });
        }
    }

    private void OnApplicationQuit()
    {
        discord.Dispose();
    }
}
