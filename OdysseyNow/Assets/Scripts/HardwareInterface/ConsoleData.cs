using UnityEngine;

namespace HardwareInterface
{
    /// <summary>
    /// Simple POJO object to read in json of console data from arduino serial message.
    /// </summary>
    [System.Serializable]
    public class ConsoleData
    {
        public float
            P1_X_READ, P1_Y_READ, P1_RESET_READ,
            P2_X_READ, P2_Y_READ, P2_RESET_READ,
            P3_X_READ, P3_Y_READ,
            P4_X_READ, P4_Y_READ,
            BALL_X_READ, BALL_Y_READ,
            WALL_X_READ,
            NOT_ENGLISH_WEAK_Q_READ,
            NOT_BALL_WEAK_Q_READ,
            NOT_CROWBAR_READ;
        public bool
            P1_IS_WRITING, P2_IS_WRITING;
    }

    [System.Serializable]
    public class ConsoleDataWrite
    {
        public int
            P1_X, P1_Y,
            P2_X, P2_Y
            ;
        public bool
            P1_W, P2_W
            ;
    }

}
