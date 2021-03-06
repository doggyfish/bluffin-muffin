﻿namespace BluffinMuffin.Protocol.Commands.Lobby.Career
{
    public class AuthenticateUserResponse : AbstractLobbyResponse<AuthenticateUserCommand>
    {
        public bool Success { get; set; }

        public AuthenticateUserResponse()
        {
        }

        public AuthenticateUserResponse(AuthenticateUserCommand command)
            : base(command)
        {
        }
    }
}
