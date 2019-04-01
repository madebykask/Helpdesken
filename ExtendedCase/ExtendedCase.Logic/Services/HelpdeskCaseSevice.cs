using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Logging;
using ExtendedCase.HelpdeskApiClient;
using ExtendedCase.HelpdeskApiClient.Responses;
using ExtendedCase.Models;

namespace ExtendedCase.Logic.Services
{
    public class HelpdeskCaseSevice : IHelpdeskCaseSevice
    {
        private readonly IHelpdeskCaseApiClient _caseApiClient;
        private readonly ILogger _logger;

        #region ctor()

        public HelpdeskCaseSevice(IHelpdeskCaseApiClient caseApiClient, ILogger logger)
        {
            _caseApiClient = caseApiClient;
            _logger = logger;
        }

        #endregion

        public async Task<List<FieldValueModel>> GetCaseFields(int caseId)
        {
            List<FieldValueModel> items = new List<FieldValueModel>();

            var response = await _caseApiClient.GetCase(caseId);

            var err = "";
            if (!HandleApiResponse(response, out err))
            {
                _logger.Error(err);
                throw new Exception(err);
            }

            var values = response.Data.ToObject<Dictionary<string, object>>();
            items = values.Select(o => new FieldValueModel(o.Key, o.Value?.ToString(), null, new FieldProperties())).ToList();
            return items;
        }

        #region Helper Methods

        private bool HandleApiResponse(ApiResponse response, out string errorMessage)
        {
            errorMessage = "";

            if (!response.StatusIsSuccessful)
            {
                errorMessage = HandleApiResponseError(response);
                return false;

            }

            return true;
        }

        protected string HandleApiResponseError(ApiResponse response)
        {
            var error = string.Empty;

            //check if its authorisation error first:
            if (response.ErrorState.ModelState != null)
            {
                var errors = response.ErrorState.ModelState.Values.Select(o => o.ToString()).ToArray();
                var errorSummary = string.Join($".{Environment.NewLine}", errors);
                error = errorSummary;
            }
            else
            {
                error = error ?? response.ResponseResult ?? "Request has failed with unknown error";
            }
            
            return error;
        }

        #endregion
    }

    public interface IHelpdeskCaseSevice
    {
        Task<List<FieldValueModel>> GetCaseFields(int caseId);
    }
}
