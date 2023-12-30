using System.Data;
using Dapper;
using DotNetAPI.Data;
using DotNetAPI.Models;

namespace DotNetAPI.Helpers {
    public class ReusableSql {

        private readonly DataContextDapper _dapper;
        public ReusableSql(IConfiguration config) {
            _dapper = new DataContextDapper(config);
        }

        public bool UpsertUser(UserComplete userComplete) {

            string sql = @"EXEC app.spUser_Upsert
                @FirstName = @FirstNameParameter, 
                @LastName = @LastNameParameter, 
                @Email = @EmailParameter, 
                @Gender = @GenderParameter, 
                @Active = @ActiveParameter, 
                @JobTitle = @JobTitleParameter, 
                @Department = @DepartmentParameter, 
                @Salary = @SalaryParameter, 
                @UserId = @UserIdParameter";

            DynamicParameters sqlParameters = new DynamicParameters();

            sqlParameters.Add("@FirstNameParameter", userComplete.FirstName, DbType.String);
            sqlParameters.Add("@LastNameParameter", userComplete.LastName, DbType.String);
            sqlParameters.Add("@EmailParameter", userComplete.Email, DbType.String);
            sqlParameters.Add("@GenderParameter", userComplete.Gender, DbType.String);
            sqlParameters.Add("@ActiveParameter", userComplete.Active, DbType.Boolean);
            sqlParameters.Add("@JobTitleParameter", userComplete.JobTitle, DbType.String);
            sqlParameters.Add("@DepartmentParameter", userComplete.Department, DbType.String);
            sqlParameters.Add("@SalaryParameter", userComplete.Salary, DbType.Decimal);
            sqlParameters.Add("@UserIdParameter", userComplete.UserId, DbType.Int32);

            return _dapper.ExecuteSqlWithDynamicParameters(sql, sqlParameters);
        }
    }
}