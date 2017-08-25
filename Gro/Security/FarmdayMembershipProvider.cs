using System;
using System.Web.Security;
using Gro.Core.Interfaces;
using System.Web.Mvc;

namespace Gro.Security
{
    public class FarmdayMembershipProvider : MembershipProvider
    {
        private readonly ISecurityRepository _securityRepository;

        public FarmdayMembershipProvider()
        {
            _securityRepository = DependencyResolver.Current.GetService<ISecurityRepository>();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotSupportedException();
        }

        public override string GetPassword(string username, string answer) => _securityRepository.GetPassword(username, answer);

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotSupportedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotSupportedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotSupportedException("Updating users is not available in admin mode");
        }

        public override bool ValidateUser(string username, string password) => _securityRepository.ValidateUser(username, password);

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotSupportedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            var user = _securityRepository.QueryUser(username);
            if (user == null) return null;

            return new MembershipUser(user.ProviderName, user.UserName, user.ProviderKey, user.Email,
                    user.PassWordquestion, user.Comment, user.IsActive, user.IsLocketOut, user.CreationDate,
                    user.LastLoginDate, user.LastActivityDate, user.LastPasswordChangedDate, user.LastLockedOutDate);
        }

        public override string GetUserNameByEmail(string email) => _securityRepository.GetUserNameByEmail(email);

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotSupportedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            var allusers = _securityRepository.GetAllUsers(pageIndex, pageSize, out totalRecords);
            return allusers;
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotSupportedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
            => _securityRepository.FindUsersByName(usernameToMatch, pageIndex, pageSize, out totalRecords);

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
            => _securityRepository.FindUsersByEmail(emailToMatch, pageIndex, pageSize, out totalRecords);

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotSupportedException("Cannot create user in admin mode");
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordRetrieval => false;
        public override bool EnablePasswordReset => false;
        public override bool RequiresQuestionAndAnswer => false;
        public override string ApplicationName { get; set; }
        public override int MaxInvalidPasswordAttempts { get; }
        public override int PasswordAttemptWindow { get; }
        public override bool RequiresUniqueEmail => true;
        public override MembershipPasswordFormat PasswordFormat { get; }
        public override int MinRequiredPasswordLength { get; }
        public override int MinRequiredNonAlphanumericCharacters { get; }
        public override string PasswordStrengthRegularExpression { get; }
    }
}
