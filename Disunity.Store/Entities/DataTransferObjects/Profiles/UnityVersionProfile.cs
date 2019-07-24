using AutoMapper;


namespace Disunity.Store.Entities.DataTransferObjects.Profiles {

    public class UnityVersionProfile : Profile {

        public UnityVersionProfile() {
            CreateMap<UnityVersion, string>().ConvertUsing(u => u.VersionNumber);
        }

    }

}