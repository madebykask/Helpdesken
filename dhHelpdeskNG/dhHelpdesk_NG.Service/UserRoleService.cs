using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs;

namespace dhHelpdesk_NG.Service
{
    public interface IUserRoleService
    {
        IList<UserRole> GetUserRolesByUser(int userid);

    }

    public class UserRoleService : IUserRoleService
    {

        public readonly IUserRoleRepository _userRoleRepository;

        public UserRoleService(

            IUserRoleRepository userRoleRepository)
        {

            _userRoleRepository = userRoleRepository;

        }

        public IList<UserRole> GetUserRolesByUser(int userid)
        {
            return _userRoleRepository.GetUserRolesByUser(userid);
        }


    }
}
