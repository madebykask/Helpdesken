using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using DH.Helpdesk.EForm.Model.Abstract;
using DH.Helpdesk.EForm.Model.Entities;

namespace DH.Helpdesk.EForm.Model.Contrete
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public User Get(string identity, string userId)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                using(var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@identity", SqlDbType.NVarChar)).Value = identity;
                    command.Parameters.Add(new SqlParameter("@userId", SqlDbType.NVarChar)).Value = userId;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Login";
                    connection.Open();

                    User user = null;

                    using(var dr = command.ExecuteReader())
                    {
                        while(dr.Read())
                        {
                            user = new User
                            {
                                Id = dr.SafeGetInteger("Id"),
                                UserId = dr.SafeGetString("UserId"),
                                FirstName = dr.SafeGetString("FirstName"),
                                Surname = dr.SafeGetString("SurName"),
                                Email = dr.SafeGetString("Email"),
                                CustomerId = dr.SafeGetInteger("Customer_Id"),
                                WorkingGroups = GetWorkingGroups(dr.SafeGetInteger("Customer_Id"), dr.SafeGetInteger("Id")).ToList()
                            };
                        }
                    }

                    return user;
                }
            }
        }

        public IEnumerable<User> GetUsers(int customerId, int userGroupId)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                using(var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@Customer_Id", SqlDbType.Int)).Value = customerId;
                    command.Parameters.Add(new SqlParameter("@UserGroup_Id", SqlDbType.Int)).Value = userGroupId;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_Users";
                    connection.Open();

                    using(var dr = command.ExecuteReader())
                    {
                        while(dr.Read())
                        {
                            yield return new User
                            {
                                Id = dr.SafeGetInteger("Id"),
                                UserId = dr.SafeGetString("UserId"),
                                FirstName = dr.SafeGetString("FirstName"),
                                Surname = dr.SafeGetString("SurName"),
                                Email = dr.SafeGetString("Email"),
                            };
                        }
                    }
                }
            }
        }



        public IEnumerable<WorkingGroup> GetWorkingGroups(int customerId, int userId)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                using(var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@Customer_Id", SqlDbType.Int)).Value = customerId;
                    command.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.Int)).Value = userId;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_UserWorkingGroups";
                    connection.Open();

                    using(var dr = command.ExecuteReader())
                    {
                        while(dr.Read())
                        {
                            yield return new WorkingGroup
                            {
                                Id = dr.SafeGetInteger("Id"),
                                CustomerId = dr.SafeGetInteger("Customer_Id"),
                                Name = dr.SafeGetString("WorkingGroup")
                            };
                        }
                    }
                }
            }
        }
    }
}
