using AutoMapper;
using FinalTaskFacebook.DTOModels;
using FinalTaskFacebook.Models;

namespace FinalTaskFacebook.Mappers
{
    public class UserFriendsProfile : Profile
    {
        public UserFriendsProfile()
        {
            CreateMap<UserFriendFb, UserFriend>()
                .ForMember(destination => destination.UriPicture,
                    options => options.MapFrom(src => src.UriPicture.PictureInfo.Url))
                .ForMember(destination => destination.MusicCollection,
                    options => options.MapFrom(src => src.MusicCollection.MusicInfo));
            CreateMap<MusicInfo, MusicFriends>();
        }
    }
}
