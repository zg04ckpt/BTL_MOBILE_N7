using System.ComponentModel.DataAnnotations;

namespace Models.Events.Requests
{
    public class UpdateMyQuizMilestoneChallengeProgressRequest : IValidatableObject
    {
        [Required(ErrorMessage = "Event id is required.")]
        public int EventId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Threshold id must be greater than 0.")]
        public int ThresholdId { get; set; }

        public bool? RewardClaimed { get; set; }

        public bool? Completed { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!RewardClaimed.HasValue && !Completed.HasValue)
            {
                yield return new ValidationResult(
                    "At least one of RewardClaimed or Completed is required.",
                    new[] { nameof(RewardClaimed), nameof(Completed) });
            }
        }
    }
}
