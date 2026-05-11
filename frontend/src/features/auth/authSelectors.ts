import type { RootState } from "../../app/store";

export const selectAuthState = (state: RootState) => state.auth;

export const selectCurrentUser = (state: RootState) => state.auth.user;

export const selectAccessToken = (state: RootState) => state.auth.accessToken;

export const selectRefreshToken = (state: RootState) => state.auth.refreshToken;

export const selectIsAuthenticated = (state: RootState) =>
  state.auth.isAuthenticated;

export const selectAuthLoading = (state: RootState) => state.auth.isLoading;

export const selectAuthError = (state: RootState) => state.auth.error;

export const selectAuthErrorMessage = (state: RootState) =>
  state.auth.error?.message;

export const selectCurrentUserRole = (state: RootState) =>
  state.auth.user?.role;

export const selectIsAdmin = (state: RootState) =>
  state.auth.user?.role === "Admin";

export const selectIsAgent = (state: RootState) =>
  state.auth.user?.role === "Agent";

export const selectIsNormalUser = (state: RootState) =>
  state.auth.user?.role === "User";
