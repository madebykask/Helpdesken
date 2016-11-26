using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DH.Helpdesk.BusinessData.Models.ExternalInvoice;
using DH.Helpdesk.Dal.Mappers;
using DH.Helpdesk.Dal.Repositories.Cases;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Services.Services
{
	public class ExternalInvoiceService : IExternalInvoiceService
	{
		private readonly ICaseInvoiceRowRepository _caseInvoiceRowRepository;
		private readonly IBusinessModelToEntityMapper<ExternalInvoice, CaseInvoiceRow> _externalInvoiceToEntityMapper;
		private readonly IEntityToBusinessModelMapper<CaseInvoiceRow, ExternalInvoice> _caseInvoiceRowToBusinessModelMapper;

		public ExternalInvoiceService(ICaseInvoiceRowRepository caseInvoiceRowRepository,
			IBusinessModelToEntityMapper<ExternalInvoice, CaseInvoiceRow> externalInvoiceToEntityMapper,
			IEntityToBusinessModelMapper<CaseInvoiceRow, ExternalInvoice> caseInvoiceRowToBusinessModelMapper)
		{
			_caseInvoiceRowRepository = caseInvoiceRowRepository;
			_externalInvoiceToEntityMapper = externalInvoiceToEntityMapper;
			_caseInvoiceRowToBusinessModelMapper = caseInvoiceRowToBusinessModelMapper;
		}

		public List<ExternalInvoice> GetExternalInvoices(int caseId)
		{
			var data = _caseInvoiceRowRepository.GetCaseInvoiceRows(caseId);
			return data.Select(x => _caseInvoiceRowToBusinessModelMapper.Map(x)).ToList();
		}

		public void SaveExternalInvoices(int caseId, List<ExternalInvoice> rows)
		{
			var data = rows.Select(x =>
			{
				var ret = new CaseInvoiceRow();
				_externalInvoiceToEntityMapper.Map(x, ret);
				return ret;
			}).ToList();

			_caseInvoiceRowRepository.SaveCaseInvoiceRows(caseId, data);
		}
	}

	public interface IExternalInvoiceService
	{
		List<ExternalInvoice> GetExternalInvoices(int caseId);
		void SaveExternalInvoices(int caseId, List<ExternalInvoice> rows);
	}
}
