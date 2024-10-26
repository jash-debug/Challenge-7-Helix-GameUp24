using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> names; // UI elements for usernames
    [SerializeField] private List<TextMeshProUGUI> scores; // UI elements for scores
    private string publicLeaderboardKey = "e2e4429ded60f4db1799d1d51097c15c2be346c480e7c02f2aed290e150b399f";

    // A list to store the leaderboard entries
    private List<LeaderboardEntry> leaderboardEntries = new List<LeaderboardEntry>();

    public void GetLeaderBoard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, (msg) => {
            leaderboardEntries.Clear(); // Clear the local list for fresh entries

            int loopLength = Mathf.Min(msg.Length, names.Count);
            for (int i = 0; i < loopLength; i++)
            {
                leaderboardEntries.Add(new LeaderboardEntry(msg[i].Username, msg[i].Score));
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
            }
        });
    }

    public void SetLeaderBoardEntry(string username, int score)
    {
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score, (msg) =>
        {
            // Add the new entry to the local list
                if (leaderboardEntries.Count < names.Count) // Ensure there's space
            {
                leaderboardEntries.Add(new LeaderboardEntry(username, score));

                // Update the UI directly
                names[leaderboardEntries.Count - 1].text = username;
                scores[leaderboardEntries.Count - 1].text = score.ToString();
            }
            else
            {
                // Optionally handle the case where the leaderboard is full
                Debug.LogWarning("Leaderboard is full. Cannot add more entries.");
            }
        });
    }

    void Start()
    {
        GetLeaderBoard();
    }

    void Update()
    {
        // Update logic if needed
    }
}

// Helper class to store leaderboard entries
[System.Serializable]
public class LeaderboardEntry
{
    public string Username;
    public int Score;

    public LeaderboardEntry(string username, int score)
    {
        Username = username;
        Score = score;
    }
}
