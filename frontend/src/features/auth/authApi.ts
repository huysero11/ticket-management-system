import axiosClient from "../../services/axiosClient";
import type { ApiResponse } from "../../types/common";
import type { AuthResponse, LoginRequest, RegisterRequest } from "./authTypes";

const BACKEND_URL = import.meta.env.BACKEND_URL || "http://localhost:5049/api";
const AUTH_URL = `${BACKEND_URL}/auth`;

export const authApi = {
  login(payload: LoginRequest): Promise<ApiResponse<AuthResponse>> {
    return axiosClient.post<
      ApiResponse<AuthResponse>,
      ApiResponse<AuthResponse>,
      LoginRequest
    >(`${AUTH_URL}/login`, payload);
  },
  register(payload: RegisterRequest): Promise<ApiResponse<AuthResponse>> {
    return axiosClient.post<
      ApiResponse<AuthResponse>,
      ApiResponse<AuthResponse>,
      RegisterRequest
    >(`${AUTH_URL}/register`, payload);
  },
  logout(refreshToken: string): Promise<ApiResponse<null>> {
    return axiosClient.post<
      ApiResponse<null>,
      ApiResponse<null>,
      { refreshToken: string }
    >(`${AUTH_URL}/logout`, { refreshToken });
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
