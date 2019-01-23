namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class CaseLockSettings
    {
        public CaseLockSettings(int caseLockTimer, int caseLockBufferTime, int caseLockExtendTime)
        {
            CaseLockTimer = caseLockTimer;
            CaseLockBufferTime = caseLockBufferTime;
            CaseLockExtendTime = caseLockExtendTime;
        }

        public int CaseLockTimer { get; private set; }
        public int CaseLockBufferTime { get; private set; }
        public int CaseLockExtendTime { get; private set; }
    }
}
