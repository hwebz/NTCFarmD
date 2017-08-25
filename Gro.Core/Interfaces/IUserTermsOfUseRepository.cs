namespace Gro.Core.Interfaces
{
    public interface IUserTermsOfUseRepository
    {
        bool CheckUserAccepts(int userId, string termIdentity);
        bool UpdateInsertTermOfUse(int newVersion, string userAgreementIdentity);
        bool InsertUpdateUserAccepts(int userId, string term, int version);
        bool CheckTerm(string termId, int version);
    }
}
