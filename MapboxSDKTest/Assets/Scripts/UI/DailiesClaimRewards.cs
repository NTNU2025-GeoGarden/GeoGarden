using UnityEngine;

namespace UI
{
    public class DailiesClaimRewards : MonoBehaviour
    {
        public DailyChallenge challenge;

        public void HandleClaimRewards()
        {
            challenge.Collect();
        }
    }
}