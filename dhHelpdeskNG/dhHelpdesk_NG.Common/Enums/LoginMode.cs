
namespace DH.Helpdesk.Common.Enums
{
    //NOTE: if you need string values that differ from enum values pls use DescriptionAttribute and some handy extension method to get value from attribute (google examples)
    //      or as an alternative use type-safe enum pattern: http://www.javacamp.org/designPattern/enum.html
    // 
    //      if you need string value which matches enum value just use Enum.GetName: Enum.GetName(typeof(LoginMode), loginMode);
    public enum LoginMode
    {
        None,

        Application,

        Windows,
         
        Mixed, // Windows + Forms

        //[Description("singlesignon")]
        SSO,

        Anonymous
    }
}
