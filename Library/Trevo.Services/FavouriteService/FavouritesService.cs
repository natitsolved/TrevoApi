using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Trevo.Core.Data;
using Trevo.Core.DTO;
using Trevo.Core.Model;
using Trevo.Core.Model.Favourites;

namespace Trevo.Services.FavouriteService
{
  public  class FavouritesService : IFavouritesService
    {
        private readonly IRepository<FavouritesDetails> _favRepository;
        private const string PROC_INSERT_FAVOURITES = "spl_InsertFavourites @message,@favouriteUserId,@isSender,@senderRecieverId,@momentId,@addedDate,@localMessageId";
        private const string PROC_DELETE_FAV_BY_USER_AND_MOMENT_ID = "spl_DeleteFavBasedOnUserandMomentId @favouriteUserId,@momentId";
        private const string PROC_DELETE_FAV_BY_ID = "spl_DeleteFavoriteById @favouritesId";
        private const string PROC_GET_ALL_FAVOURIES_BY_USERID = "spl_GetAllFavouritesByUserId @favouriteUserId";
        private const string PROC_GET_ALL_FAVOURIES_BY_MOMENTID = "spl_GetFavouritesListByMomentId @momentId";
        public FavouritesService(IRepository<FavouritesDetails> favRepository)
        {
            _favRepository = favRepository;
        }

        public bool InsertFavourites(FavouritesDetails details)
        {


            List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@message",Value= details.Message,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@favouriteUserId",Value = details.FavouriteUserId,SqlDbType=SqlDbType.BigInt},
     new SqlParameter() {ParameterName = "@isSender",Value = details.IsSender,SqlDbType=SqlDbType.Int},
     new SqlParameter() {ParameterName = "@senderRecieverId",Value = details.SenderRecieverId,SqlDbType=SqlDbType.BigInt},
     new SqlParameter() {ParameterName = "@momentId",Value = details.MomentId,SqlDbType=SqlDbType.BigInt},
      new SqlParameter() {ParameterName = "@addedDate",Value = details.AddedDate,SqlDbType=SqlDbType.VarChar},
       new SqlParameter() {ParameterName = "@localMessageId",Value = details.LocalMessageId,SqlDbType=SqlDbType.Int},
};

            object[] parameters = sp.ToArray();
            var users = _favRepository.ExecuteStoredProcedureList<AuthUser>(PROC_INSERT_FAVOURITES, parameters);
            if (users != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public List<FavouriteDetailsWithUserName> GetFavouritesListByUserId(long id)
        {
            SqlParameter param = new SqlParameter("@favouriteUserId", id);

            var favList = _favRepository.ExecuteStoredProcedureList<FavouriteDetailsWithUserName>(PROC_GET_ALL_FAVOURIES_BY_USERID, param).ToList();
            return favList;
        }
        public List<FavouritesDetails> GetFavouritesListBymomentId(long id)
        {
            SqlParameter param = new SqlParameter("@momentId", id);

            var favList = _favRepository.ExecuteStoredProcedureList<FavouritesDetails>(PROC_GET_ALL_FAVOURIES_BY_MOMENTID, param).ToList();
            return favList;
        }

        public ReturnMsg DeleteFavByUserAndMomentId(long userId,long momentId)
        {
            ReturnMsg msg = new ReturnMsg();
            try
            {
                List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@favouriteUserId",Value= userId,SqlDbType=SqlDbType.BigInt},
    new SqlParameter() {ParameterName = "@momentId",Value = momentId,SqlDbType=SqlDbType.BigInt},
};
                object[] parameters = sp.ToArray();
                _favRepository.ExecuteStoredProcedure(PROC_DELETE_FAV_BY_USER_AND_MOMENT_ID, parameters);
                msg.IsSuccess = true;
            }
            catch (Exception e)
            {
                msg.IsSuccess = false;
                msg.Message = e.Message;
            }
            return msg;
        }


        public ReturnMsg DeleteFavoritesById(long id)
        {
            ReturnMsg msg = new ReturnMsg();
            try
            {
                SqlParameter param = new SqlParameter("@favouritesId", id);
                _favRepository.ExecuteStoredProcedure(PROC_DELETE_FAV_BY_ID, param);
                msg.IsSuccess = true;
            }
            catch (Exception e)
            {
                msg.IsSuccess = false;
                msg.Message = e.Message;
            }
            return msg;
        }
    }
}
