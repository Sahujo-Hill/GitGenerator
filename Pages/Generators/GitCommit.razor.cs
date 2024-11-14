using Microsoft.JSInterop;

namespace GitGenerator.Pages.Generators;

public partial class GitCommit
{
    public string commitType { get; set; } = string.Empty;
    public string commitScope { get; set; } = string.Empty;
    public string commitDescription { get; set; } = string.Empty;

    public bool breakingChange { get; set; } = false;
    public string breakingChangeDescription { get; set; } = string.Empty;

    public bool extraFooters { get; set; } = false;
    public string extraFooterReviewedBy { get; set; } = string.Empty;
    public string extraFooterReferences { get; set; } = string.Empty;

    public string commitMessage { get; set; }



    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // Initialize tooltips
            await JSRuntime.InvokeVoidAsync("initializeTooltips");
        }
    }

    private void SetCommitTypeToFix()
    {
        commitType = "fix";
        UpdateCommitMessage();
    }

    private void SetCommitTypeToFeature()
    {
        commitType = "feat";
        UpdateCommitMessage();
    }

    private void SetCommitScopeToClient()
    {
        commitScope = "client";
        UpdateCommitMessage();
    }

    private void SetCommitScopeToApi()
    {
        commitScope = "api";
        UpdateCommitMessage();
    }

    private void SetCommitScopeToShared()
    {
        commitScope = "sharedPackage";
        UpdateCommitMessage();
    }

    private void SetBreakingChangeToTrue()
    {
        breakingChange = true;
        UpdateCommitMessage();
    }

    private void SetBreakingChangeToFalse()
    {
        breakingChange = false;
        ClearBreakingChange();
        UpdateCommitMessage();
    }

    private void ClearBreakingChange()
    {
        breakingChangeDescription = string.Empty;
    }

    private void SetExtraFootersToTrue()
    {
        extraFooters = true;
        UpdateCommitMessage();
    }

    private void SetExtraFootersToFalse()
    {
        extraFooters = false;
        ClearExtraFooters();
        UpdateCommitMessage();
    }

    private void ClearExtraFooters()
    {
        extraFooterReviewedBy = string.Empty;
        extraFooterReferences = string.Empty;
    }

    public void commitConstructor() => UpdateCommitMessage();

    public void UpdateCommitMessage()
    {
        commitMessage = BuildCommitMessage();
    }

    private string BuildCommitMessage()
    {
        string scopeSection = string.IsNullOrWhiteSpace(commitScope) ? "" : $"({commitScope})";

        string breakingChangeMarker = breakingChange ? "!" : "";
        string breakingChangeSection = string.IsNullOrWhiteSpace(breakingChangeDescription) ? "" : $"\n\nBREAKING CHANGE: {breakingChangeDescription}";

        string extraFooterReviews = string.IsNullOrWhiteSpace(extraFooterReviewedBy) ? "" : $"\nReviewed-by: {extraFooterReviewedBy}";
        string extraFooterRefs = string.IsNullOrWhiteSpace(extraFooterReferences) ? "" : $"\nRef: {extraFooterReferences}";
        string extraFooterSection = extraFooters ? $"\n{extraFooterReviews}{extraFooterRefs}" : "";

        return $"{commitType}{scopeSection}{breakingChangeMarker}: {commitDescription}{breakingChangeSection}{extraFooterSection}";
    }

    private async Task CopyToClipboard()
    {
        await JSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", commitMessage);
    }

    public void ClearForm()
    {
        ClearBreakingChange();
        ClearExtraFooters();

        commitType = string.Empty;
        commitScope = string.Empty;
        commitDescription = string.Empty;
        breakingChange = false;
        extraFooters = false;

        UpdateCommitMessage();
    }

}
