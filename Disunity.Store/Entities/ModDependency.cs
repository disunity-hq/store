using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Disunity.Store.Entities {

    public class ModDependency {

        public int DependantId { get; set; }
        public ModVersion Dependant { get; set; }

        public int DependencyId { get; set; }
        public Mod Dependency { get; set; }

        public int? MinVersionId { get; set; }
        public ModVersion MinVersion { get; set; }

        public int? MaxVersionId { get; set; }
        public ModVersion MaxVersion { get; set; }
        
        public class ModDependencyConfiguration: IEntityTypeConfiguration<ModDependency> {

            public void Configure(EntityTypeBuilder<ModDependency> builder) {
                builder.HasKey(c => new {c.DependantId, c.DependencyId});
            }

        }

    }

}