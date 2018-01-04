using System.Data.SqlClient;
using System.Linq;
using Trevo.Core.Data;
using Trevo.Core.DTO;
using Trevo.Core.Model.User;

namespace Trevo.Services.Token
{
    public class TokenServices:ITokenServices
    {
         #region Private member variables.
        //private readonly IRepository<TokenEntity> _tokenEntityRepository;
        private readonly IRepository<TrevoUsers> _userRepository;
       private const string PROC_USER_AUTH = "UserAuthentication_ByMailId @email";
         #endregion

        #region Public constructor.
        /// <summary>
        /// Public constructor.
        /// </summary>
       public TokenServices(IRepository<TrevoUsers> userRepository)
        {
            //_tokenEntityRepository = tokenEntityRepository;
            _userRepository = userRepository;
        }
        #endregion


        #region Public member methods.

        /// <summary>
        ///  Function to generate unique token with expiry against the provided userId.
        ///  Also add a record in database for generated token.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public AuthUser GenerateToken(string emailId)
        {

            //need to implement validaton checks! 
            SqlParameter param = new SqlParameter("email", emailId);

            var users = _userRepository.ExecuteStoredProcedureList<AuthUser>(PROC_USER_AUTH, param);

            return users.FirstOrDefault();

            //string token = Guid.NewGuid().ToString();
            //DateTime issuedOn = DateTime.Now;
            //DateTime expiredOn = DateTime.Now.AddSeconds(
            //                                  Convert.ToDouble(ConfigurationManager.AppSettings["AuthTokenExpiry"]));
            //var tokendomain = new Token
            //                      {
            //                          UserId = userId,
            //                          AuthToken = token,
            //                          IssuedOn = issuedOn,
            //                          ExpiresOn = expiredOn
            //                      };

            //_unitOfWork.TokenRepository.Insert(tokendomain);
            //_unitOfWork.Save();
            //var tokenModel = new TokenEntity()
            //                     {
            //                         UserId = userId,
            //                         IssuedOn = issuedOn,
            //                         ExpiresOn = expiredOn,
            //                         AuthToken = token
            //                     };



           
        }

        /// <summary>
        /// Method to validate token against expiry and existence in database.
        /// </summary>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        public bool ValidateToken(string tokenId)
        {
            //var token = _unitOfWork.TokenRepository.Get(t => t.AuthToken == tokenId && t.ExpiresOn > DateTime.Now);
            //if (token != null && !(DateTime.Now > token.ExpiresOn))
            //{
            //    token.ExpiresOn = token.ExpiresOn.AddSeconds(
            //                                  Convert.ToDouble(ConfigurationManager.AppSettings["AuthTokenExpiry"]));
            //    _unitOfWork.TokenRepository.Update(token);
            //    _unitOfWork.Save();
            //    return true;
            //}
            //return false;


            if (tokenId != null && tokenId=="123456")
            {
                //token.ExpiresOn = token.ExpiresOn.AddSeconds(
                //                              Convert.ToDouble(ConfigurationManager.AppSettings["AuthTokenExpiry"]));
                //_unitOfWork.TokenRepository.Update(token);
                //_unitOfWork.Save();
                return true;
            }
            return false;
        }

       

        #endregion
    }
}
