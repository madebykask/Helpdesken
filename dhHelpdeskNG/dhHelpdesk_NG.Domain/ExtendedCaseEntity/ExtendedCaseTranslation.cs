﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Domain.ExtendedCaseEntity
{
	public class ExtendedCaseTranslationEntity : Entity
	{
		public string Property { get; set; }
		public string Text { get; set; }

		public int LanguageId { get; set; }

        public int? ExtendedCaseForm_Id { get; set; }
    }
}
