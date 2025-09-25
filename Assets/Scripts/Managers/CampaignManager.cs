using System.Collections.Generic;
using UnityEngine;

namespace EclipseProtocol.Managers
{
    [CreateAssetMenu(menuName = "Eclipse Protocol/Campaign/Campaign Definition")]
    public class CampaignManager : ScriptableObject
    {
        [System.Serializable]
        public class Mission
        {
            public string missionName;
            public string sceneName;
            public Sprite keyArt;
            [TextArea] public string briefing;
            public string[] objectives;
        }

        [SerializeField] private List<Mission> missions = new();

        public IReadOnlyList<Mission> Missions => missions;
    }
}
