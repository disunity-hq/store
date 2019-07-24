using AutoMapper;


namespace Disunity.Store.Entities.DataTransferObjects.Profiles {

    public class OrgProfile : Profile {

        public OrgProfile() {
            CreateMap<Org, OrgDto>();
            CreateMap<OrgMember, OrgMemberDto>().ForMember(d => d.UserName, m => m.MapFrom(o => o.User.UserName));
        }

    }

}