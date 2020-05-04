﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace EGID.Application.DbInitializer
{
    public class InitializeDbCommand : IRequest
    {
        public class InitializeDbHandler : IRequestHandler<InitializeDbCommand>
        {
            private readonly IRoleManagerService _roleManager;
            private readonly ICardManagerService _cardManager;
            private readonly IKeysGeneratorService _keysGenerator;

            public InitializeDbHandler(
                ICardManagerService cardManager,
                IRoleManagerService roleManager,
                IKeysGeneratorService keysGenerator)
            {
                _roleManager = roleManager;
                _keysGenerator = keysGenerator;
                _cardManager = cardManager;
            }

            public async Task<Unit> Handle(InitializeDbCommand request, CancellationToken _)
            {
                await InitializeRoles();
                await InitializeAdmin();

                return Unit.Value;
            }

            private async Task InitializeRoles()
            {
                if (await _roleManager.AnyAsync) return;

                foreach (var role in Roles.GetRoles())
                    await _roleManager.CreateRoleAsync(role);

            }

            private async Task InitializeAdmin()
            {
                if (await _cardManager.AnyAsync) return;

                var (result, id) = await _cardManager.RegisterAdminAsync(
                    Guid.NewGuid().ToString(),
                    "#m1234567",
                    "admin@admin.com",
                    "010000000000000",
                    _keysGenerator.PublicKeyXml,
                    _keysGenerator.PrivateKeyXml
                );

                
                if (!result.Succeeded) throw new Exception("failed to initialize admin");

                result = await _cardManager.AddToRoleAsync(id, Roles.Admin);

                if (!result.Succeeded) throw new Exception("failed to add admin to the admin role");
            }
        }
    }
}
