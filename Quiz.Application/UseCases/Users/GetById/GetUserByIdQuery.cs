using Quiz.Application.Abstractions.Messaging;

namespace Quiz.Application.UseCases.Users.GetById;

public sealed record GetUserByIdQuery(Guid UserId) : IQuery<UserResponse>;
