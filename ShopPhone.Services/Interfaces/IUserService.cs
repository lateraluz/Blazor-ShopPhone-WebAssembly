﻿using ShopPhone.Shared.Request;
using ShopPhone.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Services.Interfaces;

public interface IUserService
{
    Task<LoginResponseDTO> LoginAsync(LoginRequestDTO request);
    Task<RefreshTokenDTO> RefreshAsync(RefreshTokenDTO request);
}
