using Quiz.Application.Abstractions.Messaging;

namespace Quiz.Application.UseCases.Users.GetByGames;

public sealed record GetGamesByUserQuery(Guid UserId) : IQuery<IReadOnlyCollection<GetGamesByUserResponse>>;