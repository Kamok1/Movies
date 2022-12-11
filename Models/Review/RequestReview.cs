namespace Models.Review;

public record RequestReview
{
    public string Title { get; init; }
    public string Body { get; init; }
    public float Rate { get; init; }
}