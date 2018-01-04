using System.Collections.Generic;
using Trevo.Core.Model;
using Trevo.Core.Model.Favourites;

namespace Trevo.Services.FavouriteService
{
   public interface IFavouritesService
    {
        bool InsertFavourites(FavouritesDetails details);
        List<FavouriteDetailsWithUserName> GetFavouritesListByUserId(long id);
        ReturnMsg DeleteFavByUserAndMomentId(long userId, long momentId);
        List<FavouritesDetails> GetFavouritesListBymomentId(long id);
        ReturnMsg DeleteFavoritesById(long id);
    }
}
