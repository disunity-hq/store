using AutoMapper;


namespace Disunity.Store.Entities.DataTransferObjects {

    public class UnityVersionProfile : Profile {

        public UnityVersionProfile() {
            CreateMap<UnityVersion, string>().ConvertUsing(u => u.Version);
        }

    }

}