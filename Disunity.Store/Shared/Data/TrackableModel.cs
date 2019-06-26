using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Disunity.Store.Shared.Data
{
    public class TrackableModel : AutoModel, ITrackableModel
    {
        [DataType(DataType.Date)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; }

        public override void OnBeforeCreate(ApplicationDbContext dbContext)
        {
            CreatedAt = DateTime.Now;
        }

        public override void OnBeforeSave(ApplicationDbContext dbContext)
        {
            UpdatedAt = DateTime.Now;
        }
    }
}