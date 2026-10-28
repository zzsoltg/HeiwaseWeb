using System.ComponentModel.DataAnnotations;

using Heiwase.App.Blazor.ViewModels;
using Heiwase.App.Shared.Enums;

namespace Heiwase.App.Tests.Blazor;

public sealed class ApplicantModelTests
{
    private static DateOnly YearsAgo(int years, int extraDays = 0)
        => DateOnly.FromDateTime(DateTime.Today).AddYears(-years).AddDays(extraDays);

    private static ApplicantModel Valid() => new()
    {
        Name = "Test Elek",
        Email = "elek@example.com",
        Sex = GenderType.Male,
        DateOfBirth = YearsAgo(30),
        TrainingTypes = [TrainingType.Adult],
    };

    private static List<ValidationResult> RunValidate(ApplicantModel model)
        => [.. model.Validate(new ValidationContext(model))];

    private static List<ValidationResult> RunDataAnnotations(ApplicantModel model)
    {
        List<ValidationResult> results = [];
        Validator.TryValidateObject(model, new ValidationContext(model), results, validateAllProperties: true);
        return results;
    }

    [Fact]
    public void IsMinor_Is_False_Without_Birth_Date()
        => Assert.False(new ApplicantModel().IsMinor);

    [Fact]
    public void IsMinor_Is_True_The_Day_Before_The_18th_Birthday()
        => Assert.True(new ApplicantModel { DateOfBirth = YearsAgo(18, extraDays: 1) }.IsMinor);

    [Fact]
    public void IsMinor_Is_False_On_The_18th_Birthday()
        => Assert.False(new ApplicantModel { DateOfBirth = YearsAgo(18) }.IsMinor);

    [Fact]
    public void IsMinor_Is_True_For_A_Child()
        => Assert.True(new ApplicantModel { DateOfBirth = YearsAgo(10) }.IsMinor);

    [Fact]
    public void Valid_Adult_Application_Has_No_Errors()
        => Assert.Empty(RunValidate(Valid()));

    [Fact]
    public void Minor_Without_Guardian_Is_Rejected()
    {
        ApplicantModel model = Valid();
        model.DateOfBirth = YearsAgo(10);
        model.TrainingTypes = [TrainingType.Child];

        ValidationResult error = Assert.Single(RunValidate(model));
        Assert.Contains(nameof(ApplicantModel.GuardianName), error.MemberNames);
    }

    [Fact]
    public void Minor_With_Guardian_Is_Accepted()
    {
        ApplicantModel model = Valid();
        model.DateOfBirth = YearsAgo(10);
        model.TrainingTypes = [TrainingType.Child];
        model.GuardianName = "Szülő";

        Assert.Empty(RunValidate(model));
    }

    [Theory]
    [InlineData(GenderType.Male)]
    [InlineData(GenderType.Unknown)]
    public void WomenSelfDefense_Requires_Female(GenderType sex)
    {
        ApplicantModel model = Valid();
        model.Sex = sex;
        model.TrainingTypes = [TrainingType.WomenSelfDefense];

        ValidationResult error = Assert.Single(RunValidate(model));
        Assert.Contains(nameof(ApplicantModel.TrainingTypes), error.MemberNames);
    }

    [Fact]
    public void WomenSelfDefense_Is_Accepted_For_Female()
    {
        ApplicantModel model = Valid();
        model.Sex = GenderType.Female;
        model.TrainingTypes = [TrainingType.WomenSelfDefense];

        Assert.Empty(RunValidate(model));
    }

    [Fact]
    public void Child_And_Adult_Cannot_Be_Combined()
    {
        ApplicantModel model = Valid();
        model.DateOfBirth = null;
        model.TrainingTypes = [TrainingType.Child, TrainingType.Adult];

        ValidationResult error = Assert.Single(RunValidate(model));
        Assert.Contains(nameof(ApplicantModel.TrainingTypes), error.MemberNames);
    }

    [Fact]
    public void Adult_Cannot_Apply_To_Child_Training()
    {
        ApplicantModel model = Valid();
        model.TrainingTypes = [TrainingType.Child];

        ValidationResult error = Assert.Single(RunValidate(model));
        Assert.Contains(nameof(ApplicantModel.TrainingTypes), error.MemberNames);
    }

    [Fact]
    public void Child_Training_Is_Accepted_When_Birth_Date_Is_Unknown()
    {
        ApplicantModel model = Valid();
        model.DateOfBirth = null;
        model.TrainingTypes = [TrainingType.Child];

        Assert.Empty(RunValidate(model));
    }

    [Fact]
    public void Required_Fields_Are_Enforced_By_Data_Annotations()
    {
        ApplicantModel model = new();

        string[] members = [.. RunDataAnnotations(model).SelectMany(e => e.MemberNames)];

        Assert.Contains(nameof(ApplicantModel.Name), members);
        Assert.Contains(nameof(ApplicantModel.Email), members);
    }

    [Fact]
    public void Invalid_Email_Is_Rejected()
    {
        ApplicantModel model = Valid();
        model.Email = "not-an-email";

        Assert.Contains(RunDataAnnotations(model), e => e.MemberNames.Contains(nameof(ApplicantModel.Email)));
    }

    [Fact]
    public void Valid_Model_Passes_Data_Annotations()
        => Assert.Empty(RunDataAnnotations(Valid()));
}
