﻿using System;

namespace Trevo.Core.Model.Favourites
{
    public  class FavouriteDetailsWithUserName :BaseEntity
    {
        public long FavouritesId { get; set; }

        public string Message { get; set; }

        public long FavouriteUserId { get; set; }

        public int IsSender { get; set; }

        public long SenderRecieverId { get; set; }

        public long MomentId { get; set; }

        public string FavouritesUserName { get; set; }

        public string SenderRecieverName { get; set; }

        public string ImagePath { get; set; }

        public long CountryId { get; set; }

        public string AddedDate { get; set; }

        public Nullable<int> LocalMessageId { get; set; }

    }
}
