using Discord;
using UnityEngine;

public class DiscordRPC : Singleton<DiscordRPC>
{
    private ActivityManager activityManager;
    public Discord.Discord discord;

    private void Start()
    {
        discord = new Discord.Discord(750020541259448391, (ulong) CreateFlags.Default);

        activityManager = discord.GetActivityManager();
        var activity = new Activity
        {
            Details = "On the main menu",
            State = "Playing Spaceship Defender"
        };

        activityManager.UpdateActivity(activity, result =>
        {
            if (result == Result.Ok) Debug.Log("Discord status set");
        });

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    private void Update()
    {
        discord.RunCallbacks();


        if (Input.GetKey(KeyCode.B)) activityManager.ClearActivity(result => { });
    }

    private void OnApplicationQuit()
    {
        discord.Dispose();
    }
}