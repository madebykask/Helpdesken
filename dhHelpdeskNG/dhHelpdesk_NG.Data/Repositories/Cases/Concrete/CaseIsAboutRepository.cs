namespace DH.Helpdesk.Dal.Repositories.Cases.Concrete
{    
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using System.Collections.Generic;
    using System.Linq;
    using System;

    public sealed class CaseIsAboutRepository : Repository, ICaseIsAboutRepository
    {
        #region Constructors and Destructors

        public CaseIsAboutRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        #endregion

        #region Public Methods and Operators

        public int SaveCaseIsAbout(Case c, out IDictionary<string, string> errors)
        {
            if (c == null || c.IsAbout == null)
                throw new ArgumentNullException("caseIsAbout");

            errors = new Dictionary<string, string>();
            var cIsAbout = this.MapToEntity(c);

            var oldIsAbout = this.DbContext.CaseIsAbout.Where(i => i.Id == c.Id).Select(
                            i =>
                            new
                            {
                                i.Id,
                                CostCentre = i.CostCentre,
                                Department_Id = i.Department_Id,
                                OU_Id = i.OU_Id,
                                Person_Cellphone = i.Person_Cellphone,
                                Person_Email = i.Person_Email,
                                Person_Name = i.Person_Name,
                                Person_Phone = i.Person_Phone,
                                Place = i.Place,
                                ReportedBy = i.ReportedBy,
                                UserCode = i.UserCode
                            }).FirstOrDefault(); ;
           
            if (oldIsAbout == null)
                this.AddIsAbout(c);
            else
                this.UpdateIsAbout(c);


            //if (errors.Count == 0)
            //    this._caseIsAboutRepository.Commit();

            return cIsAbout.Id;
        }

        private CaseIsAboutEntity MapToEntity(Case c)
        {
            var cIsAbout = new CaseIsAboutEntity();
            cIsAbout.Id = c.IsAbout.Id;
            cIsAbout.CostCentre = c.IsAbout.CostCentre;
            cIsAbout.Department_Id = c.IsAbout.Department_Id;
            cIsAbout.OU_Id = c.IsAbout.OU_Id;
            cIsAbout.Person_Cellphone = c.IsAbout.Person_Cellphone;
            cIsAbout.Person_Email = c.IsAbout.Person_Email;
            cIsAbout.Person_Name = c.IsAbout.Person_Name;
            cIsAbout.Person_Phone = c.IsAbout.Person_Phone;
            cIsAbout.Place = c.IsAbout.Place;
            cIsAbout.ReportedBy = c.IsAbout.ReportedBy;
            cIsAbout.UserCode = c.IsAbout.UserCode;

            return cIsAbout;
        }

        private void AddIsAbout(Case caseEntity)
        {
            //ToDO: Check if exist any data for IsAbout

            if (caseEntity.IsAbout != null)
            {
                var caseIsAboutEntity = new CaseIsAboutEntity
                {
                    Id = caseEntity.Id,
                    ReportedBy = caseEntity.IsAbout.ReportedBy,
                    Person_Name = caseEntity.IsAbout.Person_Name,
                    Person_Email = caseEntity.IsAbout.Person_Email,
                    Person_Phone = caseEntity.IsAbout.Person_Phone,
                    Person_Cellphone = caseEntity.IsAbout.Person_Cellphone,
                    Region_Id = caseEntity.IsAbout.Region_Id,
                    Department_Id = caseEntity.IsAbout.Department_Id,
                    OU_Id = caseEntity.IsAbout.OU_Id,
                    CostCentre = caseEntity.IsAbout.CostCentre,
                    Place = caseEntity.IsAbout.Place,
                    UserCode = caseEntity.IsAbout.UserCode
                };
                this.DbContext.CaseIsAbout.Add(caseIsAboutEntity);
                //this.InitializeAfterCommit(questionnaire, caseIsAboutEntity);
            }
        }

        private void UpdateIsAbout(Case caseEntity)
        {
            //ToDO: Check if exist any data for IsAbout
            if (caseEntity.IsAbout != null)
            {
                var caseIsAboutEntity = this.DbContext.CaseIsAbout.SingleOrDefault(i=> i.Id == caseEntity.Id);
                if (caseIsAboutEntity != null)
                {
                    caseIsAboutEntity.ReportedBy = caseEntity.IsAbout.ReportedBy;
                    caseIsAboutEntity.Person_Name = caseEntity.IsAbout.Person_Name;
                    caseIsAboutEntity.Person_Email = caseEntity.IsAbout.Person_Email;
                    caseIsAboutEntity.Person_Phone = caseEntity.IsAbout.Person_Phone;
                    caseIsAboutEntity.Person_Cellphone = caseEntity.IsAbout.Person_Cellphone;
                    caseIsAboutEntity.Region_Id = caseEntity.IsAbout.Region_Id;
                    caseIsAboutEntity.Department_Id = caseEntity.IsAbout.Department_Id;
                    caseIsAboutEntity.OU_Id = caseEntity.IsAbout.OU_Id;
                    caseIsAboutEntity.CostCentre = caseEntity.IsAbout.CostCentre;
                    caseIsAboutEntity.Place = caseEntity.IsAbout.Place;
                    caseIsAboutEntity.UserCode = caseEntity.IsAbout.UserCode;
                }

            }
            else
            {
                var caseIsAboutEntity = this.DbContext.CaseIsAbout.Find(caseEntity.Id);

                if (caseIsAboutEntity != null)
                    this.DbContext.CaseIsAbout.Remove(caseIsAboutEntity);    
            }
  
        }

        #endregion
    }
}
