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
            P1X, P1Y,
            P2X, P2Y;
        public int
            P1W, P2W;
        //public int
        //    p1rst;
    }

    [System.Serializable]
    public class ConsoleResetWrite
    {
        //this class is for the JSON data that conatains the reset message
        public int
            P1R, P2R;

    }

}
