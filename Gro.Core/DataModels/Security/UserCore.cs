namespace Gro.Core.DataModels.Security
{
    /// <summary>
    /// User's information saved in user's session
    /// </summary>
    public class UserCore
    {
        /// <summary>
        /// User's id in CGI database
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// User universal name identification
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// ITIM person id
        /// </summary>
        public string PersonDn { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name => $"{FirstName} {LastName}";
        public string ProfilePicUrl { get; set; }
        public string Email { get; set; }
    }
}
