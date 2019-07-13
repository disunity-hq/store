using System.ComponentModel.DataAnnotations;


namespace Disunity.Store.Entities.DataTransferObjects {

    public class OrgMemberDto {

        /// <summary>
        /// The id of a user that is in this organization
        /// </summary>
        [Required]
        public string UserId { get; set; }

        /// <summary>
        /// The organization's id
        /// </summary>
        [Required]
        public int OrgId { get; set; }

        /// <summary>
        /// The role this user has within this organization
        /// </summary>
        [Required]
        public OrgMemberRole Role { get; set; }

    }

}