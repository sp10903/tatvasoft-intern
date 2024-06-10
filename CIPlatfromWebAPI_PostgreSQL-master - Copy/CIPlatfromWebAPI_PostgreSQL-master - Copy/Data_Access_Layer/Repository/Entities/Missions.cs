using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;


namespace Data_Access_Layer.Repository.Entities
{
    
    public class Missions : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string MissionTitle { get; set; }
        public string MissionDescription { get; set; }
        [MaybeNull,AllowNull]
        public string MissionOrganisationName { get; set; } = null;
        [MaybeNull, AllowNull]
        public string MissionOrganisationDetail { get; set; } = null;
        public int CountryId { get; set; }       
        public int CityId { get; set; }
        // Change StartDate and EndDate data types
        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }
        [MaybeNull, AllowNull]
        public string MissionType { get; set; } = null;
        public int? TotalSheets { get; set; }
        [Column(TypeName = "date")]
        [MaybeNull, AllowNull]
        public DateTime? RegistrationDeadLine { get; set; } = null;
        public string MissionThemeId { get; set; }
        public string MissionSkillId { get; set; } 
        public string MissionImages { get; set; }
        [MaybeNull, AllowNull]
        public string MissionDocuments { get; set; } = null;
        [MaybeNull, AllowNull]
        public string MissionAvilability { get; set; } = null;
        [MaybeNull, AllowNull]
        public string MissionVideoUrl { get; set; } = null;
        [NotMapped]
        public string CountryName { get; set; }
        [NotMapped]
        public string CityName { get; set; }
        [NotMapped]
        public string MissionThemeName { get; set; }
        [NotMapped]
        public string MissionSkillName { get; set; }
        [NotMapped]
        [MaybeNull, AllowNull]
        public string MissionStatus { get; set; } = null;
        [NotMapped]
        [MaybeNull,AllowNull]
        public string MissionApplyStatus { get; set; } = null;
        [NotMapped]
        [MaybeNull, AllowNull]
        public string MissionApproveStatus { get; set; } = null;
        [NotMapped]
        [MaybeNull, AllowNull]
        public string MissionDateStatus { get; set; } = null;
        [NotMapped]
        [MaybeNull, AllowNull]
        public string MissionDeadLineStatus { get; set; } = null;
        [NotMapped]
        [MaybeNull, AllowNull]
        public string MissionFavouriteStatus { get; set; } = null;
        [NotMapped]
        [MaybeNull, AllowNull]
        public int Rating { get; set; } = 0;
    }

    public class SortestData
    {
        public int UserId { get; set; }
        public string SortestValue { get; set; }
        public int MissionId { get; set; }
    }
}
