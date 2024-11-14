using Microsoft.JSInterop;

namespace GitGenerator.Pages.Generators;

public partial class PullRequest
{
    public string pullRequest { get; set; }

    public bool HasFunctionalityMetAcceptanceCriteria { get; set; }
    public bool HasUpdatedUnitTests { get; set; }
    public bool HasUpdatedEndToEndTests { get; set; }
    public bool HasReviewedSonarQubeIssues { get; set; }
    public bool HasUpdatedDocumentation { get; set; }
    public bool HasAddressedSecurityImplications { get; set; }
    public bool HasAddressedPerformanceImplications { get; set; }

    private List<string> Changes { get; set; } = new List<string>();
    private string newChange { get; set; } = string.Empty;
    private void AddChange()
    {
        if (!string.IsNullOrWhiteSpace(newChange))
        {
            Changes.Add(newChange);
            newChange = string.Empty;
            UpdatePullRequest();
        }
    }
    private void RemoveChange(int index)
    {
        if (index >= 0 && index < Changes.Count)
        {
            Changes.RemoveAt(index);
            UpdatePullRequest();
        }
    }

    private List<string> Test { get; set; } = new List<string>();
    private string newInstruction { get; set; } = string.Empty;
    private void AddInstructionToTest()
    {
        if (!string.IsNullOrWhiteSpace(newInstruction))
        {
            Test.Add(newInstruction);
            newInstruction = string.Empty;
            UpdatePullRequest();
        }
    }
    private void RemoveInstructionFromTest(int index)
    {
        if (index >= 0 && index < Test.Count)
        {
            Test.RemoveAt(index);
            UpdatePullRequest();
        }
    }

    private List<string> RelatedPullRequests { get; set; } = new List<string>();
    private string newRelatedPullRequest { get; set; } = string.Empty;
    private void AddRelatedPullRequest()
    {
        if (!string.IsNullOrWhiteSpace(newRelatedPullRequest))
        {
            RelatedPullRequests.Add(newRelatedPullRequest);
            newRelatedPullRequest = string.Empty;
            UpdatePullRequest();
        }
    }
    private void RemoveRelatedPullRequest(int index)
    {
        if (index >= 0 && index < RelatedPullRequests.Count)
        {
            RelatedPullRequests.RemoveAt(index);
            UpdatePullRequest();
        }
    }


    public void PullRequestConstructor() => UpdatePullRequest();

    public void UpdatePullRequest()
    {
        pullRequest = BuildPullRequest();
    }

    private string BuildPullRequest()
    {
        string changesSection = string.Join("\n", Changes.Select(change => $"- {change}"));

        string testSection = string.Join("\n", Test.Select((instruction, index) => $"{index + 1}. {instruction}"));

        string relatedPullRequestSection = string.Join("\n", RelatedPullRequests.Select(pullRequest => $"- {pullRequest}"));

        string checklistSection = string.Empty; //To do: add mapping here

        return $"##What changes have been made?\n" +
               $"{changesSection}\n\n" +
               $"##How can the changes be tested?\n" +
               $"{testSection}\n\n" +
               $"##Related PRs\n" +
               $"{relatedPullRequestSection}\n\n" +
               $"##Checklist\n" +
               $"Before submitting this PR for review, I have:\n" +
               $"{checklistSection}";
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // Initialize tooltips
            await JSRuntime.InvokeVoidAsync("initializeTooltips");
        }
    }

    private async Task CopyToClipboard()
    {
        await JSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", pullRequest);
    }

    public void ClearForm()
    {
        pullRequest = string.Empty;
        Changes.Clear();
        Test.Clear();
        RelatedPullRequests.Clear();
    }

}
