using AutoMapper;
using FacebookClient.DTOModels;
using FacebookClient.Models;

namespace FacebookClient.Mappers
{
    public class UserFriendsProfile : Profile
    {
        public UserFriendsProfile()
        {
            CreateMap<UserFriendFb, UserFriend>()
                .ForMember(destination => destination.UriPicture,
                    options => options.MapFrom(src => src.UriPicture.PictureInfoFb.Url))
                .ForMember(destination => destination.MusicCollection,
                    options => options.MapFrom(src => src.MusicCollection.MusicInfo));
            CreateMap<MusicInfoFb, MusicFriends>();
        }
    }
}
