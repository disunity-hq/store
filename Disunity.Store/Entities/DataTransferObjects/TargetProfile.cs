using AutoMapper;


namespace Disunity.Store.Entities.DataTransferObjects {

    public class TargetProfile : Profile {

        public TargetProfile() {
            CreateMap<Target, TargetDto>();

            // BUG Allegedly AutoMapper can figure this out on it's own. Not sure why it isnt'
            CreateMap<TargetVersion, TargetVersionDto>()
                .ForMember(
                    v => v.MaxCompatibleVersion,
                    m => m.MapFrom(
                        s => s.DisunityCompatibility.MaxCompatibleVersion.Version))
                .ForMember(
                    v => v.MinCompatibleVersion,
                    m => m.MapFrom(
                        s => s.DisunityCompatibility.MinCompatibleVersion.Version));
        }

    }

}