using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Disunity.Store.Entities {

    public class ModDependency {

        public int DependantId { get; set; }
        /// <summary>
        /// The <see cref="ModVersion"/> whose dependency is being described
        /// </summary>
        public ModVersion Dependant { get; set; }

        public int DependencyId { get; set; }
        /// <summary>
        /// The dependency required by <see cref="Dependant"/>
        /// </summary>
        public Mod Dependency { get; set; }

        public int? MinVersionId { get; set; }
        /// <summary>
        /// The min version compatible with <see cref="Dependant"/>.
        /// May be null, signifying all versions below <see cref="MaxVersion"/> are compatible
        /// </summary>
        public ModVersion MinVersion { get; set; }

        public int? MaxVersionId { get; set; }
        /// <summary>
        /// The max version compatible with <see cref="Dependant"/>.
        /// May be null, signifying all versions above <see cref="MinVersion"/> are compatible
        /// </summary>
        public ModVersion MaxVersion { get; set; }
        
        public class ModDependencyConfiguration: IEntityTypeConfiguration<ModDependency> {

            public void Configure(EntityTypeBuilder<ModDependency> builder) {
                builder.HasKey(c => new {c.DependantId, c.DependencyId});
            }

        }

    }

}