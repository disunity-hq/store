using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Disunity.Store.Shared.Data
{
    public class TrackableModel : ITrackableModel, IBeforeCreate, IBeforeSave
    {
        [DataType(DataType.DateTime)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; }

        public virtual void OnBeforeCreate(IServiceProvider serviceProvider)
        {
            CreatedAt = DateTime.Now;
        }

        public virtual void OnBeforeSave(IServiceProvider serviceProvider)
        {
            UpdatedAt = DateTime.Now;
        }
    }
}