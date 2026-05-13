import axiosClient from "../../services/axiosClient";
import type { ApiResponse } from "../../types/common";
import type {
  AuthResponse,
  LoginRequest,
  RegisterRequest,
  RegisterResponse,
} from "./authTypes";

const AUTH_URL = "/auth";

export const authApi = {
  login(payload: LoginRequest): Promise<ApiResponse<AuthResponse>> {
    return axiosClient.post<
      ApiResponse<AuthResponse>,
      ApiResponse<AuthResponse>,
      LoginRequest
    >(`${AUTH_URL}/login`, payload);
  },

  register(payload: RegisterRequest): Promise<ApiResponse<RegisterResponse>> {
    return axiosClient.post<
      ApiResponse<RegisterResponse>,
      ApiResponse<RegisterResponse>,
      RegisterRequest
    >(`${AUTH_URL}/register`, payload);
  },

  logout(refreshToken: string): Promise<ApiResponse<null>> {
    return axiosClient.post<
      ApiResponse<null>,
      ApiResponse<null>,
      { refreshToken: string }
    >(`${AUTH_URL}/logout`, {
      refreshToken,
    });
  },

  refreshToken(refreshToken: string): Promise<ApiResponse<AuthResponse>> {
    return axiosClient.post<
      ApiResponse<AuthResponse>,
      ApiResponse<AuthResponse>,
      { refreshToken: string }
    >(`${AUTH_URL}/refresh`, {
      refreshToken,
    });
  },
};
