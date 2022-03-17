using AutoMapper;
using MvpVendingMachineApp.Api.Controllers.v1.Users.Requests;
using MvpVendingMachineApp.Application.Users.Command;
using MvpVendingMachineApp.Application.Users.Query;
using MvpVendingMachineApp.Domain.Entities.Users;

namespace MvpVendingMachineApp.Api.AutoMapperProfiles.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<SignUpRequest, CreateUserCommand>();

            CreateMap<LoginRequest, LoginCommand>();
            CreateMap<UpdateUserRequest, UpdateUserCommand>();
            CreateMap<DeleteUserRequest, DeleteUserCommand>();
            CreateMap<DepositFundRequest, DepositFundCommand>();
            CreateMap<UserBuyProductRequest, UserBuyProductCommand>();
            CreateMap<GetUserByIdRequest, GetUserbyIdQuery>();
        }
    }
}
