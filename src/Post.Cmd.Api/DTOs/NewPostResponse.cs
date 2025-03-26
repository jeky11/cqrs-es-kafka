using Post.Common.DTOs;

namespace Post.Cmd.Api.DTOs;

public record NewPostResponse : BaseResponse
{
    public Guid Id { get; set; }
}