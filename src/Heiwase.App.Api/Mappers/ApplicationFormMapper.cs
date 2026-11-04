using Heiwase.App.Infrastructure.Entities;
using Heiwase.App.Shared.Models;

namespace Heiwase.App.Api.Mappers;

internal static class ApplicationFormMapper
{
    public static ApplicationFormEntity ToEntity(this ApplicationFormDto application)
    {
        ArgumentNullException.ThrowIfNull(application);

        return new ApplicationFormEntity
        {
            Name = application.Name,
            Email = application.Email,
            Phone = application.Phone,
            Sex = application.Sex.ToString(),
            DateOfBirth = application.DateOfBirth,
            GuardianName = application.GuardianName,
            TrainingTypes = [.. application.TrainingTypes.Select(trainingType => trainingType.ToString())],
            Message = application.Message,
            SubmittedAt = DateTime.UtcNow,
            IsReviewed = false
        };
    }
}
