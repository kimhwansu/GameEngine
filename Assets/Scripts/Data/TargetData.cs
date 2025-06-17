using UnityEngine;
using System.Collections.Generic;

namespace UJ.Data
{
    [System.Serializable]
    public class TargetData
    {
        public string prefabName;
        public int pointValue;
        public string tag;
        public float minSpeed;
        public float maxSpeed;
        public float maxTorque;
        public float xRange;
        public float ySpawnPos;
        public GameObject prefab;
    }

    [System.Serializable]
    public class TargetDataList
    {
        public List<TargetData> targets = new List<TargetData>();
    }
}
