using AutoMapper;


namespace Disunity.Store.Entities.DataTransferObjects {

    public class ModProfile : Profile {

        public ModProfile() {
            CreateMap<Mod, ModDto>();
            CreateMap<ModVersion, ModVersionDto>();
            CreateMap<ModDependency, ModDependencyDto>();
            CreateMap<ModTargetCompatibility, ModTargetCompatibilityDto>();
        }

    }

}