using MediatR;
using Quiz.Common.Results;

namespace Quiz.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;