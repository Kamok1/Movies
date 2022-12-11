using Models.User;

namespace Models.Review;

public record DtoReview
{
    public int Id { get; init; }
    public string Title { get; init; }
    public string Body { get; init; }
    public float Rate { get; init; }
    public DtoUser User { get; init; }

    public DtoReview(Data.Models.Review review)
    {
        Rate = review.Rate;
        Title = review.Title;
        Body = review.Body;
        Id = review.Id;
        User = new DtoUser(review.User);
    }
}

