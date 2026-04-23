using System.ComponentModel.DataAnnotations;

namespace Models.Events.Requests
{
    public class ClaimEventRewardRequest : IValidatableObject
    {
        [Required(ErrorMessage = "Event id is required.")]
        public int EventId { get; set; }

        public int? ThresholdId { get; set; }

        public int? TaskId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!ThresholdId.HasValue && !TaskId.HasValue)
            {
                yield return new ValidationResult(
                    "Either thresholdId or taskId is required.",
                    new[] { nameof(ThresholdId), nameof(TaskId) });
            }

            if (ThresholdId.HasValue && ThresholdId <= 0)
            {
                yield return new ValidationResult(
                    "Threshold id must be greater than 0.",
                    new[] { nameof(ThresholdId) });
            }

            if (TaskId.HasValue && TaskId <= 0)
            {
                yield return new ValidationResult(
                    "Task id must be greater than 0.",
                    new[] { nameof(TaskId) });
            }
        }
    }
}