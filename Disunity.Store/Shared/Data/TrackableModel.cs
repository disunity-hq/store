using System;
using System.ComponentModel.DataAnnotations;

using EFCoreHooks.Attributes;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Shared.Data {

    public class TrackableModel : ITrackableModel {

        [DataType(DataType.DateTime)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; }

        [OnBeforeCreate(WatchDescendants = true)]
        public static void OnBeforeCreate(TrackableModel entity, IServiceProvider services) {
            var now = DateTime.Now;
            entity.CreatedAt = now;
            entity.UpdatedAt = now;

            var logger = services.GetRequiredService<ILogger<TrackableModel>>();
            logger.LogInformation($"TrackableModel setting time to {now}");
        }

        [OnBeforeUpdate(WatchDescendants = true)]
        public static void OnBeforeUpdate(TrackableModel entity, IServiceProvider serviceProvider) {
            entity.UpdatedAt = DateTime.Now;
        }

    }

}