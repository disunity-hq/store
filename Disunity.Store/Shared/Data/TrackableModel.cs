using System;
using System.ComponentModel.DataAnnotations;
using Disunity.Store.Shared.Data.Hooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disunity.Store.Shared.Data
{
    public class TrackableModel : ITrackableModel, IBeforeUpdate
    {
        [DataType(DataType.DateTime)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; }

        [OnBeforeCreate]
        public static void OnBeforeCreate(TrackableModel entity, IServiceProvider services) {
            var now = DateTime.Now;
            entity.CreatedAt = now;
            entity.UpdatedAt = now;

            var logger = services.GetRequiredService<ILogger<TrackableModel>>();
            logger.LogInformation($"TrackableModel setting time to {now}");
        }

        public virtual void OnBeforeUpdate(IServiceProvider serviceProvider)
        {
            UpdatedAt = DateTime.Now;
        }
    }
}