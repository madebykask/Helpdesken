namespace DH.Helpdesk.BusinessData.Enums.Users
{
    /// <summary>
    /// Notification to user about automatic time zone detection 
    /// </summary>
    public enum TimeZoneAutodetectResult
    {
        /// <summary>
        /// Does not report anything
        /// </summary>
        None = 0,

        /// <summary>
        /// Failed to detect time zone
        /// </summary>
        Failure = 1,

        /// <summary>
        /// Time zone was successfully detected 
        /// </summary>
        Success = 2,

        /// <summary>
        /// More than one variants is suitable for users UTC offset
        /// </summary>
        MoreThanOne = 3,

        /// <summary>
        /// Reminder to user about change his time zone due to contradictions between detected time zone and time zone stored in DB
        /// </summary>
        Notice = 4
    }
}
