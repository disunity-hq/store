using AutoMapper;


namespace Disunity.Store.Entities.DataTransferObjects {

    public class OrgProfile : Profile {

        public OrgProfile() {
            CreateMap<Org, OrgDto>();
            CreateMap<OrgMember, OrgMemberDto>();
        }

    }

}