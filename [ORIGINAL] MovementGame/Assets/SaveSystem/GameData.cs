using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public float[] respawnPoint;
    public float[] transportTo;
    public bool doTransport;
    public int score;
    public Dictionary<string, bool> booleans;

    public string scene = "tutorial_level";

    public GameData(Vector3 point, int score_) {
        scene = "tutorial_level";
        respawnPoint = new float[3]{ point.x, point.y, point.z};
        score = score_;
        doTransport = false;

        booleans = new Dictionary<string, bool>
        {
            { "testTag", false }
        };
    }
    public void SetPoint(Vector3 point) {
        respawnPoint = new float[3] { point.x, point.y, point.z };
    }

    public void SetTransport(Vector3 point)
    {
        transportTo = new float[3] { point.x, point.y, point.z };
        doTransport = true;
    }
}
