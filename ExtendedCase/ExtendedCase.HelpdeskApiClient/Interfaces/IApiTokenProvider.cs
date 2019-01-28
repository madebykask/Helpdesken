namespace ExtendedCase.HelpdeskApiClient.Interfaces
{
    public interface IApiTokenProvider
    {
        string GetToken();
        void SetToken(string token);

        string GetRefreshToken();
        void SetRefreshToken(string token);
    }
}