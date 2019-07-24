using AutoMapper;


namespace Disunity.Store.Entities.DataTransferObjects.Profiles {

    public class ModProfile : Profile {

        public ModProfile() {
            CreateMap<Mod, ModDto>();
            CreateMap<ModVersion, ModVersionDto>();
            CreateMap<ModDependency, ModDependencyDto>();
            CreateMap<ModTargetCompatibility, ModTargetCompatibilityDto>();
        }

    }

}