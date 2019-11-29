using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Dal.Repositories;
using System;

namespace DH.Helpdesk.Services.Services
{
	public interface IFeatureToggleService
    {
        bool IsActive(FeatureToggleTypes featureToggleType);
		FeatureToggleModel Get(FeatureToggleTypes featureToggleType);
	}
	public class FeatureToggleService : IFeatureToggleService
	{
		private readonly IFeatureToggleRepository _featureToggleRepository;

		public FeatureToggleService(IFeatureToggleRepository featureToggleRepository)
		{
			_featureToggleRepository = featureToggleRepository;
		}

        public bool IsActive(FeatureToggleTypes featureToggleType)
        {
            return Get(featureToggleType).Active;
        }

		public FeatureToggleModel Get(FeatureToggleTypes featureToggleType)
		{
			var strongName = featureToggleType.ToString();
			var featureToggle = _featureToggleRepository.GetByStrongName(strongName);
			FeatureToggleModel model;
			if (featureToggle != null)
			{
				model = new FeatureToggleModel
				{
					Active = featureToggle.Active,
					ChangeDate = featureToggle.ChangeDate,
					Description = featureToggle.Description,
					ToggleType = featureToggleType
				};
			}
			else
			{
				// use default feature toggle setting (inactive) to avoid null values
				model = new FeatureToggleModel
				{
					Active = false,
					ChangeDate = DateTime.MinValue,
					Description = "Not found in database",
					ToggleType = featureToggleType
				};
			}
			return model;
		}
	}
}
