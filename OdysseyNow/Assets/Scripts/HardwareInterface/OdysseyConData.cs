using UnityEngine;

namespace HardwareInterface
{
    /// <summary>
    /// A simple POJO used when reading in JSONified arduino serial message.
    /// This holds all data the OdysseyCon sends to unity
    /// </summary>
    [System.Serializable]
    public class OdysseyConData
    {
        public float P1_X_READ, P1_Y_READ, P1_ENG_READ, P1_RESET_READ, P2_X_READ, P2_Y_READ, P2_ENG_READ, P2_RESET_READ, BALL_SPEED_READ, SELECT_READ, ENTER_READ, CROWBAR_READ, CROWBAR_RESET_READ, ENCODER_READ;
    }
}
