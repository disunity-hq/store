using Disunity.Store.Entities;
using Disunity.Store.Shared.Data;

using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Disunity.Store.Shared.Extensions {

    public static class EntityTypeBuilderExtensions {

        public static IndexBuilder HasVersionIndex<T>(this EntityTypeBuilder<T> builder)
            where T : class, IVersionModel {
            return builder.HasIndex(v => v.VersionNumberId);
        }

    }

}