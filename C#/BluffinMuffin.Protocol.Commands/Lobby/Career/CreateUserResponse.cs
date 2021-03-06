﻿namespace BluffinMuffin.Protocol.Commands.Lobby.Career
{
    public class CreateUserResponse : AbstractLobbyResponse<CreateUserCommand>
    {
        public bool Success { get; set; }

        public CreateUserResponse()
        {
        }

        public CreateUserResponse(CreateUserCommand command)
            : base(command)
        {
        }
    }
}
